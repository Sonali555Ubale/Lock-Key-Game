﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Mirror;
using UnityEngine.SceneManagement;
using System;

public class TimerFromserver : NetworkBehaviour
{
    [SyncVar(hook = nameof(OnTimeChanged))]
    public int Sec;

    public static Timer Instance;
    public Image LoadingImage;
    public TextMeshProUGUI TimeLeft;

    private int TOTAL_SEC = 0;
    private float lastUpdateTime; // Store the time of the last update
    float FillAmount;
    GameOverManager gameOverObj;

    public void Start()
    {
        TimeLeft.text = Sec.ToString();
        if (Sec > 0)
            TOTAL_SEC = Sec;

        lastUpdateTime = Time.time; // Initialize the lastUpdateTime
        StartCoroutine(updateTime());
    }

   
    // The SyncVar hook to handle time updates
    private void OnTimeChanged(int oldTime, int newTime)
    {
        Sec = newTime;
        TimeLeft.text = Sec.ToString();
        UpdateLoadingUI(Sec);

        if (Sec <= 0)
        {
            TimeLeft.text = "Time's Up";
            StopCoroutine(updateTime());
            SceneManager.LoadScene(2);
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
            if (isServer)
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
