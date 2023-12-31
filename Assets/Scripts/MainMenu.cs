using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private NetworkManager networkManager;
    

    [Header("UI")]
    [SerializeField] private GameObject landingPagePanel;

    public void HostLobby()
    {
        networkManager.StartHost();
        landingPagePanel.SetActive(true);
    }
}
