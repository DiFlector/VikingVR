using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cloudMove : MonoBehaviour
{
    public GameObject[] cloudList;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void FixedUpdate()
    {
        foreach (var cloud in cloudList)
        {
            if (cloud.transform.position.z > -700)
            {
                cloud.transform.position = new Vector3(cloud.transform.position.x, cloud.transform.position.y, cloud.transform.position.z - 0.025f);
            }
            else
            {
                cloud.transform.position = new Vector3(cloud.transform.position.x, cloud.transform.position.y, cloud.transform.position.z + 1400);
            }
        }
    }
}
