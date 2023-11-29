using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class JoinLobbyMenu : MonoBehaviour
{
    [SerializeField]
    private NetworkManagerLobby networkManager = null;

    [Header("UI")]
    [SerializeField]
    private TMP_InputField ipAddressInputField = null;
    [SerializeField]
    private Button joinBtn;    
    [SerializeField] private GameObject landingPagePanel = null;


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
    }

    private void HandleClientConnected()
    {
        joinBtn.interactable = true;

        gameObject.SetActive(false);
        landingPagePanel.SetActive(false);
    }
}
