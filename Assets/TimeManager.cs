using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Networking;

public class TimeManager : MonoBehaviour
{
    public static TimeManager instance;
    public bool isLoaded = false;

    struct ServerDateTime
    {
        public string datetime;
        public string utc_datetime;
    }

    DateTime LocalDateTime, UTCDateTime;

    void Awake()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
        } else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        StartCoroutine(GetDateTimeFromServer());
    }

    public DateTime getServerDateTime()
    {
        return LocalDateTime.AddSeconds(Time.realtimeSinceStartup);
    }

    public DateTime getServerDateTimeUTC()
    {
        return UTCDateTime.AddSeconds(Time.realtimeSinceStartup);
    }

    IEnumerator GetDateTimeFromServer()
    {
        UnityWebRequest request = UnityWebRequest.Get("https://worldtimeapi.org/api/ip");
        yield return request.SendWebRequest();
        if (request.result == UnityWebRequest.Result.Success)
        {
            ServerDateTime serverDateTime = JsonUtility.FromJson<ServerDateTime>(request.downloadHandler.text);
            LocalDateTime = parseToDateTime(serverDateTime.datetime);
            UTCDateTime = parseToDateTime(serverDateTime.utc_datetime);
            isLoaded = true;
        }
        else
            Debug.Log("Failed to load datetime from server with error" + request.result.ToString());
    }

    DateTime parseToDateTime(string value)
    {
        string date = Regex.Match(value, @"^\d{4}-\d{2}-\d{2}").Value;
        string time = Regex.Match(value, @"\d{2}:\d{2}:\d{2}").Value;
        return DateTime.Parse(string.Format("{0} {1}", date, time));
    }
}
