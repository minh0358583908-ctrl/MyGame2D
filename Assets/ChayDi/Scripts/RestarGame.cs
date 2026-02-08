using UnityEngine;
using UnityEngine.SceneManagement;

namespace ChayDi
{
    public class RestartGame : MonoBehaviour
    {
        public void Restart()
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
