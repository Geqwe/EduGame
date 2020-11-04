using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelSelector : MonoBehaviour
{
    public Image fade;

    public Button[] levelButtons;

    public AudioClip startLevel;
    AudioSource audio;

    private void Start() {
        audio = GetComponent<AudioSource>();
        audio.PlayOneShot(startLevel);
        fade.enabled = false;
        int levelReached = PlayerPrefs.GetInt("levelReached", 0);
        for (int i = 1; i < levelButtons.Length; i++) {
            if(i>levelReached) {
                levelButtons[i].interactable = false;
            }
            
        }
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
