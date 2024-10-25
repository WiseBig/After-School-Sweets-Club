using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SpawnManager : MonoBehaviour
{
    float minSpeed = 3f;
    float maxSpeed = 10f;
    public GameObject enemyPrefab;
    public int destroyEnemy = 0;

    public Text text_wave;
    public Text text_destoryEnemyCount;
    public Text text_waveCount;

    public GameObject gameOverCanvas;
    public GameObject stageClear;
    public GameObject result;
    public GameObject soundManager;
    public GameObject setup;

    public GameObject gameOverUi;
    public GameObject victoryUi;

    public Image startImage;
    public Image startLogo;

    private float spawnRange = 16;
    private int waveNumber = 1;
    private int enemyCount;
    private int endWaveNumber = 1;
    private float startGameNum = 2f;
    private int randomEnemyCount;

    private bool isSettingOpen = false;
    private bool isMovemenetStop = false;
    private bool isSettingOpens = false;
    private bool isMovementStop = false;
    private bool gameClear = false;
    
    private PlayerController playerController;
    private Rigidbody[] playerRigidbodies;
    private Timer timer;

    // Start is called before the first frame update
    void Start()
    {
        playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        StartCoroutine(StartCountDown());
        soundManager.gameObject.SetActive(true);
        setup.gameObject.SetActive(false);
        gameOverCanvas.gameObject.SetActive(false);
        result.gameObject.SetActive(false);
        playerRigidbodies = GameObject.FindWithTag("Player").GetComponentsInChildren<Rigidbody>();
        timer = FindObjectOfType<Timer>();

        if(MoveScene.timeAttackMode)
        {
            Debug.Log("타임어택 모드");
            minSpeed = 3f;
            maxSpeed = 10f;
            destroyEnemy = 0;

            playerController.gameOver = false;
            spawnRange = 16;
            waveNumber = 1;
            endWaveNumber = 3;
            startGameNum = 2f;
            gameClear = false;

            Time.timeScale = 1;
        }
        else if(MoveScene.endlessMode)
        {
            Debug.Log("무한 모드");
            minSpeed = 3f;
            maxSpeed = 10f;
            destroyEnemy = 0;

            playerController.gameOver = false;
            spawnRange = 16;
            waveNumber = 1;
            startGameNum = 2f;
            gameClear = false;

            Time.timeScale = 1;
        }
    }
    // Update is called once per frame
    void Update()
    { 
        if(playerController.gameStart)
        {
            enemyCount = FindObjectsOfType<Enemy>().Length;
            if (enemyCount == 0 && !playerController.gameOver)
            {
                waveNumber++;
                spawnEnemyWave(waveNumber);

                if (MoveScene.timeAttackMode && endWaveNumber < waveNumber)
                {
                    Enemy[] enemies = FindObjectsOfType<Enemy>();
                    foreach (var enemy in enemies)
                    {
                        Destroy(enemy.gameObject);
                    }
                    if (playerController != null)
                    {
                        playerController.DestoryPlayer();
                    }
                    timer.text_timer.gameObject.SetActive(false);
                    text_wave.gameObject.SetActive(false);
                    soundManager.gameObject.SetActive(false);
                    stageClear.gameObject.SetActive(true);
                    waveNumber -= 1;
                    enemyCount -= 1;
                    Time.timeScale = 0f;
                    gameClear = true;
                }
            }
            if (Input.GetKeyDown(KeyCode.Escape) && !playerController.gameOver && !gameClear)
            {
                if (!isSettingOpen)
                {
                    OpenSettings();
                }
                else
                {
                    CloseSettings();
                }
            }
            if (MoveScene.endlessMode)
            {
                text_wave.text = waveNumber.ToString() + " Wave ";
            }
            else if(MoveScene.timeAttackMode)
            {
                text_wave.text = waveNumber.ToString() + " / " + endWaveNumber.ToString();
            }
            text_waveCount.text = waveNumber.ToString();
            text_destoryEnemyCount.text = destroyEnemy.ToString();
        }
    }
    IEnumerator StartCountDown()
    {
        while(startGameNum > 0)
        {
            startImage.gameObject.SetActive(true);
            startLogo.gameObject.SetActive(true);
            yield return new WaitForSeconds(1f);
            startGameNum -= 1f;
        }
        startImage.gameObject.SetActive(false);
        yield return new WaitForSeconds(1f);
        startLogo.gameObject.SetActive(false);

        yield return new WaitForSeconds(0.3f);
        playerController.gameStart = true;

        spawnEnemyWave(waveNumber);
    }
    void spawnEnemyWave(int waveNumber)
    {
        for (int i = 0; i < waveNumber; i++)
        {
            GameObject enemy = Instantiate(enemyPrefab, spawnPosition(), enemyPrefab.transform.rotation);

            float randomSpeed = Random.Range(minSpeed, maxSpeed);
            enemy.GetComponent<Enemy>().speed = randomSpeed;
        }
    }
    private Vector3 spawnPosition()
    {
        float spawnPosX = Random.Range(-spawnRange, spawnRange);
        float spawnPosZ = Random.Range(-spawnRange, spawnRange);
        Vector3 randomPos = new Vector3(spawnPosX, 0, spawnPosZ);

        return randomPos;
    }
    public void EnemyDestroyed()
    {
        destroyEnemy++;
    }
    void OpenSettings()
    {
        setup.gameObject.SetActive(true);
        StopAllMovement();
        isSettingOpen = true;
    }
    void CloseSettings()
    {
        setup.gameObject.SetActive(false);
        ResumeAllMovement();
        isSettingOpen = false;
    }
    void StopAllMovement()
    {
        isMovementStop = true;
        Time.timeScale = 0f;
        foreach (Rigidbody rb in playerRigidbodies)
        {
            rb.isKinematic = true;
        }
        Rigidbody[] enemyRigidbodies = FindObjectsOfType<Rigidbody>();
        foreach (Rigidbody rb in enemyRigidbodies)
        {
            if (rb.gameObject.CompareTag("Enemy"))
            {
                rb.isKinematic = true;
            }
        }
    }
    void ResumeAllMovement()
    {
        isMovementStop = false;
        Time.timeScale = 1.0f;
        foreach (Rigidbody rb in playerRigidbodies)
        {
            rb.isKinematic = false;
        }
        Rigidbody[] enemyRigidbodies = FindObjectsOfType<Rigidbody>();
        foreach (Rigidbody rb in enemyRigidbodies)
        {
            if (rb.gameObject.CompareTag("Enemy"))
            {
                rb.isKinematic = false;
            }
        }
    }
    public void GameOver()
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
        Time.timeScale = 0f;
        timer.text_timer.gameObject.SetActive(false);
        text_wave.gameObject.SetActive(false);
        soundManager.gameObject.SetActive(false);
        setup.gameObject.SetActive(false);
        gameOverCanvas.gameObject.SetActive(true);
    }
    public void RestartGame()
    {
        SceneManager.LoadScene("PlayScene");
        StopAllCoroutines();
        playerController.StopAllCoroutines();
        // 플레이어 위치 초기화 (원하는 시작 위치로 설정)
        playerController.transform.position = Vector3.zero;
        playerController.transform.rotation = Quaternion.Euler(0, 180, 0);

        // 플레이어 애니메이션 초기화
        Animator playerAnimator = playerController.GetComponent<Animator>();
        playerAnimator.Rebind(); // 애니메이터 상태 초기화
        playerAnimator.Update(0f);

        // 게임과 관련된 변수를 초기 상태로 리셋
        waveNumber = 1;
        playerController.gameOver = false;
        destroyEnemy = 0;
        startGameNum = 2f;
        gameClear = false;
        gameOverCanvas.gameObject.SetActive(false);
        playerController.gameStart = false;
        playerController.attackCount = 1;
        playerController.skillCoolTime.fillAmount = 1;
        if(MoveScene.timeAttackMode)
        {
            timer.leftTime = 100;
            endWaveNumber = 3;
        }
        else if(MoveScene.endlessMode)
        {
            timer.nowTime = 0;
        }

        // UI 텍스트 리셋
        text_wave.text = waveNumber.ToString() + " / " + endWaveNumber.ToString();

        CloseSettings();

        // 기존의 모든 적 제거 및 코루틴 정지
        foreach (var enemy in FindObjectsOfType<Enemy>())
        {
            Destroy(enemy.gameObject);
            enemy.StopAllCoroutines();
        }

        // 카운트다운 다시 시작
        StartCoroutine(StartCountDown());
    }     
    public void Result()
    {
        if(gameOverUi.gameObject.activeSelf)
            gameOverUi.gameObject.SetActive(false);

        if(victoryUi.gameObject.activeSelf)
            victoryUi.gameObject.SetActive(false);

        result.gameObject.SetActive(true);
    }
    public void CloseSetupBtn()
    {
        CloseSettings();
    }
}
