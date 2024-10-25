using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public GameObject player;

    private Vector3 offset = new Vector3(0, 6, -12);

    // Update is called once per frame
    void Update()
    {
        if(player != null)
        {
            transform.position = player.transform.position + offset;
        }
        else
        {
            enabled = false;
        }
    }
    public void SetPlayer(GameObject newPlayer)
    {
        player = newPlayer;
    }
}
