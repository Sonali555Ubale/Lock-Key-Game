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
    NetworkRoomManager RoomManager = null;
    bool Processing = false;

    private static PlayerRoomUIManager instance = null;


    private void OnEnable()
    {
        if (RoomManager == null) RoomManager = (NetworkRoomManager)FindObjectOfType(typeof(NetworkRoomManager));
        OnPLayerChange();
    }


    private void Update()
    {
        if(CurrentPlayersCount < RoomManager.roomSlots.Count && !Processing)
        {
            OnPLayerChange();
        }
    }


    private void OnPLayerChange()
    {
        Processing = true;
        foreach (var obj in RoomManager.roomSlots)
        {
            if (!NWRoomPlayerList.Contains(obj)) { 
                NWRoomPlayerList.Add(obj);
                CurrentPlayersCount++;
            }
        }
        SetPlayerTable();
    }

    private void SetPlayerTable()
    {
        PlayerRoomTitleElementList.Clear();
        foreach (Transform child in VerticalLayoutObject.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        foreach (var obj in NWRoomPlayerList)
        {
            AddPlayer(obj.index, obj.name, obj.readyToBegin);
        }

        Processing = false;
    }

    public void AddPlayer(int index, string playername, bool readystatus)
    {
        GameObject Element = Instantiate(PlayerRoomTitleElement, VerticalLayoutObject == null ? this.transform : VerticalLayoutObject.transform); 
        
        TextMeshProUGUI[] list = Element.GetComponentsInChildren<TextMeshProUGUI>();
        list[0].text = "Player [ " + (index + 1) + " ]";
        list[1].text = readystatus ? "Ready" : "Not Ready";
        PlayerRoomTitleElementList.Add(Element);
    }

}
