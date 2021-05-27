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

    private void Update() {
        transform.position = transform.position + new Vector3(0, (Mathf.Sin(Time.time) * 0.5f) * Time.deltaTime, 0);
        transform.Rotate(Vector3.up, Space.Self);
    }

    void Pickup(Collider player) {
        am.Play("CoinPickup");

        player.GetComponent<PlayerController>().addCoin();

        Destroy(gameObject);
    }

}
