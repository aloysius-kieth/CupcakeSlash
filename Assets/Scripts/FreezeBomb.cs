using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeBomb : Powerup
{
    public override void OnHit()
    {
        TimeManager.Instance.StartSlowMotion();
        base.OnHit();
    }

}
