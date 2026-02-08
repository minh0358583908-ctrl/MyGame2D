using UnityEngine;
using TMPro;

namespace ChayDi
{
    public class ScoreManager : MonoBehaviour
    {
        public TextMeshProUGUI scoreText;
        float score;

        void Update()
        {
            if (Time.timeScale == 0) return;

            score += 10 * Time.deltaTime;
            scoreText.text = "Điểm Số: " + Mathf.FloorToInt(score);
        }
    }
}