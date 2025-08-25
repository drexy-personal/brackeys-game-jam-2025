using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI bonesCounter;

    public TextMeshProUGUI roundCounter;

    public TMP_InputField betAmount;

    public Button bet1Button;

    public TextMeshProUGUI validBetText;

    public TextMeshProUGUI lossText;

    public int bones;

    private int round;

    private int bonesBet;

    // public TextMeshProUGUI  BiscuitCounter;

    void Start()
    {
        bones = 100;
        SetBoneText();
        round = 0;
        SetRoundText();
        validBetText.gameObject.SetActive(false);
        lossText.gameObject.SetActive(false);
    }

    void SetBoneText()
    {
        bonesCounter.text = ": " + bones.ToString();
    }

    void SetRoundText()
    {
        int roundsLeft = 3 - (round % 3);
        roundCounter.text = "Rounds until exchange: " + roundsLeft.ToString();
    }

    public void PlaceBet1()
    {
        int result = Random.Range(0,2);
        if(result == 0)
        {
            bones = bones + bonesBet;
        }
        else
        {
            bones = bones - bonesBet;
        }
        CheckLoss();
        SetBoneText();

        round++;
        SetRoundText();
    }

    void CheckLoss()
    {
        if(bones <= 0)
        {
            Destroy(gameObject);
            lossText.text = "You ran out of bones!";
            lossText.gameObject.SetActive(true);
        }
    }

    public void CheckAmount(string betAmount)
    {
        bonesBet = int.Parse(betAmount);
        if(bonesBet <= 0 || bonesBet > bones){
            validBetText.gameObject.SetActive(true);
        }
        else{
            validBetText.gameObject.SetActive(false);
        }
    }
}
