using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mawScript : MonoBehaviour
{
    public enemyHit eh;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Food"))
        {
            eh.hp = eh.hp + 20;
            Destroy(other.gameObject);
        }
    }
}
