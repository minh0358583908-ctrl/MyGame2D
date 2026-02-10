using UnityEngine;

namespace Runner
{
    public class ObstacleBaseCtrl : ElementBaseCtrl
    {
        public bool UpdateMove(float checkPosX)
        {
            var oldPos = transform.position;
            var moveSpace = moveSpeed * Time.deltaTime;
            transform.position = new Vector3(oldPos.x - moveSpace, oldPos.y, oldPos.z);
            return transform.position.x < checkPosX;
        }
    }
}