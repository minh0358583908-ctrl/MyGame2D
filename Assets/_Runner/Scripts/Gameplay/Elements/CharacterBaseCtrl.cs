using System;
using UnityEngine;

namespace Runner
{
    public class CharacterBaseCtrl : ElementBaseCtrl
    {
        public bool onJump = false;

        public override void Respawn()
        {
            onJump = false;
            base.Respawn();
        }

        public void TriggerJump()
        {
            onJump = true;
            rgb.AddForce(Vector3.up * jumpForce, ForceMode2D.Impulse);
        }
        private void ResetJumpStatus()
        {
            onJump = false;
        }
        
        // private void 

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Ground"))
                ResetJumpStatus();
            else if (other.CompareTag("Obstacle"))
            {
                GameplayController.HandleCharacterCollide();
            }
        }
    }
}