using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.UI;

public class CanvasController : MonoBehaviour {

    public CanvasGroup[] pageList;
    //public int gameStageIndex;
    // Use this for initialization
    void Start () {
        if (pageList.Length == 0) return;
        for (int i = 1; i < pageList.Length; ++i) {
            pageList[i].alpha = 0;
            pageList[i].gameObject.SetActive(false);
        }
        for (int i = 1; i < pageList.Length; ++i) {
            pageList[i].alpha = 0;
            pageList[i].gameObject.SetActive(false);
        }
        pageList[0].alpha = 1;
        pageList[0].gameObject.SetActive(true);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public int getCanvasIndex() {
        int index = 0;
        for (; index < pageList.Length; ++index) {
            if (pageList[index].alpha > 0.5) {
                break;
            }
        }
        return index;
    }

    public void TransitToCanvas(int tobeActive, float duration, System.Action callback = null) {
        int oldCanvas = getCanvasIndex();
        if (oldCanvas == tobeActive) {
            Debug.LogWarning("Fading to same Canvas");
            return;
        }
        Button[] canvasButtons = pageList[tobeActive].GetComponentsInChildren<Button>();
        if (canvasButtons != null)
        {
            foreach (Button obj in canvasButtons)
            {
                obj.interactable = false;
            }
        }
        else { Debug.Log("No buttons found in canvas!"); }
 
        pageList[oldCanvas].DOFade(0, duration).OnComplete(() => {
            pageList[oldCanvas].gameObject.SetActive(false);
            /*
            if (oldCanvas == gameStageIndex) {
                isGameMode = false;
                pageList[1].alpha = 0;
                pageList[1].gameObject.SetActive(false);
                //VuforiaBehaviour.Instance.enabled = true;
            }
            */
        });
        pageList[tobeActive].gameObject.SetActive(true);


        pageList[tobeActive].DOFade(1, duration).OnComplete(() => {

            foreach (Button obj in canvasButtons) {
                obj.interactable = true;
            }

            if(callback != null)
            {
                callback();
            }
        });
    }
}
