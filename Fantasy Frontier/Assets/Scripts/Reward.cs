using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.ProBuilder;


public class Reward : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI claimText;
    TimeSpan refreshTime = new TimeSpan(1, 0, 0, 0);


    // Start is called before the first frame update
    void Start()
    {
  
    }

    // Update is called once per frame
    void Update()
    {
        SetText();
        ResetClaim();
   
    }

    //Save claim Date
    public void SaveDate()
    {
        DateTime dateNextClaimTime = DateTime.Now;
        dateNextClaimTime = dateNextClaimTime.Add(refreshTime);
        
        string nextClaimTime = dateNextClaimTime.ToString();

        PlayerPrefs.SetString("nextClaimDatePersist", nextClaimTime);
       
    }

    //Debug tool to reset claim
    public void ResetClaim()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            DateTime dateNextClaimTime = DateTime.Now;
            dateNextClaimTime = dateNextClaimTime.Subtract(refreshTime);

            string nextClaimTime = dateNextClaimTime.ToString();
            PlayerPrefs.SetString("nextClaimDatePersist", nextClaimTime);
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
       /* DateTime claimDate;
        claimDate = DateTime.Parse(PlayerPrefs.GetString("nextClaimDatePersist"));

        claimText.text = PlayerPrefs.GetString("nextClaimDatePersist");*/
       
        if(CheckIfCanClaim())
        {
            claimText.text = "Can Claim";
        }
        else
        {
            claimText.text = "Time Remaining: " + GetTimeLeftClaim();
        }

    }
}
