using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEditor.Experimental.GraphView;

public class Player : MonoBehaviour
{
    public int score;
    public string scene;
    public int hp;
    public int defaultHp;
    public GameObject DeathPoint;
    public GameObject Grave;
    public GameObject XROrigin;
    public GameObject GraveStone;
    public TextMeshPro ScoreTm;

    void Start()
    {
        score = 0;
        hp = gameObject.transform.parent.GetComponentInChildren<enemyHit>().hp;
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
        gameObject.transform.parent.GetComponentInChildren<enemyHit>().hp = defaultHp;
        XROrigin.GetComponent<Transform>().position = DeathPoint.transform.position;
    }
}