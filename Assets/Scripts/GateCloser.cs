using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateCloser : MonoBehaviour {
    public Transform leftGate;
    public Transform rightGate;

    AudioManager audioManager = null;

    bool closed = false;

    float speed = 0.25f;

    Quaternion fromPositionLeft;
    Quaternion fromPositionRight;

    float startTime;

    private void Start() {
        audioManager = FindObjectOfType<AudioManager>();
    }

    private void OnTriggerEnter(Collider other) {
        if (!closed && other.CompareTag("Player")) {
            fromPositionLeft = leftGate.rotation;
            fromPositionRight = rightGate.rotation;

            startTime = Time.time;

            closed = true;

            audioManager.Play("Gate");
        }
    }

    private void Update() {
        if (closed) {
            leftGate.rotation = Quaternion.Lerp(fromPositionLeft, Quaternion.Euler(new Vector3(0, 90, 0)), (Time.time - startTime) * speed);
            rightGate.rotation = Quaternion.Lerp(fromPositionRight, Quaternion.Euler(new Vector3(0, 90, 0)), (Time.time - startTime) * speed);
        }
    }
}
