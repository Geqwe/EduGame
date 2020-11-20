using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

//Firebase
// using Firebase;
// using Firebase.Database;
// using Firebase.Unity.Editor;

//WEBGL
using UnityEngine.Networking;

public class InfoSubmit : MonoBehaviour
{
    public static InfoSubmit instance;


    public TMP_InputField firstName;
    public TMP_InputField lastName;
    public TMP_InputField classroom;
    public TMP_InputField age;
    public TMP_InputField school;
    public TMP_InputField city;
    public GameObject menu;
    public AudioClip startGame;

    AudioSource audio;

    //Firebase
    // DatabaseReference reference;

    //WEBGL
    string url= "https://us-central1-edugame-7353d.cloudfunctions.net/mydatabase/data";
    public string dataa;

    void Start()
    {
        //Firebase
        // FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://edugame-7353d.firebaseio.com/");
        // reference = FirebaseDatabase.DefaultInstance.RootReference;

        audio = GetComponent<AudioSource>();
    }

    private void Awake() {
        instance = this;
    }

    public void SubmitInfo() {
        string username = PlayerPrefs.GetString("Username");
        int levelReached = PlayerPrefs.GetInt("levelReached", 0);
        string lastTimePlayed = PlayerPrefs.GetString("lastTimePlayed");
        string game = PlayerPrefs.GetString("gameName", "\"default\"");

        //Firebase
        // reference.Child("users").Child(username).Child("levelReached").SetValueAsync(levelReached);
        // reference.Child("users").Child(username).Child("TimePlayed").SetValueAsync(lastTimePlayed);
        // reference.Child("users").Child(username).Child("firstName").SetValueAsync(firstName.text);
        // reference.Child("users").Child(username).Child("lastName").SetValueAsync(lastName.text);
        // reference.Child("users").Child(username).Child("classroom").SetValueAsync(classroom.text);
        // reference.Child("users").Child(username).Child("age").SetValueAsync(age.text);
        // reference.Child("users").Child(username).Child("school").SetValueAsync(school.text);
        // reference.Child("users").Child(username).Child("city").SetValueAsync(city.text);
        // reference.Child("users").Child(username).Child("hasPlayed").SetValueAsync("0");
        
        //WEBGL
        string option = "save";
        string data = "{ \"firstName\":\"" + firstName.text.ToString() + "\"," + "\"lastName\":\"" + lastName.text.ToString() + "\"," + "\"classroom\":\"" + classroom.text.ToString() + "\"," + "\"age\":\"" + age.text.ToString() + "\"," + "\"school\":\"" + school.text.ToString() + "\"," + "\"city\":\"" + city.text.ToString() + "\","+game+":{\"levelReached\":\"" + "0" + "\"," +  "\"firstScore\":\"" + "0" + "\"," + "\"hasPlayed\":\"" + "0" + "\"," + "\"TimePlayed\":\"" + "0" + "\"}}";
        
        dataa = "{\"option\":\"" + option + "\",\"username\":\"" + username + "\"," + "\"game\":" + game + ",\"data\":" + data + "}";
        Debug.Log(dataa);
        StartCoroutine(Upload());

        audio.PlayOneShot(startGame);
        menu.SetActive(false);
    }

    public void StartNewGame() {
        string username = PlayerPrefs.GetString("Username");
        string game = PlayerPrefs.GetString("gameName", "\"default\"");

        string data1 = "{\"hasPlayed\":\"" + "0" + "\"," + "\"firstScore\":\"" + "0" + "\"" + "," + "\"levelReached\":\"" + "0" + "\"" + "," + "\"TimePlayed\":\"" + "0" + "\"}";
        string option2 = "save2";
        dataa = "{\"option\":\"" + option2 + "\",\"username\":\"" + username + "\" ,\"data\":" + data1 + ",\"path\":" + game + "}";
        Debug.Log(dataa);
        StartCoroutine(Upload());
    }

    IEnumerator Upload()
    {
        Debug.Log("here");
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
