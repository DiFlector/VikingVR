using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int score;
    void Start()
    {
        score = 0;
    }

    void Update()
    {
        
    }

    public void UpScore(int num)
    {
        score += num;
    }
}
