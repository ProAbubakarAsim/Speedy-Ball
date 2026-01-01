/// <summary>
/// This Script is already used on "Ads Handler" child object, The purpose of this script to use to Load and Call Interstial Ad as per Google and Admob Ads Policy
/// </summary>

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class intestitialManager : MonoBehaviour
{
    public void OnEnable()
    {
        if(FindObjectOfType<ADsHandler>())
        {
            FindObjectOfType<ADsHandler>().loadInterstitialAD();
        }
    }

    public void OnDisable()
    {
        if (FindObjectOfType<ADsHandler>())
        {
            FindObjectOfType<ADsHandler>().showInterstitialAD();
        }
    }
}
