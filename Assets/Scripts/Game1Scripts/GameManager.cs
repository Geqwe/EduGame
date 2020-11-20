using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System;
using TMPro;

//Firebase
// using Firebase;
// using Firebase.Database;
// using Firebase.Unity.Editor;

//WEBGL
using UnityEngine.Networking;


public class GameManager : MonoBehaviour
{
    //Firebase
    // DatabaseReference reference;

    //WEBGL
    string url= "https://us-central1-edugame-7353d.cloudfunctions.net/mydatabase/data";
    public string dataa;

    public Image[] imgsToDiss;
    public int numberOfDesicions;
    public CanvasGroup canvasGroup;
    public GameObject failureMenu;
    public GameObject successMenu;
    public GameObject fadeGM;
    public GameObject helpMenu;
    public GameObject scoreMenu;
    public TMP_Text score;
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
    public AudioClip successRepeat;

    AudioSource audio;
    Image fade;

    // Start is called before the first frame update
    void Start()
    {
        //Firebase
        // FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://edugame-7353d.firebaseio.com/");
        // reference = FirebaseDatabase.DefaultInstance.RootReference;

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
        if(sceneName!="Level1" && sceneName!="Level2" && sceneName!="Level3") {
            yield return new WaitForSeconds(4f);
        }
        for (int i = 0; i < imgsToDiss.Length;i++) {
            if(imgsToDiss[i]==null) {
                break;
            }
            if(sceneName=="Level1" || sceneName=="Level2" || sceneName=="Level3") {
                imgsToDiss[i].CrossFadeColor(Color.red, 3f, true, true);
                // yield return new WaitForSeconds(3f);
            }
            audio.PlayOneShot(dissolve);
            imgsToDiss[i].CrossFadeAlpha(0f, 2f, false);
            // yield return new WaitForSeconds(1f);
        }
        yield return new WaitForSeconds(2f);
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
            string game = PlayerPrefs.GetString("gameName", "\"First\"");
            string timeOnStart = PlayerPrefs.GetString("TimeOnStart", Time.time.ToString("f3"));
            string timeNow = Time.time.ToString("f3");
            if(PlayerPrefs.GetInt("levelReached", 0) < levelToUnlock) {
                PlayerPrefs.SetInt("levelReached", levelToUnlock);
                string username = PlayerPrefs.GetString("Username");
                //Firebase
                // reference.Child("users").Child(PlayerPrefs.GetString("Username","default")).Child("levelReached").SetValueAsync(levelToUnlock);

                //WebGl
                string data1 = "{\"hasPlayed\":\"" + PlayerPrefs.GetString("hasPlayed","0").ToString() + "\"," + "\"firstScore\":\"" + PlayerPrefs.GetString("firstScore","0") + "\"" + "," + "\"levelReached\":\"" + levelToUnlock.ToString() + "\"" + "," + "\"TimePlayed\":\"" + PlayerPrefs.GetString("lastTimePlayed") + "\"}";
                string option2 = "save2";
                dataa = "{\"option\":\"" + option2 + "\",\"username\":\"" + username + "\" ,\"data\":" + data1 + ",\"path\":" + game + "}";
                Debug.Log(dataa);
                StartCoroutine(Upload());
            }
            audio.PlayOneShot(success);
            if(sceneName=="Level7") {
                //change toAdd
                float toAdd = Single.Parse(PlayerPrefs.GetString("lastTimePlayed")) + Convert.ToSingle(timeNow) - Convert.ToSingle(timeOnStart);
                string username = PlayerPrefs.GetString("Username");
                if(PlayerPrefs.GetString("hasPlayed") == "0") {
                    //Firebase
                    // reference.Child("users").Child(username).Child("hasPlayed").SetValueAsync("1");
                    // reference.Child("users").Child(username).Child("firstScore").SetValueAsync(toAdd.ToString());
                    PlayerPrefs.SetString("firstScore", toAdd.ToString());
                    PlayerPrefs.SetString("hasPlayed","1");
                    //WEBGL
                    string data1 = "{\"hasPlayed\":\"" + "1" + "\"," + "\"firstScore\":\"" + toAdd.ToString() + "\"" + "," + "\"levelReached\":\"" + "0" + "\"" + "," + "\"TimePlayed\":\"" + "0" + "\"}";
                    string option2 = "save2";
                    dataa = "{\"option\":\"" + option2 + "\",\"username\":\"" + username + "\" ,\"data\":" + data1 + ",\"path\":" + game + "}";
                    Debug.Log(dataa);
                    StartCoroutine(Upload());
                }
                else {
                    string data1 = "{\"hasPlayed\":\"" + "1" + "\"," + "\"firstScore\":\"" + PlayerPrefs.GetString("firstScore","0") + "\"" + "," + "\"levelReached\":\"" + "0" + "\"" + "," + "\"TimePlayed\":\"" + "0" + "\"}";
                    string option2 = "save2";
                    dataa = "{\"option\":\"" + option2 + "\",\"username\":\"" + username + "\" ,\"data\":" + data1 + ",\"path\":" + game + "}";
                    Debug.Log(dataa);
                    StartCoroutine(Upload());
                }

                //Firebase
                // reference.Child("users").Child(username).Child("TimePlayed").SetValueAsync("1");
                // reference.Child("users").Child(username).Child("levelReached").SetValueAsync(0);

                //WEBGL
                PlayerPrefs.SetInt("levelReached",0);
                PlayerPrefs.SetString("lastTimePlayed","0");
                PlayerPrefs.SetString("TimeOnStart",Time.time.ToString("f3"));
                score.text = toAdd.ToString();
                scoreMenu.SetActive(true);
                audio.PlayOneShot(successRepeat);
            }
            else {
                successMenu.SetActive(true);
                audio.PlayOneShot(successVoice);
            }
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
        helpMenu.SetActive(false);
        audio.Stop();
    }

    private void OnApplicationQuit() {
        string lastTimePlayed = PlayerPrefs.GetString("lastTimePlayed");
        string timeOnStart = PlayerPrefs.GetString("TimeOnStart", Time.time.ToString("f3"));
        string timeNow = Time.time.ToString("f3");
        string levelReached = PlayerPrefs.GetInt("levelReached",0).ToString();
        string firstScore = PlayerPrefs.GetString("firstScore","0");
        Debug.Log("Level reached: "+levelReached+" firstScore: "+firstScore);
        if(lastTimePlayed!="0") {
            lastTimePlayed = lastTimePlayed.Substring(0,6);
        }
        
        float toAdd = Single.Parse(lastTimePlayed) + Convert.ToSingle(timeNow) - Convert.ToSingle(timeOnStart);
        string username = PlayerPrefs.GetString("Username","default");
        string game = PlayerPrefs.GetString("gameName", "\"First\"");

        //Firebase
        // reference.Child("users").Child(username).Child("TimePlayed").SetValueAsync(toAdd.ToString());
        // reference.Child("users").Child(username).Child("levelReached").SetValueAsync(PlayerPrefs.GetInt("levelReached", 0));
    
        //WEBGL
        string data1 = "{\"hasPlayed\":\"" + PlayerPrefs.GetString("hasPlayed","0").ToString() + "\"," + "\"firstScore\":\"" + firstScore + "\"" + "," + "\"levelReached\":\"" + levelReached + "\"" + "," + "\"TimePlayed\":\"" + toAdd.ToString() + "\"}";
        string option2 = "save2";
        dataa = "{\"option\":\"" + option2 + "\",\"username\":\"" + username + "\" ,\"data\":" + data1 + ",\"path\":" + game + "}";
        Debug.Log(dataa);
        StartCoroutine(Upload());
    }

    IEnumerator Upload()
    {
        var request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(dataa);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        yield return request.SendWebRequest();
        Debug.Log("Status Code: " + request.responseCode);
        Debug.Log("Status body: " + request.downloadHandler.text);
    }
}
