using System;
using UnityEngine;
using TMPro;

public class BillboardUI : MonoBehaviour
{
    [field: SerializeField] private TMP_Text Score;
    [field: SerializeField] private TMP_Text Timer;

    [field: SerializeField] private bool HidePlainText = true;

    private float minutes;
    private float seconds;

    private void Update()
    {
        if (!GameHandler.Instance.IsGameRunning()) return;

        float timeValue = GameHandler.Instance.GetTimeElapsed();

        minutes = Mathf.Floor(timeValue / 60);
        seconds = Mathf.Round(timeValue % 60);

        string minutesText = minutes.ToString();
        string secondsText = seconds.ToString();

        if (minutes < 10) minutesText = "0" + minutes.ToString();
        if (seconds < 10) secondsText = "0" + Mathf.Round(seconds).ToString();

        string scoreAddition = (!HidePlainText) ? "Score: " : string.Empty;
        string timeAddition = (!HidePlainText) ? "Time Left: " : string.Empty;

        Score.text = scoreAddition + GameHandler.Instance.GetPlayerScore().ToString();
        Timer.text = timeAddition + minutesText + ":" + secondsText;
    }
}
