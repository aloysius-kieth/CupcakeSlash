using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SFXEFFECTS
{
    CONFETTI,
    BALLOON,
}

// Not robust class at all
public class SFXMethods : MonoBehaviour
{
    public GameObject[] sfxs;

    public void DisableAll()
    {
        foreach (GameObject go in sfxs)
        {
            go.SetActive(false);
        }
    }

    public void EnableAll()
    {
        foreach (GameObject go in sfxs)
        {
            go.SetActive(true);
        }
    }

    public void ToggleSFX(SFXEFFECTS sfx)
    {
        int index = (int)sfx;
        for (int i = 0; i < sfxs.Length; i++)
        {
            GameObject s = sfxs[i];
            if (index == i)
            {
                //Debug.Log(sfxs[i].name);
                s.SetActive(true);
            }
            else s.SetActive(false);
 
        }
    }

}
