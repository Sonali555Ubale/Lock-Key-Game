using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Mirror;
using UnityEngine.SceneManagement;
using System;

public class UnlockTimer : NetworkBehaviour
{
    [SyncVar(hook = nameof(OnTimeChanged))]
    public int Sec;

    public static UnlockTimer Instance;
    public Image LoadingImage;
    
    private int TOTAL_SEC = 0;
    private float lastUpdateTime; // Store the time of the last update
    float FillAmount;
   
    public void Start()
    {
            if (Sec > 0)
            TOTAL_SEC = Sec;

        lastUpdateTime = Time.time; // Initialize the lastUpdateTime
        StartCoroutine(updateTime());
    }


    // The SyncVar hook to handle time updates
    private void OnTimeChanged(int oldTime, int newTime)
    {
        Sec = newTime;
        UpdateLoadingUI(Sec);

        if (Sec <= 0)
        {
            StopCoroutine(updateTime());
        }
    }

    [Command]
    public void CmdSetTimer(int time)
    {
        Sec = time;
    }

    IEnumerator updateTime()
    {
        while (Sec > 0)
        {

                // Calculate time since the last update
                float elapsedTime = Time.time - lastUpdateTime;

                // If one second has passed, decrement the timer
                if (elapsedTime >= 1f)
                {
                    lastUpdateTime = Time.time; // Update lastUpdateTime
                    CmdSetTimer(Sec);
                    Sec--;
                }
            

            yield return null; // This is a more CPU-friendly way to yield in FixedUpdate
        }
    }

    public void UpdateLoadingUI(float secs)
    {
        FillAmount = (float)secs / TOTAL_SEC;
       LoadingImage.fillAmount = FillAmount;
      
    }
}
