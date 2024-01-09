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


    private static PlayerRoomUIManager instance;
    public string Pname;
    public Color PColor;
    public Image PreviewImg;

    public static PlayerRoomUIManager Instance
    {
        get
        {
            if (instance == null)
            {
                // Optionally, find the instance in the scene if it's not already set.
                instance = FindObjectOfType<PlayerRoomUIManager>();
                if (instance == null)
                {
                    // Create a new GameObject with this component if instance not found.
                    instance = new GameObject("PlayerRoomUIManager").AddComponent<PlayerRoomUIManager>();
                }
            }
            return instance;
        }
    }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject); // Ensures only one instance is active.
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Optionally keep this object alive when loading new scenes.
        }
    }




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
            PColor = player.DisplayColor;
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
        Transform childTransform = element.transform.Find("Preview_Image");
        Image previewImg = childTransform.GetComponent<Image>();
        if (PreviewImg != null)
        {
           // PreviewImg.color = playerColor;
            previewImg.color = playerColor;


        }

        PlayerRoomTitleElementList.Add(element);
    }

    public void UpdatePlayerUIColor(LAK_NetworkRoomPlayer player, Color color)
    {
        // Assuming each player's UI element has a unique identifier, like the player's name
        foreach (var uiElement in PlayerRoomTitleElementList)
        {
            var tmpText = uiElement.GetComponentInChildren<TextMeshProUGUI>();
            if (tmpText != null && tmpText.text == player.DisplayName)
            {
                Transform childTransform = uiElement.transform.Find("Preview_Image");
                Image previewImg = childTransform.GetComponent<Image>();
                if (PreviewImg != null)
                {
                    previewImg.color = color;
                    break;

                }
            }
        }
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
