using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class LAK_NetworkRoomManager : NetworkRoomManager 
{
    public UnityEvent OnClientListChange = new UnityEvent();
    public UnityEvent OnClientReadyStateChanged = new UnityEvent();
    // public UnityEvent OnPlayerColorSelection = new UnityEvent();
    


    public override void OnRoomClientEnter()
    {
        OnClientListChange.Invoke();
        Debug.Log("Client connected ");
    }

    public override void OnRoomClientExit()
    {
        OnClientListChange.Invoke();
        Debug.Log("Client Gela ree ");
    }

    public void AnyClientUpdate(LAK_NetworkRoomPlayer player)
    {
        //optimize logic later hence taking extra parameters
        OnClientReadyStateChanged.Invoke();
    }

    public override void ReadyStatusChanged()
    {
        int CurrentPlayers = 0;
        int ReadyPlayers = 0;

        foreach (NetworkRoomPlayer item in roomSlots)
        {
            if (item != null)
            {
                CurrentPlayers++;
                if (item.readyToBegin)
                    ReadyPlayers++;
            }
        }

        if (CurrentPlayers == ReadyPlayers)
            CheckReadyToBegin();
        else
            allPlayersReady = false;

        OnClientReadyStateChanged.Invoke();
    }

   
}
