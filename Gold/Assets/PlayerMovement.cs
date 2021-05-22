using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    [SerializeField]
    CharacterController controller;

    [SerializeField]
    Transform groundCheck;

    [SerializeField]
    float speed = 12f;

    [SerializeField]
    float gravity = -9.81f;

    [SerializeField]
    float groundDistance = 0.4f;

    [SerializeField]
    LayerMask groundMask;

    Vector3 velocity;
    bool isGrounded;
    float jumpTime = 32f / 30f;

    float jumpVel;

    private void Start() {
        jumpVel = (jumpTime * -gravity) / 2f;
    }

    // Update is called once per frame
    void Update() {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0) {
            velocity.y = -2f;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        controller.Move(move * speed * Time.deltaTime);

        if (Input.GetButtonDown("Jump") && isGrounded) {
            velocity.y = jumpVel;
        }

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
    }
}
