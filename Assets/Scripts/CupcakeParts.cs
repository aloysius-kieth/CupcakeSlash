using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CupcakeParts : MonoBehaviour
{
    public Rigidbody2D rb2d;

    public float torqueAmt = 2.5f;
    float amtToTorque;

    public float forceAmt = 50;
    Vector2 forceVec;

    public Vector2 startPos;

    private void OnEnable()
    {
        //if (!gameObject.activeSelf) gameObject.SetActive(true);


        if (Random.value > 0.5f)
        {
            amtToTorque = -torqueAmt;
            forceVec = new Vector2(forceAmt, forceAmt);
        }
        else
        {
            amtToTorque = torqueAmt;
            forceVec = new Vector2(-forceAmt, forceAmt);
        }
        rb2d.AddForce(forceVec, ForceMode2D.Impulse);
    }

    private void FixedUpdate()
    {
        if (GameManager.Instance.IsGameOver || !GameManager.Instance.IsReady) return;
        rb2d.AddTorque(amtToTorque);
    }

    public void DeactivateSelf()
    {
        transform.localPosition = startPos;
        transform.localRotation = Quaternion.identity;
        rb2d.velocity = Vector2.zero;
        gameObject.SetActive(false);
    }

}
