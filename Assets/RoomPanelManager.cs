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

    LAK_NetworkRoomPlayer CurrentNetworkPlayer;

    [SerializeField]
    LAK_NetworkRoomManager RoomManager = null;

    [SerializeField] Button ReadyButton;
    [SerializeField] Button StartButton;
    [SerializeField] Button CancelButton;
    LAK_NetworkRoomPlayer CurrentPlayer;
    private bool ReadyState = false;
    private void OnEnable()
    {
        if (RoomManager == null) RoomManager = (LAK_NetworkRoomManager)FindObjectOfType(typeof(LAK_NetworkRoomManager));
        if (CurrentPlayer == null) CurrentPlayer = (LAK_NetworkRoomPlayer)FindObjectOfType(typeof(LAK_NetworkRoomPlayer));

        
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

        ReadyButton.interactable = false;
        CancelButton.gameObject.SetActive(true);

       

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

        if (CurrentNetworkPlayer = null)
              FindNetworkRoomPlayer()?.CmdChangeReadyState(false);
        else
            CurrentNetworkPlayer?.CmdChangeReadyState(false);


        ReadyButton.interactable = true;
        CancelButton.gameObject.SetActive(false);
        ReadyButton.gameObject.SetActive(true);

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
