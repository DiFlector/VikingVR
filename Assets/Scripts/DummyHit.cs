using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DummyHit : enemyHit
{
    private void enemyDie()
    {
        //print("destroying");
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