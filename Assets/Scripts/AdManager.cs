using System.Collections;
using System.Collections.Generic;
using UnityEditor.Advertisements;
using UnityEngine;
using UnityEngine.Advertisements;

public class AdManager : MonoBehaviour
{
    public static AdManager Instance { get { return instance; } }
    public static AdManager instance;

    [SerializeField] private string gameID;
    [SerializeField] private string rewardedVideoPlacementID;




    private void Awake()
    {
        instance = this; 
        
    }

}
