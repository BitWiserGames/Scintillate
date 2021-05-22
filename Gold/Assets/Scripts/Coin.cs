using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour {

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            Pickup(other);
        }
    }

    void Pickup(Collider player) {
        // Play sound

        player.GetComponent<PlayerController>().addCoin();

        Destroy(gameObject);
    }

}
