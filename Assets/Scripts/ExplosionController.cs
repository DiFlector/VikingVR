using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ParticleSystemJobs;

public class ExplosionController : MonoBehaviour
{
    public ParticleSystem ps;
    public GameObject parent;

    private void OnTriggerEnter(Collider other)
    {
        parent.SetActive(true);
        ps.Play();
    }
}
