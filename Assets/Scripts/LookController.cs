using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookController : MonoBehaviour
{
    public Transform player;
    public float sensivity = 100f;

    public float verticalRotation;

    void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        float x = Input.GetAxis("Mouse X") * sensivity * Time.deltaTime;
        float y = Input.GetAxis("Mouse Y") * sensivity * Time.deltaTime;

        verticalRotation -= y;
        verticalRotation = Mathf.Clamp(verticalRotation, -90, 90);
        transform.localRotation = Quaternion.Euler(verticalRotation, 0, 0);

        player.Rotate(Vector3.up * x);
    }
}
