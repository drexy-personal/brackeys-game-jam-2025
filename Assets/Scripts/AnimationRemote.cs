using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AnimationRemote : MonoBehaviour
{
    public GameManager gamemanager;
    public Animator targetAnimator; // Assign your Animator here in the Inspector

    public void HandleGameEnd(bool result)
    {
        targetAnimator = GetComponent<Animator>();
        switch (result)
        {
            case true:
                targetAnimator.SetTrigger("WinTrigger");
                break;
            case false:
                targetAnimator.SetTrigger("LoseTrigger");
                break;
        }
    }
}
