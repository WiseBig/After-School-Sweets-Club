using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BtnManager : MonoBehaviour
{
    public Image AiriImage;
    public Image KazusaImage;
    public Image NatsuImage;
    public Image ReisaImage;
    public Image YoshimiImage;

    private bool isAiriClicked = false;
    private bool isKazusaClicked = false;
    private bool isNatsuClicked = false;
    private bool isReisaClicked = false;
    private bool isYoshimiClicked = false;

    private MoveScene moveScene;

    private void Start()
    {
        AiriImage.gameObject.SetActive(false);
        KazusaImage.gameObject.SetActive(false);
        NatsuImage.gameObject.SetActive(false);
        ReisaImage.gameObject.SetActive(false);
        YoshimiImage.gameObject.SetActive(false);

        moveScene = FindObjectOfType<MoveScene>();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("LobbyScene");
        }
    }

    public void TimeAttackAiriBtn()
    {
        if (isAiriClicked)
        {
            CharacterManager.instance.selectedCharacter = "Airi";
            LoadingSceneController.LoadScene("PlayScene");
        }
        else
        {
            ResetClickStates();
            AiriImage.gameObject.SetActive(true);
            isAiriClicked = true;
        }
    }

    public void TimeAttackKazusaBtn()
    {
        if (isKazusaClicked)
        {
            CharacterManager.instance.selectedCharacter = "Kazusa";
            LoadingSceneController.LoadScene("PlayScene");
        }
        else
        {
            ResetClickStates();
            KazusaImage.gameObject.SetActive(true);
            isKazusaClicked = true;
        }
    }  

    public void TimeAttackNatsuBtn()
    {
        if (isNatsuClicked)
        {
            CharacterManager.instance.selectedCharacter = "Natsu";
            LoadingSceneController.LoadScene("PlayScene");
        }
        else
        {
            ResetClickStates();
            NatsuImage.gameObject.SetActive(true);
            isNatsuClicked = true;
        }
    }
    public void TimeAttackYoshimiBtn()
    {
        if (isYoshimiClicked)
        {
            CharacterManager.instance.selectedCharacter = "Yoshimi";
            LoadingSceneController.LoadScene("PlayScene");
        }
        else
        {
            ResetClickStates();
            YoshimiImage.gameObject.SetActive(true);
            isYoshimiClicked = true;
        }
    }
    public void TimeAttackReisaBtn()
    {
        if (isReisaClicked)
        {
            CharacterManager.instance.selectedCharacter = "Reisa";
            LoadingSceneController.LoadScene("PlayScene");
        }
        else
        {
            ResetClickStates();
            ReisaImage.gameObject.SetActive(true);
            isReisaClicked = true;
        }
    }
    private void ResetClickStates()
    {
        isAiriClicked = false;
        isKazusaClicked = false;
        isNatsuClicked = false;
        isReisaClicked = false;
        isYoshimiClicked = false;

        AiriImage.gameObject.SetActive(false);
        KazusaImage.gameObject.SetActive(false);
        NatsuImage.gameObject.SetActive(false);
        ReisaImage.gameObject.SetActive(false);
        YoshimiImage.gameObject.SetActive(false);
    }
}
