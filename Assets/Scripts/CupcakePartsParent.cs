using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CupcakePartsParent : MonoBehaviour
{
    public CupcakeParts[] cakePart;

    private void OnEnable()
    {
        if (GameManager.Instance.IsGameOver || !GameManager.Instance.IsReady) return;
        foreach (CupcakeParts part in cakePart)
        {
            part.gameObject.SetActive(true);
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "DeadZone") DeactivateSelf();
    }

    void DeactivateSelf()
    {
        foreach (CupcakeParts part in cakePart)
        {
            part.DeactivateSelf();
        }
        gameObject.SetActive(false);
    }
}
