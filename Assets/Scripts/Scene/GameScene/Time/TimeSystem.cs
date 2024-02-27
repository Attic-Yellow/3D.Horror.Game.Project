using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using UnityEngine;

public class TimeSystem : MonoBehaviour
{
    [SerializeField] private DateTime initTime;
    [SerializeField] private DateTime gameTime;

    private void Start()
    {
        initTime = DateTime.Now;
        gameTime = new DateTime(1988, 12, 23, 20, 0, 0);
    }

    private void Update()
    {
        UpdateTime();
    }

    private void UpdateTime()
    {
        TimeSpan realTimeElapsed = DateTime.Now - initTime;
        TimeSpan gameTimeElapsed = TimeSpan.FromHours(realTimeElapsed.TotalMinutes / 5.0f);

        gameTime = new DateTime(1988, 12, 23, 20, 0, 0).Add(gameTimeElapsed);
        // print($"{gameTime.ToString("yyyy : MM : dd : HH : mm : ss : tt", new CultureInfo("en-US"))}");
    }

    public string GetDigitalTimeString()
    {
        return gameTime.ToString("HH:mm tt", new CultureInfo("en-US"));
    }

    public DateTime GetAnalogGameTime()
    {
        return gameTime;
    }

    public string GetGameTime()
    {
        return gameTime.ToString("HH:mm");
    }
}
