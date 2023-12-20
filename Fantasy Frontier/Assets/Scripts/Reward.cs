using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.ProBuilder;
using UnityEngine.UI;

public class Reward : MonoBehaviour
{
    [SerializeField] Button claimButton;
  

    [SerializeField] TextMeshProUGUI claimText;
    [SerializeField] TextMeshProUGUI coinText;
    [SerializeField] Sprite canClaimSprite;
    [SerializeField] Sprite cannotClaimSprite;
    [SerializeField] GameObject mainPanel;
    [SerializeField] GameObject shopPanel;

    bool shopIsOpen = false;

    TimeSpan refreshTime = new TimeSpan(1, 0, 0, 0);
    [SerializeField] int totalClaimReward = 100;
    

    // Start is called before the first frame update
    void Start()
    {
        shopIsOpen = false;
        mainPanel.SetActive(true);
        shopPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        SetText();
        ResetClaim();
        ResetCoin();
    }

    //Claim Reward
    public void ClaimReward()
    {
        Debug.Log("Claim");

        DateTime dateNextClaimTime = DateTime.Now;
        dateNextClaimTime = dateNextClaimTime.Add(refreshTime);
        
        string nextClaimTime = dateNextClaimTime.ToString();

        PlayerPrefs.SetString("nextClaimDatePersist", nextClaimTime);

        PlayerPrefs.SetInt("playerCoin", PlayerPrefs.GetInt("playerCoin") + totalClaimReward);
       
    }

    //Debug tool to reset claim
    public void ResetClaim()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log("Reset Claim");
            
            DateTime dateNextClaimTime = DateTime.Now;
            dateNextClaimTime = dateNextClaimTime.Subtract(refreshTime);

            string nextClaimTime = dateNextClaimTime.ToString();
            PlayerPrefs.SetString("nextClaimDatePersist", nextClaimTime);
        }
    }
    //Debug tool to reset coin
    public void ResetCoin()
    {
        

        if (Input.GetKeyDown(KeyCode.T))
        {
            Debug.Log("Reset Coin");
            PlayerPrefs.SetInt("playerCoin", 0);
        }
    }


    //Return true if can claim
    public bool CheckIfCanClaim()
    {
        DateTime claimDate;
        claimDate = DateTime.Parse(PlayerPrefs.GetString("nextClaimDatePersist"));

        if (claimDate < DateTime.Now) 
        {
            return true;
        }
        else
        {
            return false; 
        }
    }

    public string GetTimeLeftClaim()
    {

        TimeSpan timeDifference;
        DateTime claimDate;
        DateTime currentDate = DateTime.Now;
        claimDate = DateTime.Parse(PlayerPrefs.GetString("nextClaimDatePersist"));

        timeDifference = currentDate.Subtract(claimDate) ;

        return timeDifference.ToString(@"hh\:mm\:ss");


    }


    public void SetText()
    {
        coinText.text = PlayerPrefs.GetInt("playerCoin").ToString();

        if(CheckIfCanClaim())
        {
            claimButton.enabled = true;
            claimButton.image.sprite = canClaimSprite;
            claimText.text = "Claim Now!";
        }
        else
        {
            claimButton.enabled = false;
            claimButton.image.sprite = cannotClaimSprite;
            claimText.text = "Time Remaining: " + GetTimeLeftClaim();
        }

    }

    public void OpenShop()
    {
        if(!shopIsOpen)
        {
            shopIsOpen = true;
            mainPanel.SetActive(false);
            shopPanel.SetActive(true);
        }
        else
        {
            shopIsOpen = false;
            mainPanel.SetActive(true);
            shopPanel.SetActive(false);

        }
       
    }
}
