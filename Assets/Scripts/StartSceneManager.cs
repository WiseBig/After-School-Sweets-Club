using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartSceneManager : MonoBehaviour
{
    public GameObject endPanel;

    private bool isPanelOpen = false;
    // Start is called before the first frame update
    void Start()
    {
        endPanel.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            if (!isPanelOpen)
            {
                endPanel.gameObject.SetActive(true);
                isPanelOpen = true;
            }
            else
            {
                endPanel.gameObject.SetActive(false);
                isPanelOpen = false;
            }
        }
    }
    public void Cancel()
    {
        endPanel.gameObject.SetActive(false);
    }
}
