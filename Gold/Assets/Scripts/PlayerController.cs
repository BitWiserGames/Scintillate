using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    [SerializeField]
    Animator animator;

    [SerializeField]
    CharacterController controller;

    [SerializeField]
    Transform coinPrefab;

    [SerializeField]
    Light flashlightLight;

    [SerializeField]
    float speed = 12f;

    [SerializeField]
    LayerMask groundMask;

    [SerializeField]
    LayerMask enemyLayer;

    public MouseLook mouseLookScript = null;

    private float interactDistance = 3;

    CoinDisplay coinDisplay;

    AudioManager audioManager = null;

    int coins = 0;

    WorldState worldState = null;

    bool coinJiggleEnabled = false;
    bool runSoundEnabled = false;
    bool walkSoundEnabled = false;

    public void KillPlayer() {
        // Disable enemies
        for (int i = 0; i < 4; ++i) {
            WorldState.enemies[i].GetComponent<EnemyController>().enabled = false;
            WorldState.enemies[i].GetComponent<Animator>().enabled = false;
        }

        // Disable player
        enabled = false;
        animator.SetFloat("Speed", 0);
        audioManager.Stop("CoinBagShake");
        audioManager.Stop("PlayerWalk");
        audioManager.Stop("PlayerRun");

        GameObject[] enemyNoises = GameObject.FindGameObjectsWithTag("EnemyAudio");

        foreach (GameObject gameObject in enemyNoises) {
            gameObject.SetActive(false);
        }

        mouseLookScript.enabled = false;

        // Death sounds
        audioManager.Stop("ThemeDoom");

        audioManager.Play("MonsterScream");
        audioManager.Play("PlayerDeathSfx");
        audioManager.Play("PlayerDeathSong");

        worldState.LoseGame();
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
            } else {
                coin.position = transform.position + (transform.forward * -3) + (transform.up * 0.8f);
            }

            // Remove coin
            --coins;
            coinDisplay.SetCoinCount(coins);
        }
    }

    public void removeCoin(int amount) {
        coins -= amount;
        coinDisplay.SetCoinCount(coins);
    }

    public int getCoin() {
        return coins;
    }

    private void Start() {
        coinDisplay = FindObjectOfType<CoinDisplay>();
        worldState = FindObjectOfType<WorldState>();
        audioManager = FindObjectOfType<AudioManager>();
    }

    // Update is called once per frame
    void Update() {
        if (!WorldState.IsPaused()) {
            speed = getSpeed(coins);

            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");

            Ray ray = mouseLookScript.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            Vector3 move = ((transform.right * x + transform.forward * z).normalized / 2) * speed;

            if (Input.GetButton("Sprint") && z > 0) { // Moving forward, not backwards
                move *= 2;

                if (walkSoundEnabled) {
                    walkSoundEnabled = false;
                    audioManager.Stop("PlayerWalk");
                }

                if (!coinJiggleEnabled && coins >= 2) {
                    coinJiggleEnabled = true;
                    audioManager.Play("CoinBagShake");
                }

                if (!runSoundEnabled) {
                    runSoundEnabled = true;
                    audioManager.Play("PlayerRun");
                }

                animator.SetBool("Sprint", true);
            } else {
                if (coinJiggleEnabled) {
                    coinJiggleEnabled = false;
                    audioManager.Stop("CoinBagShake");
                }

                if (runSoundEnabled) {
                    runSoundEnabled = false;
                    audioManager.Stop("PlayerRun");
                }

                if (!walkSoundEnabled && move.magnitude != 0) {
                    walkSoundEnabled = true;
                    audioManager.Play("PlayerWalk");
                } else if (walkSoundEnabled && move.magnitude == 0) {
                    walkSoundEnabled = false;
                    audioManager.Stop("PlayerWalk");
                }

                animator.SetBool("Sprint", false);
            }

            if (coinJiggleEnabled) {
                Collider[] hitColliders = Physics.OverlapSphere(transform.position, 35, enemyLayer);
                foreach (var hitCollider in hitColliders) {
                    hitCollider.GetComponent<EnemyController>().SetTarget(transform.position);
                }
            }

            if (z < 0) {
                move *= 0.4f;
            }

            if (Physics.Raycast(ray, out hit, interactDistance)) {
                Interactable interactable = hit.collider.GetComponent<Interactable>();
                if (interactable != null)
                    if (!interactable.isSwitch && !interactable.activated)
                        worldState.SetLookingAtDoor(true);

            } else
                worldState.SetLookingAtDoor(false);
            animator.SetFloat("Speed", move.magnitude);

            controller.Move(move * Time.deltaTime);

            if (Input.GetButtonDown("Drop")) {
                removeCoin();
            }

            if (Input.GetButtonDown("Flashlight")) {
                flashlightLight.enabled = !flashlightLight.enabled;
                audioManager.Play("Flashlight");
            }

            if (Input.GetButtonDown("Interact")) {
                if (Physics.Raycast(ray, out hit, interactDistance)) {
                    Interactable interactable = hit.collider.GetComponent<Interactable>();
                    if (interactable != null) {
                        interactable.Interact();
                    }
                }
            }

            if (Input.GetButtonDown("Cancel")) {
                worldState.PauseGame();
            }
        } else {
            animator.SetFloat("Speed", 0);
            walkSoundEnabled = false;
            audioManager.Stop("PlayerWalk");
            runSoundEnabled = false;
            audioManager.Stop("PlayerRun");
            coinJiggleEnabled = false;
            audioManager.Stop("CoinBagShake");
        }
    }

    float getSpeed(int coins) {
        return 12f + Mathf.Round(Mathf.Exp((coins - 14.9f) / -6) - 12);
    }

}
