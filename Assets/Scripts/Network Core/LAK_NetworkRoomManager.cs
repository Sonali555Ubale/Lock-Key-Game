using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static UnityEditor.Progress;


public class LAK_NetworkRoomManager : NetworkRoomManager 
{
    // for UI
    public UnityEvent OnClientListChange = new UnityEvent();
    public UnityEvent OnClientReadyStateChanged = new UnityEvent();
    public UnityEvent OnPlayerColorSelection = new UnityEvent();

    public int indexVal = 0;
    /// <summary>
    /// ///// actual list of avalible colors
    /// </summary>
    public List<bool> ColorsAvaliblity = new List<bool> { true, true, true, true, true, true, true, true, true, true };

    public void UpdateColorVal(int index, bool val)
    {
        if (index >= 0 && index < 10)
        {
            ColorsAvaliblity[index] = val;
            indexVal = index;
            Debug.Log(indexVal + "val of idexVal is sent to UI");
        }


        // remove later
        for (int i = 0; i < 10; i++)
        {
            Debug.LogError("Color (" + i + ") " + ColorsAvaliblity[i]);
        }

        OnPlayerColorSelection.Invoke();

    }
   

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
