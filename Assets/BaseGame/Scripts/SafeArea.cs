using UnityEngine;

namespace BaseGame
{
    public class SafeArea : MonoBehaviour
    {
        public bool ignoreXAxis = false;
        public bool ignoreYAxis = false;
        private RectTransform _panel;

        private void Awake()
        {
            _panel = GetComponent<RectTransform>();
            if (_panel == null)
            {
                Debug.LogError("Cannot apply safe area - no RectTransform found on " + name);
                Destroy(gameObject);
                return;
            }
            ApplySafeArea();
        }

        private void ApplySafeArea()
        {
            var realSafeArea = GetRealSafeArea();
            if (ignoreXAxis)
            {
                realSafeArea.x = 0;
                realSafeArea.width = Screen.width;
            }
            if (ignoreYAxis)
            {
                realSafeArea.y = 0;
                realSafeArea.height = Screen.height;
            }

            var anchorMin = realSafeArea.position;
            var anchorMax = realSafeArea.position + realSafeArea.size;
            _panel.anchorMin = new Vector2(anchorMin.x / Screen.width, anchorMin.y / Screen.height);
            _panel.anchorMax = new Vector2(anchorMax.x / Screen.width, anchorMax.y / Screen.height);

            Debug.LogFormat(
                "New safe area applied to {0}: x={1}, y={2}, w={3}, h={4} on full extents w={5}, h={6}",
                name, realSafeArea.x, realSafeArea.y, realSafeArea.width, realSafeArea.height, Screen.width, Screen.height);
        }

        private static Rect GetRealSafeArea()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            try
            {
                // Lấy Unity-safe area để dùng top inset của Unity
                Rect unitySafe = Screen.safeArea;
                int unityTopInset = Mathf.RoundToInt(Screen.height - (unitySafe.y + unitySafe.height));

                using (var unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
                {
                    var activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
                    var window = activity.Call<AndroidJavaObject>("getWindow");
                    var decorView = window.Call<AndroidJavaObject>("getDecorView");

                    // getRootWindowInsets có thể null hoặc không tồn tại trên API thấp hơn
                    var insets = decorView.Call<AndroidJavaObject>("getRootWindowInsets");
                    if (insets == null)
                    {
                        Debug.LogWarning("RootWindowInsets null -> fallback to Screen.safeArea");
                        return unitySafe;
                    }

                    // Lấy type cho systemBars (API 30+). Nếu không tồn tại, fallback.
                    int systemBarsType = 0;
                    try
                    {
                        var typeClass = new AndroidJavaClass("android.view.WindowInsets$Type");
                        systemBarsType = typeClass.CallStatic<int>("systemBars");
                    }
                    catch
                    {
                        // Nếu không có WindowInsets$Type (rất cũ), fallback
                        Debug.LogWarning("WindowInsets.Type.systemBars not available -> fallback to Screen.safeArea");
                        return unitySafe;
                    }

                    var rectInsets = insets.Call<AndroidJavaObject>("getInsets", systemBarsType);
                    if (rectInsets == null)
                    {
                        Debug.LogWarning("getInsets returned null -> fallback to Screen.safeArea");
                        return unitySafe;
                    }

                    int left = rectInsets.Get<int>("left");
                    // TOP: sử dụng giá trị từ Unity (để khớp yêu cầu)
                    int top = unityTopInset;
                    int right = rectInsets.Get<int>("right");
                    int bottom = rectInsets.Get<int>("bottom");

                    float screenWidth = Screen.width;
                    float screenHeight = Screen.height;

                    // Trả về Rect safe area with top từ Unity, left/right/bottom từ Android insets
                    return new Rect(
                        left,
                        bottom,
                        Mathf.Max(0f, screenWidth - left - right),
                        Mathf.Max(0f, screenHeight - top - bottom)
                    );
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError("GetRealSafeArea Error: " + e);
                return Screen.safeArea;
            }
#else
            return Screen.safeArea;
#endif
        }
    }
}