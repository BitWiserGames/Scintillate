using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {
    [SerializeField]
    UnityEngine.AI.NavMeshAgent agent;

    [SerializeField]
    Animator anim;

    // Update is called once per frame
    void Update() {
        agent.SetDestination(new Vector3(0, 0, 0));

        anim.SetFloat("Speed", agent.velocity.magnitude / (agent.speed * 2.2f));
        Debug.Log(agent.velocity.magnitude / agent.speed);
    }
}
