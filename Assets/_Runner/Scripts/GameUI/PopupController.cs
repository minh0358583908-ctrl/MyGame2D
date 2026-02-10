using System.Collections.Generic;
using BaseGame;
using UnityEngine;

namespace Runner
{
    public class PopupController : SingletonX<PopupController>
    {
        public Transform popupLocation;
        public Transform popupSystemLocation;
        public List<GameObject> listPopupPrefab;

        public static T CreateInstance<T>() where T : new()
        {
            foreach (var prefab in Ins.listPopupPrefab)
            {
                if (prefab.GetComponent<T>() != null)
                {
                    var popup = Instantiate(prefab, Ins.popupLocation);
                    return popup.GetComponent<T>();
                }
            }
            Debug.LogError($"MISSING PREFAB: {typeof(T)}");
            return new T();
        }
        public static T CreateInstance<T>(Transform parent) where T : new()
        {
            foreach (var prefab in Ins.listPopupPrefab)
            {
                if (prefab.GetComponent<T>() != null)
                {
                    var popup = Instantiate(prefab, parent);
                    return popup.GetComponent<T>();
                }
            }
            Debug.LogError($"MISSING PREFAB: {typeof(T)}");
            return new T();
        }
        public static bool HasPopupActive()
        {
            return Ins.popupLocation.childCount > 0 || Ins.popupSystemLocation.childCount > 0;
        }
        public static void CloseInstance<T>()
        {
            for (var i = 0; i < Ins.popupLocation.childCount; i++)
            {
                var popup = Ins.popupLocation.GetChild(i);
                if (popup.GetComponent<T>() != null)
                    popup.GetComponent<PopupBaseCtrl>().Close();
            }
        }
        public static void CloseAll()
        {
            for (var i = 0; i < Ins.popupLocation.childCount; i++)
            {
                var popup = Ins.popupLocation.GetChild(i);
                popup.GetComponent<PopupBaseCtrl>().Close();
            }
        }
    }
}