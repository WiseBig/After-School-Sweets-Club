using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Result : MonoBehaviour
{
    private Timer timer;
    private SpawnManager spawnManager;

    public Text elapseTime;
    public Text destroyEnemy;

    private int destoryCount;
    // Start is called before the first frame update
    void Start()
    {
        timer = FindObjectOfType<Timer>();
        spawnManager = FindObjectOfType<SpawnManager>();
        
        destoryCount = spawnManager.destroyEnemy;

        elapseTime.text = "경과 시간 : ";
        destroyEnemy.text = "적 처치 : " + destoryCount.ToString();
    }
}
