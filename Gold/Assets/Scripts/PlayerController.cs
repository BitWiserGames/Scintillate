using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    [SerializeField]
    Animator animator;

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

    int coins = 0;

    public void addCoin() {
        ++coins;
    }

    public void removeCoin() {
        --coins;
    }

    public int getCoins() {
        return coins;
    }

    private void Start() {
        jumpVel = (jumpTime * -gravity) / 2f;
    }

    // Update is called once per frame
    void Update() {
        speed = getSpeed(coins);

        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0) {
            velocity.y = -2f;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = ((transform.right * x + transform.forward * z).normalized / 2) * speed;

        if (Input.GetButton("Sprint") && isGrounded && z > 0) {
            move *= 2;
        }

        if (z < 0) {
            move *= 0.4f;
        }

        animator.SetFloat("Speed", (move.magnitude * (z > 0 ? 1 : -1)) / 12f);

        controller.Move(move * Time.deltaTime);

        if (Input.GetButtonDown("Jump") && isGrounded) {
            velocity.y = jumpVel;
            animator.SetTrigger("Jump");
        }

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
    }

    float getSpeed(int coins) {
        return 12f + Mathf.Round(Mathf.Exp((coins - 14.9f) / -6) - 12);
    }

}
