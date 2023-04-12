using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponObject : MonoBehaviour
{
    public string type;
    public float damageMultiplier;
    public float damage;
    public Vector3 velocity;
    public GameObject blade;
    public Vector3 pos1;
    public Vector3 pos2;
    public bool enemyHit = false;
    public Vector3 enemyVelocity = new Vector3(0, 0, 0);
    public WeaponObject(string type, float damageMultiplier, Vector3 velocity, GameObject blade = null)
    {
        this.type = type;
        this.damageMultiplier = damageMultiplier;
        this.damage = 0;
        this.velocity = new Vector3(0, 0, 0);
        if (blade != null ) { this.blade = new GameObject(); }
        this.pos1 = blade.transform.position;
        this.pos2 = blade.transform.position;
    }
    public Vector3 getVelocity(Vector3 pos1, Vector3 pos2, float time)
    {
        return (pos2 - pos1)/time;
    }
    public float getHitVelocity(Vector3 weaponVelocity, Vector3 enemyVelocity)
    {
        Vector3 absVelocity = weaponVelocity - enemyVelocity;
        return absVelocity.magnitude;
    }
    public float getDamage(string type, float damageMultiplier, float hitVelocity)
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
        return dmg;
    }
    private void Update()
    {
        pos1 = pos2;
        if (blade != null)
        {
            pos2 = blade.transform.position;
        }
        velocity = getVelocity(pos1, pos2, Time.deltaTime);
    }
}