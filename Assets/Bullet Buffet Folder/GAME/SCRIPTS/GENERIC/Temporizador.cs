using System.Collections;
using TMPro;
using UnityEngine;

public class Temporizador : MonoBehaviour
{
    [SerializeField] private float totalTime = 120f; // Total del tiempo en segundos, 120 segundos es igual a 2 minutos
    [SerializeField] private TMP_Text timerText;

    public float remainingTime;
    private bool isRunning = true;

    private void Start()
    {
        remainingTime = totalTime;
        Start_TimerText();
        StartCoroutine(Countdown());
    }

    private IEnumerator Countdown()
    {
        while (remainingTime > 0)
        {
            yield return new WaitForSeconds(1f);
            remainingTime--;
            Start_TimerText();
        }
        isRunning = false;
        TimerEnded();
    }

    private void TimerEnded()
    {
        timerText.text = "00:00";
    }

    private void Start_TimerText()
    {
        int minutes = Mathf.FloorToInt(remainingTime / 60);
        int seconds = Mathf.FloorToInt(remainingTime % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
    

}
