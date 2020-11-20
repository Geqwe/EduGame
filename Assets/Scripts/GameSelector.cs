using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameSelector : MonoBehaviour
{
    public Image fade;

    public Button[] levelButtons;

    public AudioClip startLevel;
    AudioSource audio;

    private void Start() {
        PlayerPrefs.SetString("Username", "default");
        audio = GetComponent<AudioSource>();
        audio.PlayOneShot(startLevel);
        fade.enabled = false;
        for (int i = 4; i < levelButtons.Length; i++) {
            levelButtons[i].interactable = false;
        }
    }

    public void GetGameName(string gameName) {
        PlayerPrefs.SetString("gameName",gameName);
        Debug.Log(gameName);
    }

    public void Select(string levelName) {
        fade.canvasRenderer.SetAlpha(0.0f);
        fade.enabled = true;
        StartCoroutine(Fade(levelName));
    }

    IEnumerator Fade(string levelName) {
        fade.CrossFadeAlpha(1,2,false);
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene(levelName);
    }
}
