using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;

    public Vector3 offset = new Vector3(0, 5, -8);

    public float smooth = 5f;

    void LateUpdate()
    {
        Vector3 desiredPos = target.position + target.TransformDirection(offset);
        transform.position = Vector3.Lerp(transform.position, desiredPos, Time.deltaTime * smooth);

        transform.LookAt(target);
    }
}
