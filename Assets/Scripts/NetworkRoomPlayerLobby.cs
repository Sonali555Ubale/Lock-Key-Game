using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;
using UnityEngine.UI;

public class NetworkRoomPlayerLobby : NetworkBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject LobbyUI;
    [SerializeField] private TMP_Text[] PlayerNameTxt = new TMP_Text[5];
    [SerializeField] private TMP_Text[] PlayerReadyTxt = new TMP_Text[5];
    [SerializeField] private Button BtnStartGame;

    [SyncVar(hook = nameof(HandleDisplayNameChanged))]
    public string DisplayName = "Loading...";
    [SyncVar(hook = nameof(HandleDisplayColorChanged))]
    public Color DisplayColor = new Color();
    [SyncVar(hook = nameof(HandleReadyStateChanged))]
    public bool isReady = false;

    private bool isDenner = false;

    public bool IsDenner
    {
        set
        {
            if (isServer) isDenner = true;
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
        CmdSetDisplayColor(ColorSelectionUI.DisplayColor);
        LobbyUI.SetActive(true);

    }
   /* public override void OnStartClient()
    {
        Room.RoomPlayers.Add(this);  //update room player list
        UpdateDisplay();
    }*/
   /* public override void OnStopClient()
    {
        Room.RoomPlayers.Remove(this);

        UpdateDisplay();
    }*/
       
    public void HandleReadyStateChanged(bool _, bool newVal) => UpdateDisplay();
    public void HandleDisplayNameChanged(string _, string newVal) => UpdateDisplay();
    public void HandleDisplayColorChanged(Color _, Color newVal) => UpdateDisplay();
    private void UpdateDisplay()
    {
        if (!isOwned )                //hasAuthority is renamed as isOwnwd
        {
            foreach (var player in Room.RoomPlayers)    //if the player does not bolongs to us or does not have authority then check which players having authoryty and Update the DisplayName
            {
                if (player.isOwned )
                {
                  
                    player.UpdateDisplay();
                    player.netIdentity.AssignClientAuthority(connectionToClient);
                    break;
                }
            }
            return;
        }

       for (int i=0; i< PlayerNameTxt.Length;i++)
        {
            PlayerNameTxt[i].text = DisplayName;
            PlayerReadyTxt[i].text = "Not Ready";
        }

        for (int i = 0; i < Room.RoomPlayers.Count; i++)
        {
            if (Room.RoomPlayers[i] != null)
            {
                PlayerNameTxt[i].text = Room.RoomPlayers[i].DisplayName;
                PlayerReadyTxt[i].text = Room.RoomPlayers[i].isReady ? "<color=green>Ready</color>" : "<color=red>Not Ready</color>";
            }
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
    public void CmdSetDisplayColor(Color displayColor)
    {
        DisplayColor = displayColor;
    }

    public void OnReadyButtonClick()
    {
        isReady = true;
        CmdReadyUp(isReady);
    }

    [Command]
    private void CmdReadyUp(bool isReady)    //called when Ui Ready button is clicked
    {
      //  isReady = isReady;
       // isReady = !isReady;   //on off toggle of ready btn
        Room.NotifyReadyState();
        Debug.Log("Player Readyyyyy!!!!");
       

          NetworkRoomManager room = NetworkManager.singleton as NetworkRoomManager;
        if (room != null)
        {
            room.ReadyStatusChanged();
        }
    }

    [Command]
    public void CmdStartGame()             //called when Ui start button clicked
    {
        if (Room.RoomPlayers[0].connectionToClient != connectionToClient) { return; } //if room player does not have authority then return else Start game.
       
        Room.StartGame();
              
        Debug.Log(" Game Started...");

    }
}
