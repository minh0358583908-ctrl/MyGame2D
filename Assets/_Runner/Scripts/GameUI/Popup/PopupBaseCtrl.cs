using System;
using System.Threading.Tasks;
using BaseGame;
using Runner.BaseGame;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Runner
{
    [Serializable]
    public enum PopupAction
    {
        None, OpenImmediate, CloseImmediate, OpenAlpha, CloseAlpha,
        OpenScale, OpenMoveUp, OpenMoveDown, OpenMoveLeft, OpenMoveRight,
        CloseScale, CloseMoveUp, CloseMoveDown, CloseMoveLeft, CloseMoveRight,
    }

    public class PopupBaseCtrl : MonoBehaviour
    {
        [FoldoutGroup("BASE")] public Animator anim;
        [FoldoutGroup("BASE")] public bool playSound = false;
        [FoldoutGroup("BASE")] public bool closeDestroy = true;
        [FoldoutGroup("BASE")] private bool _onAction = false;

        public virtual bool OnAction() { return _onAction; }

        public virtual void Open() { Open(PopupAction.OpenScale, null); }
        public virtual void Open(PopupAction action) { Open(action, null); }
        public virtual void Open(Action callback) { Open(PopupAction.OpenScale, callback); }
        public virtual async void Open(PopupAction action, Action callback)
        {
            if (_onAction) return;
            if (playSound) DeviceManager.Audio.PlaySound(AudioID.PopupOpen);
            anim.SetActive(true);
            if (action != PopupAction.OpenImmediate)
            {
                _onAction = true;
                anim.SetTrigger(action.ToString());
                await Task.Delay(333);
            }
            _onAction = false;
            anim.SetTrigger(PopupAction.OpenImmediate.ToString());
            callback?.Invoke();
        }

        public virtual void Close() { Close(PopupAction.CloseScale, null); }
        public virtual void Close(PopupAction action) { Close(action, null); }
        public virtual void Close(Action callback) { Close(PopupAction.CloseScale, callback); }
        public virtual void CloseScale() { Close(PopupAction.CloseScale); }
        public virtual void CloseAlpha() { Close(PopupAction.CloseAlpha); }
        public virtual void CloseMoveUp() { Close(PopupAction.CloseMoveUp); }
        public virtual void CloseMoveDown() { Close(PopupAction.CloseMoveDown); }
        public virtual void CloseMoveLeft() { Close(PopupAction.CloseMoveLeft); }
        public virtual void CloseMoveRight() { Close(PopupAction.CloseMoveRight); }
        public virtual async void Close(PopupAction action, Action callback)
        {
            if (_onAction) return;
            if (playSound) DeviceManager.Audio.PlaySound(AudioID.PopupClose);
            if (action != PopupAction.CloseImmediate)
            {
                _onAction = true;
                anim.SetTrigger(action.ToString());
                await Task.Delay(333);
            }
            _onAction = false;
            anim.SetTrigger(PopupAction.CloseImmediate.ToString());
            callback?.Invoke();
            if (closeDestroy) GameObject.Destroy(anim.gameObject);
            else anim.SetActive(false);
        }
    }

    public class PopupSingletonCtrl<T> : PopupBaseCtrl where T : PopupBaseCtrl
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
}