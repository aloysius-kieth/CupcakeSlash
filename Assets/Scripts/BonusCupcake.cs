using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusCupcake : MonoBehaviour
{
    public int numOfTimesToCut = 25;
    int cutCount;
    public float diameter;
    CircleCollider2D col;

    private void Awake()
    {
        col = GetComponent<CircleCollider2D>();
        diameter = col.radius * 2;
    }

    //Vector2 startPos;
    //private void OnCollisionEnter2D(Collision2D col)
    //{
    //    if (col.transform.tag == "Blade")
    //    {

    //    }
    //}

    //Vector2 endPos;
    //Vector2 calculatedPos;
    //private void OnCollisionExit2D(Collision2D col)
    //{
    //    if (col.transform.tag == "Blade")
    //    {
    //        endPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    //        Debug.Log("endPos " + endPos);
    //        calculatedPos = endPos - startPos;
  
    //        Debug.Log("calculatedPos " + calculatedPos);
    //        float lengthOfCut = calculatedPos.magnitude;
    //        Debug.Log("lengthofcut " + lengthOfCut);
    //        if (lengthOfCut > diameter)
    //        {
    //            cutCount++;
    //            startPos = Vector2.zero;
    //            endPos = Vector2.zero;
    //            calculatedPos = Vector2.zero;
    //            Debug.Log("Cut: " + cutCount);
    //        }
 
    //    }
    //}
}
