using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class UserInput : MonoBehaviour
{

    public TMP_InputField usernameInputField;
    public Button ConfirmButton;
   
    void Start()
    {   
        ConfirmButton.onClick.AddListener(InputUser);
    }

    private void InputUser(){
        string username = usernameInputField.text;
        if (!string.IsNullOrEmpty(username)){
            Debug.Log("Username set to:" + username);
            PlayerPrefs.SetString("Username", username);
            SceneManager.LoadScene("PlayScene");
        }
        else{
            Debug.Log("Username is empty");
        }
    }
    void Update(){
          if (Input.GetKeyDown(KeyCode.Escape)){
            SceneManager.LoadScene("MenuScene");
        }
    }
    
}
