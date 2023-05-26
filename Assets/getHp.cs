using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class getHp : MonoBehaviour
{
    public TextMeshPro text;
    public enemyHit hpObject;

    void Update()
    {
        text.text = System.Convert.ToString(hpObject.hp);
    }
}