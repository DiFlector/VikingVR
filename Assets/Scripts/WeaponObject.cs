using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WeaponObject : MonoBehaviour
{
    public string type;
    public float damageMultiplier;
    private float damage;
    private Vector3 velocity;
    [SerializeField]
    private GameObject blade;
    private Vector3 pos1;
    private Vector3 pos2;
    public bool enemyHit;
    public DummyHit enemy;
    private List<Collider> collisionList = new List<Collider>();
    public Vector3 getVelocity(Vector3 pos1, Vector3 pos2, float time)
    {
        return (pos2 - pos1)/time;
    }
    public float getHitVelocity(Vector3 weaponVelocity, Vector3 enemyVelocity)
    {
        Vector3 absVelocity = weaponVelocity - enemyVelocity;
        return absVelocity.magnitude;
    }
    public float getMultiplier(List<Collider> collisionList, List<Collider> enemyColliders, List<float> multipliers)
    {
        float res = -1;
        float m = multipliers[0];
        for (int i = 0; i < enemyColliders.Count; i++)
        {
            if (collisionList.Contains(enemyColliders[i]))
            {
                if (multipliers[i] > res)
                {
                    res = multipliers[i];
                }
            }
            if (multipliers[i] < m)
            {
                m = multipliers[i];
            }
        }
        if (res == -1)
        {
            return m;
        }
        else
        {
            return res;
        }
    }
    public float getDamage(string type, float damageMultiplier, float hitVelocity, float enemyMultiplier)
    {
        float dmg = 1;
        switch (type)
        {
            case "bladeWeapon":
                switch (hitVelocity)
                {
                    case < 1:
                        dmg *= damageMultiplier ;
                        break;
                    case < 5:
                        dmg *= damageMultiplier * 1.5f;
                        break;
                    case < 10:
                        dmg *= damageMultiplier * 2f;
                        break;
                    default:
                        dmg *= damageMultiplier * 2.5f;
                        break;
                }
                break;
            case "bluntWeapon":
                switch (hitVelocity)
                {
                    case < 1:
                        dmg = 0;
                        break;
                    case < 5:
                        dmg *= damageMultiplier * 1f;
                        break;
                    case < 10:
                        dmg *= damageMultiplier * 2f;
                        break;
                    default:
                        dmg *= damageMultiplier * 2.5f;
                        break;
                }
                break;
            default:
                dmg = 0;
                break;
        }
        return dmg * enemyMultiplier;
    }
    private void Update()
    {
        pos1 = pos2;
        if (blade != null)
        {
            pos2 = blade.transform.position;
        }
        velocity = getVelocity(pos1, pos2, Time.deltaTime);
        if (enemyHit)
        {
            float damage = getDamage(type, damageMultiplier, getHitVelocity(velocity, enemy.velocity), getMultiplier(collisionList, enemy.colliders, enemy.damageList));
            enemy.hp -= (int)Mathf.Ceil(damage);
            enemyHit = false;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        collisionList.Add(collision.collider);
    }
    private void OnCollisionExit(Collision collision)
    {
        collisionList.Remove(collision.collider);
    }
}