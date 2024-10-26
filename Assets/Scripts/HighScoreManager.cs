using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighScoreManager : MonoBehaviour
{
    public Text text_highWave;
    public Text text_highElapseTime;
    void Start()
    {
        int saveWaveNumber = PlayerPrefs.GetInt("waveNumber", 0);
        text_highWave.text = saveWaveNumber.ToString() + " Wave";

        int savedMinutes = PlayerPrefs.GetInt("elapsedMinutes", 0);
        int savedSeconds = PlayerPrefs.GetInt("elapsedSeconds", 0);
        text_highElapseTime.text = string.Format("{0:00}:{1:00}", savedMinutes, savedSeconds);
    }
}
