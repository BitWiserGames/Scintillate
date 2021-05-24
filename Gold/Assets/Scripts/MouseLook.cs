using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour {
    public float mouseSensitivity = 500f;

    [SerializeField]
    Transform playerBody;

    public static float mouseMult = 1.0f;

    public GameObject setTarget;
    public GameObject camTarget;

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

            setTarget.transform.position = camTarget.transform.position;

            //setTarget.transform.position = playerBody.forward + playerBody.position;
                //new Vector3(transform.position.x, transform.position.y + Mathf.Sin(Mathf.Deg2Rad * -1 * transform.rotation.x), transform.position.z + Mathf.Cos(Mathf.Deg2Rad * -1 * transform.rotation.x));

        }
    }
}