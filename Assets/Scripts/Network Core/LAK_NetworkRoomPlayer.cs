using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.EditorTools;
using UnityEngine;
using UnityEngine.Events;

public class LAK_NetworkRoomPlayer : NetworkRoomPlayer
{
 
    [Tooltip("Diagnostic Player name")]
    [SyncVar(hook = nameof(PlayerNameUpdate))]
    public string playerName;

    public UnityEvent OnClientReadyStateChange = new UnityEvent();

    public override void ReadyStateChanged(bool oldReadyState, bool newReadyState)
    {
        var onj = (LAK_NetworkRoomManager)FindObjectOfType(typeof(LAK_NetworkRoomManager));
        onj.ClientStatusUpdate(this, oldReadyState,newReadyState); 
        OnClientReadyStateChange.Invoke();
    }

    public void PlayerNameUpdate(string oldName, string newName)
    {
        playerName = newName;
    }

   
}

