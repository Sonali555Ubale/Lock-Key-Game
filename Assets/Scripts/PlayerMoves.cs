using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;
using UnityEngine.UI;

public class PlayerMoves : NetworkBehaviour
{
    float HorizontalMove, VerticalMove;
    [SerializeField]
    Rigidbody2D PlayerRigidBody;
    [SerializeField]
    private SpriteRenderer HappyEmogy;
    [SerializeField]
    private Sprite SadEmogySprite;
    [SerializeField]
    private Sprite HappyEmogySprite;
 
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

    [SyncVar(hook = nameof(OnUnlockTimerChanged))]
    private float unlockTimer = 0f;
   
    private bool isUnlocking = false;

   /* public Image LoadingImage;
    float FillAmount;*/


    private PlayerMoves hostPlayer;
    private void Start()
    {
        CmdSetCrown(isServer);
        // Find the host player
        hostPlayer = FindObjectOfType<PlayerMoves>();

    }
    void Update()
    {
        DennerCrown.SetActive(isHost);
        //  Bullet.SetActive(isHost);
        
        LockedPlayerAura.SetActive(isLocked && !isHost);
        SwitchFaceExpression();
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

    public void SwitchFaceExpression()
    {
        if (isLocked && !isHost)

            HappyEmogy.sprite = SadEmogySprite;
        else 
            HappyEmogy.sprite = HappyEmogySprite;
    }

    private void OnUnlockTimerChanged(float _, float newValue)
    {
        unlockTimer = newValue;
    }
    public void unlockPlayer()
    {
        if (isUnlocking)
        {
            unlockTimer += Time.deltaTime;
            Debug.Log("Unlock Timer is::" + unlockTimer);

            // Check if the timer has reached 5 seconds and the player is not the host
            if (unlockTimer >= 3.0f && !isHost && isClient && isLocked)
            {
                // Unlock the player's movement
                isLocked = false;
                CmdSetPlayerAura(false);
                CmdSetPlayerEmogy(true);
              //  UnlockTimer.Instance.LoadingImage.gameObject.SetActive(true);

                // Reset the timer and unlocking state
                unlockTimer = 0f;
                isUnlocking = false;
            }
        }
    }

    /*private void OnCollisionEnter2D(Collision2D collision)
    {
        //on collision isHost condition checked and Set the CmdSetPlayerAura value
        if (collision.gameObject.tag == "Player")
        {
            CmdSetPlayerAura(collision.gameObject.GetComponent<PlayerMoves>().isHost && isLocalPlayer);

            CmdSetPlayerEmogy(collision.gameObject.GetComponent<PlayerMoves>().isHost && isLocalPlayer);
        }
    }  */

   
    private void OnCollisionStay2D(Collision2D collision)
    {
        // Check if the other player is not the host
        if (collision.gameObject.CompareTag("Player") && collision.gameObject.GetComponent<PlayerMoves>().isHost && isLocalPlayer)
        {
           // isLocked = true;
               CmdSetPlayerAura(true);
            CmdSetPlayerEmogy(false);

              //  CmdSetPlayerEmogy(collision.gameObject.GetComponent<PlayerMoves>().isHost && isLocalPlayer);
        }
        if (collision.gameObject.CompareTag("Player") && !isLocalPlayer && !isHost)
        {
            isUnlocking = true;
             unlockPlayer();
           
        }
       
    }


    private void OnCollisionExit2D(Collision2D collision)
    {
        // Reset the unlocking process when the collision ends
        if (collision.gameObject.CompareTag("Player") && !isLocked && isLocalPlayer && !isHost)
        {
            isUnlocking = false;
            unlockTimer = 0f;
        }
    }




}
