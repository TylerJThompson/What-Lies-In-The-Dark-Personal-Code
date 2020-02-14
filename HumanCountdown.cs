using UnityEngine;
using UnityEngine.UI;

public class HumanCountdown : MonoBehaviour
{
    [SerializeField] float gameLengthInMinutes;

    private Text countdown;

    // Start is called before the first frame update
    void Start()
    {
        countdown = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (countdown != null)
        {
            int totalSecondsRemaining = (int)(gameLengthInMinutes * 60f - Time.timeSinceLevelLoad);
            int minutesRemaining = totalSecondsRemaining / 60;
            int secondsRemaing = totalSecondsRemaining % 60;
            string countdownText = minutesRemaining + ":";
            if (secondsRemaing >= 10) countdownText += secondsRemaing;
            else countdownText += ("0" + secondsRemaing);
            countdown.text = countdownText;
        }
    }
}
