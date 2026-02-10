using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PixelAdventure
{
    public enum CharacterJumpState
    {
        Grounded, // đang ở mặt đất
        Jump1, // sau khi bấm nhảy lần 1 => cho phép bấm nhảy thêm
        Jump2, // sau khi bấm nhẩy lần 2 => ko thể bấm nhảy thêm => rơi tự do theo gia tốc trọng trường
    }

    public enum CharacterMoveState
    {
        None, Left, Right, LeftToRight, RightToLeft // trạng thái di chuyển sang 2 bên
    }

    public class CharacterBaseCtrl : MonoBehaviour
    {
        public Animator charAnim;
        public float moveSpeed = 5f;
        public float jumpForce = 15f;
        public Rigidbody2D rgb;
        public CharacterJumpState jumpState = CharacterJumpState.Jump2; // trạng thái hiện tại của nhân vật là gì
        public CharacterMoveState moveState = CharacterMoveState.None;

        void Awake()
        {
            rgb ??= GetComponent<Rigidbody2D>();
            // khởi điểm trạng thái của nhân vật là rơi tự do
            jumpState = CharacterJumpState.Jump2;
            moveState = CharacterMoveState.None;
            // charAnim.ResetAndSetTrigger("Fall");
        }

        void Update()
        {
            CheckUpdateMove();
            CheckUpdateJump();
        }

        // mới ban đầu học thì học theo kiểu ý hiểu di chuyển theo thực tế
        private void CheckUpdateMove()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                // nhấn xuống (bao gồm cả giữ) => di chuyển sang trái
                if (moveState == CharacterMoveState.Right)
                    moveState = CharacterMoveState.RightToLeft;
                else moveState = CharacterMoveState.Left;
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                // nhấn xuống (bao gồm cả giữ) => di chuyển sang phải
                if (moveState == CharacterMoveState.Left)
                    moveState = CharacterMoveState.LeftToRight;
                else moveState = CharacterMoveState.Right;
            }
            else if (Input.GetKeyUp(KeyCode.A))
            {
                // thả tay ra => ko di chuyển
                if (moveState == CharacterMoveState.RightToLeft ||
                    moveState == CharacterMoveState.LeftToRight)
                    moveState = CharacterMoveState.Right;
                else moveState = CharacterMoveState.None;
            }
            else if (Input.GetKeyUp(KeyCode.D))
            {
                // thả tay ra => ko di chuyển
                if (moveState == CharacterMoveState.RightToLeft ||
                    moveState == CharacterMoveState.LeftToRight)
                    moveState = CharacterMoveState.Left;
                else moveState = CharacterMoveState.None;
            }

            if (moveState == CharacterMoveState.None)
            {
                rgb.velocity = new Vector2(
                    0, // ko cho phép di chuyển thêm ngay khi thả tay
                    rgb.velocity.y
                );
                
                // // thay đổi hình ảnh nhân vật
                // if (jumpState == CharacterJumpState.Grounded)
                //     charAnim.ResetAndSetTrigger("Idle");
                return;
            }

            rgb.velocity = new Vector2(
                moveState is CharacterMoveState.Left or CharacterMoveState.RightToLeft ? -moveSpeed : moveSpeed, // chỉ điều khiển X
                rgb.velocity.y // giữ nguyên lực rơi / nhảy
            );
            // if (jumpState == CharacterJumpState.Grounded)
            //     charAnim.ResetAndSetTrigger("Idle");

            // // tính quãng đường di chuyển bằng vận tốc nhân thời gian
            // var moveSpace = moveSpeed * Time.deltaTime; // Time.deltaTime là thời gian gọi cách nhau của mỗi lần hàm Update bên trên kia được gọi

            // if (moveState is CharacterMoveState.Left or CharacterMoveState.RightToLeft)
            // {
            //     moveSpace = -moveSpace;
            // }
            // if (moveState == CharacterMoveState.Right)
            // {
            //     moveSpace = moveSpace; // giữ nguyên
            // }

            // di chuyển sang trái nên sẽ chỉ thay đổi vị trí theo trục x
            // transform.position = new Vector3(transform.position.x + moveSpace, transform.position.y, transform.position.z);
        }

        private void CheckUpdateJump()
        {
            var hasTriggerJump = Input.GetKeyDown(KeyCode.W);
            if (!hasTriggerJump) return;

            Debug.Log("HandleTriggerJump");
            if (jumpState == CharacterJumpState.Grounded)
            {
                Debug.Log("HandleTriggerJump => Jump1");
                jumpState = CharacterJumpState.Jump1;
                rgb.AddForce(Vector3.up * jumpForce, ForceMode2D.Impulse);
            }
            else if (jumpState == CharacterJumpState.Jump1)
            {
                Debug.Log("HandleTriggerJump => Jump2");
                jumpState = CharacterJumpState.Jump2;

                // tại jump 1 ko cho phép cộng dồn lực nhảy => nhảy cao tối đa bằng 2 lần jump 1
                rgb.velocity = new Vector2(rgb.velocity.x, 0); // commend dòng này và test thử

                rgb.AddForce(Vector3.up * jumpForce, ForceMode2D.Impulse);
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
                jumpState = CharacterJumpState.Grounded;
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

    public static class Helper
    {
        public static void ResetAndSetTrigger(this Animator animator, string state)
        {
            foreach (var param in animator.parameters)
                if (param.type == AnimatorControllerParameterType.Trigger)
                    animator.ResetTrigger(param.name);
            animator.SetTrigger(state);
        }
    }
}