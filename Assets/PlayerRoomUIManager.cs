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
    [SerializeField]
    NetworkRoomManager RoomManager = null;
    bool Processing = false;
   

    public static PlayerRoomUIManager instance = null;


    private void OnEnable()
    {
        if (RoomManager == null) RoomManager = (NetworkRoomManager)FindObjectOfType(typeof(NetworkRoomManager));
        //OnPLayerChange();
    }


    private void Update()
    {
        //if (CurrentPlayersCount != RoomManager.roomSlots.Count && !Processing)
        //{
        //OnPLayerChange();
        //}

        // Loop over the roomSlots
        foreach (var item in RoomManager.roomSlots)
        {
            var layoutChildrenCount = VerticalLayoutObject.transform.childCount;
            if (layoutChildrenCount == 0)
            {
                    var playerData = item;
                    GameObject Element = Instantiate(PlayerRoomTitleElement, VerticalLayoutObject == null ? this.transform : VerticalLayoutObject.transform);
                    var playerInLayout = VerticalLayoutObject.transform.GetChild(0).gameObject;

                    TextMeshProUGUI[] list = Element.GetComponentsInChildren<TextMeshProUGUI>();
                    //list[0].text = PlayerNameInput.DisplayName;
                    list[0].text = playerInLayout.GetComponent<PlayerFloatingName>().playerName;
                    //list[1].text = playerInLayout.GetComponent<PlayerFloatingName>().readystatus ? "Ready" : "Not Ready";
                    PlayerRoomTitleElementList.Add(Element);
            }
            for (int i = 0; i < layoutChildrenCount; i++)
            {
                var playerInLayout = VerticalLayoutObject.transform.GetChild(i).gameObject;
                var playerData = RoomManager.roomSlots.Find(t => t.netId == playerInLayout.GetComponent<NetworkRoomPlayer>().netId);
                if (playerData == null)
                {
                    GameObject Element = Instantiate(PlayerRoomTitleElement, VerticalLayoutObject == null ? this.transform : VerticalLayoutObject.transform);

                    TextMeshProUGUI[] list = Element.GetComponentsInChildren<TextMeshProUGUI>();
                    // list[0].text = PlayerNameInput.DisplayName;
                    list[0].text = playerInLayout.GetComponent<PlayerFloatingName>().playerName;
                    //list[1].text = playerInLayout.GetComponent<PlayerFloatingName>().readystatus ? "Ready" : "Not Ready";
                    PlayerRoomTitleElementList.Add(Element);
                } else
                {
                    // Delete it from the UI

                }
            }
        }

    }

    /*
        private void OnPLayerChange()
        {
            Processing = true;
            foreach (var player in RoomManager.roomSlots)
            {
                if (!NWRoomPlayerList.Contains(player)) { 
                    NWRoomPlayerList.Add(player);
                    CurrentPlayersCount++;
                }
            }
            SetPlayerTable();
        }*/



   /* private void SetPlayerTable()
    {
        PlayerRoomTitleElementList.Clear();
        foreach (Transform child in VerticalLayoutObject.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        foreach (var player in NWRoomPlayerList)
        {
            AddPlayer(player.index, player.name, player.readyToBegin);
        }

        Processing = false;
    }*/

    /*public void AddPlayer(int index, string playername, bool readystatus)
    {
        GameObject Element = Instantiate(PlayerRoomTitleElement, VerticalLayoutObject == null ? this.transform : VerticalLayoutObject.transform);

        TextMeshProUGUI[] list = Element.GetComponentsInChildren<TextMeshProUGUI>();
        // list[0].text = PlayerNameInput.DisplayName;
        list[0].text = "Player[" + index + "]";
        list[1].text = readystatus ? "Ready" : "Not Ready";
        PlayerRoomTitleElementList.Add(Element);

    }*/

}
