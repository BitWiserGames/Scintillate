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
    Transform coinPrefab;

    [SerializeField]
    Light flashlightLight;

    [SerializeField]
    float speed = 12f;

    [SerializeField]
    float gravity = -9.81f;

    [SerializeField]
    float groundDistance = 0.4f;

    [SerializeField]
    LayerMask groundMask;

    [SerializeField]
    AudioSource footstepSource = null;

    [SerializeField]
    LayerMask enemyLayer;

    public MouseLook mouseLookScript = null;

    CoinDisplay coinDisplay;

    Vector3 velocity;

    AudioManager audioManager = null;

    bool isGrounded;
    float jumpTime = 32f / 30f;
    float jumpVel;

    int coins = 0;

    WorldState worldState = null;

    bool coinJiggleEnabled = false;

    public void KillPlayer() {
        // Disable enemies
        for (int i = 0; i < 4; ++i) {
            WorldState.enemies[i].GetComponent<EnemyController>().enabled = false;
            WorldState.enemies[i].GetComponent<Animator>().enabled = false;
        }

        // Disable player
        this.enabled = false;
        this.animator.SetFloat("Speed", 0);
        this.audioManager.Stop("CoinBagShake");
        footstepSource.enabled = false;
        mouseLookScript.enabled = false;

        // Ragdoll

        // Show death message
        // Show score
    }

    public void addCoin() {
        ++coins;
        coinDisplay.SetCoinCount(coins);

        if (!worldState.isInDoomMode() && coins >= 3) {
            worldState.startDoomMode();
        }
    }

    public void removeCoin() {
        if (coins > 0) {
            // Physically drop coin
            Transform coin = Instantiate(coinPrefab);

            if (Physics.Raycast(transform.position, transform.forward * -3, 1)) {
                coin.position = transform.position + (transform.forward * 3) + (transform.up * 0.8f);
            }
            else {
                coin.position = transform.position + (transform.forward * -3) + (transform.up * 0.8f);
            }

            // Remove coin
            --coins;
            coinDisplay.SetCoinCount(coins);
        }
    }

    private void Start() {
        coinDisplay = FindObjectOfType<CoinDisplay>();
        worldState = FindObjectOfType<WorldState>();
        audioManager = FindObjectOfType<AudioManager>();
    }

    // Update is called once per frame
    void Update() {
        speed = getSpeed(coins);
        jumpVel = getJump(coins);

        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0) {
            velocity.y = -2f;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = ((transform.right * x + transform.forward * z).normalized / 2) * speed;

        if (Input.GetButton("Sprint") && isGrounded && z > 0) { // Moving forward, not backwards
            move *= 2;

            if (!coinJiggleEnabled && coins >= 2) {
                coinJiggleEnabled = true;
                audioManager.Play("CoinBagShake");
            }
        }
        else {
            if (coinJiggleEnabled) {
                coinJiggleEnabled = false;
                audioManager.Stop("CoinBagShake");

                Collider[] hitColliders = Physics.OverlapSphere(transform.position, 50, enemyLayer);
                foreach (var hitCollider in hitColliders) {
                    hitCollider.GetComponent<EnemyController>().SetTarget(transform.position);
                }
            }
        }

        if (z < 0) {
            move *= 0.4f;
        }

        animator.SetFloat("Speed", (move.magnitude * (z > 0 ? 1 : -1)) / 12f);

        controller.Move(move * Time.deltaTime);

        if (move.magnitude > 0) {
            footstepSource.enabled = true;
        }
        else {
            footstepSource.enabled = false;
        }

        if (Input.GetButtonDown("Jump") && isGrounded) {
            velocity.y = jumpVel;
            animator.SetTrigger("Jump");
        }

        if (Input.GetButtonDown("Drop")) {
            removeCoin();
        }

        if (Input.GetButtonDown("Flashlight")) {
            flashlightLight.enabled = !flashlightLight.enabled;
            audioManager.Play("Flashlight");
        }

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
    }

    float getSpeed(int coins) {
        return 12f + Mathf.Round(Mathf.Exp((coins - 14.9f) / -6) - 12);
    }

    float getJump(int coins) {
        return ((jumpTime * -gravity) / 2f) * ((20f - coins) / 20f);
    }

}
