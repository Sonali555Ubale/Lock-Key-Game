using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.EditorTools;
using UnityEngine;
using UnityEngine.Events;
using static UnityEditor.Progress;

public class LAK_NetworkRoomPlayer : NetworkRoomPlayer
{

    [Tooltip("Diagnostic Player name")]
    [SyncVar(hook = nameof(PlayerNameUpdate))]
    public string DisplayName;

    public readonly SyncList<bool> ColorActiveList = new SyncList<bool>();

    LAK_NetworkRoomPlayer()
    {
        if (ColorActiveList.Count != 10)
        {
            for (int i = 0; i < 10; i++)
            {
                ColorActiveList.Add(true);
            }
        }


        ColorActiveList.Callback += OnColorSelectCallback;

    }



    private void OnColorSelectCallback(SyncList<bool>.Operation op, int itemIndex, bool oldItem, bool newItem)
    {
        
    }

   [Command]
    public void ColorSelected(int index, bool val)
    {
        if (index >= 0 && index < ColorActiveList.Count)
        {
            ColorActiveList[index] = val;
            RpcUpdateColorSelected(index, val);
        }
        
    }

    [ClientRpc]
    void RpcUpdateColorSelected(int index, bool val)
    {
       roomManager?.UpdateColorVal(index,val);
        
    }


    private LAK_NetworkRoomManager roomManager { get { return getRoom(); }  }

    public UnityEvent OnClientReadyStateChange = new UnityEvent();

    private LAK_NetworkRoomManager getRoom()
    {
        return (LAK_NetworkRoomManager)FindObjectOfType(typeof(LAK_NetworkRoomManager));

    }

    public void ColorListUpdate(bool[] oldColorList, bool[] newColorList)
    {

    }

  /*  [Server]
    public override void OnStartServer()
    {
        ColorList = new bool[10];
        for (int i = 0; i < 10; i++)
        {
            ColorList[i] = true;
        }

        Debug.Log(ColorList);
    }*/

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

    public void OnSelectionUIColor(Color oldColor, Color newColor)
    {
       
            //ColorToHide = newColor;
      
     }
}

