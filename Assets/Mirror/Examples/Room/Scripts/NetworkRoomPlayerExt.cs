using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

namespace Mirror.Examples.NetworkRoom
{
    [AddComponentMenu("")]
    public class NetworkRoomPlayerExt : NetworkRoomPlayer
    {
        [SerializeField] private GameObject LobbyUI;
        [SerializeField] private TMP_Text[] PlayerNameTxt = new TMP_Text[5];
        [SerializeField] private TMP_Text[] PlayerReadyTxt = new TMP_Text[5];
        [SerializeField] private Button BtnStartGame;

        [SyncVar(hook = nameof(HandleDisplayNameChanged))]
        public string DisplayName = "Loading...";
        
        [SyncVar(hook = nameof(HandleReadyStateChanged))]
        public bool isReady = false;


        private bool isDenner = true;

        public bool IsDenner
        {
            set
            {
                isDenner = value;
                BtnStartGame.gameObject.SetActive(value);
            }
        }

        public override void OnStartAuthority()
        {
          //  CmdSetDisplayName(PlayerNameInput.DisplayName);    //set the display name of the player
            LobbyUI.SetActive(true);

        }

        private NetworkRoomManager room;

       
        public override void OnStartClient()
        {
            room.roomSlots.Add(this);
            UpdateDisplay();
            

        }

        public override void OnStopClient()
        {
            room.roomSlots.Remove(this);
            UpdateDisplay();
        }
       

        public override void OnClientEnterRoom()
        {
            //Debug.Log($"OnClientEnterRoom {SceneManager.GetActiveScene().path}");
        }

        public override void OnClientExitRoom()
        {
            //Debug.Log($"OnClientExitRoom {SceneManager.GetActiveScene().path}");
        }

        public override void IndexChanged(int oldIndex, int newIndex)
        {
            
            //Debug.Log($"IndexChanged {newIndex}");

        }

        public override void ReadyStateChanged(bool oldReadyState, bool newReadyState)
        {
            //Debug.Log($"ReadyStateChanged {newReadyState}");
        }

        public void HandleReadyStateChanged(bool _, bool newVal) => UpdateDisplay();
        public void HandleDisplayNameChanged(string _, string newVal) => UpdateDisplay();

        //public void HandleDisplayColorChanged(Color _, Color newVal) => UpdateDisplay();
        public void UpdateDisplay()
        {
            if (!isOwned)                //hasAuthority is renamed as isOwnwd
            {
                foreach (var player in room.roomSlots)    //if the player does not bolongs to us or does not have authority then check which players having authoryty and Update the DisplayName
                {
                    if (player.isOwned)
                    {
                        UpdateDisplay();
                        player.netIdentity.AssignClientAuthority(connectionToClient);
                        break;
                    }
                }
                return;
            }

            for (int i = 0; i < PlayerNameTxt.Length; i++)
            {
                PlayerNameTxt[i].text = DisplayName;
                PlayerReadyTxt[i].text = string.Empty;
            }

            for (int i = 0; i < room.roomSlots.Count; i++)
            {
                //PlayerNameTxt[i].text = room.roomSlots[i].DisplayName;
                //PlayerReadyTxt[i].text = room.roomSlots[i].isReady ? "<color=green>Ready</color>" : "<color=red>Not Ready</color>";
                PlayerNameTxt[i].text = room.roomSlots[i].name;
                PlayerReadyTxt[i].text = isReady ? "<color=green>Ready</color>" : "<color=red>Not Ready</color>";
            }
        }


        public override void OnGUI()
        {
            LobbyUI.SetActive(true);
        }

        public void handleReadyToStart(bool readyToStart)
        {
            if (!isDenner) { return; }
            BtnStartGame.interactable = readyToStart;
        }

        [Command]
        public void CmdSetDisplayName(string displayName)
        {
            DisplayName = displayName;
        }

       
        

        public void OnReadyButtonClick()
        {
            isReady = true;
            CmdChangeReadyState(isReady);
        }

        [Command]
        public void CmdChangeReadyState(bool readyState)
        {
            isReady = readyState;

            NetworkRoomManager room = NetworkManager.singleton as NetworkRoomManager;
            if (room != null)
            {
                room.ReadyStatusChanged();
            }
        }

        [Command]
        public void CmdStartGame()             //called when Ui start button clicked
        {
            
            if (room.roomSlots[0].connectionToClient != connectionToClient) { return; } //if room player does not have authority then return else Start game.

               room.startGame();

            Debug.Log(" Game Started...");

        }

    }
}
