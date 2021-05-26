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

    public AudioSource walkSfx;
    public AudioSource runSfx;

    AudioManager audioManager = null;

    RenderTexture lightCheckTexture = null;

    float lightLevel;

    public static PlayerController player;

    bool sprint = false;

    bool runSoundEnabled = false;
    bool walkSoundEnabled = false;

    bool firstUpdateFree = true;

    float lastScream = 0;

    public void SetTexture(RenderTexture renderTexture) {
        cam.targetTexture = renderTexture;
        this.lightCheckTexture = renderTexture;
    }

    public void SetTarget(Vector3 position) {
        agent.SetDestination(position);
        sprint = true;

        if (!runSoundEnabled) {
            if (walkSoundEnabled) {
                walkSoundEnabled = false;
                walkSfx.mute = true;
            }

            runSoundEnabled = true;
            runSfx.mute = false;
        }

        if (Time.time - lastScream >= 10f) {
            lastScream = Time.time;
            audioManager.Play("MonsterScream");
        }

        agent.speed = 7f;
    }

    private void Start() {
        audioManager = FindObjectOfType<AudioManager>();
        player = FindObjectOfType<PlayerController>();
    }

    // Update is called once per frame
    void Update() {
        if (!WorldState.IsPaused()) {
            if (firstUpdateFree) {
                firstUpdateFree = false;

                agent.speed = 3.5f;
            }

            if (Vector3.Distance(transform.position, player.transform.position) <= 20) {
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
            }

            float dist = agent.remainingDistance;

            if (dist != Mathf.Infinity && agent.pathStatus == UnityEngine.AI.NavMeshPathStatus.PathComplete && agent.remainingDistance == 0) {
                agent.speed = 3.5f;
                sprint = false;

                float x = Random.Range(transform.position.x - (10f * MazeRenderer.size), transform.position.x + (10f * MazeRenderer.size));
                float z = Random.Range(transform.position.z - (10f * MazeRenderer.size), transform.position.z + (10f * MazeRenderer.size));

                x = Mathf.Min(Mathf.Max(x, -0.5f * MazeRenderer.width * MazeRenderer.size), 0.5f * MazeRenderer.width * MazeRenderer.size);
                z = Mathf.Min(Mathf.Max(z, -0.5f * MazeRenderer.height * MazeRenderer.size), 0.5f * MazeRenderer.height * MazeRenderer.size);

                agent.SetDestination(new Vector3(x, 0, z));

                if (!walkSoundEnabled) {
                    if (runSoundEnabled) {
                        runSoundEnabled = false;
                        runSfx.mute = true;
                    }

                    walkSoundEnabled = true;
                    walkSfx.mute = false;
                }
            }

            if (Vector3.Distance(transform.position, player.transform.position) < 2) {
                player.KillPlayer();
            }

            anim.SetFloat("Speed", (agent.velocity.magnitude / (agent.speed * (sprint ? 1f : 2.2f))));
        }
        else {
            anim.SetFloat("Speed", 0);
            agent.speed = 0;

            runSoundEnabled = false;
            runSfx.mute = true;
            walkSoundEnabled = false;
            walkSfx.mute = true;

            firstUpdateFree = true;
        }
    }
}
