using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using System;

public class SubmitToDb : MonoBehaviour
{
    DatabaseReference reference;

     void Start()
    {
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://edugame-7353d.firebaseio.com/");
        reference = FirebaseDatabase.DefaultInstance.RootReference;
    }

    public void OnApplicationQuit() {
        string username = PlayerPrefs.GetString("Username");
        int levelReached = PlayerPrefs.GetInt("levelReached", 0);
        string lastTimePlayed = PlayerPrefs.GetString("lastTimePlayed");
        if(lastTimePlayed!="0") {
            lastTimePlayed = lastTimePlayed.Substring(0,6);
        }
        float toAdd = Single.Parse(lastTimePlayed) + Convert.ToSingle(Time.realtimeSinceStartup);
        Debug.Log("Time played "+lastTimePlayed);
        Debug.Log("Real time since startup played "+Convert.ToSingle(Time.realtimeSinceStartup));
        Debug.Log("Final time "+toAdd);
        reference.Child("users").Child(username).Child("levelReached").SetValueAsync(levelReached);
        reference.Child("users").Child(username).Child("TimePlayed").SetValueAsync(toAdd.ToString());
        PlayerPrefs.SetString("Username","default");
    }
}
