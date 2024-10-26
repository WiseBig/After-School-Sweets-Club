using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour
{
    public float leftTime;
    public float nowTime;
    public Text text_timer;
    public Text text_elapseTime;

    private PlayerController playerController;
    private SpawnManager spawnManager;

    void Start()
    {
        playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        spawnManager = FindObjectOfType<SpawnManager>();
        leftTime = 70;
        nowTime = 0;

        Time.timeScale = 1;
    }
    // Update is called once per frame
    void Update()
    {
        if(playerController.gameStart)
        {
            if (MoveScene.timeAttackMode)
            {
                if (leftTime > 0)
                {
                    int minutes = Mathf.FloorToInt(leftTime / 60); //남은 시간을 분으로 변환
                    int seconds = Mathf.FloorToInt(leftTime % 60); //남은 시간을 초로 변환
                    text_timer.text = string.Format("{0:00}:{1:00}", minutes, seconds); // "00:00" 형식으로 시간 표시

                    leftTime -= Time.deltaTime;

                    int nowMinutes = Mathf.FloorToInt(nowTime / 60);
                    int nowSeconds = Mathf.FloorToInt(nowTime % 60);
                    text_elapseTime.text = string.Format("{0:00}:{1:00}", nowMinutes, nowSeconds);
                    nowTime += Time.deltaTime;

                    PlayerPrefs.SetInt("elapsedMinutes", nowMinutes);
                    PlayerPrefs.SetInt("elapsedSeconds", nowSeconds);
                    PlayerPrefs.Save();
                }
                else
                {
                    leftTime = 0;
                    GameOver();
                }
            }
            else if (MoveScene.endlessMode)
            {
                int minutes = Mathf.FloorToInt(nowTime / 60); //남은 시간을 분으로 변환
                int seconds = Mathf.FloorToInt(nowTime % 60); //남은 시간을 초로 변환
                text_timer.text = string.Format("{0:00}:{1:00}", minutes, seconds); // "00:00" 형식으로 시간 표시

                int nowMinutes = Mathf.FloorToInt(nowTime / 60);
                int nowSeconds = Mathf.FloorToInt(nowTime % 60);
                text_elapseTime.text = string.Format("{0:00}:{1:00}", nowMinutes, nowSeconds);
                nowTime += Time.deltaTime;
            }
        }    
    }
    void GameOver()
    {
        if(leftTime == 0)
        {
            Enemy[] enemies = FindObjectsOfType<Enemy>();
            foreach (var enemy in enemies)
            {
                Destroy(enemy.gameObject);
            }
            if (playerController != null)
            {
                playerController.DestoryPlayer();
                playerController.gameOver = true;
            }
            Time.timeScale = 0;
            text_timer.gameObject.SetActive(false);
            spawnManager.text_wave.gameObject.SetActive(false);
            spawnManager.soundManager.gameObject.SetActive(false);
            spawnManager.gameOverCanvas.gameObject.SetActive(true);
        }
    }
}
