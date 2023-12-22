using Mirror;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEditor.EditorTools;
using UnityEngine;

public class RoomPanelManager : MonoBehaviour
{

    NetworkRoomPlayer CurrentNetworkPlayer;

    [SerializeField]
    NetworkRoomManager RoomManager = null;

    private void OnEnable()
    {
        if (RoomManager == null) RoomManager = (NetworkRoomManager)FindObjectOfType(typeof(NetworkRoomManager));
       
    }

    public void OnPlayerReady()
    {
        if (CurrentNetworkPlayer = null)
        {
            FindNetworkRoomPlayer()?.CmdChangeReadyStateOfPlayer(true);
        }
        else
        {
            CurrentNetworkPlayer?.CmdChangeReadyStateOfPlayer(true);
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
