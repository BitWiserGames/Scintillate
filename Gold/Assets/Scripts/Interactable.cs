using UnityEngine;

public class Interactable : MonoBehaviour
{
    public float radius = 3f;
    public bool isSwitch = false;

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
    
    public void Interact() {
        if (isSwitch) {

        }
        else {
            OpenGate();
        }
    }

    public void OpenGate() {

    }
}
