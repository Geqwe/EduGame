using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//Firebase
// using Firebase;
// using Firebase.Database;
// using Firebase.Unity.Editor;

//WEBGL
using UnityEngine.Networking;

public class SubmitToDb : MonoBehaviour
{
    //Firebase
    // DatabaseReference reference;

    public static SubmitToDb instance;

    //WEBGL
    string url= "https://us-central1-edugame-7353d.cloudfunctions.net/mydatabase/data";
    public string dataa;

     void Start()
    {
        //Firebase
        // FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://edugame-7353d.firebaseio.com/");
        // reference = FirebaseDatabase.DefaultInstance.RootReference;
    }

    private void Awake() {
        instance = this;
    }
    
    public void QuitGame() {
        Application.Quit();
    }

    public void OnApplicationQuit() {
        string username = PlayerPrefs.GetString("Username");
        if(username=="default"){
            return;
        }
        string levelReached = PlayerPrefs.GetInt("levelReached", 0).ToString();
        string lastTimePlayed = PlayerPrefs.GetString("lastTimePlayed");
        string game = PlayerPrefs.GetString("gameName", "\"First\"");
        string timeOnStart = PlayerPrefs.GetString("TimeOnStart", Time.time.ToString("f3"));
        string timeNow = Time.time.ToString("f3");
        string firstScore = PlayerPrefs.GetString("firstScore","0");
        Debug.Log("Level reached: "+levelReached+" firstScore: "+firstScore);
        if(lastTimePlayed!="0" && lastTimePlayed!="1") {
            lastTimePlayed = lastTimePlayed.Substring(0,5);
        }
        //change toAdd
        float toAdd;
        float subTime = Convert.ToSingle(timeNow) - Convert.ToSingle(timeOnStart);
        if(subTime>0) {
            toAdd = Single.Parse(lastTimePlayed) + subTime;
        }
        else {
            toAdd = Single.Parse(lastTimePlayed);
        }
        
        Debug.Log("Time played "+lastTimePlayed);
        Debug.Log("Real time since startup played "+Convert.ToSingle(Time.realtimeSinceStartup));
        Debug.Log("Final time "+toAdd);

        //Firebase
        // reference.Child("users").Child(username).Child("levelReached").SetValueAsync(levelReached);
        // reference.Child("users").Child(username).Child("TimePlayed").SetValueAsync(toAdd.ToString());
        
        string data1 = "{\"hasPlayed\":\"" + PlayerPrefs.GetString("hasPlayed","0") + "\"," + "\"firstScore\":\"" + firstScore + "\"" + "," + "\"levelReached\":\"" + levelReached + "\"" + "," + "\"TimePlayed\":\"" + toAdd.ToString() + "\"}";
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
        // request.SetRequestHeader("Access-Control-Allow-Credentials", "true");
        // request.SetRequestHeader("Access-Control-Allow-Headers", "Accept, Content-Type, X-Access-Token, X-Application-Name, X-Request-Sent-Time");
        // request.SetRequestHeader("Access-Control-Allow-Methods", "GET, POST, PUT, OPTIONS");
        // request.SetRequestHeader("Access-Control-Allow-Origin", "*");
        // request.SetRequestHeader("Access-Control-Expose-Headers", "X-App-Signature");
        request.SetRequestHeader("Content-Type", "application/json");
        yield return request.SendWebRequest();
        Debug.Log("Status Code: " + request.responseCode);
        Debug.Log("Status body: " + request.downloadHandler.text);
    }
}
