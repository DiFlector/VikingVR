using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerHpText : MonoBehaviour
{
    public TextMeshPro tm;
    public Player player;


    // Update is called once per frame
    void Update()
    {
        tm.text = player.hp + "/" + player.maxHp;
    }
}
