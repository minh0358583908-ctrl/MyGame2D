using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ChayDi
{
    public class matdat : MonoBehaviour
    {

        public int diemSo = 0;
        public Text textdieemso;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        private void FixedUpdate()
        {
            transform.Translate(Vector2.left * 4 * Time.fixedDeltaTime);
            if (transform.position.x < -28)
            {
                transform.position = new Vector2(0, transform.position.y);
            }
            diemSo = diemSo + 1;
            textdieemso.text = "Diem so: " + diemSo;
        }
    }

}