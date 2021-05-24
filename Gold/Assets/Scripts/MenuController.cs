using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    public static float masterVal = 1f;
    public static float sfxVal = 1f;
    public static float musicVal = 1f;



    public AudioMixer mixer;
    public Slider master;
    public Slider sfx;
    public Slider music;
    public Slider sensitivity;


    private void Start() {
        sensitivity.value = MouseLook.mouseMult;
        master.value = Mathf.Pow(10f, masterVal / 20f);
        sfx.value = Mathf.Pow(10f, sfxVal / 20f);
        music.value = Mathf.Pow(10f, musicVal / 20f);
    }

    public void SetMaster(float volume) {
        masterVal = Mathf.Log10(volume) * 20f;
        mixer.SetFloat("Master", masterVal);
    }

    public void SetSFX(float volume) {
        sfxVal = Mathf.Log10(volume) * 20f;
        mixer.SetFloat("SFX", sfxVal);
    }

    public void SetMusic(float volume) {
        musicVal = Mathf.Log10(volume) * 20f;
        mixer.SetFloat("Music", musicVal);
    }

    public void SetSensitivity(float sensitivity) {
        MouseLook.mouseMult = sensitivity;
    }

    public void ChangeScene(int index) {
        SceneManager.LoadScene(index);
    }

    public static void CloseGame() {
        Application.Quit();
    }
}