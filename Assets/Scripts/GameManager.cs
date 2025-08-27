using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI bonesCounter;

    public TextMeshProUGUI roundCounter;

    public TMP_InputField betAmount;

    public Button bet1Button;

    public TextMeshProUGUI validBetText;

    public TextMeshProUGUI lossText;

    public TextMeshProUGUI BiscuitCounter;

    public int bones;

    private int round;

    public int biscuits;

    private int bonesBet;

    private int biscuitRequest;

    public Button buyButton;
    public SpriteRenderer bonesbuyButton;
    public SpriteRenderer biscuitbuyButton;
    public TextMeshProUGUI bonesBuyNum;
    public TextMeshProUGUI biscuitBuyNum;

    public TMP_InputField biscuitAmount;

    public TextMeshProUGUI validBiscuitText;

    public TextMeshProUGUI biscuitDirectionText;

    void Start()
    {
        bones = 100;
        SetBoneText();
        round = 0;
        SetRoundText();
        biscuitAmount.gameObject.SetActive(false);
        biscuitDirectionText.gameObject.SetActive(false);
        toggleConfirmSprites(false);
        validBetText.gameObject.SetActive(false);
    }

    void toggleConfirmSprites(bool show)
    {
        validBiscuitText.gameObject.SetActive(!show);
        bet1Button.gameObject.SetActive(!show);
        betAmount.gameObject.SetActive(!show);
        buyButton.gameObject.SetActive(show);
        bonesbuyButton.enabled = show;
        biscuitbuyButton.enabled = show;
        bonesBuyNum.gameObject.SetActive(show);
        biscuitBuyNum.gameObject.SetActive(show);
        biscuitAmount.gameObject.SetActive(show);
        biscuitDirectionText.gameObject.SetActive(show);
    }

    void SetBoneText()
    {
        bonesCounter.text = ": " + bones.ToString();
    }
    
    void SetBiscuitText()
    {
        BiscuitCounter.text = ": " + biscuits.ToString();
    }

    void SetRoundText()
    {
        int roundsLeft = 3 - (round % 3) -1;
        roundCounter.text = "Rounds until exchange: " + roundsLeft.ToString();

        if(roundsLeft == 0){
            toggleConfirmSprites(true);
        }
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

        betAmount.text = "";
        bonesBet = 0;
    }

    void CheckLoss()
    {
        if(bones <= 0)
        {
            Destroy(gameObject);
            lossText.text = "You ran out of bones and ended with " + biscuits + " biscuits";
            lossText.gameObject.SetActive(true);
        }
    }

    public void CheckAmount(string betAmount)
    {
        bonesBet = int.Parse(betAmount);
        if(bonesBet <= 0 || bonesBet > bones){
            bet1Button.interactable = false;
            validBetText.gameObject.SetActive(true);
        }
        else{
            bet1Button.interactable = true;
            validBetText.gameObject.SetActive(false);
        }
    }

    public void CheckBiscuitAmount(string biscuitAmount1)
    {
        biscuitRequest = int.Parse(biscuitAmount1);
        int bonesRequest = biscuitRequest * 100;
        if(bonesRequest >= bones){
            buyButton.interactable = false;
            validBiscuitText.gameObject.SetActive(true);
        }
        else{
            buyButton.interactable = true;
            validBiscuitText.gameObject.SetActive(false);
        }
    }

    public void ExchangeBones()
    {
        biscuits += biscuitRequest;
        bones -= biscuitRequest * 100;
        SetBiscuitText();

        round++;
        SetRoundText();
        SetBoneText();

        toggleConfirmSprites(false);

        biscuitAmount.text = "";
        biscuitRequest = 0;
    }
    
}
