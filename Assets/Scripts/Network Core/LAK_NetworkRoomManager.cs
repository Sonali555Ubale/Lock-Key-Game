using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LAK_NetworkRoomManager : NetworkRoomManager
{
    public UnityEvent OnClientListChange = new UnityEvent();

    public override void OnRoomClientEnter()
    {
        OnClientListChange.Invoke();
        Debug.Log("Client Ala ree ");
    }

    public override void OnRoomClientExit()
    {
        OnClientListChange.Invoke();
        Debug.Log("Client Gela ree ");
    }
}
