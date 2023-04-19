using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using VirtualGrasp;

public class DummyHit : MonoBehaviour
{
    public List<Collider> colliders;
    public List<float> damageList;
    private Vector3 pos1 = Vector3.zero;
    private Vector3 pos2 = Vector3.zero;
    public Vector3 velocity = Vector3.zero;
    public GameObject centerObject;
    public bool alive = true;
    private int _hp = 100;
    public int hp
    {
        get { return _hp; }
        set
        {
            if (alive)
            {
                if (value <= 0)
                {
                    _hp = 0;
                    alive = false;
                    dummyDestroy();
                }
                else { _hp = value; }
            }
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (alive)
        {
            WeaponObject obj = collision.collider.gameObject.GetComponentInParent<WeaponObject>();
            if (obj != null)
            {
                obj.enemyHit = true;
                obj.enemy = gameObject.GetComponent<DummyHit>();
            }
        }
    }
    private void Update()
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
    private void dummyDestroy()
    {
        GameObject obj = gameObject;
        Destroy(obj.transform.parent.parent.gameObject.transform.GetChild(2).gameObject);
        obj = obj.transform.GetChild(0).gameObject;
        foreach (Collider col in obj.GetComponents<Collider>())
        {
            col.enabled = false;
        }
        obj = obj.transform.GetChild(0).GetChild(0).gameObject;
        obj.AddComponent<SphereCollider>();
        obj.AddComponent<Rigidbody>();
        obj.transform.position += new Vector3(0.001f, 0.004f, 0.001f);
        obj = obj.transform.parent.gameObject;
        obj.AddComponent<CapsuleCollider>();
        obj.AddComponent<Rigidbody>();
        obj.transform.position += new Vector3(-0.001f, 0.003f, -0.001f);
        obj = obj.transform.parent.parent.gameObject;
        obj.GetComponent<Rigidbody>().useGravity = true;
        obj.GetComponent<Rigidbody>().freezeRotation = false;
        obj.GetComponent<Rigidbody>().constraints = new RigidbodyConstraints();
        obj.transform.position += new Vector3(0.001f, 0.002f, 0.001f);
        obj = obj.transform.parent.GetChild(1).gameObject;
        obj.GetComponent<Rigidbody>().useGravity = true;
        obj.GetComponent<Rigidbody>().freezeRotation = false;
        obj.GetComponent<Rigidbody>().constraints = new RigidbodyConstraints();
        obj.transform.position += new Vector3(0.001f, 0.001f, 0.001f);
        obj.transform.rotation = new Quaternion(0.001f, 0.001f, 0.001f, 0.001f);
        obj = obj.transform.parent.parent.GetChild(0).gameObject;
        obj.GetComponent<Rigidbody>().useGravity = true;
        obj.GetComponent<Rigidbody>().isKinematic = false;
        obj.GetComponent<Rigidbody>().freezeRotation = false;
        obj.GetComponent<Rigidbody>().constraints = new RigidbodyConstraints();
        obj.transform.position += new Vector3(0.001f, 0.001f, 0.001f);
    }
}