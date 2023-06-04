using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mawScript : MonoBehaviour
{
    public int hp;
    int maxhp = 110;
    void Start()
    {
        hp = 50;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Food"))
        {
            if (hp + 20 <= maxhp)
            {
                hp += 20;
                Destroy(other.gameObject);
            }
        }
    }
    
    void Update()
    {
        
    }
}
