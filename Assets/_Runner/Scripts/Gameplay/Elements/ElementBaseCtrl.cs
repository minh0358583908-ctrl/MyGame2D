using System;
using UnityEngine;

namespace Runner
{
    public class ElementBaseCtrl : MonoBehaviour
    {
        [Header("ASSET")]
        public Animator anim;
        public Rigidbody2D rgb;
        [Header("CONFIG")]
        public int index;
        public float moveSpeed;
        public float jumpForce;
        [Header("RUNTIME")]
        public bool isActive;

        public virtual void Respawn()
        {
            isActive = true;
        }
        public virtual void Despawn()
        {
            isActive = false;
        }

        protected virtual void Update()
        {
        }
    }
}