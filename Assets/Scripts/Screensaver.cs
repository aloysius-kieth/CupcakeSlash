//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;
//using DG.Tweening;

//using System;

//public enum SCREENSAVER
//{
//    MAINMENU,
//    LEADERBOARD,
//}

//public class Screensaver : MonoBehaviour
//{
//    public SCREENSAVER state;
//    float timer;
//    float timeToChange = 10f;

//    [HideInInspector]
//    public bool transitToLB = true;

//    Animator anim;

//    private void OnDisable()
//    {
//        timer = 0;  
//    }

//    private void Start()
//    {
//        anim = GetComponent<Animator>();
//        timeToChange = TrinaxGlobal.Instance.timeToLoop;
//    }

//    private void Update()
//    {
//        //if (!MainManager.Instance.IsPageActive(STATES.SCREENSAVER)) return;

//        timer += Time.deltaTime;

//        if (timer > timeToChange)
//        {
//            if (transitToLB)
//            {
//                ToLeaderboard();
//            }
//            else if (!transitToLB)
//            {
//                ToMainMenu();
//            }
//            timer = 0;
//            transitToLB = !transitToLB;
//        }

//        if (state == SCREENSAVER.LEADERBOARD)
//        {
//            if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
//            {
//                timer = 0;
//                ToMainMenu();
//                transitToLB = !transitToLB;
//            }
//        }
//    }

//    public void ToLeaderboard()
//    {
//        state = SCREENSAVER.LEADERBOARD;
//        anim.SetTrigger("ToLeaderboard");
//    }

//    void ToMainMenu()
//    {
//        state = SCREENSAVER.MAINMENU;
//        anim.SetTrigger("ToMainMenu");
//    }

//}
