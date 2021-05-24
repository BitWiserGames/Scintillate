using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {
    [SerializeField]
    UnityEngine.AI.NavMeshAgent agent;

    [SerializeField]
    Animator anim;

    [SerializeField]
    Camera cam;

    RenderTexture lightCheckTexture = null;

    float lightLevel;

    public static PlayerController player;

    bool sprint = false;

    public void SetTexture(RenderTexture renderTexture) {
        cam.targetTexture = renderTexture;
        this.lightCheckTexture = renderTexture;
    }

    public void SetTarget(Vector3 position) {
        agent.SetDestination(position);
        sprint = true;

        agent.speed = 7f;
    }

    private void Start() {
        agent.speed = 3.5f;
        player = FindObjectOfType<PlayerController>();
    }

    // Update is called once per frame
    void Update() {
        if (!WorldState.IsPaused()) {
            RenderTexture tmpTexture = RenderTexture.GetTemporary(lightCheckTexture.width, lightCheckTexture.height, 0, RenderTextureFormat.Default, RenderTextureReadWrite.Linear);
            Graphics.Blit(lightCheckTexture, tmpTexture);

            RenderTexture previousTexture = RenderTexture.active;

            RenderTexture.active = tmpTexture;

            Texture2D tmp2DTexture = new Texture2D(lightCheckTexture.width, lightCheckTexture.height);
            tmp2DTexture.ReadPixels(new Rect(0, 0, tmpTexture.width, tmpTexture.height), 0, 0);
            tmp2DTexture.Apply();

            RenderTexture.active = previousTexture;
            RenderTexture.ReleaseTemporary(tmpTexture);

            Color32[] colors = tmp2DTexture.GetPixels32();

            lightLevel = 0;
            for (int i = 0; i < colors.Length; ++i) {
                lightLevel += (0.2126f * colors[i].r) + (0.7152f * colors[i].g) + (0.0722f * colors[i].b);
            }

            if (lightLevel > 8000) {
                SetTarget(player.transform.position);
            }

            if (agent.remainingDistance < 0.1) {
                agent.speed = 3.5f;
                sprint = false;

                float x = Random.Range(transform.position.x - (10f * MazeRenderer.size), transform.position.x + (10f * MazeRenderer.size));
                float z = Random.Range(transform.position.z - (10f * MazeRenderer.size), transform.position.z + (10f * MazeRenderer.size));

                x = Mathf.Min(Mathf.Max(x, -0.5f * MazeRenderer.width * MazeRenderer.size), 0.5f * MazeRenderer.width * MazeRenderer.size);
                z = Mathf.Min(Mathf.Max(z, -0.5f * MazeRenderer.height * MazeRenderer.size), 0.5f * MazeRenderer.height * MazeRenderer.size);

                agent.SetDestination(new Vector3(x, 0, z));
            }

            if (Vector3.Distance(transform.position, player.transform.position) < 2) {
                player.KillPlayer();
            }

            anim.SetFloat("Speed", (agent.velocity.magnitude / (agent.speed * (sprint ? 1f : 2.2f))));
        }
        else {
            anim.SetFloat("Speed", 0);
            agent.speed = 0;
        }
    }
}
