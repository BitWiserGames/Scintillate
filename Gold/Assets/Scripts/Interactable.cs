using UnityEngine;

public class Interactable : MonoBehaviour {

    private static int doorCost = 5;
    public bool isSwitch = false;
    public GameObject interactableLeft;
    public GameObject interactableCenter;
    public GameObject interactableRight;
    public bool activated = false;
    public bool interacted = false;
    public float speed = 0.25f;
    private float startTime;
    private Quaternion switchRotation;
    private Quaternion doorLeftRot;
    private Quaternion doorRightRot;

    public void Update() {
        if (activated) {
            interacted = true;
            if (isSwitch) {
                if (interactableCenter.transform.localRotation.x < 0) {
                    interactableCenter.transform.localRotation = Quaternion.Lerp(switchRotation, Quaternion.Euler(new Vector3(0, 0, 0)), (Time.time - startTime) * speed);
                    if (interactableCenter.transform.localRotation.x >= 0 && interactableCenter.transform.localRotation.x < 90)
                        startTime = Time.time;
                }
                if (interactableCenter.transform.localRotation.x >= 0 && interactableCenter.transform.localRotation.x < 90) {
                    interactableLeft.SetActive(false);
                    interactableRight.SetActive(true);

                    interactableCenter.transform.localRotation = Quaternion.Lerp(Quaternion.Euler(new Vector3(0, 0, 0)), Quaternion.Euler(new Vector3(90, 0, 0)), (Time.time - startTime) * speed);
                }
            }
            else {
                interactableLeft.transform.localRotation = Quaternion.Lerp(doorLeftRot, Quaternion.Euler(new Vector3(0, 90, 0)), (Time.time - startTime) * speed);
                interactableRight.transform.localRotation = Quaternion.Lerp(doorRightRot, Quaternion.Euler(new Vector3(0, -90, 0)), (Time.time - startTime) * speed);
            }
        }
    }

    public void Interact() {
        if (!interacted) {
            if (!activated) {
                if (isSwitch) {
                    WorldState.ActivateSwitch();
                    switchRotation = interactableCenter.transform.localRotation;
                    FindObjectOfType<AudioManager>().Play("Switch");
                    activated = true;
                    startTime = Time.time;
                } else {
                    PlayerController pc = FindObjectOfType<PlayerController>();
                    if(pc.getCoin() >= doorCost) {
                        pc.removeCoin(doorCost);
                        doorLeftRot = interactableLeft.transform.localRotation;
                        doorRightRot = interactableRight.transform.localRotation;
                        FindObjectOfType<AudioManager>().Play("Gate");
                        activated = true;
                        startTime = Time.time;
                    }
                }
            }
        }
    }

    public void OpenGate() {

    }

    public void ActivateSwitch() {

    }
}
