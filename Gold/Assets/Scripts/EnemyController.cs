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
        if (agent.remainingDistance < 0.1) {
            float x = Random.Range(transform.position.x - (10f * MazeRenderer.size), transform.position.x + (10f * MazeRenderer.size));
            float z = Random.Range(transform.position.z - (10f * MazeRenderer.size), transform.position.z + (10f * MazeRenderer.size));

            x = Mathf.Min(Mathf.Max(x, -0.5f * MazeRenderer.width * MazeRenderer.size), 0.5f * MazeRenderer.width * MazeRenderer.size);
            z = Mathf.Min(Mathf.Max(z, -0.5f * MazeRenderer.height * MazeRenderer.size), 0.5f * MazeRenderer.height * MazeRenderer.size);

            agent.SetDestination(new Vector3(x, 0, z));
        }

        anim.SetFloat("Speed", agent.velocity.magnitude / (agent.speed * 2.2f));
    }
}
