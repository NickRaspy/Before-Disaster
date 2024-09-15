using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class GameTimer
{
    [Inject] GameSettings gameSettings;

    public delegate void TimeHandler();
    public event TimeHandler OnTimeEnded;

    public GameTime gameTime;

    private Text timerText;
    public GameTimer(Text timerText)
    {
        this.timerText = timerText;
    }

    public void Init(GameTime gameTime)
    {
        this.gameTime = gameTime;
    }

    public IEnumerator StartTimer()
    {
        int allSeconds = gameTime.minutes * 60 + gameTime.seconds;
        int currentSeconds = 0;
        TimeSpan newTime;

        while (currentSeconds <= allSeconds)
        {
            newTime = TimeSpan.FromSeconds(allSeconds - currentSeconds);
            timerText.text = string.Format("{0:00}:{1:00}", newTime.Minutes, newTime.Seconds);
            yield return new WaitForSeconds(1);
            currentSeconds++;
        }
        OnTimeEnded();
    }
}
[Serializable]
public class GameTime
{
    public int minutes;
    public int seconds;
}
