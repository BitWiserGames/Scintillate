using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideEyeLight : MonoBehaviour {
    public Behaviour leftEye;
    public Behaviour rightEye;

    private void OnPreCull() {
        leftEye.enabled = false;
        rightEye.enabled = false;
    }

    private void OnPreRender() {
        leftEye.enabled = false;
        rightEye.enabled = false;
    }

    private void OnPostRender() {
        leftEye.enabled = true;
        rightEye.enabled = true;
    }
}
