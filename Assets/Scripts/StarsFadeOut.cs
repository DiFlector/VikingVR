using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarsFadeOut : MonoBehaviour
{
    [SerializeField]
    private Transform Star;
    int i = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Star.transform.position(1 + i, 1 + i, 1 + i);
        i++;
    }
}
