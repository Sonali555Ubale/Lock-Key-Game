using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class JoinLobbyMenu : MonoBehaviour
{
    [SerializeField]
    private NetworkManagerLobby networkManager;
    [SerializeField]
    private TMP_InputField ipAddressInputField;
    [SerializeField]
    private Button joinBtn;

    [Header("UI")]
    [SerializeField] private GameObject landingPagePanel;


    private void OnEnable()
    {
        NetworkManagerLobby.OnClientConnected += HandleClientConnected;
        NetworkManagerLobby.OnClientDisconnected += HandleClientDisconnected;
    }

    private void OnDisable()
    {
        NetworkManagerLobby.OnClientConnected -= HandleClientConnected;
        NetworkManagerLobby.OnClientDisconnected -= HandleClientDisconnected;
    }

    public void JoinLobby()
    {
        string ipAddress = ipAddressInputField.text;

        networkManager.networkAddress = ipAddress;
        networkManager.StartClient();
        joinBtn.interactable = false;
    }

    private void HandleClientDisconnected()
    {
        joinBtn.interactable = true;
        gameObject.SetActive(false);
        landingPagePanel.SetActive(false);
    }

    private void HandleClientConnected()
    {
        joinBtn.interactable = true;
    }
}
