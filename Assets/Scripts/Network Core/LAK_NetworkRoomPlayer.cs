using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.EditorTools;
using UnityEngine;
using UnityEngine.Events;

public class LAK_NetworkRoomPlayer : NetworkRoomPlayer
{

    [Tooltip("Diagnostic Player name")]
    [SyncVar(hook = nameof(PlayerNameUpdate))]
    public string DisplayName;
   

   /* [SyncVar(hook = nameof(OnSelectionUIColor))] 
    public Color ColorToHide;*/
    //  public Dictionary<Color, bool> colorTodisable = new Dictionary<Color, bool>();



    private LAK_NetworkRoomManager roomManager { get { return getRoom(); }  }

    public UnityEvent OnClientReadyStateChange = new UnityEvent();

    private LAK_NetworkRoomManager getRoom()
    {
    return (LAK_NetworkRoomManager)FindObjectOfType(typeof(LAK_NetworkRoomManager));

    }

    public override void ReadyStateChanged(bool oldReadyState, bool newReadyState)
    {

        roomManager?.AnyClientUpdate(this);

    }

    public void PlayerNameUpdate(string oldName, string newName)
    {
        DisplayName = newName;

        roomManager?.AnyClientUpdate(this);
    }


    public override void OnStartLocalPlayer()
    {
       // roomManager = (LAK_NetworkRoomManager)FindObjectOfType(typeof(LAK_NetworkRoomManager));
        string name = PlayerNameInput.DisplayName;
        CmdSetupPlayerName(name);
        roomManager?.AnyClientUpdate(this);

    }

    [Command]
    public void CmdSetupPlayerName(string _name)
    {
        DisplayName = _name;
        RpcUpdatePlayerName(_name);
       
    }

    [ClientRpc]
    void RpcUpdatePlayerName(string _name)
    {
        DisplayName = _name;
       roomManager?.AnyClientUpdate(this);
    }

    /*public void OnSelectionUIColor(Color oldColor, Color newColor)
    {
       
            ColorToHide = newColor;
      
     }*/
}

