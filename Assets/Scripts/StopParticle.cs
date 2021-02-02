using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopParticle : MonoBehaviour
{
    ParticleSystem ps;
    private void Start()
    {
        ps = GetComponent<ParticleSystem>();
    }

    private void Update()
    {
        if (!ps.isPlaying)
        {
            ps.Stop();
            ps.gameObject.SetActive(false);
        }
        else return;
    }

}
