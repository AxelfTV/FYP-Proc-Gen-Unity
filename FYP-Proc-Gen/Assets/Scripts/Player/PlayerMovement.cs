using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;

    public float speed = 12f;
    public float gravity = -9.81f;
    public float jumpHeight = 3f;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    Vector3 velocity;
    bool isGrounded;
    bool jumpInput;
    private void Start()
    {
        transform.position = new Vector3(MapManager.xSize/2, 25f, MapManager.xSize / 2);
    }
    private void Update()
    {
        if(Input.GetButtonDown("Jump")) jumpInput = true;
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0) { velocity.y = -2f; }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        controller.Move(move * speed * Time.fixedDeltaTime);

        if(jumpInput && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
            
        }

        velocity.y += gravity * Time.fixedDeltaTime;

        controller.Move(velocity * Time.fixedDeltaTime);
        jumpInput = false;
    }
}
