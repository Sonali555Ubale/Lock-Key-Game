using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Mirror;
using UnityEngine.SceneManagement;
using System;

public class Timer : NetworkBehaviour
{
    [SyncVar]
    public int Sec;

    public static Timer Instance;   
    public Image LoadingImage;
    public TextMeshProUGUI TimeLeft;
    
    private int TOTAL_SEC = 0;
    float FillAmount;
    GameOverManager gameOverObj;

   public void Start()
    {
        TimeLeft.text = Sec.ToString();
        if(Sec>0)
        TOTAL_SEC = Sec;

        StartCoroutine(second());

    }

    public void Update()
    {

        if (Sec <= 0)
        {
            TimeLeft.text = "Time's Up";
          
          StopCoroutine(second());
            SceneManager.LoadScene(2);
        }

      
    }
    [Command]
    public void CmdSetTimer(int time)
    {
        Sec = time;

    }

    IEnumerator second()
    {
        yield return new WaitForSeconds(1f);
        if (Sec > 0)
            Sec--;

        TimeLeft.text = Sec.ToString();
        UpdateLoadingUI(Sec);
        StartCoroutine(second());

        if (isServer)
        {
            CmdSetTimer(Sec);
        }
    }

    public void UpdateLoadingUI(float secs)
    {
        FillAmount = (float)secs / TOTAL_SEC;
        LoadingImage.fillAmount = FillAmount;
    }


  
    

}
