using System.Collections.Generic;
using BaseGame;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Runner
{
    public class PopupScoreHistoryCtrl : PopupBaseCtrl
    {
        public List<TextMeshProUGUI> listTextScore;
        public RectTransform listTextScoreLayout;

        public override void Open()
        {
            var scoreHistory = GameManager.GameData.scoreHistory;
            for (var i = 0; i < scoreHistory.Count; i++)
            {
                var textScore = i < listTextScore.Count ? listTextScore[i]
                    : Instantiate(listTextScore[0], listTextScoreLayout);
                textScore.SetText($"{scoreHistory[i].score} - {scoreHistory[i].time.ToDateTimeLocal().ToString("MM/dd/yyyy")}");
            }
            for (var i = scoreHistory.Count; i < listTextScore.Count; i++)
                listTextScore[i].SetActive(false);
            LayoutRebuilder.ForceRebuildLayoutImmediate(listTextScoreLayout);
            base.Open();
        }
    }
}