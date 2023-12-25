using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.IO.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEditor.EditorTools;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Rendering;
using static UnityEditor.Rendering.FilterWindow;


public class PlayerRoomUIManager : MonoBehaviour
{
    [SerializeField]
    GameObject PlayerRoomTitleElement;
    [SerializeField]
    List<GameObject> PlayerRoomTitleElementList;
    [SerializeField]
    GameObject VerticalLayoutObject;
    List<NetworkRoomPlayer> NWRoomPlayerList = new List<NetworkRoomPlayer>();
    
    [SerializeField]
    int CurrentPlayersCount = 0;
    [SerializeField]
    LAK_NetworkRoomManager RoomManager = null;

    private static PlayerRoomUIManager instance = null;


    private void OnEnable()
    {
        if (RoomManager == null) RoomManager = (LAK_NetworkRoomManager)FindObjectOfType(typeof(LAK_NetworkRoomManager));


        // Listen  to event of change in client list
        RoomManager.OnClientListChange.AddListener(OnPLayerChange);
    }

    //resets UI here we are simply reseting the entire Ui a burte force method we change it to be more optimized later
    private void OnPLayerChange()
    {
 
        NWRoomPlayerList.Clear();
        foreach (var player in RoomManager.roomSlots)
        {
           NWRoomPlayerList.Add(player);
           CurrentPlayersCount++;
        }
        ResetPlayerTableUI();
    }

    private void ResetPlayerTableUI()
    {
        foreach (var child in PlayerRoomTitleElementList)
        {
            GameObject.Destroy(child);
        }

        //simply delete all 
        // simple add all current clients  // we can change it later to only delete specific

        foreach (var player in NWRoomPlayerList)
        {
            string pName = PlayerNameInput.DisplayName;
            AddPlayer(player.index, pName, player.readyToBegin);
        }


    }

    public void AddPlayer(int index, string playername, bool readystatus)
    {
        GameObject Element = Instantiate(PlayerRoomTitleElement, VerticalLayoutObject == null ? this.transform : VerticalLayoutObject.transform);

        TextMeshProUGUI[] list = Element.GetComponentsInChildren<TextMeshProUGUI>();
        list[0].text = index + " " + playername;
        list[1].text = readystatus ? "Ready" : "Not Ready";
        
        PlayerRoomTitleElementList.Add(Element);



    }



}
