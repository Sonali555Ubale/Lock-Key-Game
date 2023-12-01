using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerMoves : NetworkBehaviour
{
    float HorizontalMove, VerticalMove;
    [SerializeField]
    Rigidbody2D PlayerRigidBody;
    [SerializeField]
    private SpriteRenderer HappyEmogy;
    [SerializeField]
    public Sprite SadEmogySprite;
    [SerializeField]
    public Sprite HappyEmogySprite;

    [SerializeField]
    GameObject DennerCrown;
    [SerializeField]
    GameObject LockedPlayerAura;

    [SyncVar(hook = nameof(SetCrown))]
    public bool isHost = false;

    [SyncVar(hook = nameof(setPlayerLockAura))]
    public bool isLocked = false;

    [SyncVar(hook = nameof(setPlayerEmogy))]
    public bool isHappy = true;

    // private bool isFreeze = false;
    private void Start()
    {
        CmdSetCrown(isServer);
      
    }
    void Update()
    {
        DennerCrown.SetActive(isHost);
        
        LockedPlayerAura.SetActive(isLocked && !isHost);
        if (isLocked && !isHost)

            HappyEmogy.sprite = SadEmogySprite;
        else
            HappyEmogy.sprite = HappyEmogySprite;

        movePlayer();
     }

    public void movePlayer()
    {
        //checking if the state is freeze and move if it is false
        if (isLocked && !isHost || !isLocalPlayer) return;              
                HorizontalMove = Input.GetAxis("Horizontal") * 2.5f * Time.deltaTime;
                VerticalMove = Input.GetAxis("Vertical") * 2.5f * Time.deltaTime;
                Vector3 b = new Vector3(HorizontalMove, VerticalMove, 0f);
                transform.position = transform.position + b;
    }

    public void SetCrown(bool _, bool newVal)
    {
        isHost = newVal;
    }

    [Command]
    public void CmdSetCrown(bool val)
    {
        isHost = val;
    }

    public void setPlayerLockAura(bool _, bool newVal)
    {
        if(!isHost)
        isLocked = newVal;
    }
    public void setPlayerEmogy(bool _, bool newVal)
    {
        if (!isHost)
            isHappy = newVal;
    }
    

    [Command]
    public void CmdSetPlayerAura(bool val)
    {
        isLocked = val;
    }

    [Command]
    public void CmdSetPlayerEmogy(bool val)
    {
       isHappy = val;
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        //on collision isHost condition checked and Set the CmdSetPlayerAura value
        CmdSetPlayerAura(collision.gameObject.GetComponent<PlayerMoves>().isHost && isLocalPlayer);
        CmdSetPlayerEmogy(collision.gameObject.GetComponent<PlayerMoves>().isHost && isLocalPlayer);
    }           


}
