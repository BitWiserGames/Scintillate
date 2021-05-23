using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.Audio;

public class MenuController : MonoBehaviour
{
    public AudioMixer mixer;

    public void SetMaster(float volume) {
        mixer.SetFloat("Master", Mathf.Log10(volume) * 20);
    }

    public void SetSFX(float volume) {
        mixer.SetFloat("SFX", Mathf.Log10(volume) * 20);
    }

    public void SetMusic(float volume) {
        mixer.SetFloat("Music", Mathf.Log10(volume) * 20);
    }

    public void ChangeScene(int index) {
        SceneManager.LoadScene(index);
    }

    public static void CloseGame() {
        Application.Quit();
    }
}
