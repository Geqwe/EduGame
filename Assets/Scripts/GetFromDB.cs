using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;
using Newtonsoft.Json;

//Firebase
// using Firebase;
// using Firebase.Database;
// using Firebase.Unity.Editor;

//WEBGL
using UnityEngine.Networking;

public class GetFromDB : MonoBehaviour
{
    // WEBGL
    string url="https://us-central1-edugame-7353d.cloudfunctions.net/mydatabase/data";
    public class mydata
    {
        public Game First;
        public Game Second;
        public Game Third;
        public Game Fourth;
        public Game Fifth;
        public Game Sixth;
        public Game Seventh;
        public Game Eighth;
        public string TimePlayed;
        public string age;
    }

    public class Game{
        public string TimePlayed;
        public string levelReached;
        public string hasPlayed;
        public string firstScore;
    }

    //Firebase
    // DatabaseReference reference;
    
    public GameObject storeValuesMenu;
    public GameObject putUserNameMenu;
    int checkDB = 0;
    public TMP_InputField inputUsername;
    AudioSource audio;
    public AudioClip startClip;
    bool checkPlay = false;

    string username;
    string levelReached;
    string lastTimePlayed;
    string firstScore;
    string hasPlayed = "0";

    mydata allInfo;
    
    
    void Start()
    {
        //Firebase
        // FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://edugame-7353d.firebaseio.com/");
        // reference = FirebaseDatabase.DefaultInstance.RootReference;

        audio = GetComponent<AudioSource>();
    }

    public void SubmitUsername() {
        //Firebase
        // loadData();

        //WEBGL
        CheckHere();
    }

    private void CheckHere() {
        if(checkPlay==true) 
            return;

        if(checkDB == 1) { //found
            PlayerPrefs.SetString("Username", username);
            PlayerPrefs.SetInt("levelReached", int.Parse(levelReached));
            PlayerPrefs.SetString("lastTimePlayed", lastTimePlayed.ToString());
            PlayerPrefs.SetString("hasPlayed", hasPlayed);
            PlayerPrefs.SetString("TimeOnStart", Time.time.ToString("f3"));
            PlayerPrefs.SetString("firstScore", firstScore);
            putUserNameMenu.SetActive(false);
            audio.PlayOneShot(startClip);
            checkPlay = true;
        }
        else if(checkDB == 2) {
            PlayerPrefs.SetString("Username", username);
            PlayerPrefs.SetInt("levelReached", 0);
            PlayerPrefs.SetString("lastTimePlayed", "0");
            PlayerPrefs.SetString("hasPlayed", "0");
            PlayerPrefs.SetString("TimeOnStart", Time.time.ToString("f3"));
            PlayerPrefs.SetString("firstScore", "0");
            checkPlay = true;
            putUserNameMenu.SetActive(false);
            storeValuesMenu.SetActive(true);
        }

        //WEBGL
        if(checkPlay==true)
            return;

        StartCoroutine(Upload());
    }

    // public void loadData() {
    //     Debug.Log(inputUsername.text);
    //     var getTask = FirebaseDatabase.DefaultInstance.GetReference("users").Child(inputUsername.text).GetValueAsync().ContinueWith(task => {
    //         if (task.IsCompleted) {
    //             if(task.Result.Exists) {
    //                 lastTimePlayed = task.Result.Child("TimePlayed").Value.ToString();
    //                 username = inputUsername.text;
    //                 levelReached = task.Result.Child("levelReached").Value.ToString();
    //                 hasPlayed = task.Result.Child("hasPlayed").Value.ToString();
    //                 checkDB = 1;
    //             }
    //             else{
    //                 Debug.Log("dont exist");
    //                 username = inputUsername.text;
    //                 checkDB = 2;
    //             }
    //         }
    //         else {
    //             storeValuesMenu.SetActive(true);
    //         }
    //     });
    //     InvokeRepeating("CheckHere",1f,1f);
    // }

    IEnumerator Upload()
    {

        Debug.Log("start request");
        var request = new UnityWebRequest(url, "POST");
        mydata playerData = new mydata();
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes("{\"option\":\"" + "load" + "\",\"username\":\"" + inputUsername.text + "\"}" );
        Debug.Log(bodyRaw.ToString());
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        // request.SetRequestHeader("Access-Control-Allow-Credentials", "true");
        // request.SetRequestHeader("Access-Control-Allow-Headers", "Accept, Content-Type, X-Access-Token, X-Application-Name, X-Request-Sent-Time");
        // request.SetRequestHeader("Access-Control-Allow-Methods", "GET, POST, PUT, OPTIONS");
        // request.SetRequestHeader("Access-Control-Allow-Origin", "*");
        // request.SetRequestHeader("Access-Control-Expose-Headers", "X-App-Signature");
        request.SetRequestHeader("Content-Type", "application/json");
        yield return request.SendWebRequest();
        Debug.Log("Status Code: " + request.responseCode);
        Debug.Log("Status body: " + request.downloadHandler.text);
        if (request.responseCode == 200)
        {
            if (request.downloadHandler.text!="noResult")
            {
                mydata task= JsonConvert.DeserializeObject<mydata>(request.downloadHandler.text);
                username = inputUsername.text;
                string gameName = PlayerPrefs.GetString("gameName","\"default\"");
                if(gameName.Contains("First")) {
                    if(task.First!=null) {
                        lastTimePlayed = task.First.TimePlayed;
                        levelReached = task.First.levelReached;
                        hasPlayed = task.First.hasPlayed;
                        firstScore = task.First.firstScore;
                    }
                    else {
                        PlayNewGame();
                    }
                }
                else if(gameName.Contains("Second")) {
                    if(task.Second!=null) {
                        lastTimePlayed = task.Second.TimePlayed;
                        levelReached = task.Second.levelReached;
                        hasPlayed = task.Second.hasPlayed;
                        firstScore = task.Second.firstScore;
                    }
                    else{
                        PlayNewGame();
                    }
                }
                else if(gameName.Contains("Third")) {
                    if(task.Third!=null) {
                        lastTimePlayed = task.Third.TimePlayed;
                        levelReached = task.Third.levelReached;
                        hasPlayed = task.Third.hasPlayed;
                        firstScore = task.Third.firstScore;
                    }
                    else{
                        PlayNewGame();
                    }
                }
                else if(gameName.Contains("Fourth")) {
                    if(task.Fourth!=null) {
                        lastTimePlayed = task.Fourth.TimePlayed;
                        levelReached = task.Fourth.levelReached;
                        hasPlayed = task.Fourth.hasPlayed;
                        firstScore = task.Fourth.firstScore;
                    }
                    else{
                        PlayNewGame();
                    }
                }
                else if(gameName.Contains("Fifth")) {
                    if(task.Fifth!=null) {
                        lastTimePlayed = task.Fifth.TimePlayed;
                        levelReached = task.Fifth.levelReached;
                        hasPlayed = task.Fifth.hasPlayed;
                        firstScore = task.Fifth.firstScore;
                    }
                    else{
                        PlayNewGame();
                    }
                }
                else if(gameName.Contains("Sixth")) {
                    if(task.Sixth!=null) {
                        lastTimePlayed = task.Sixth.TimePlayed;
                        levelReached = task.Sixth.levelReached;
                        hasPlayed = task.Sixth.hasPlayed;
                        firstScore = task.Sixth.firstScore;
                    }
                    else{
                        PlayNewGame();
                    }
                }
                else if(gameName.Contains("Seventh")) {
                    if(task.Seventh!=null) {
                        lastTimePlayed = task.Seventh.TimePlayed;
                        levelReached = task.Seventh.levelReached;
                        hasPlayed = task.Seventh.hasPlayed;
                        firstScore = task.Seventh.firstScore;
                    }
                    else{
                        PlayNewGame();
                    }
                }
                else if(gameName.Contains("Eighth")) {
                    if(task.Eighth!=null) {
                        lastTimePlayed = task.Eighth.TimePlayed;
                        levelReached = task.Eighth.levelReached;
                        hasPlayed = task.Eighth.hasPlayed;
                        firstScore = task.Eighth.firstScore;
                    }
                    else{
                        PlayNewGame();
                    }
                }
                checkDB = 1;
                Debug.Log("Username "+ username+ ", lastTimePlayed:  "+lastTimePlayed+", levelReached: "+levelReached+", firstScore: "+firstScore);
            }
            else
            {
                Debug.Log("dont exist");
                username = inputUsername.text;
                checkDB = 2;
            }
        }
        else
        {
            storeValuesMenu.SetActive(true);
        }
        yield return new WaitForSeconds(2f);
        CheckHere();
    }

    void PlayNewGame() {
        PlayerPrefs.SetString("Username", username);
        PlayerPrefs.SetInt("levelReached", 0);
        PlayerPrefs.SetString("lastTimePlayed", "0");
        PlayerPrefs.SetString("hasPlayed", "0");
        PlayerPrefs.SetString("TimeOnStart", Time.time.ToString("f3"));
        PlayerPrefs.SetString("firstScore", "0");
        checkPlay = true;
        putUserNameMenu.SetActive(false);
        InfoSubmit.instance.StartNewGame();
    }
}
