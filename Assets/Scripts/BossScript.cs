using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.XR.Interaction.Toolkit;

public class BossScript : MonoBehaviour
{
    public enemyHit eh;
    public GameObject xr;
    public TextMeshPro tmScore;
    public Player player;
    public GameObject WinPoint;
    public GameObject CameraOffset;
    private bool c = false;

    void Update()
    {
        if(eh.hp <= 1 && !c)
        {
            Win();
        }
    }

    public void Win()
    {
        c = true;
        xr.GetComponent<ActionBasedContinuousMoveProvider>().moveSpeed = 0f;
        tmScore.text = player.score.ToString();
        CameraOffset.transform.position = WinPoint.transform.position;

    }
}
