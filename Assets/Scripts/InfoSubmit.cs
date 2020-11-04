using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;

public class InfoSubmit : MonoBehaviour
{
    public TMP_InputField firstName;
    public TMP_InputField lastName;
    public TMP_InputField classroom;
    public TMP_InputField age;
    public TMP_InputField school;
    public TMP_InputField city;
    public GameObject menu;
    public AudioClip startGame;

    AudioSource audio;
    DatabaseReference reference;

    void Start()
    {
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://edugame-7353d.firebaseio.com/");
        reference = FirebaseDatabase.DefaultInstance.RootReference;
        audio = GetComponent<AudioSource>();
    }

    public void SubmitInfo() {
        string username = PlayerPrefs.GetString("Username");
        int levelReached = PlayerPrefs.GetInt("levelReached", 0);
        string lastTimePlayed = PlayerPrefs.GetString("lastTimePlayed");

        reference.Child("users").Child(username).Child("levelReached").SetValueAsync(levelReached);
        reference.Child("users").Child(username).Child("TimePlayed").SetValueAsync(lastTimePlayed);
        reference.Child("users").Child(username).Child("firstName").SetValueAsync(firstName.text);
        reference.Child("users").Child(username).Child("lastName").SetValueAsync(lastName.text);
        reference.Child("users").Child(username).Child("classroom").SetValueAsync(classroom.text);
        reference.Child("users").Child(username).Child("age").SetValueAsync(age.text);
        reference.Child("users").Child(username).Child("school").SetValueAsync(school.text);
        reference.Child("users").Child(username).Child("city").SetValueAsync(city.text);

        audio.PlayOneShot(startGame);
        menu.SetActive(false);
    }
}
