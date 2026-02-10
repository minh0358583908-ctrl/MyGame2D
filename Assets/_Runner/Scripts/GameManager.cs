using System;
using System.Collections;
using System.Collections.Generic;
using BaseGame;
using Runner.BaseGame;
using UnityEngine;

namespace Runner
{
    [Serializable]
    public class ScoreHistory
    {
        public long score;
        public long time;
    }
    [Serializable]
    public class GameData
    {
        public List<ScoreHistory> scoreHistory;

        public void LoadSavedData()
        {
            var savedData = SaveFile.Load<GameData>();
            if (savedData != null)
            {
                scoreHistory = savedData.scoreHistory;
            }
            else
            {
                scoreHistory = new List<ScoreHistory>();
                SaveFile.Save<GameData>(this);
            }
        }

        public void SaveNewScore(long score)
        {
            var newScore = new ScoreHistory()
            {
                score = score,
                time = DateTime.UtcNow.ToUnixTimeSeconds()
            };

            // sắp xếp theo điểm cao đến thấp
            for (var i = 0; i < scoreHistory.Count; i++)
            {
                if (newScore.score > scoreHistory[i].score)
                {
                    scoreHistory.Insert(i, newScore);
                    return;
                }
            }
            scoreHistory.Add(newScore);
        }
    }

    public class GameManager : SingletonManager<GameManager>
    {
        public static GameData GameData;

        private void Start()
        {
            LoadGameData();
            GameUIController.Init();
            GameplayController.Init();
            DeviceManager.Init();
        }

        private void LoadGameData()
        {
            SaveFile.SetBasePath();
            GameData ??= new GameData();
            GameData.LoadSavedData();
        }
    }
}