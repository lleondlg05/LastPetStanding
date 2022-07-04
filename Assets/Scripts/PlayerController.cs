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
    [SerializeField] private float turnSmoothTime = 0.1f;
    private float turnSmoothVelocity;
    [SerializeField] private Transform cam;

    //Jump parameters
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float jumpForce = 2f;
    [SerializeField] private LayerMask groundMask;
    private bool isGrounded;
    private float groundDistance = 0.2f;
    private Vector3 velocity;

    //Camera Parameters
    [SerializeField] private GameObject followTransform;
    public float xSensivity = 100;
    public float ySensivity = 100;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        WalkController();
        JumpController();
    }

    void WalkController()
    {
        float hInput = Input.GetAxis("Horizontal");
        float vInput = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(hInput, 0, vInput).normalized;

        if(direction.magnitude > 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0, angle, 0);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir.normalized * speed * Time.deltaTime);
        }
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

}
