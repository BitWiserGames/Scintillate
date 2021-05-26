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

    public GameObject pauseMenu;
    public GameObject costText;
    public GameObject loseScreen;
    public GameObject winScreen;

    Camera cam;

    bool doomModeStarted = false;

    AudioManager am = null;

    static bool paused = false;

    public static Transform[] enemies = new Transform[4];

    private static int switchesActivated = 0;
    private static int switchesNeeded = 3;

    public void SetLookingAtDoor(bool state) {

        costText.SetActive(state);

    }

    public void ActivateSwitch() {
        ++switchesActivated;

        if (switchesActivated >= switchesNeeded) {
            StartCoroutine(WinGameCo());
        }
    }

    IEnumerator WinGameCo() {

        cam.backgroundColor = new Color(49, 77, 121);
        sun.enabled = true;

        yield return new WaitForSeconds(2);

        WinGame();
    }

    public void WinGame() {
        PauseGame();
        winScreen.SetActive(true);
    }

    public void LoseGame() {
        PauseGame();
        loseScreen.SetActive(true);
    }

    public void PauseGame() {
        paused = !paused;

        if (paused) {
            pauseMenu.SetActive(true);
            Cursor.lockState = CursorLockMode.Confined;
        } else {
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    public static bool IsPaused() {
        return paused;
    }

    public bool isInDoomMode() {
        return doomModeStarted;
    }

    public void startDoomMode() {
        cam = FindObjectOfType<Camera>();

        Interactable[] interactables = FindObjectsOfType<Interactable>();

        foreach (Interactable n in interactables)
            if (n.isSwitch)
                n.SetSwitch();

        doomModeStarted = true;

        am.Stop("ThemeCalm");
        am.Play("ThemeDoom");
        am.Play("MonsterScream");
        am.Play("Switch");

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

        pauseMenu.SetActive(false);
        loseScreen.SetActive(false);
        winScreen.SetActive(false);
    }
}
