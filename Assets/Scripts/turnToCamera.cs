using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class turnToCamera : MonoBehaviour
{
    public Transform camera;
    public RectTransform textMesh;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        textMesh.rotation = camera.rotation;
    }
}
