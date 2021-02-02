using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CupcakeRainbow : Powerup
{
    public int rewardAmt = 100;

    public override void OnHit()
    {
        ScoreManager.Instance.ScoreRainbow(rewardAmt);
        base.OnHit();
    }

    public override void OnCollisionEnter2D(Collision2D col)
    {
        base.OnCollisionEnter2D(col);
        if (col.gameObject.tag == "Blade") SpawnSplit(col);
    }

    void SpawnSplit(Collision2D col)
    {
        ScoreManager.Instance.comboDisplay.UpdateBonusScoreText();
        TrinaxAudioManager.Instance.PlaySFX(TrinaxAudioManager.AUDIOS.RAINBOW, TrinaxAudioManager.AUDIOPLAYER.SFX4);
        GameManager.Instance.pDisplayText.DoAnim(POWERUPS.cupcakeRainbow);
        //GameObject splitCupcakes = ObjectPooler.Instance.GetPooledObject("RainbowCupcakeSplit");
        GameObject scoreParticle = ObjectPooler.Instance.GetPooledObject("100PointPS");

        //if (splitCupcakes != null)
        //{
        //    splitCupcakes.transform.position = transform.position;
        //    splitCupcakes.SetActive(true);
        //}

        if (scoreParticle != null)
        {
            scoreParticle.SetActive(true);
            scoreParticle.GetComponent<ParticleSystem>().Play();
        }
    }
}
