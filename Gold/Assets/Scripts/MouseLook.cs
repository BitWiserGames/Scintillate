using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour {
    public float mouseSensitivity = 500f;

    [SerializeField]
    Transform playerBody;

    public static float mouseMult = 1.0f;

    float xRotation = 0f;

    // Start is called before the first frame update
    void Start() {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update() {
        if (!WorldState.IsPaused()) {
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * mouseMult * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * mouseMult * Time.deltaTime;

            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 65f);

            transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            playerBody.Rotate(Vector3.up * mouseX);
        }
    }
}