using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateCloser : MonoBehaviour {
    public Transform leftGate;
    public Transform rightGate;

    bool closed = false;

    private void OnTriggerEnter(Collider other) {
        if (!closed && other.CompareTag("Player")) {
            leftGate.Rotate(new Vector3(0, 90, 0));
            rightGate.Rotate(new Vector3(0, -90, 0));

            closed = true;
        }
    }
}
