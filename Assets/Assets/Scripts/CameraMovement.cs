using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Vector2 verticalMinMax;

    public float sensitivityX;
    public float sensitivityY;
    public float zRot;

    float pitch;
    float yaw;
    float smoothedZRot;

    void LateUpdate()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        yaw += Input.GetAxis("Mouse X") * sensitivityX * Time.deltaTime;
        pitch -= Input.GetAxis("Mouse Y") * sensitivityY * Time.deltaTime;

        pitch = Mathf.Clamp(pitch, verticalMinMax.x, verticalMinMax.y);

        smoothedZRot = Mathf.Lerp(smoothedZRot, zRot, 10 * Time.deltaTime);
        Vector3 targetRot = new Vector3(pitch, yaw, smoothedZRot);
        transform.eulerAngles = targetRot;
    }
}
