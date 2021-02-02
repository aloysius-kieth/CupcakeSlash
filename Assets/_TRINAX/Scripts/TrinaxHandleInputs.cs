using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Inputs relating to debugging, put them here!
public class TrinaxHandleInputs : MonoBehaviour
{
    //TrinaxAdminPanel aP;
    bool isReady = false;

    IEnumerator Start()
    {
        isReady = false;
        yield return new WaitUntil(() => TrinaxGlobal.Instance.isReady);
        //await new WaitUntil(() => TrinaxGlobal.Instance.isReady);

        isReady = true;

        //aP = TrinaxCanvas.Instance.adminPanel;
    }

    private void Update()
    {
        if (!isReady) return;

        //if (aP != null && aP.gameObject.activeSelf)
        //{
            if (Input.GetKeyDown(KeyCode.F5))
            {
                Debug.Log("Going back to scene 0");
                TrinaxHelperMethods.ChangeLevel((int)SCENE.MAIN);
            }
        //}

        if (Input.GetKeyDown(KeyCode.F11))
        {
            Cursor.visible = !Cursor.visible;
        }
    }
}
