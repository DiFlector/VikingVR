using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class getHp : MonoBehaviour
{
    public TextMeshPro text;
    public DummyHit hpObject;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        text.text = System.Convert.ToString(hpObject.hp);
    }
}