using System;
using System.Collections.Generic;
using UnityEngine;

namespace BaseGame
{
    public class SingletonX<T> : MonoBehaviour where T : MonoBehaviour
    {
        public static T Ins => _instance;
        private static T _instance = null;

        protected virtual void Awake()
        {
            if (_instance == null) _instance = this as T;
            else Destroy(gameObject);
        }
        protected virtual void OnDestroy()
        {
            _instance = null;
        }
    }

    public class SingletonManager<T> : MonoBehaviour where T : MonoBehaviour
    {
        public static T Ins => instance;
        protected static T instance = null;

        protected virtual void Awake()
        {
            if (instance == null) { instance = this as T; }
            else Destroy(gameObject);
        }
        protected virtual void OnDestroy()
        {
            instance = null;
        }

        protected static int callbackDataIndex = 0;
        protected static Dictionary<int, System.Action> callbackData = new();
        public static void AddCallbackData(System.Action callback)
        {
            callback?.Invoke();
            callbackData ??= new();
            callbackData.Add(callbackDataIndex++, callback);
        }
        public static void InvokeCallbackData()
        {
            var listCallbackInactive = new List<int>();
            foreach (var callback in callbackData)
            {
                if (callback.Value != null)
                {
                    try { callback.Value?.Invoke(); }
                    catch (Exception e)
                    {
                        Debug.Log(e.Message);
                        listCallbackInactive.Add(callback.Key);
                    }
                }
                else listCallbackInactive.Add(callback.Key);
            }
            foreach (var index in listCallbackInactive)
                callbackData.Remove(index);
        }
    }
}