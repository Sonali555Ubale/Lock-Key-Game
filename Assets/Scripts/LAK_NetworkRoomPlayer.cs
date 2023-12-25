using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LAK_NetworkRoomPlayer : NetworkRoomPlayer
{
    [Tooltip("Diagnostic flag indicating whether this player is ready for the game to begin")]
    [SyncVar(hook = nameof(ReadyStateChanged))]
    public bool readyToStart;
    [Tooltip("Diagnostic Player name")]
    [SyncVar(hook = nameof(PlayerNameUpdate))]
    public string playerName;

   public override void ReadyStateChanged(bool oldReadyState, bool newReadyState) 
    {
        readyToStart = newReadyState;
    }

    public void PlayerNameUpdate(string oldName, string newName)
    {
        playerName = newName;
    }
}
