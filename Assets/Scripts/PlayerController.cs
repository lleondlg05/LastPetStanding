using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //Walk parameters 
    private CharacterController controller;
    [SerializeField] private float speed;

    //Jump parameters
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float jumpForce = 2f;
    [SerializeField] private LayerMask groundMask;
    private bool isGrounded;
    private float groundDistance = 0.2f;
    private Vector3 velocity;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
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

        Vector3 moveDirection = hInput * Vector3.right + vInput * Vector3.forward;

        controller.Move(moveDirection * Time.deltaTime * speed);
    }

    void JumpController()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (Input.GetButton("Jump") && isGrounded)
            velocity.y = Mathf.Sqrt(jumpForce * -2 * gravity);

        if(isGrounded && velocity.y < 0)
            velocity.y = -2;

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
    }
}
