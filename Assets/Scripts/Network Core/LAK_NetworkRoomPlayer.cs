using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LAK_NetworkRoomPlayer : NetworkRoomPlayer
{
 
    [Tooltip("Diagnostic Player name")]
    [SyncVar(hook = nameof(PlayerNameUpdate))]
    public string playerName;

    public UnityEvent OnClientReadyStateChange = new UnityEvent();

   

    public void PlayerNameUpdate(string oldName, string newName)
    {
        playerName = newName;
    }

   
}

