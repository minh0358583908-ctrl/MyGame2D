using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform MainCharacter;

    void LateUpdate()
    {
        if (MainCharacter == null) return;

        transform.position = new Vector3(
            MainCharacter.position.x,
            MainCharacter.position.y,
            -10f
        );
    }
}
