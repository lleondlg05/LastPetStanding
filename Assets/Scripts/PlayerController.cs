using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //Walk parameters
    private CharacterController controller;
    [SerializeField] private float speed;
    [SerializeField] private float speedRotation;
    [SerializeField] private Transform player;

    //Jump parameters
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float jumpForce = 2f;
    [SerializeField] private LayerMask groundMask;
    private bool isGrounded;
    private float groundDistance = 0.2f;
    private Vector3 velocity;

    //Camera Parameters
    public float xSensivity = 100;
    public float ySensivity = 100;
    public GameObject followTransform;
    private Vector3 angles;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        CameraController();
        WalkController();
        JumpController();
    }

    void WalkController()
    {
        float hInput = Input.GetAxis("Horizontal");
        float vInput = Input.GetAxis("Vertical");

        Vector3 moveDirection = hInput * followTransform.transform.right + vInput * followTransform.transform.forward;

        controller.Move(moveDirection * Time.deltaTime * speed);

        //relocate player
        if(moveDirection != Vector3.zero)
            player.transform.rotation = Quaternion.Euler(0, followTransform.transform.rotation.eulerAngles.y, 0);
    }

    void JumpController()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (Input.GetButtonDown("Jump") && isGrounded)
            velocity.y = Mathf.Sqrt(jumpForce * -2 * gravity);

        if(isGrounded && velocity.y < 0)
            velocity.y = -2;

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
    }

    void CameraController()
    {
        float x = Input.GetAxis("Mouse X") * xSensivity * Time.deltaTime;

        followTransform.transform.rotation *= Quaternion.AngleAxis(x, Vector2.up);

        float y = Input.GetAxis("Mouse Y") * ySensivity * Time.deltaTime;

        followTransform.transform.rotation *= Quaternion.AngleAxis(-y, Vector2.right);

        angles = followTransform.transform.localEulerAngles;
        angles.z = 0;

        var angle = followTransform.transform.localEulerAngles.x;

        //Clamp the up/down rotation

        if (angle > 180 && angle < 340)
            angles.x = 340;
        else if (angle < 180 && angle > 40)
            angles.x = 40;

        followTransform.transform.localEulerAngles = angles;
    }
}
