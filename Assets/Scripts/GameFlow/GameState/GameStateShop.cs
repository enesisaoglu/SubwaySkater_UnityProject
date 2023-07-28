using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class GameStateShop : GameState
{
    public GameObject shopUI;
    public TextMeshProUGUI totalFish;
    public TextMeshProUGUI currentHatName;
    public HatLogic hatLogic;

    private bool isInit = false;

    // Shop Item...
    public GameObject hatPrefab;
    public Transform hatContainer;
    private Hat[] hats;



    public override void Construct()
    {
        GameManager.Instance.ChangeCamera(GameManager.GameCamera.Shop);
        hats = Resources.LoadAll<Hat>("Hat");
        shopUI.SetActive(true);

        if (!isInit)
        {
            totalFish.text = SaveManager.Instance.save.Fish.ToString("000");
            currentHatName.text = hats[SaveManager.Instance.save.currentHatIndex].ItemName;
            PopulateShop();
            isInit = true;
        }
    }

    public override void Destruct()
    {
        shopUI.SetActive(false);
    }

    private void PopulateShop()
    {
        for(int i = 0; i < hats.Length; i++)
        {
            int index = i;
            GameObject hat =  Instantiate(hatPrefab, hatContainer) as GameObject;
            //Button
            hat.GetComponent<Button>().onClick.AddListener(() => OnHatClick(index));
            //Thumbnail
            hat.transform.GetChild(1).GetComponent<Image>().sprite = this.hats[index].Thumbnail;
            //ItemName
            hat.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = this.hats[index].ItemName;
            //Price
            if (SaveManager.Instance.save.UnlockedHatFlag[i] == 0)
            {
                hat.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = this.hats[index].ItemPrice.ToString();
            }
            else
            {
                hat.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "";
            }
        }
    }

    private void OnHatClick(int i)
    {
        if (SaveManager.Instance.save.UnlockedHatFlag[i] == 1)
        {
            SaveManager.Instance.save.currentHatIndex = i;
            currentHatName.text = hats[i].ItemName;
            hatLogic.SelectHatInIndex(i);
            SaveManager.Instance.Save();
        }
        // If we don't have it, can we buy it?
        else if (hats[i].ItemPrice <= SaveManager.Instance.save.Fish)
        {
            SaveManager.Instance.save.Fish -= hats[i].ItemPrice;
            SaveManager.Instance.save.UnlockedHatFlag[i] = 1;
            currentHatName.text = hats[i].ItemName;
            hatLogic.SelectHatInIndex(i);
            totalFish.text = SaveManager.Instance.save.Fish.ToString("000");
            SaveManager.Instance.Save();
            hatContainer.GetChild(i).transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "";
        }
        // Don't have it, can't buy it...
        else
        {

        }
    }

    public void OnHomeClick()
    {
        gameManager.ChangeState(GetComponent<GameStateInit>());
    }
}
