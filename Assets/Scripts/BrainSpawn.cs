//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class BrainSpawn : StateMachineBehaviour
//{
//    float timer;
//    float interval;

//    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
//    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
//    {
//        Brain.Instance.brainState = BRAINSTATE.SPAWN;
//        Brain.Instance.brainDoSpawn = true;

//        interval = Random.Range(TrinaxGlobal.Instance.gameSettings.minSpawnTime, TrinaxGlobal.Instance.gameSettings.maxSpawnTime);
//        Debug.Log(interval);
//    }

//    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
//    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
//    {
//        timer += Time.deltaTime;
//        if (Brain.Instance.spawnInARow)
//        {
//            if (timer > interval /*+ Brain.Instance.listOfObjects.Count*/)
//            {
//                Debug.Log("spawnInARow");
//                timer = 0;
//                // Exit out of this state
//                animator.SetBool("IdleNow", true);
//            }
//        }
//        if (Brain.Instance.spawnAllAtOnce)
//        {
//            if (timer > interval)
//            {
//                Debug.Log("spawnAllAtOnce");
//                timer = 0;
//                // Exit out of this state
//                animator.SetBool("IdleNow", true);
//            }
//        }
//    }

//    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
//    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
//    {
//        Brain.Instance.spawnAllAtOnce = false;
//        Brain.Instance.spawnInARow = false;
//        animator.SetBool("IdleNow", false);
//    }
//}
