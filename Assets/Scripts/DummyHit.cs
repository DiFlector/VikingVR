using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DummyHit : MonoBehaviour
{
    public List<Collider> colliders;
    public List<float> damageList;
    private Vector3 pos1 = Vector3.zero;
    private Vector3 pos2 = Vector3.zero;
    public Vector3 velocity = Vector3.zero;
    public GameObject centerObject;
    protected bool hitAllowed = true;
    public int hp = 0;
    private void OnCollisionEnter(Collision collision)
    {
        WeaponObject obj = collision.collider.gameObject.GetComponentInParent<WeaponObject>();
        if (obj != null)
        {
            if (hitAllowed)
            {
                obj.enemyHit = true;
                obj.enemy = gameObject.GetComponent<DummyHit>();
                hitAllowed = false;
                StartCoroutine(attackTimer(2f));
            }
        }
    }
    private void Update()
    {
        pos1 = pos2;
        if (centerObject != null)
        {
            pos2 = centerObject.transform.position;
        }
        velocity = (pos2 - pos1)/Time.deltaTime;
    }
    IEnumerator attackTimer(float time)
    {
        yield return new WaitForSeconds(time);
        hitAllowed = true; 
    }
}