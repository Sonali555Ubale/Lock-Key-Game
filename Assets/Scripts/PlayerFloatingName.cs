using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;

public class PlayerFloatingName : NetworkBehaviour
{
    public TextMeshPro playerNameText;
    public GameObject floatingInfo;
    public SpriteRenderer PlayerSprite;
    public ColorSelectionUI colorSelectionUI;

    [SyncVar(hook = nameof(OnNameChanged))]
    public string playerName;
    [SyncVar(hook = nameof(OnColorChanged))]
    public Color PlayerColor;

    void OnNameChanged(string _Old, string _New)
    {
        playerNameText.text = _New;
    }

    void OnColorChanged(Color _Old, Color _New)
    {
        PlayerColor = _New;
        PlayerSprite.color = PlayerColor;
        Debug.Log("Selected Color: " + _New);
    }
    public override void OnStartLocalPlayer()
    {
        Camera.main.transform.SetParent(transform);
        Camera.main.transform.localPosition = new Vector3(0, 0, 0);

        floatingInfo.transform.localPosition = new Vector3(0, -0.3f, 0.6f);
        floatingInfo.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);

        string name = PlayerNameInput.DisplayName;

        Color playerColor = ColorSelectionUI.DisplayColor;

        CmdSetupPlayerName(name);
        CmdSetupPlayerColor(playerColor);
    }

    [Command]
    public void CmdSetupPlayerName(string _name)
    {
        // player info sent to server, then server updates sync vars which handles it on all clients
        playerName = _name;
        playerNameText.text = _name;
      
        RpcUpdatePlayerName(_name);
    }

    [ClientRpc]
    void RpcUpdatePlayerName(string _name)
    {
        playerNameText.text = _name; // Update the 3D text UI with the player's name for all clients
    }

    [Command]
    public void CmdSetupPlayerColor(Color _color)
    {
        PlayerColor = _color;
        PlayerSprite.color = _color;

        RpcUpdatePlayerColor( _color);
    }

    [ClientRpc]
    void RpcUpdatePlayerColor(Color _color)
    {
       PlayerSprite.color = _color;
    }



    private void Start()
    {
        OnStartLocalPlayer();
        colorSelectionUI = (ColorSelectionUI)FindAnyObjectByType(typeof(ColorSelectionUI));
      
    }
    void Update()
    {
     
        if (!isLocalPlayer)
        {
            // make non-local players run this
            floatingInfo.transform.LookAt(floatingInfo.transform.position + Camera.main.transform.rotation * Vector3.forward, Camera.main.transform.rotation * Vector3.up);
            return;
        }
    }
}
