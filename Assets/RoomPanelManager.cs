using Mirror;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEditor.EditorTools;
using UnityEngine;
using UnityEngine.UI;


public class RoomPanelManager : NetworkBehaviour
{

    LAK_NetworkRoomPlayer CurrentNetworkPlayer;
    LAK_NetworkRoomManager RoomManager = null;
    LAK_NetworkRoomPlayer CurrentPlayer;
    PlayerRoomUIManager playerRoomUIManager;


    [SerializeField] Button ReadyButton;
    [SerializeField] Button StartButton;
    [SerializeField] Button CancelButton;


    private bool ReadyState = false;
    private void OnEnable()
    {
        if (RoomManager == null) RoomManager = (LAK_NetworkRoomManager)FindObjectOfType(typeof(LAK_NetworkRoomManager));
        if (CurrentPlayer == null) CurrentPlayer = (LAK_NetworkRoomPlayer)FindObjectOfType(typeof(LAK_NetworkRoomPlayer));
        if(playerRoomUIManager==null) playerRoomUIManager = (PlayerRoomUIManager)FindObjectOfType(typeof(PlayerRoomUIManager));

    }
    public void Update()
    {
         if(RoomManager?.allPlayersReady == true && isServer) 
            StartButton.gameObject.SetActive(true);
    }

  
    public void OnPlayerReady()
    {
        if (CurrentNetworkPlayer == null)
            FindNetworkRoomPlayer()?.CmdChangeReadyState(true);
          else
            CurrentNetworkPlayer?.CmdChangeReadyState(true);
        Debug.Log("OnPlayerReady Called::");


        CancelButton.gameObject.SetActive(true);
        CancelButton.interactable = true;
        ReadyButton.interactable = false;
        ReadyButton.gameObject.SetActive(false);
       // playerRoomUIManager.ResetPlayerTableUI();

        /* if (isServer )
             StartButton.gameObject.SetActive(true);

         
         ReadyButton.interactable = false;
         CancelButton.gameObject.SetActive(true);

         if (CurrentNetworkPlayer = null)
         {
             FindNetworkRoomPlayer()?.CmdChangeReadyStateOfPlayer(ReadyState);
         }
         else
         {
             CurrentNetworkPlayer?.CmdChangeReadyStateOfPlayer(ReadyState);

         }*/
    }

        public void OnPlayerCancel()
    {
        /*if (isServer)
            StartButton.gameObject.SetActive(false);

        ReadyState = false;
        ReadyButton.interactable = true;
        CancelButton.gameObject.SetActive(false);
        ReadyButton.gameObject.SetActive(true);*/

        if (CurrentNetworkPlayer == null)
              FindNetworkRoomPlayer()?.CmdChangeReadyState(false);
        else
            CurrentNetworkPlayer?.CmdChangeReadyState(false);

        Debug.Log("OnPlayerCancle Called::");

        CancelButton.interactable = false;
        CancelButton.gameObject.SetActive(false);
        ReadyButton.gameObject.SetActive(true);
        ReadyButton.interactable = true;
       // playerRoomUIManager.ResetPlayerTableUI();
    }



    LAK_NetworkRoomPlayer FindNetworkRoomPlayer()
    {
        var list = Resources.FindObjectsOfTypeAll(typeof(LAK_NetworkRoomPlayer));

        foreach (var player in list)
        {
            var temp = player as LAK_NetworkRoomPlayer;
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
