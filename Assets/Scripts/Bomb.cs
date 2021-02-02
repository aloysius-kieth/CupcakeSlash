using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class Bomb : Interactable
{
    public int minusAmt = 50;

    public override void Awake()
    {
        base.Awake();
        minForce = TrinaxGlobal.Instance.gameSettings.minForceCupcake;
        maxForce = TrinaxGlobal.Instance.gameSettings.maxForceCupcake;
        minSideForce = TrinaxGlobal.Instance.gameSettings.minSideForceCupcake;
        maxSideForce = TrinaxGlobal.Instance.gameSettings.maxSideForceCupcake;
    }

    public override void OnCollisionEnter2D(Collision2D col)
    {
        base.OnCollisionEnter2D(col);

        if (col.gameObject.tag == "Blade")
        {
            TrinaxAudioManager.Instance.PlaySFX(TrinaxAudioManager.AUDIOS.BOMB, TrinaxAudioManager.AUDIOPLAYER.SFX4);
            GameObject scoreParticle = ObjectPooler.Instance.GetPooledObject("50PointPS");
            GameObject bombParticle = ObjectPooler.Instance.GetPooledObject("SmallExplosionFire");

            if (scoreParticle != null)
            {
                scoreParticle.SetActive(true);
                scoreParticle.GetComponent<ParticleSystem>().Play();
            }

            if (bombParticle != null)
            {
                bombParticle.transform.position = transform.position;
                bombParticle.SetActive(true);
                bombParticle.GetComponent<ParticleSystem>().Play();
            }
            SpawnManager.Instance.inPlayList.Remove(gameObject);
            DeactivateSelf();
        }
    }

    public override void DeactivateSelf()
    {
        base.DeactivateSelf();
        SpawnManager.Instance.bombList.Remove(_type);
    }
}
