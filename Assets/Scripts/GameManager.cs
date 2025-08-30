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

    public Button bet3070Button;

    public Button bet7030Button;

    public TextMeshProUGUI validBetText;

    public TextMeshProUGUI lossText;

    public TextMeshProUGUI BiscuitCounter;

    public TextMeshProUGUI ShopDirection;

    public int bones;

    private int round;

    public int biscuits;

    private int bonesBet;

    private int biscuitRequest;

    public Button buyButton;

    public TMP_InputField biscuitAmount;

    public TextMeshProUGUI validBiscuitText;

    public TextMeshProUGUI biscuitDirectionText;

    public SpriteRenderer wheelSprite;

    public Image DogHouse;

    public AnimationRemote animationManager;
    
    public SoundManager soundManager;

    public AudioClip buySound;

    public AudioClip endSound;

    public AudioClip winSound;

    public AudioClip loseSound;

    public Animator animator; // Assign your Animator here in the Inspector

    public Button plus1Button;

    public Button halfButton;

    public Button allButton;

    void Start()
    {
        betAmount.text = "0";
        bones = 100;
        SetBoneText();
        round = 0;
        SetRoundText();
        biscuitAmount.gameObject.SetActive(false);
        biscuitDirectionText.gameObject.SetActive(false);
        toggleConfirmSprites(false);
        validBetText.gameObject.SetActive(false);
        bet1Button.gameObject.SetActive(true);
        bet3070Button.gameObject.SetActive(true);
        bet7030Button.gameObject.SetActive(true);
        ShopDirection.gameObject.SetActive(false);
        wheelSprite.enabled = false;
        DogHouse.enabled = false;
    }

    void toggleConfirmSprites(bool show)
    {
        validBiscuitText.gameObject.SetActive(!show);
        bet1Button.gameObject.SetActive(!show);
        bet3070Button.gameObject.SetActive(!show);
        bet7030Button.gameObject.SetActive(!show);
        betAmount.gameObject.SetActive(!show);
        buyButton.gameObject.SetActive(show);
        biscuitAmount.gameObject.SetActive(show);
        biscuitDirectionText.gameObject.SetActive(show);
        ShopDirection.gameObject.SetActive(show);
    }

    void SetBoneText()
    {
        bonesCounter.text = bones.ToString();
    }
    
    void SetBiscuitText()
    {
        BiscuitCounter.text = biscuits.ToString();
    }

    void SetRoundText()
    {
        int roundsLeft = 3 - (round % 3) -1;
        roundCounter.text = "Rounds until exchange: " + roundsLeft.ToString();

        if(roundsLeft == 0){
            toggleConfirmSprites(true);
        }
    }

    public void UpdateUIAfterBet(bool win, int boneDiff)
    {
        if(win)
        {
            soundManager.PlaySoundClip(winSound);
            lossText.text = "You won " + boneDiff + " bones";
        }
        else{
            lossText.text = "You lost " + boneDiff + " bones";
            CheckLoss();
        }
        SetBoneText();

        round++;
        SetRoundText();

        betAmount.text = "0";
        bonesBet = 0;
        lossText.gameObject.SetActive(true);
    }

    public void PlaceBet5050()
    {
        int result = Random.Range(0,2);
        wheelSprite.enabled = true;
        DogHouse.enabled = true;
        if(result == 0)
        {
            bones = bones + bonesBet;
            animationManager.HandleGameEnd(true);
        }
        else
        {            
            bones = bones - bonesBet;
            animationManager.HandleGameEnd(false);
        }
        UpdateUIAfterBet(result == 0, bonesBet);
    }

    public void PlaceBet3070()
    {
        int result = Random.Range(0,10);
        wheelSprite.enabled = true;
        DogHouse.enabled = true;
        double payOut = (3.33 * bonesBet) - bonesBet;
        if(result < 3)
        {
            bones = bones + (int)System.Math.Floor(payOut);
            animationManager.HandleGameEnd(true);
            UpdateUIAfterBet(true, (int)System.Math.Floor(payOut));
        }
        else
        {
            bones = bones - bonesBet;
            animationManager.HandleGameEnd(false);
            UpdateUIAfterBet(false, bonesBet);
        }
    }

    public void PlaceBet7030()
    {
        int result = Random.Range(0,10);
        wheelSprite.enabled = true;
        DogHouse.enabled = true;
        double payOut = (1.43 *bonesBet) - bonesBet;
        if(result >= 3)
        {
            animationManager.HandleGameEnd(true);
            bones = bones + (int)System.Math.Floor(payOut);
            UpdateUIAfterBet(true, (int)System.Math.Floor(payOut));
        }
        else
        {
            animationManager.HandleGameEnd(false);
            bones = bones - bonesBet;
            UpdateUIAfterBet(false, bonesBet);
        }
    }

    void CheckLoss()
    {
        if(bones <= 0)
        {
            soundManager.PlaySoundClip(endSound);
            bet1Button.gameObject.SetActive(false);
            bet3070Button.gameObject.SetActive(false);
            bet7030Button.gameObject.SetActive(false);
            lossText.text = "You ran out of bones and ended with " + biscuits + " biscuits";
            lossText.gameObject.SetActive(true);
            Destroy(gameObject);
        }
        else{
            soundManager.PlaySoundClip(loseSound);
        }
    }

    public void CheckAmount(string betAmount)
    {
        lossText.gameObject.SetActive(false);
        bonesBet = int.Parse(betAmount);
        if(bonesBet <= 0 || bonesBet > bones){
            bet1Button.interactable = false;
            bet3070Button.interactable = false;
            bet7030Button.interactable = false;
            validBetText.gameObject.SetActive(true);
        }
        else{
            bet1Button.interactable = true;
            bet3070Button.interactable = true;
            bet7030Button.interactable = true;
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

        biscuitAmount.text = "0";
        biscuitRequest = 0;
        soundManager.PlaySoundClip(buySound);
    }

    public void OnHitPlus1Button()
    {
        int roundsLeft = 3 - (round % 3) -1;
        if(roundsLeft == 0)
        {
            int prevBiscuitInput = int.Parse(biscuitAmount.text);
            biscuitAmount.text = (prevBiscuitInput + 1).ToString();
        }
        else
        {
            int prevBetInput = int.Parse(betAmount.text);
            betAmount.text = (prevBetInput + 1).ToString();
        }
    }

    public void OnHitHalfButton()
    {
        int roundsLeft = 3 - (round % 3) -1;
        if(roundsLeft == 0)
        {
            int halfBiscuitCanBuy = bones/200;
            biscuitAmount.text = halfBiscuitCanBuy.ToString();
        }
        else
        {
            int halfBones = bones/2;
            betAmount.text = halfBones.ToString();
        }
    }

    public void OnHitAllButton()
    {
        int roundsLeft = 3 - (round % 3) -1;
        if(roundsLeft == 0)
        {
            int AllBiscuits = bones/100;
            biscuitAmount.text = AllBiscuits.ToString();
        }
        else
        {
            int AllBones = bones;
            betAmount.text = AllBones.ToString();
        }
    }

}
