using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuSounds : MonoBehaviour
{
    public AudioClip hover;
    public AudioClip click;
    AudioSource audio;

    private void Start() {
        audio = GetComponent<AudioSource>();
    }
    public void Hover() {
        audio.PlayOneShot(hover);
    }

    public void Click() {
        audio.PlayOneShot(click);
    }

    public void LevelSelect() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
    }

    public void QuitGame() {
        SubmitToDb.instance.OnApplicationQuit();
        Application.Quit();
    }

    public void BackToMenu() {
        SubmitToDb.instance.OnApplicationQuit();
        SceneManager.LoadScene("GamesMenu");
    }
}
