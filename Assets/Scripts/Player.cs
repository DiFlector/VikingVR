using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Player : MonoBehaviour
{
    public int score;
    public string scene;
    public int hp;
    public int maxHp;
    public GameObject DeathPoint;
    public GameObject Grave;
    public GameObject XROrigin;
    public GameObject GraveStone;
    public TextMeshPro ScoreTm;

    void Start()
    {
        score = 0;
        hp = gameObject.transform.parent.GetComponentInChildren<enemyHit>().hp;
        maxHp = gameObject.transform.parent.GetComponentInChildren<enemyHit>().maxHp;
    }

    private void Update()
    {
        hp = gameObject.transform.parent.GetComponentInChildren<enemyHit>().hp;
        if (hp == 0)
        {
            Death();
        }
    }

    public void UpScore(int num)
    {
        score += num;
    }

    public void Death()
    {
        Grave.gameObject.SetActive(true);
        ScoreTm.text = score.ToString();
        gameObject.transform.parent.GetComponentInChildren<enemyHit>().hp = maxHp;
        XROrigin.GetComponent<Transform>().position = DeathPoint.transform.position;
    }
}