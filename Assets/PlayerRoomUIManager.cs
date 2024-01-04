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

   

    private void OnEnable()
    {
        if (RoomManager == null) RoomManager = (LAK_NetworkRoomManager)FindObjectOfType(typeof(LAK_NetworkRoomManager));
       

        Pname = PlayerNameInput.DisplayName;
        PColor = ColorSelectionUI.DisplayColor;

     
        // Listen  to event of change in client list
        RoomManager.OnClientListChange.AddListener(OnPlayerChange);
        RoomManager.OnClientReadyStateChanged.AddListener(OnPlayerReadyStateChange);
        RoomManager.OnPlayerColorSelection.AddListener(UpdatePlayerUIList);

        
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
           NWRoomPlayerList.Add(player as LAK_NetworkRoomPlayer);
           //CurrentPlayersCount++;
        }
        ResetPlayerTableUI();
    }

    public void ResetPlayerTableUI()
    {
      
        foreach (var child in PlayerRoomTitleElementList)
        {
            GameObject.Destroy(child);
        }

        //simply delete all 
        // simple add all current clients  // we can change it later to only delete specific

        foreach (var player in NWRoomPlayerList)
        {

            Transform childTransform = PlayerRoomTitleElement.transform.Find("Preview_Image");
            if (childTransform != null)
            {
                PreviewImg = childTransform.GetComponent<Image>();
                if (PreviewImg != null)
                {
                    PreviewImg.color = player.DisplayColor;
                }
            }
            AddPlayer(player.index, player.DisplayName, player.readyToBegin, player.DisplayColor);
        }
       
    }

    public void AddPlayer(int index, string playername, bool readystatus, Color PlayerColor)
    {
        GameObject Element = Instantiate(PlayerRoomTitleElement, VerticalLayoutObject == null ? this.transform : VerticalLayoutObject.transform);

       //SetPlayerEmogyColor();
        TextMeshProUGUI[] list = Element.GetComponentsInChildren<TextMeshProUGUI>();
        list[0].text = playername;
        list[1].text = readystatus ? "Ready" : "Not Ready";
      //  list[2].color = PlayerColor;
        
        PlayerRoomTitleElementList.Add(Element);

    }



}
