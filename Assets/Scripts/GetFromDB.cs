using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using System;
using TMPro;

public class GetFromDB : MonoBehaviour
{
    DatabaseReference reference;
    public GameObject storeValuesMenu;
    public GameObject putUserNameMenu;
    int checkDB = 0;
    string username;
    public TMP_InputField inputUsername;
    string levelReached;
    string lastTimePlayed;
    AudioSource audio;
    public AudioClip startClip;
    bool checkPlay = false;

    void Start()
    {
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://edugame-7353d.firebaseio.com/");
        reference = FirebaseDatabase.DefaultInstance.RootReference;
        audio = GetComponent<AudioSource>();
    }

    public void SubmitUsername() {
        loadData();
    }

    private void CheckHere() {
        if(checkPlay==true) 
            return;

        if(checkDB == 1) {
            PlayerPrefs.SetString("Username", username);
            PlayerPrefs.SetInt("levelReached", int.Parse(levelReached));
            PlayerPrefs.SetString("lastTimePlayed", lastTimePlayed.ToString());
            putUserNameMenu.SetActive(false);
            audio.PlayOneShot(startClip);
            Debug.Log("username is "+username+" time played "+lastTimePlayed);
            checkPlay = true;
        }
        else if(checkDB == 2) {
            PlayerPrefs.SetString("Username", username);
            PlayerPrefs.SetInt("levelReached", 0);
            PlayerPrefs.SetString("lastTimePlayed", "0");
            checkPlay = true;
            putUserNameMenu.SetActive(false);
            storeValuesMenu.SetActive(true);
        }
    }

    public void loadData() {
        Debug.Log(inputUsername.text);
        var getTask = FirebaseDatabase.DefaultInstance.GetReference("users").Child(inputUsername.text).GetValueAsync().ContinueWith(task => {
            if (task.IsCompleted) {
                if(task.Result.Exists) {
                    lastTimePlayed = task.Result.Child("TimePlayed").Value.ToString();
                    username = inputUsername.text;
                    levelReached = task.Result.Child("levelReached").Value.ToString();
                    checkDB = 1;
                }
                else{
                    Debug.Log("dont exist");
                    username = inputUsername.text;
                    checkDB = 2;
                }
            }
            else {
                storeValuesMenu.SetActive(true);
            }
        });
        InvokeRepeating("CheckHere",1f,1f);
    }
}
