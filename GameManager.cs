using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    public bool levelReady = true;
    public int shrinesHit;
    
    [SerializeField] float gameLengthInMinutes;
    [SerializeField] EnemyGenerator[] totems;
    [SerializeField] int nextScene;
    [SerializeField] Text countdown;

    private GameObject bgm;

    public bool flashlightOn = true;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
        bgm = GameObject.FindGameObjectWithTag("BGM");
    }

    private void Update()
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

        if (nextScene != 0)
        {
            bool moveToNextLevel = true;
            for (int i = 0; i < totems.Length; i++) if (totems[i].StillSpawning()) moveToNextLevel = false;
            if (moveToNextLevel) SceneManager.LoadScene(2);
        }

        if (Time.timeSinceLevelLoad >= (gameLengthInMinutes * 60f))
        {
            if (nextScene == 0)
            {
                Destroy(bgm);
                SceneManager.LoadScene(0);
            }
            else SceneManager.LoadScene(3);
        }
    }
}
