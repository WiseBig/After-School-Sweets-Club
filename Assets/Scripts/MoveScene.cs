using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MoveScene : MonoBehaviour
{
    public static bool timeAttackMode = false;
    public static bool endlessMode = false;

    private static MoveScene moveScene;
    public void EndlessChoicePlayer()
    {
        endlessMode = true;
        timeAttackMode = false;
        SceneManager.LoadScene("ChoicePlayer");
    }
    public void TImeAttackChoicePlayer()
    {
        endlessMode = false;
        timeAttackMode = true;
        SceneManager.LoadScene("ChoicePlayer");
    }

    public void LobbyScene()
    {
        LoadingSceneController.LoadScene("LobbyScene");
    }
   
    public void StartScene()
    {
        LoadingSceneController.LoadScene("StartScene");
    }
    public void PvpLobbyScene()
    {
        LoadingSceneController.LoadScene("PVPLobbyScene");
    }
    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}

