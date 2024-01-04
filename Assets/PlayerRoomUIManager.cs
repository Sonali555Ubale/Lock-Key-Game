using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerRoomUIManager : MonoBehaviour
{
    [SerializeField]
    GameObject PlayerRoomTitleElement;
    [SerializeField]
    List<GameObject> PlayerRoomTitleElementList;
    [SerializeField]
    GameObject VerticalLayoutObject;
    List<LAK_NetworkRoomPlayer> NWRoomPlayerList = new List<LAK_NetworkRoomPlayer>();

    [SerializeField]
    int CurrentPlayersCount = 0;
    [SerializeField]
    LAK_NetworkRoomManager RoomManager = null;

    private static PlayerRoomUIManager instance = null;
    public string Pname;
    public Color PColor;
    public Image PreviewImg;


    private void Start()
    {
        PreviewImg.color = Color.gray;
    }
    private void OnEnable()
    {
        if (RoomManager == null) RoomManager = (LAK_NetworkRoomManager)FindObjectOfType(typeof(LAK_NetworkRoomManager));


        Pname = PlayerNameInput.DisplayName;
       // PColor = ColorSelectionUI.DisplayColor;


        // Listen  to event of change in client list
        RoomManager.OnClientListChange.AddListener(OnPlayerChange);
        RoomManager.OnClientReadyStateChanged.AddListener(OnPlayerReadyStateChange);
      //  RoomManager.OnPlayerColorSelectionEmogy.AddListener(UpdatePlayerUIList);


    }


    private void OnPlayerReadyStateChange()
    {
        ResetPlayerTableUI();
    }

    private void UpdatePlayerUIList()
    {
        PreviewImg.color = PColor;
        Debug.Log("PreviwImage taggg:" + PreviewImg.name);
        ResetPlayerTableUI();
    }
    //resets UI here we are simply reseting the entire Ui a burte force method we change it to be more optimized later
    private void OnPlayerChange()
    {
        NWRoomPlayerList.Clear();
        foreach (var player in RoomManager.roomSlots)
        {
            var nwPlayer = player as LAK_NetworkRoomPlayer;
            if (nwPlayer != null)
            {
                NWRoomPlayerList.Add(nwPlayer);
            }
        }
        ResetPlayerTableUI();
    }


    public void ResetPlayerTableUI()
    {
        // Clear existing UI elements
        foreach (var child in PlayerRoomTitleElementList)
        {
            Destroy(child);
        }
        PlayerRoomTitleElementList.Clear();

        // Add all current players to the UI
        foreach (var player in NWRoomPlayerList)
        {
            AddPlayer(player.index, player.DisplayName, player.readyToBegin, player.DisplayColor);
        }
    }


    public void AddPlayer(int index, string playername, bool readystatus, Color playerColor)
    {
        GameObject element = Instantiate(PlayerRoomTitleElement, VerticalLayoutObject != null ? VerticalLayoutObject.transform : transform);

        TextMeshProUGUI[] list = element.GetComponentsInChildren<TextMeshProUGUI>();
        list[0].text = playername;
        list[1].text = readystatus ? "Ready" : "Not Ready";

        // Find the PreviewImage and set its color
        Transform childTransform =element.transform.Find("Preview_Image");
         Image previewImg = childTransform.GetComponent<Image>();
        if (PreviewImg != null)
        {
            PreviewImg.color = playerColor;
            previewImg.color = playerColor;

            Debug.Log("PreviewImage NAmr**" +PreviewImg.name);
            Debug.Log(" smallllPreviewImage NAmr**" +previewImg.name);
        }

        PlayerRoomTitleElementList.Add(element);
    }

    /* Transform childTransform = PlayerRoomTitleElement.transform.Find("Preview_Image");
       if (childTransform != null)
       {
           PreviewImg = childTransform.GetComponent<Image>();
           if (PreviewImg != null)
           {
               PreviewImg.color = PlayerColor;
           }
       }*/


}
