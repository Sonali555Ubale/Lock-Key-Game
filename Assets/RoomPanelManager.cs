using Mirror;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEditor.EditorTools;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class RoomPanelManager : NetworkBehaviour
{

    NetworkRoomPlayer CurrentNetworkPlayer;

    [SerializeField]
    NetworkRoomManager RoomManager = null;

    [SerializeField] Button ReadyButton;
    [SerializeField] Button StartButton;
    [SerializeField] Button CancelButton;
    public PlayerRoomUIManager PRUM;


    private void OnEnable()
    {
        if (RoomManager == null) RoomManager = (NetworkRoomManager)FindObjectOfType(typeof(NetworkRoomManager));
       
    }

    public void OnPlayerReady()
    {
        if (isServer )
            StartButton.gameObject.SetActive(true);
        ReadyButton.interactable = false;
        CancelButton.gameObject.SetActive(true);

        if (CurrentNetworkPlayer = null)
        {
            FindNetworkRoomPlayer()?.CmdChangeReadyStateOfPlayer(true);
           
        }
        else
        {
            CurrentNetworkPlayer?.CmdChangeReadyStateOfPlayer(true);
           
        }
    }

    public void OnPlayerCancel()
    {
        if (isServer)
            StartButton.gameObject.SetActive(false);
        ReadyButton.interactable = true;
        CancelButton.gameObject.SetActive(false);
        ReadyButton.gameObject.SetActive(true);

        if (CurrentNetworkPlayer = null)
        {
            FindNetworkRoomPlayer()?.CmdChangeReadyStateOfPlayer(false);
          
        }
        else
        {
            CurrentNetworkPlayer?.CmdChangeReadyStateOfPlayer(false);
          
        }
    }

    NetworkRoomPlayer FindNetworkRoomPlayer()
    {
        var list = Resources.FindObjectsOfTypeAll(typeof(NetworkRoomPlayer));

        foreach (var player in list)
        {
            var temp = player as NetworkRoomPlayer;
            Debug.Log(temp.index);
            if (temp.isLocalPlayer)
            {
                CurrentNetworkPlayer = temp;
                return CurrentNetworkPlayer;
            }
        }

        return null;
    }
}
