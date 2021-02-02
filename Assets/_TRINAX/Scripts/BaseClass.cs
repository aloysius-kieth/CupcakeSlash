using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(CanvasController))]
public abstract class BaseClass : MonoBehaviour
{
    // Component References
    protected CanvasController canvasController;
    protected TrinaxAdminPanel aP;

    public virtual IEnumerator Start()
    {
        //await new WaitUntil(() => TrinaxGlobal.Instance.isReady);
        yield return new WaitUntil(() => TrinaxGlobal.Instance.isReady);

        // Cache any component references
        canvasController = GetComponent<CanvasController>();

        //aP = TrinaxCanvas.Instance.adminPanel;

        //if (string.IsNullOrEmpty(TrinaxGlobal.Instance.gSettings.IP))
        //{
        //    Debug.Log("Mandatory fields in admin panel not filled!" + "\n" + "Opening admin panel...");
        //    aP.gameObject.SetActive(true);
        //}
        //else
        //{
        //    aP.gameObject.SetActive(false);
        //}
    }

    public virtual void Init()
    {
        RefreshSettings(TrinaxGlobal.Instance.gSettings, TrinaxGlobal.Instance.gameSettings, TrinaxGlobal.Instance.kinectSettings);
    }

    public virtual void RefreshSettings(GlobalSettings settings, GameSettings _gameSettings, KinectSettings _kinectSettings)
    {
        TrinaxGlobal.Instance.RefreshSettings(settings, _gameSettings, _kinectSettings);
        //wrmhlRead.Instance.Init();



        Debug.Log("Settings refreshed!");
    }
       
}
