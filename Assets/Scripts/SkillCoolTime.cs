using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillCoolTime : MonoBehaviour
{
    public Image skillCoolTime;

    private PlayerController playerController;

    void Start()
    {
        playerController = gameObject.GetComponent<PlayerController>();
    }
    void Update()
    {
        if(playerController.gameStart)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                StartCoroutine(CoolTime(4.4f));
            }
        }
    }  
    IEnumerator CoolTime(float cool)
    {

        float elapsedTime = 0f;
        float originalCool = cool;

        while (elapsedTime < originalCool)
        {
            elapsedTime += Time.deltaTime;
            skillCoolTime.fillAmount = elapsedTime / originalCool;
            yield return new WaitForFixedUpdate();
        }
        skillCoolTime.fillAmount = 1;
    }
}
