using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldState : MonoBehaviour {
    [SerializeField]
    Transform enemyPrefab;

    [SerializeField]
    Light sun;

    [SerializeField]
    RenderTexture[] renderTextures;

    Camera cam;

    bool doomModeStarted = false;

    AudioManager am = null;

    public static Transform[] enemies = new Transform[4];

    public bool isInDoomMode() {
        return doomModeStarted;
    }

    public void startDoomMode() {
        cam = FindObjectOfType<Camera>();

        doomModeStarted = true;

        am.Stop("ThemeCalm");
        am.Play("ThemeDoom");

        // Spawn enemy
        enemies[0] = Instantiate(enemyPrefab, new Vector3(-0.5f * MazeRenderer.width * MazeRenderer.size, 0.2f, -0.5f * MazeRenderer.height * MazeRenderer.size), transform.rotation);
        enemies[1] = Instantiate(enemyPrefab, new Vector3(0.5f * MazeRenderer.width * MazeRenderer.size, 0.2f, -0.5f * MazeRenderer.height * MazeRenderer.size), transform.rotation);
        enemies[2] = Instantiate(enemyPrefab, new Vector3(-0.5f * MazeRenderer.width * MazeRenderer.size, 0.2f, 0.5f * MazeRenderer.height * MazeRenderer.size), transform.rotation);
        enemies[3] = Instantiate(enemyPrefab, new Vector3(0.5f * MazeRenderer.width * MazeRenderer.size, 0.2f, 0.5f * MazeRenderer.height * MazeRenderer.size), transform.rotation);

        enemies[0].GetComponent<EnemyController>().SetTexture(renderTextures[0]);
        enemies[1].GetComponent<EnemyController>().SetTexture(renderTextures[1]);
        enemies[2].GetComponent<EnemyController>().SetTexture(renderTextures[2]);
        enemies[3].GetComponent<EnemyController>().SetTexture(renderTextures[3]);

        // Set lighting
        cam.backgroundColor = Color.black;
        sun.enabled = false;
    }

    private void Start() {
        am = FindObjectOfType<AudioManager>();
    }
}
