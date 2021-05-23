using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldState : MonoBehaviour {
    [SerializeField]
    Transform enemyPrefab;

    [SerializeField]
    Light sun;

    Camera cam;

    bool doomModeStarted = false;

    AudioManager am = null;

    public bool isInDoomMode() {
        return doomModeStarted;
    }

    public void startDoomMode() {
        cam = FindObjectOfType<Camera>();

        doomModeStarted = true;

        am.Stop("ThemeCalm");
        am.Play("ThemeDoom");

        // Spawn enemy
        Transform enemy1 = Instantiate(enemyPrefab, new Vector3(-0.5f * MazeRenderer.width * MazeRenderer.size, 0.2f, -0.5f * MazeRenderer.height * MazeRenderer.size), transform.rotation);
        Transform enemy2 = Instantiate(enemyPrefab, new Vector3(0.5f * MazeRenderer.width * MazeRenderer.size, 0.2f, -0.5f * MazeRenderer.height * MazeRenderer.size), transform.rotation);
        Transform enemy3 = Instantiate(enemyPrefab, new Vector3(-0.5f * MazeRenderer.width * MazeRenderer.size, 0.2f, 0.5f * MazeRenderer.height * MazeRenderer.size), transform.rotation);
        Transform enemy4 = Instantiate(enemyPrefab, new Vector3(0.5f * MazeRenderer.width * MazeRenderer.size, 0.2f, 0.5f * MazeRenderer.height * MazeRenderer.size), transform.rotation);

        // Set lighting
        cam.backgroundColor = Color.black;
        sun.enabled = false;
    }

    private void Start() {
        am = FindObjectOfType<AudioManager>();
    }
}
