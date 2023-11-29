using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class PlayerNameInput : MonoBehaviour
{
    [Header("UI")]
    [SerializeField]
    private TMP_InputField playerNameInputField = null;
    [SerializeField]
    private Button SubmitBtn = null;

    public static string DisplayName { get; private set; }
    private const string PlayerPrefsNameKey = "PlayerName";

    private void Start()
    {
        SetInputField();
    }

    private void SetInputField()   //if no name is saved in player pref then return else get the playerName from input field and set
    {
        if (!PlayerPrefs.HasKey(PlayerPrefsNameKey)) return; 

        string PlayerName = PlayerPrefs.GetString(PlayerPrefsNameKey);
        playerNameInputField.text = PlayerName;
        SetPlayerName(PlayerName);
    }

    public void SetPlayerName(string playerName)
    {
        SubmitBtn.interactable = !string.IsNullOrEmpty(name);
    }

    public void SavePlayeraname()
    {
        DisplayName = playerNameInputField.text;
        PlayerPrefs.SetString(PlayerPrefsNameKey, DisplayName);
    }
}