using UnityEngine;
using UnityEngine.UI;

namespace ChayDi
{
    public class nhanvat : MonoBehaviour
    {
        public Rigidbody2D rb;
        public Image imageLose;

        private bool isGrounded = true;

        void Start()
        {
            imageLose.gameObject.SetActive(false);
            Time.timeScale = 1f;
        }

        void Update()
        {
            if (Time.timeScale == 0) return;

            if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
            {
                rb.velocity = new Vector2(rb.velocity.x, 12f);
                isGrounded = false;
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("nen"))
            {
                isGrounded = true;
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("vatcan"))
            {
                GameOver();
            }
        }

        void GameOver()
        {
            imageLose.gameObject.SetActive(true);
            Time.timeScale = 0f;
        }

    }
}
namespace GameTemp
{
    public class nhanvat : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space)) ;
            Debug.Log("nhay nao");
        }
    }

}