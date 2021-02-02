using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cupcake : Interactable
{
    public int rewardAmt;
    SpriteRenderer spriteRend;

    float scaleFactor;
    float sizeMax = 0.3f;
    float sizeMin = 0.15f;

    BladeController blade;

    public override void Awake()
    {
        base.Awake();
        spriteRend = GetComponent<SpriteRenderer>();
    }

    void RandomCupcake()
    {
        int index = Random.Range(0, SpawnManager.Instance.cupcakeSprites.Length);

        spriteRend.sprite = SpawnManager.Instance.cupcakeSprites[index];
        _type = (INTERACTABLE_TYPE)index;
    }

    public override void OnEnable()
    {
        RandomCupcake();
        minForce = TrinaxGlobal.Instance.gameSettings.minForceCupcake;
        maxForce = TrinaxGlobal.Instance.gameSettings.maxForceCupcake;
        minSideForce = TrinaxGlobal.Instance.gameSettings.minSideForceCupcake;
        maxSideForce = TrinaxGlobal.Instance.gameSettings.maxSideForceCupcake;
        rewardAmt = TrinaxGlobal.Instance.gameSettings.rewardAmt;
        base.OnEnable();

        scaleFactor = Random.Range(sizeMin, sizeMax);
        transform.localScale = new Vector3(scaleFactor, scaleFactor, 1);
    }

    public override void OnCollisionEnter2D(Collision2D col)
    {
        base.OnCollisionEnter2D(col);

        if (col.gameObject.tag == "Blade") SpawnSplit(col);
    }

    void SpawnSplit(Collision2D col)
    {
        SpawnManager.Instance.inPlayList.Remove(gameObject);
        GameObject splitCupcakes = null;
        if (_type == INTERACTABLE_TYPE.CUPCAKERED)  splitCupcakes = ObjectPooler.Instance.GetPooledObject("RedCupcakeSplit");
        else if (_type == INTERACTABLE_TYPE.CUPCAKEPURPLE) splitCupcakes = ObjectPooler.Instance.GetPooledObject("PurpleCupcakeSplit");
        else if (_type == INTERACTABLE_TYPE.CUPCAKEORANGE)    splitCupcakes = ObjectPooler.Instance.GetPooledObject("OrangeCupcakeSplit");
        else if (_type == INTERACTABLE_TYPE.CUPCAKEGREEN) splitCupcakes = ObjectPooler.Instance.GetPooledObject("GreenCupcakeSplit");
        //blade = col.gameObject.GetComponent<BladeController>();
        //Vector3 direction = ((blade != null) ? blade.CutPos : (col.transform.position - transform.position).normalized);

        // Change rotation of sliced obj based on direction vector
        //Quaternion rotation = Quaternion.LookRotation(direction);

        if (splitCupcakes != null)
        {
            splitCupcakes.transform.localScale = new Vector3(scaleFactor, scaleFactor, 1);
            splitCupcakes.transform.position = transform.position;
            splitCupcakes.SetActive(true);
        }
    }
}
