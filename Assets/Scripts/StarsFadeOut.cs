using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarsFadeOut : MonoBehaviour
{
    public MeshRenderer Stars;
    Color clr = Color.white;
    float i = 0;
    // Start is called before the first frame update
    void Start()
    {
        /*
        Stars.material.SetColor("_Color", clr);
        Debug.Log(Stars.material.color);*/
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        /*
        i++;
        if (i >= 60) { i = 0; }
        clr.a = i/60;
        Stars.material.SetColor("_Color", clr);
        Debug.Log(Stars.material.color); */
    }
}