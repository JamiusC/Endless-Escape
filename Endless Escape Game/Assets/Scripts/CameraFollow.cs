using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(0f, 6f, -8f);
    public float smoothTime = 0.15f;

    private Vector3 velocity = Vector3.zero;

    void LateUpdate()
    {
        if (target == null)
            return;

        Vector3 desiredPosition = new Vector3(
            target.position.x + offset.x,
            target.position.y + offset.y,
            target.position.z + offset.z
        );

        transform.position = Vector3.SmoothDamp(
            transform.position,
            desiredPosition,
            ref velocity,
            smoothTime
        );
    }
}