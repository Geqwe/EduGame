using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using System;

public class GameManager : MonoBehaviour
{
    DatabaseReference reference;

    public Image[] imgsToDiss;
    public int numberOfDesicions;
    public CanvasGroup canvasGroup;
    public GameObject failureMenu;
    public GameObject successMenu;
    public GameObject fadeGM;
    public GameObject helpMenu;
    public int levelToUnlock;

    int checkRestart;
    int checkNextLevel;
    string sceneName;
    
    public AudioClip wrong;
    public AudioClip right;
    public AudioClip success;
    public AudioClip failure;
    public AudioClip dissolve;
    public AudioClip successVoice;
    public AudioClip failureVoice;
    public AudioClip startLevel;
    public AudioClip dissFromGame;

    AudioSource audio;
    Image fade;

    // Start is called before the first frame update
    void Start()
    {
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://edugame-7353d.firebaseio.com/");
        reference = FirebaseDatabase.DefaultInstance.RootReference;
        audio = GetComponent<AudioSource>();
        sceneName = SceneManager.GetActiveScene().name;
    }

    private void Awake() {
        fade = fadeGM.GetComponent<Image>();
        fade.CrossFadeAlpha(0,2,false);
        canvasGroup.alpha = 0;
        checkRestart = 0;
        checkNextLevel = 0;
        StartCoroutine(FadeColor());
    }

    IEnumerator FadeColor() {
        yield return new WaitForSeconds(2f);
        fadeGM.SetActive(false);
        helpMenu.SetActive(true);
        audio.PlayOneShot(startLevel);
        // Time.timeScale = 0f;
        yield return new WaitForSeconds(4f);
        helpMenu.SetActive(false);
        yield return new WaitForSeconds(2f);
        for (int i = 0; i < imgsToDiss.Length;i++) {
            if(imgsToDiss[i]==null) {
                break;
            }
            if(sceneName=="Level1" || sceneName=="Level2" || sceneName=="Level3") {
                imgsToDiss[i].CrossFadeColor(Color.red, 3f, true, true);
                yield return new WaitForSeconds(3f);
            }
            audio.PlayOneShot(dissolve);
            imgsToDiss[i].CrossFadeAlpha(0f, 2f, false);
            yield return new WaitForSeconds(1f);
        }
        audio.PlayOneShot(dissFromGame);
        StartCoroutine(DoFade());
    }

    IEnumerator DoFade (){
		while (canvasGroup.alpha<=0.99){
			canvasGroup.alpha += Time.deltaTime / 2;
			yield return null;
		}
		canvasGroup.interactable = true;
		yield return null;
	}

    public void Wrong() {
        audio.PlayOneShot(wrong);
        GameObject btnGM = EventSystem.current.currentSelectedGameObject;
        btnGM.GetComponent<Button>().interactable = false;
        Image gmImg = btnGM.GetComponent<Image>();
        gmImg.color = Color.red;
        checkRestart++;
        if(checkRestart==3) {
            audio.PlayOneShot(failure);
            failureMenu.SetActive(true);
            audio.PlayOneShot(failureVoice);
        }
    }

    public void Right() {
        audio.PlayOneShot(right);
        GameObject btnGM = EventSystem.current.currentSelectedGameObject;
        btnGM.GetComponent<Button>().interactable = false;
        Image gmImg = btnGM.GetComponent<Image>();
        gmImg.color = Color.green;
        checkNextLevel++;
        if(checkNextLevel==numberOfDesicions) {
            if(PlayerPrefs.GetInt("levelReached", 0) < levelToUnlock) {
                PlayerPrefs.SetInt("levelReached", levelToUnlock);
            }
            audio.PlayOneShot(success);
            successMenu.SetActive(true);
            audio.PlayOneShot(successVoice);
        }
    }

    public void Restart() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void gotoLevelSelect() {
        StartCoroutine(Fade());
    }

    IEnumerator Fade() {
        fade.canvasRenderer.SetAlpha(0.0f);
        fadeGM.SetActive(true);
        fade.CrossFadeAlpha(1,2,false);
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene("LevelSelect");
    }

    public void HideHelp() {
        Time.timeScale = 1f;
        helpMenu.SetActive(false);
    }

    private void OnApplicationQuit() {
        string lastTimePlayed = PlayerPrefs.GetString("lastTimePlayed");
        if(lastTimePlayed!="0") {
            lastTimePlayed = lastTimePlayed.Substring(0,6);
        }
        float toAdd = float.Parse(lastTimePlayed) + Convert.ToSingle(Time.realtimeSinceStartup);
        reference.Child("users").Child(PlayerPrefs.GetString("Username","default")).Child("TimePlayed").SetValueAsync(toAdd.ToString());
    }
}
