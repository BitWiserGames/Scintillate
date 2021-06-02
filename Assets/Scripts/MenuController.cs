using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MenuController : MonoBehaviour {
    public static float sen;


    [SerializeField]
    private AudioMixer mixer;
    [SerializeField]
    private Slider master;
    [SerializeField]
    private Slider sfx;
    [SerializeField]
    private Slider music;
    [SerializeField]
    private Slider sensitivity;
    [SerializeField]
    private Slider loadingBar;

    private void Start() {
        Cursor.lockState = CursorLockMode.Confined;

        
        sensitivity.value = PlayerPrefs.GetFloat("sen", 1f);
        sen = sensitivity.value;
        master.value = PlayerPrefs.GetFloat("master", 1f);
        sfx.value = PlayerPrefs.GetFloat("sfx", 1f);
        music.value = PlayerPrefs.GetFloat("music", 1f);
    }

    public void SetMaster(float volume) {
        PlayerPrefs.SetFloat("master", volume);
        PlayerPrefs.Save();
        mixer.SetFloat("Master", Mathf.Log10(volume) * 20f);
    }

    public void SetSFX(float volume) {
        PlayerPrefs.SetFloat("sfx", volume);
        PlayerPrefs.Save();
        mixer.SetFloat("SFX", Mathf.Log10(volume) * 20f);
    }

    public void SetMusic(float volume) {
        PlayerPrefs.SetFloat("music", volume);
        PlayerPrefs.Save();
        mixer.SetFloat("Music", Mathf.Log10(volume) * 20f);
    }

    public void SetSensitivity(float sensitivity) {
        PlayerPrefs.SetFloat("sen", sensitivity);
        PlayerPrefs.Save();
        
        sen = sensitivity;
    }

    public void ChangeScene(int index) {
        SceneManager.LoadScene(index);
    }

    public static void CloseGame() {
        Application.Quit();
    }
}
