using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyHit : MonoBehaviour
{
    public List<Collider> colliders;
    public List<float> damageList;
    protected Vector3 pos1 = Vector3.zero;
    protected Vector3 pos2 = Vector3.zero;
    public Vector3 velocity = Vector3.zero;
    public GameObject centerObject;
    public bool alive = true;
    protected int _hp = 20;
    public int hp
    {
        get { return _hp; }
        set
        {
            print("in setter");
            if (alive)
            {
                if (value <= 0)
                {
                    
                    print("dying");
                    _hp = 0;
                    alive = false;
                    gameObject.GetComponent<EnemyAI>().death();
                    //enemyDie();
                }
                else { _hp = value; }
            }
        }
    }
    protected void OnCollisionEnter(Collision collision)
    {
        //print("collision entered");
        if (alive)
        {
            WeaponObject obj = collision.collider.gameObject.GetComponentInParent<WeaponObject>();
            //print(obj);
            if (obj != null)
            {
                obj.enemyHit = true;
                obj.enemy = gameObject.GetComponent<enemyHit>();
            }
        }
    }
    protected void Update()
    {
        if (alive)
        {
            pos1 = pos2;
            if (centerObject != null)
            {
                pos2 = centerObject.transform.position;
            }
            velocity = (pos2 - pos1) / Time.deltaTime;
        }
    }

    protected void enemyDie()
    {
        print("enemy destroyer");
        gameObject.GetComponent<Rigidbody>().freezeRotation = false;
        Destroy(gameObject.GetComponent<EnemyAI>());
        Destroy(gameObject.GetComponent<AILerp>());
    }
}
