using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject AiriPrefab;
    public GameObject KazusaPrefab;
    public GameObject NatsuPrefab;
    public GameObject ReisaPrefab;
    public GameObject YoshimiPrefab;

    public Image AiriUi;
    public Image KazusaUi;
    public Image NatsuUi;
    public Image YoshimiUi;
    public Image ReisaUi;

    public GameObject mainCamera; 

    private PlayerController playerController;
    private SpawnManager spawnManager;
    
    private void Start()
    {
        AiriUi.gameObject.SetActive(false);
        KazusaUi.gameObject.SetActive(false);
        NatsuUi.gameObject.SetActive(false);
        YoshimiUi.gameObject.SetActive(false);
        ReisaUi.gameObject.SetActive(false);

        string selectedCharacter = CharacterManager.instance.selectedCharacter;
        GameObject characterPrefab = null;
        Image selectedCharacterUi = null;

        switch (selectedCharacter)
        {
            case "Airi":
                characterPrefab = AiriPrefab;
                selectedCharacterUi = AiriUi;
                AiriUi.gameObject.SetActive(true);
                break;
            case "Kazusa":
                characterPrefab = KazusaPrefab;
                selectedCharacterUi = KazusaUi;
                KazusaUi.gameObject.SetActive(true);
                break;
            case "Natsu":
                characterPrefab = NatsuPrefab;
                selectedCharacterUi = NatsuUi;
                NatsuUi.gameObject.SetActive(true);
                break;
            case "Reisa":
                characterPrefab = ReisaPrefab;
                selectedCharacterUi = ReisaUi;
                ReisaUi.gameObject.SetActive(true);
                break;
            case "Yoshimi":
                characterPrefab= YoshimiPrefab;
                selectedCharacterUi = YoshimiUi;
                YoshimiUi.gameObject.SetActive(true);
                break;
        }
        if(characterPrefab != null)
        {
            GameObject player = Instantiate(characterPrefab, Vector3.zero, Quaternion.Euler(0, 180, 0));
            player.name = "Player"; // 이름을 "Player"로 설정하여 적이 이를 인식하도록 합니다.

            // PlayerController 스크립트에 skillCoolTime UI 이미지 할당
            PlayerController playerController = player.GetComponent<PlayerController>();
            playerController.skillCoolTime = selectedCharacterUi;

            // 카메라가 플레이어를 따라가도록 설정
            FollowPlayer followPlayer = mainCamera.GetComponent<FollowPlayer>();
            followPlayer.SetPlayer(player);
        }
    }
   
}
