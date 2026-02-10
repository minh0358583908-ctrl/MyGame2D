using System;
using BaseGame;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Runner
{
    [Serializable]
    public class GameUILoading
    {
        public CanvasGroup uiObj;

        public void Init()
        {
            uiObj.SetActive(true, 1f);
        }

        public void Show()
        {
            uiObj.SetActive(true);
            uiObj.DOFade(1f, 0.25f);
        }

        public void Hide()
        {
            uiObj.DOFade(0f, 0.25f).OnComplete(
                () => { uiObj.SetActive(false); });
        }
    }

    [Serializable]
    public class GameUIHome
    {
        public CanvasGroup uiObj;
        public Button btnStart;
        public Button btnScoreHistory;

        public void Init()
        {
            uiObj.SetActive(false, 0f);
            btnStart.interactable = false;
            btnStart.AddOnClickListener(StartGame);
            btnScoreHistory.interactable = false;
            btnScoreHistory.AddOnClickListener(ShowScoreHistory);
        }

        public void Show()
        {
            uiObj.SetActive(true);
            uiObj.DOFade(1f, 0.25f).OnComplete(
                () =>
                {
                    btnStart.interactable = true;
                    btnScoreHistory.interactable = true;
                });
        }

        public void Hide()
        {
            uiObj.DOFade(0f, 0.25f).OnComplete(
                () => { uiObj.SetActive(false); });
            btnStart.interactable = false;
            btnScoreHistory.interactable = false;
        }

        public void StartGame()
        {
            GameplayController.StartGame();
        }

        public void ShowScoreHistory()
        {
            // PopupController.CreateInstance<>()
        }
    }

    [Serializable]
    public class GameUIGameplay
    {
        public CanvasGroup uiObj;
        public TextMeshProUGUI txtScore;
        public Button btnPause;

        public void Init()
        {
            uiObj.SetActive(false, 0f);
            btnPause.interactable = false;
            btnPause.AddOnClickListener(PauseGame);
        }

        public void Show()
        {
            uiObj.SetActive(true);
            uiObj.DOFade(1f, 0.25f).OnComplete(
                () =>
                {
                    btnPause.interactable = true;
                });
        }

        public void Hide()
        {
            uiObj.DOFade(0f, 0.25f).OnComplete(
                () => { uiObj.SetActive(false); });
            btnPause.interactable = false;
        }

        public void PauseGame()
        {
            PopupController.CreateInstance<PopupPauseGameCtrl>().Open();
        }
    }

    public class GameUIController : SingletonX<GameUIController>
    {
        public GameUILoading uiLoading;
        public GameUIHome uiHome;
        public GameUIGameplay uiGameplay;

        public static void Init()
        {
            Ins.uiLoading.Init();
            Ins.uiHome.Init();
            Ins.uiGameplay.Init();
        }

        public void ShowUILoading()
        {
            uiLoading.Show();
            uiHome.Hide();
            uiGameplay.Hide();
        }
        public void ShowUIHome()
        {
            uiLoading.Hide();
            uiHome.Show();
            uiGameplay.Hide();
        }
        public void ShowUIGameplay()
        {
            uiLoading.Hide();
            uiHome.Hide();
            uiGameplay.Show();
        }
    }
}