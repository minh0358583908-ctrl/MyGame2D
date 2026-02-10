using System;
using UnityEngine;
using UnityEngine.UI;

namespace Runner
{
    [Serializable]
    public class PopupPauseGameUISetting
    {
        public Button btnMusic;
        public Button btnSound;
        public Button btnHaptic;
    }

    public class PopupPauseGameCtrl : PopupSingletonCtrl<PopupPauseGameCtrl>
    {
        public PopupPauseGameUISetting uiSetting;

        public override void Open()
        {
            Time.timeScale = 0f;
            base.Open();
        }

        public override void Close()
        {
            base.Close(
                () => { Time.timeScale = 1f; });
        }
    }
}