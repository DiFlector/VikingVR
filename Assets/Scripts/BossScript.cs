using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class BossScript : MonoBehaviour
{
    public enemyHit eh;
    public GameObject xr;
    public TextMeshPro tmScore;
    public Player player;
    public GameObject WinPoint;
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
        tmScore.text = player.score.ToString();
        xr.transform.position = WinPoint.transform.position;

    }
}
