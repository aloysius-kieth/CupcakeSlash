using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TrinaxCanvas : MonoBehaviour
{
    public TrinaxAdminPanel adminPanel;
    public TextMeshProUGUI fpsText;
    Reporter reporter;

    bool isHide = true;

    #region SINGLETON
    public static TrinaxCanvas Instance { get; set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    #endregion

    private void Start()
    {
        reporter = FindObjectOfType<Reporter>();
    }

    private void Update()
    {
        ToggleAdminPanel(adminPanel);
        fpsText.text = "FPS: " + reporter.fps.ToString("F2");
    }

    void HideDebuggingStats()
    {
        if (isHide)
            fpsText.color = Color.clear;
        else
            fpsText.color = Color.black;
    }

    void ToggleAdminPanel(TrinaxAdminPanel _aP)
    {
        //if (Input.GetKeyDown(KeyCode.F12))
        //{
        //    isHide = !isHide;
        //    HideDebuggingStats();
        //    _aP.gameObject.SetActive(!_aP.gameObject.activeSelf);
        //    if (reporter == null) return;
        //    else
        //    {
        //        if (reporter.show)
        //        {
        //            reporter.show = !reporter.show;
        //        }
        //    }

        //}
        if (Input.GetKeyDown(KeyCode.F10) /*&& _aP.gameObject.activeSelf*/)
        {
            if (reporter == null) return;
            reporter.show = !reporter.show;
            if (reporter.show)
            {
                reporter.doShow();
            }
        }

    }

}
