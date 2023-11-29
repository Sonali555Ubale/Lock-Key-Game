using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;
using UnityEngine.UI;

public class NetworkRoomPlayerLobby : NetworkBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject LobbyUI = null;
    [SerializeField] private TMP_Text[] PlayerNameTxt = new TMP_Text[5];
    [SerializeField] private TMP_Text[] PlayerReadyTxt = new TMP_Text[5];
    [SerializeField] private Button BtnStartGame = null;

    [SyncVar(hook = nameof(HandleDisplayNameChanged))]
    public string DisplayName = "Loading...";
    [SyncVar(hook = nameof(HandleReadyStateChanged))]
    public bool isReady = false;

    private bool isDenner = true;

    public bool IsDenner
    {
        set
        {
            isDenner = value;
            BtnStartGame.gameObject.SetActive(value);
        }
    }


    private NetworkManagerLobby room;

    private NetworkManagerLobby Room
    {
        get
        {
            if (room != null) { return room; }
            return room = NetworkManager.singleton as NetworkManagerLobby;
        }
    }

    public override void OnStartAuthority()
    {
        CmdSetDisplayName(PlayerNameInput.DisplayName);    //set the display name of the player
        LobbyUI.SetActive(true);

    }
    public override void OnStartClient()
    {
        Room.RoomPlayer.Add(this);  //update room player list
        UpdateDisplay();
    }
    public override void OnStopClient()
    {
        Room.RoomPlayer.Remove(this);

        UpdateDisplay();
    }
       
    public void HandleReadyStateChanged(bool _, bool newVal) => UpdateDisplay();
    public void HandleDisplayNameChanged(string _, string newVal) => UpdateDisplay();

    private void UpdateDisplay()
    {
        if (!hasAuthority || !isOwned)                //hasAuthority is renamed as isOwnwd
        {
            foreach (var player in Room.RoomPlayer)        //if the player does not bolongs to us or does not have authority then check which players having authoryty and Update the DisplayName
            {
                if (player.isOwned || player.hasAuthority)
                {
                    player.UpdateDisplay();
                   // player.netIdentity.AssignClientAuthority(connectionToClient);
                    break;
                }
            }
            return;
        }

       for (int i=0; i< PlayerNameTxt.Length;i++)
        {
            PlayerNameTxt[i].text = DisplayName;
            PlayerReadyTxt[i].text = string.Empty;
        }

        for (int i = 0; i < Room.RoomPlayer.Count; i++)
        {
            PlayerNameTxt[i].text = Room.RoomPlayer[i].DisplayName;
            PlayerReadyTxt[i].text = Room.RoomPlayer[i].isReady ?
                "<color=green>Ready</color>" :
                "<color=red>Not Ready</color>";
        }
    }

    public void handleReadyToStart(bool readyToStart)
    {
        if (!isDenner) { return; }
        BtnStartGame.interactable = readyToStart;
    }

    [Command]
    public void CmdSetDisplayName(string displayName)
    {
        DisplayName = displayName;
    }

    [Command]
    public void CmdReadyUp()    //called when Ui Ready button is clicked
    {
        isReady = !isReady;   //on off toggle of ready btn
     
        Room.NotifyReadyState();
    }

    [Command]
    public void CmdStartGame()             //called when Ui start button clicked
    {
        if (Room.RoomPlayer[0].connectionToClient != connectionToClient) { return; } //if room player does not have authority then return else StartCoroutine game.
        Room.StartGame();
      
        Debug.Log(" Game Started...");

    }
}
