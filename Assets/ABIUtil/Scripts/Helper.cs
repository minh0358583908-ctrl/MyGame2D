using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace BaseGame
{
    public static class Helper
    {
        #region Crypto

        public static string EncodeBase64(this string input)
        {
            using (SHA256 sha = SHA256.Create())
            {
                byte[] hashBytes = sha.ComputeHash(Encoding.UTF8.GetBytes(input));
                string base64 = System.Convert.ToBase64String(hashBytes);
                return base64.Replace("/", "_").Replace("+", "-").Substring(0, 16);
            }
        }

        #endregion

        #region List

        public static void AddElements<T>(this List<T> list, params T[] elements)
        {
            list.AddRange(elements);
        }

        public static T GetRandomElement<T>(this List<T> list)
        {
            return list[Random.Range(0, list.Count)];
        }

        public static List<T> GetRandomListWithUniqueElements<T>(List<T> list, int numberOfElements)
        {
            List<T> result = new();
            List<T> tempList = new(list);
            numberOfElements = Mathf.Min(numberOfElements, tempList.Count);
            while (result.Count < numberOfElements)
            {
                int randomIndex = Random.Range(0, tempList.Count);
                T randomElement = tempList[randomIndex];
                result.Add(randomElement);
                tempList.RemoveAt(randomIndex);
            }
            return result;
        }

        public static List<T> GetNewListFromListWithEndIndex<T>(List<T> list, int index)
        {
            index++;
            List<T> result = new();
            if (index > list.Count) index = list.Count;
            for (int i = 0; i < index; i++)
            {
                result.Add(list[i]);
            }
            return result;
        }

        public static void ShuffleList<T>(List<T> list)
        {
            for (int i = list.Count - 1; i > 0; i--)
            {
                var j = Random.Range(0, i + 1);
                (list[j], list[i]) = (list[i], list[j]);
            }
        }

        public static List<T> Copy<T>(this List<T> src, List<T> dst = null)
        {
            src ??= new List<T>();
            dst ??= new List<T>();
            dst.Clear();
            foreach (var item in src)
                dst.Add(item);
            return dst;
        }

        public static Dictionary<TA, TB> Copy<TA, TB>(this Dictionary<TA, TB> src)
        {
            var dst = new Dictionary<TA, TB>();
            foreach (var item in src)
                dst.Add(item.Key, item.Value);
            return dst;
        }

        #endregion

        #region Vector3

        public static Vector3 Copy(this Vector3 src)
        {
            return new Vector3(src.x, src.y, src.z);
        }

        public static Vector3 GetDirection(Transform a, Transform b)
        {
            return (b.position - a.position).normalized;
        }

        public static bool CheckDistance(Vector3 start, Vector3 end, float threshold)
        {
            return Vector3.Distance(start, end) <= threshold;
        }

        public static Vector3 GetRandomPoint(List<Transform> transforms, float randomRange1, float randomRange2)
        {
            Vector3 position = transforms[Random.Range(0, transforms.Count)].position;
            Vector3 addition = new(Random.Range(randomRange1, randomRange2), 0f, Random.Range(randomRange1, randomRange2));
            return position + addition;
        }

        public static Vector3 GetDirection(Vector3 a, Vector3 b)
        {
            return (b - a).normalized;
        }

        public static Vector3 GetSpecificDirection(Transform a, Transform b)
        {
            Vector3 position = b.position;
            return new Vector3(position.x - a.position.x, 0, position.z - a.position.z).normalized;
        }

        public static Quaternion GetQuaternion(Vector3 targetPosition, Transform transform)
        {
            Vector3 position = transform.position;
            Vector3 lookAtPosition = new(targetPosition.x - position.x,
                position.y,
                targetPosition.z - position.z);
            return Quaternion.LookRotation(lookAtPosition);
        }

        public static void ClampValue(ref int value, int min, int max)
        {
            if (value > max)
            {
                value = min;
            }
        }

        public static Vector3 GetSpecificDirection(Vector3 a, Vector3 b)
        {
            return new Vector3(b.x - a.x, 0, b.z - a.z).normalized;
        }

        public static Vector3 GetRandomVector(Vector3 origin, float minRange, float maxRange)
        {
            return origin + new Vector3(Random.Range(minRange, maxRange), origin.y, Random.Range(minRange, maxRange));
        }

        public static Vector3 GetRandomVector(Vector3 origin, float maxRange)
        {
            return origin + new Vector3(Random.Range(0, maxRange), origin.y, Random.Range(0, maxRange));
        }

        public static Vector3 GetRandomVectorXAxis(Vector3 origin, float maxRange, float maxZ)
        {
            return origin + new Vector3(Random.Range(0, maxRange), origin.y, maxZ);
        }

        public static Vector3 GetRandomVector(Vector3 origin, Vector3 range)
        {
            return origin + new Vector3(Random.Range(-range.x, range.x), 0f, Random.Range(0, range.z));
        }

        public static Vector3 GetRandomPosition(Vector3 origin, float minRange, float maxRange)
        {
            return origin + new Vector3(Random.Range(minRange, maxRange), Random.Range(minRange, maxRange));
        }

        #endregion

        #region Object

        public static Quaternion RandomRotation()
        {
            Vector3 rotation = new(0f, Random.Range(0, 360), 0);
            return Quaternion.Euler(rotation);
        }

        public static Transform GetObjectTransform<T>(T obj)
        {
            return (obj as MonoBehaviour)?.GetComponent<Transform>();
        }

        public static RectTransform GetObjectRectTransform<T>(T obj)
        {
            return (obj as MonoBehaviour)?.GetComponent<RectTransform>();
        }

        public static List<Transform> GetObjectTransforms<T>(List<T> list)
        {
            List<Transform> transforms = new();
            for (int i = 0; i < list.Count; i++)
            {
                Transform cachedTransform = (list[i] as MonoBehaviour).GetComponent<Transform>();
                transforms.Add(cachedTransform);
            }
            return transforms;
        }

        public static Transform GetRandomTransform(List<Transform> transforms)
        {
            int randomIndex = Random.Range(0, transforms.Count);
            return transforms[randomIndex];
        }

        public static Vector3 GetObjectPosition<T>(T obj)
        {
            return (obj as MonoBehaviour).GetComponent<Transform>().position;
        }

        public static List<Vector3> GetObjectPositions<T>(List<T> list)
        {
            List<Vector3> positions = new();
            for (int i = 0; i < list.Count; i++)
            {
                Vector3 position = (list[i] as MonoBehaviour).GetComponent<Transform>().position;
                positions.Add(position);
            }
            return positions;
        }

        public static List<Quaternion> GetObjectRotations<T>(List<T> list)
        {
            List<Quaternion> quaternions = new();
            for (int i = 0; i < list.Count; i++)
            {
                Quaternion quaternion = (list[i] as MonoBehaviour).GetComponent<Transform>().rotation;
                quaternions.Add(quaternion);
            }
            return quaternions;
        }

        public static void SetObjectPosition(MonoBehaviour monoObject, Vector3 position)
        {
            monoObject.transform.position = position;
        }

        public static void SetObjectPosition(MonoBehaviour monoObject, Transform transform, float height)
        {
            monoObject.transform.position = new Vector3(transform.position.x,
                transform.position.y + height,
                transform.position.z);
        }

        public static void SetObjectPositions<T>(List<T> objects, List<Transform> points, float height)
        {
            for (int i = 0; i < objects.Count; i++)
            {
                Transform objTransform = (objects[i] as MonoBehaviour).transform;
                objTransform.position = new Vector3(points[i].position.x,
                    points[i].position.y + height,
                    points[i].position.z);
            }
        }

        public static void SetObjectRandomPositions<T>(List<T> objects, List<Transform> points, float height)
        {
            for (int i = 0; i < objects.Count; i++)
            {
                Transform randomTransform = GetRandomTransform(points);
                Transform objTransform = (objects[i] as MonoBehaviour).transform;
                objTransform.position = new Vector3(randomTransform.position.x,
                    randomTransform.position.y + height,
                    randomTransform.position.z);
                points.Remove(randomTransform);
            }
        }

        public static void SetObjectPositions(List<Transform> objectTransforms, List<Transform> points, float height)
        {
            for (int i = 0; i < objectTransforms.Count; i++)
            {
                objectTransforms[i].position = new Vector3(points[i].position.x,
                    points[i].position.y + height,
                    points[i].position.z);
            }
        }

        public static void SetObjectRotations(List<Transform> objectTransforms, List<Quaternion> quaternions)
        {
            for (int i = 0; i < objectTransforms.Count; i++)
            {
                objectTransforms[i].rotation = quaternions[i];
            }
        }

        #endregion

        #region Color

        public static string ToHex(this Color color, bool includeAlpha = true) // RGBA
        {
            return ((Color32)color).ToHex(includeAlpha);
        }
        public static string ToHex(this Color32 c, bool includeAlpha = true) // RGBA
        {
            return includeAlpha
                ? $"#{c.r:X2}{c.g:X2}{c.b:X2}{c.a:X2}"
                : $"#{c.r:X2}{c.g:X2}{c.b:X2}";
        }
        public static Color ToColor(this string hex)
        {
            if (hex.IsNullOrEmpty()) return Color.white;
            if (hex[0] == '#') hex = hex.Substring(1);

            byte r = 255, g = 255, b = 255, a = 255;
            if (hex.Length == 3) // RGB (FFF)
            {
                r = byte.Parse(new string(hex[0], 2), NumberStyles.HexNumber);
                g = byte.Parse(new string(hex[1], 2), NumberStyles.HexNumber);
                b = byte.Parse(new string(hex[2], 2), NumberStyles.HexNumber);
            }
            else if (hex.Length == 6) // RRGGBB
            {
                r = byte.Parse(hex.Substring(0, 2), NumberStyles.HexNumber);
                g = byte.Parse(hex.Substring(2, 2), NumberStyles.HexNumber);
                b = byte.Parse(hex.Substring(4, 2), NumberStyles.HexNumber);
            }
            else if (hex.Length == 8) // RRGGBBAA
            {
                r = byte.Parse(hex.Substring(0, 2), NumberStyles.HexNumber);
                g = byte.Parse(hex.Substring(2, 2), NumberStyles.HexNumber);
                b = byte.Parse(hex.Substring(4, 2), NumberStyles.HexNumber);
                a = byte.Parse(hex.Substring(6, 2), NumberStyles.HexNumber);
            }
            return new Color32(r, g, b, a);
        }

        #endregion

        #region Image

        public static void SetActive(this Image image, bool active)
        {
            if (image.gameObject.activeSelf == active)
                return;
            image.gameObject.SetActive(active);
        }
        public static void SetActive(this List<Image> images, bool active)
        {
            foreach (var image in images)
                image.gameObject.SetActive(active);
        }
        public static bool ActiveSelf(this Image image)
        {
            return image.gameObject.activeSelf;
        }

        public static void SetSprite(this Image image, Sprite sprite)
        {
            image.sprite = sprite;
        }
        public static void SetSprite(this List<Image> images, Sprite sprite)
        {
            foreach (var image in images)
                image.sprite = sprite;
        }

        #endregion

        #region Button

        public static void SetActive(this Button button, bool active)
        {
            if (button.gameObject.activeSelf == active)
                return;
            button.gameObject.SetActive(active);
        }
        public static void SetActive(this List<Button> buttons, bool active)
        {
            foreach (var button in buttons)
                button.gameObject.SetActive(active);
        }
        public static bool ActiveSelf(this Button button)
        {
            return button.gameObject.activeSelf;
        }

        public static void Interactable(this Button button, bool enable)
        {
            button.interactable = enable;
        }
        public static void Interactable(this List<Button> buttons, bool enable)
        {
            foreach (var button in buttons)
                button.interactable = enable;
        }

        public static void AddOnClickListener(this Button button, UnityAction action)
        {
            button.onClick.AddListener(action);
        }
        public static void AddOnClickListener(this List<Button> buttons, UnityAction action)
        {
            foreach (var button in buttons)
                button.onClick.AddListener(action);
        }

        #endregion

        #region Transform

        public static void SetActive(this Transform transform, bool active)
        {
            if (transform.gameObject.activeSelf == active)
                return;
            transform.gameObject.SetActive(active);
        }
        public static void SetActive(this List<Transform> transforms, bool active)
        {
            foreach (var transform in transforms)
                transform.gameObject.SetActive(active);
        }
        public static bool ActiveSelf(this Transform transform)
        {
            return transform.gameObject.activeSelf;
        }

        #endregion

        #region RectTransform

        public static void SetActive(this RectTransform rectTransform, bool active)
        {
            if (rectTransform.gameObject.activeSelf == active)
                return;
            rectTransform.gameObject.SetActive(active);
        }
        public static void SetActive(this List<RectTransform> rectTransforms, bool active)
        {
            foreach (var rectTransform in rectTransforms)
                rectTransform.gameObject.SetActive(active);
        }
        public static bool ActiveSelf(this RectTransform rectTransform)
        {
            return rectTransform.gameObject.activeSelf;
        }

        #endregion

        #region TextMeshPro

        public static void SetActive(this TextMeshProUGUI text, bool active)
        {
            if (text.gameObject.activeSelf == active) return;
            text.gameObject.SetActive(active);
        }
        public static void SetActive(this List<TextMeshProUGUI> texts, bool active)
        {
            foreach (var text in texts)
                text.gameObject.SetActive(active);
        }
        public static bool ActiveSelf(this TextMeshProUGUI text)
        {
            return text.gameObject.activeSelf;
        }

        public static void SetText(this TMP_InputField inputField, string text)
        {
            inputField.text = text;
        }
        public static void AddOnValueChangedListener(this TMP_InputField inputField, UnityAction<string> action)
        {
            inputField.onValueChanged.AddListener(action);
        }
        public static void AddOnValueChangedListener(this List<TMP_InputField> inputFields, UnityAction<string> action)
        {
            foreach (var inputField in inputFields)
                inputField.onValueChanged.AddListener(action);
        }

        #endregion

        #region ParticleSystem

        public static void SetActive(this ParticleSystem particle, bool active)
        {
            if (particle.gameObject.activeSelf == active) return;
            particle.gameObject.SetActive(active);
        }
        public static void SetActive(this List<ParticleSystem> particles, bool active)
        {
            foreach (var particle in particles)
                particle.gameObject.SetActive(active);
        }
        public static bool ActiveSelf(this ParticleSystem particle)
        {
            return particle.gameObject.activeSelf;
        }

        #endregion

        #region Action

        public static void InvokeAndClean(this Action action)
        {
            action?.Invoke();
            action = null;
        }

        #endregion

        #region SpriteRenderer

        public static void SetActive(this SpriteRenderer spriteRenderer, bool active)
        {
            if (spriteRenderer.gameObject.activeSelf == active)
                return;
            spriteRenderer.gameObject.SetActive(active);
        }
        public static void SetActive(this List<SpriteRenderer> spriteRenderers, bool active)
        {
            foreach (var spriteRenderer in spriteRenderers)
                spriteRenderer.gameObject.SetActive(active);
        }
        public static bool ActiveSelf(this SpriteRenderer spriteRenderer)
        {
            return spriteRenderer.gameObject.activeSelf;
        }

        #endregion

        #region CanvasGroup

        public static void SetActive(this CanvasGroup canvasGroup, bool active, float alpha = 1f)
        {
            canvasGroup.gameObject.SetActive(active);
            canvasGroup.alpha = alpha;
        }
        public static void SetActive(this List<CanvasGroup> canvasGroups, bool active, float alpha = 1f)
        {
            foreach (var canvasGroup in canvasGroups)
            {
                canvasGroup.gameObject.SetActive(active);
                canvasGroup.alpha = alpha;
            }
        }
        public static bool ActiveSelf(this CanvasGroup canvasGroup)
        {
            return canvasGroup.gameObject.activeSelf;
        }

        #endregion

        #region Angle

        public static float GetAngleFromTwoPositions(Transform from, Transform to)
        {
            if (from == null || to == null) return 0f;
            float xDistance = to.position.x - from.position.x;
            float zDistance = to.position.z - from.position.z;
            float angle = (Mathf.Atan2(zDistance, xDistance) * Mathf.Rad2Deg) - 90f;
            return GetNormalizedAngle(angle);
        }

        public static void SetEulerAnglesZAxis(this Transform self, float z)
        {
            Vector3 selfAngles = self.eulerAngles;
            self.rotation = Quaternion.Euler(selfAngles.x, selfAngles.y, z);
        }

        public static void SetRotationFromTwoTransforms(this Transform self, Transform from, Transform to)
        {
            Vector3 direction = from.position - to.position;
            self.rotation = Quaternion.LookRotation(direction);
        }

        public static float GetNormalizedAngle(float angle)
        {
            while (angle < 0f) angle += 360f;
            while (360f < angle) angle -= 360f;
            return angle;
        }

        #endregion

        #region Enums

        public static T GetRandomEnum<T>() where T : Enum
        {
            T[] enumValues = GetEnumList<T>();
            T randomEnumValue = enumValues[Random.Range(0, enumValues.Length)];
            return randomEnumValue;
        }
        public static T GetRandomEnum<T>(int minInclusive) where T : Enum
        {
            T[] enumValues = GetEnumList<T>();
            T randomEnumValue = enumValues[Random.Range(minInclusive, enumValues.Length)];
            return randomEnumValue;
        }
        public static T GetRandomEnum<T>(int minInclusive, int maxExclusive) where T : Enum
        {
            T[] enumValues = GetEnumList<T>();
            T randomEnumValue = enumValues[Random.Range(minInclusive, maxExclusive)];
            return randomEnumValue;
        }
        public static T[] GetEnumList<T>() where T : Enum
        {
            return (T[])Enum.GetValues(typeof(T));
        }
        public static int GetEnumLength<T>() where T : Enum
        {
            return Enum.GetValues(typeof(T)).Length;
        }

        #endregion

        #region Animation

        public static void SetActive(this Animator animator, bool active)
        {
            if (animator.gameObject.activeSelf == active) return;
            animator.gameObject.SetActive(active);
        }
        public static void SetActive(this List<Animator> animators, bool active)
        {
            foreach (var animator in animators)
                animator.gameObject.SetActive(active);
        }
        public static bool ActiveSelf(this Animator animator)
        {
            return animator.gameObject.activeSelf;
        }

        public static void ResetAndSetTrigger(this Animator animator, string name)
        {
            foreach (var param in animator.parameters)
                if (param.type == AnimatorControllerParameterType.Trigger)
                    animator.ResetTrigger(param.name);
            animator.SetTrigger(name);
        }
        public static void ResetAndSetTrigger(this List<Animator> animators, string name)
        {
            foreach (var animator in animators)
            {
                foreach (var param in animator.parameters)
                    if (param.type == AnimatorControllerParameterType.Trigger)
                        animator.ResetTrigger(param.name);
                animator.SetTrigger(name);
            }
        }

        #endregion

        #region STRING

        public static bool IsNullOrEmpty(this string input)
        {
            return string.IsNullOrEmpty(input);
        }

        public static List<string> Split(this string input, string splitter)
        {
            return input.Split(new[] { splitter }, StringSplitOptions.None).ToList();
        }

        public static string CapitalizeFirstCharacter(string input)
        {
            string capitalizedText = char.ToUpper(input[0]) + input[1..].ToLower();
            return capitalizedText;
        }

        public static string ProperNameStandardization(string name)
        {
            TextInfo textInfo = CultureInfo.CurrentCulture.TextInfo;
            return textInfo.ToTitleCase(name.ToLower());
        }

        public static string ConvertPascalCaseEnumToString<T>(T enumValue) where T : Enum
        {
            string enumString = enumValue.ToString();
            string displayString = Regex.Replace(enumString, "([a-z])([A-Z])", "$1 $2");
            return displayString;
        }

        public static string RemoveString(string origin, string removedString)
        {
            if (string.IsNullOrEmpty(origin)) return "";
            return string.IsNullOrEmpty(removedString) ? origin : origin.Replace(removedString, "");
        }

        public static UnityEngine.Vector3 StringToVector3(string str)
        {
            UnityEngine.Vector3 vector = UnityEngine.Vector3.zero;
            return vector;
        }

        public static void FormatParam(this string srcString)
        {
        }

        #endregion

        #region TIME

        private static readonly DateTime UnixTimeBegin = new
            DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public static long GetTimeBetweenDays(this DateTime time, int days = 1)
        {
            return (long)(time.Date.AddDays(days) - time).TotalSeconds;
        }

        public static long ToUnixTimeSeconds(this DateTime time)
        {
            return (long)((time - UnixTimeBegin).TotalSeconds);
        }
        public static long ToUnixTimeMilliseconds(this DateTime time)
        {
            return (long)((time - UnixTimeBegin).TotalMilliseconds);
        }

        public static DateTime ToDateTimeUtc(this long unixTime)
        {
            return UnixTimeBegin.AddSeconds(unixTime);
        }
        public static DateTime ToDateTimeLocal(this long unixTime)
        {
            return UnixTimeBegin.AddSeconds(unixTime).ToLocalTime();
        }

        public static string FormatTimeCounter(this long totalSecond)
        {
            if (totalSecond <= 3600)
            {
                var mm = (totalSecond / 60).ToString("00");
                var ss = (totalSecond % 60).ToString("00");
                return $"{mm}:{ss}";
            }
            else if (totalSecond <= 86400)
            {
                var hh = (totalSecond / 3600).ToString("00");
                var mm = (totalSecond % 3600 / 60).ToString("00");
                var ss = (totalSecond % 60).ToString("00");
                return $"{hh}:{mm}:{ss}";
            }
            else
            {
                var dd = (totalSecond / 86400).ToString("00");
                var hh = (totalSecond % 86400 / 3600).ToString("00");
                var mm = (totalSecond % 3600 / 60).ToString("00");
                var ss = (totalSecond % 60).ToString("00");
                return $"{dd}:{hh}:{mm}:{ss}";
            }
        }
        public static string FormatTimePrettyShort(this long totalSecond)
        {
            if (totalSecond < 60)
            {
                var ss = totalSecond;
                return $"{ss}s";
            }
            if (totalSecond < 3600)
            {
                var mm = totalSecond / 60;
                var ss = totalSecond % 60;
                return $"{mm}m{ss}s";
            }
            if (totalSecond < 86400)
            {
                var hh = totalSecond / 3600;
                var mm = totalSecond % 3600 / 60;
                return $"{hh}h{mm}m";
            }
            {
                var dd = totalSecond / 86400;
                var hh = totalSecond % 86400 / 3600;
                return $"{dd}d{hh}h";
            }
        }
        public static string FormatTimePrettyFull(this long totalSecond)
        {
            if (totalSecond < 60)
            {
                var ss = totalSecond;
                return $"{ss}s";
            }
            if (totalSecond < 3600)
            {
                var mm = totalSecond / 60;
                var ss = totalSecond % 60;
                return $"{mm}m{ss}s";
            }
            if (totalSecond < 86400)
            {
                var hh = totalSecond / 3600;
                var mm = totalSecond % 3600 / 60;
                var ss = totalSecond % 60;
                return $"{hh}h{mm}m{ss}s";
            }
            {
                var dd = totalSecond / 86400;
                var hh = totalSecond % 86400 / 3600;
                var mm = totalSecond % 3600 / 60;
                var ss = totalSecond % 60;
                return $"{dd}d{hh}h{mm}m{ss}s";
            }
        }

        #endregion

        #region Tweener

        public static void CompleteAndKill(this Tweener tweener)
        {
            if (tweener != null && !tweener.IsComplete())
            {
                tweener.Complete();
                tweener.Kill();
            }
        }
        public static void CompleteAndKill(this List<Tweener> tweener)
        {
            foreach (var ti in tweener)
            {
                if (ti != null && !ti.IsComplete())
                {
                    ti.Complete();
                    ti.Kill();
                }
            }
            tweener.Clear();
        }

        public static void KillWithOutComplete(this Tweener tweener)
        {
            tweener?.Kill();
        }
        public static void KillWithOutComplete(this List<Tweener> tweener)
        {
            foreach (var ti in tweener)
                ti?.Kill();
        }

        #endregion

        #region Extention

        public static string GetCurrencyFormat<T>(T value)
        {
            return double.Parse(value.ToString()).ToString("#,###", CultureInfo.GetCultureInfo("vi-VN").NumberFormat);
        }

        #endregion

        private static long MathPowLong(int x, int p) { return (long)Math.Pow(x, p); }
        public static string FormatCurrency(this long value)
        {
            if (value < MathPowLong(10, 4))
            {
                return $"{value}"; // 9999
            }
            if (value < MathPowLong(10, 5))
            {
                var v1 = value / MathPowLong(10, 3);
                var v2 = (value % MathPowLong(10, 3)) / MathPowLong(10, 2);
                return $"{v1},{v2}K"; // 99,9K
            }
            if (value < MathPowLong(10, 6))
            {
                return $"{value / MathPowLong(10, 3)}K"; // 999K
            }
            if (value < MathPowLong(10, 7))
            {
                var v1 = value / MathPowLong(10, 6);
                var v2 = (value % MathPowLong(10, 6)) / MathPowLong(10, 4);
                return $"{v1},{v2}M"; // 9,99M
            }
            if (value < MathPowLong(10, 8))
            {
                var v1 = value / MathPowLong(10, 6);
                var v2 = (value % MathPowLong(10, 6)) / MathPowLong(10, 5);
                return $"{v1},{v2}M"; // 99,9M
            }
            if (value < MathPowLong(10, 9))
            {
                return $"{value / MathPowLong(10, 6)}M"; // 999M
            }
            if (value < MathPowLong(10, 10))
            {
                var v1 = value / MathPowLong(10, 9);
                var v2 = (value % MathPowLong(10, 9)) / MathPowLong(10, 7);
                return $"{v1},{v2}B"; // 9,99B
            }
            if (value < MathPowLong(10, 11))
            {
                var v1 = value / MathPowLong(10, 9);
                var v2 = (value % MathPowLong(10, 9)) / MathPowLong(10, 8);
                return $"{v1},{v2}B"; // 99,9B
            }
            if (value < MathPowLong(10, 12))
            {
                return $"{value / MathPowLong(10, 9)}M"; // 999B
            }
            if (value < MathPowLong(10, 13))
            {
                var v1 = value / MathPowLong(10, 12);
                var v2 = (value % MathPowLong(10, 12)) / MathPowLong(10, 10);
                return $"{v1},{v2}T"; // 9,99T
            }
            if (value < MathPowLong(10, 14))
            {
                var v1 = value / MathPowLong(10, 12);
                var v2 = (value % MathPowLong(10, 12)) / MathPowLong(10, 11);
                return $"{v1},{v2}T"; // 99,9T
            }
            return $"{value / MathPowLong(10, 12)}T"; // 999T
        }
        public static string FormatRewardValue(this long value)
        {
            if (value < 100)
                return $"x{value}";
            return $"{value}";
        }
        public static string FormatRewardValueDrop(this long value)
        {
            return $"+{value}";
        }
        public static string FormatRewardHeartTime(this long value)
        {
            if (value < 60)
            {
                var ss = value;
                return $"{ss}s";
            }
            else if (value < 3600)
            {
                var mm = value / 60;
                if (value % 60 == 0)
                    return $"{mm}m";
                var ss = value % 60;
                return $"{mm}m{ss}s";
            }
            else if (value < 86400)
            {
                var hh = value / 3600;
                if (value % 3600 == 0)
                    return $"{hh}h";
                var mm = value % 3600 / 60;
                return $"{hh}h{mm}m";
            }
            else
            {
                var dd = value / 86400;
                if (value % 86400 == 0)
                    return $"{dd}d";
                var hh = value % 86400 / 3600;
                return $"{dd}d{hh}h";
            }
        }
    }
}