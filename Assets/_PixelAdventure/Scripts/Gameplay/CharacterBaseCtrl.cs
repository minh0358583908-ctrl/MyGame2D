using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PixelAdventure
{

    public enum CharacterState
    {
        Grounded, // đang ở mặt đất
        Jump1, // sau khi bấm nhảy lần 1 => cho phép bấm nhảy thêm
        Jump2, // sau khi bấm nhẩy lần 2 => ko thể bấm nhảy thêm => rơi tự do theo gia tốc trọng trường
    }


    public class CharacterBaseCtrl : MonoBehaviour
    {
        public float moveSpeed = 5f;
        public float jumpForce = 15f;

        private Rigidbody2D rb;
        private CharacterState state; // trạng thái hiện tại của nhân vật là gì

        void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            // khởi điểm trạng thái của nhân vật là rơi tự do
            state = CharacterState.Jump2;
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.A)) HandleMoveLeft();
            if (Input.GetKeyDown(KeyCode.D)) HandleMoveRight();
            if (Input.GetKeyDown(KeyCode.Space)) HandleJump();
            //rb.velocity = new Vector2(move * moveSpeed, rb.velocity.y);
        }

        // mới ban đầu học thì học theo kiểu ý hiểu di chuyển theo thực tế
        private void HandleMoveLeft()
        {
            // biết di chuyển theo hướng nào trc
            // sau đó tính quãng đường di chuyển bằng vận tốc nhân thời gian
            var moveSpace = moveSpeed * Time.deltaTime; // Time.deltaTime là thời gian gọi cách nhau của mỗi lần hàm Update bên trên kia được gọi
            transform.position = new Vector3(transform.position.x - moveSpace, transform.position.y, transform.position.z);
            // di chuyển sang trái nên sẽ chỉ thay đổi vị trí theo trục y
        }


        private void HandleMoveRight()
        {
            var moveSpace = moveSpeed * Time.deltaTime;
            transform.position = new Vector3(transform.position.x + moveSpace, transform.position.y, transform.position.z);
        }

        private void HandleJump()
        {
            if (state == CharacterState.Grounded)
            {
                state = CharacterState.Jump1;
                rb.AddForce(Vector3.up * jumpForce);
            }
            else if (state == CharacterState.Jump1)
            {
                state = CharacterState.Jump2;
                rb.AddForce(Vector3.up * jumpForce);
            }
            else
            {
                // ko làm gì cả
            }
        }

        // khi nhân vật chạm đất thì reset trạng thái
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Ground"))
            {
                state = CharacterState.Grounded;
            }
        }

        // exit là khi nhân vật đã rời khỏi mặt đất rồi, thì ko check reset trạng thái ở đây
        //private void OnCollisionExit2D(Collision2D collision)
        //{
        //    if (collision.gameObject.CompareTag("Ground"))
        //    {
        //        isGrounded = false;
        //    }
        //}
    }
}