using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour {
    AudioManager am = null;

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            Pickup(other);
        }
    }

    private void Start() {
        am = FindObjectOfType<AudioManager>();
    }

    void Pickup(Collider player) {
        am.Play("CoinPickup");

        player.GetComponent<PlayerController>().addCoin();

        Destroy(gameObject);
    }

}
