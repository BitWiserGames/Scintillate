using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldState : MonoBehaviour {
    [SerializeField]
    Transform enemyPrefab;

    bool doomModeStarted = false;

    AudioManager am = null;

    public bool isInDoomMode() {
        return doomModeStarted;
    }

    public void startDoomMode() {
        doomModeStarted = true;

        am.Stop("ThemeCalm");
        am.Play("ThemeDoom");

        // Spawn enemy
        Transform player = Instantiate(enemyPrefab, new Vector3(-0.5f * MazeRenderer.width * MazeRenderer.size, 0.2f, -0.5f * MazeRenderer.height * MazeRenderer.size), transform.rotation);
    }

    private void Start() {
        am = FindObjectOfType<AudioManager>();
    }
}
