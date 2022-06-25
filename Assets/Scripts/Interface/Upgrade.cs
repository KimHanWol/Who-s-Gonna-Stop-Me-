using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Upgrade : MonoBehaviour
{
    public string UpgradeName;

    private int upgradeLevel
    {
        get
        {
            return PlayerPrefs.GetInt(UpgradeName + "Level", 1);
        }
        set
        {
            PlayerPrefs.SetInt(UpgradeName + "Level", value);
        }
    }

    public void AddUpgradeLevel()
    {
        upgradeLevel++;
    }

    //Price
    public bool isPowerPrice;

    public double startPrice;

    private double price;

    public float priceIncreasement;

    //content
    public bool isPlusContent;

    private double content
    {
        //UpgradeName을 playerPrefs의 키값으로 하고 받아서 바로 업그레이드
        get
        {
            return double.Parse(PlayerPrefs.GetString(UpgradeName));
        }
        set
        {
            PlayerPrefs.SetString(UpgradeName, value.ToString());
        }
    }

    public double StartContent;

    public float ContentIncreasement;


    // Start is called before the first frame update
    void Start()
    {
        UpdatePrice();
        this.transform.GetChild(0).GetComponent<Text>().text = UpgradeName;
    }

    public void UpgradeClick()
    {
        if (isPowerPrice)
        {
            if (DataController.GetInstance().m_power > price)
            {
                DataController.GetInstance().m_power -= price;
            }
            else return;
        }
        else
        {
            if (DataController.GetInstance().m_Gem > price)
            {
                DataController.GetInstance().m_Gem -= int.Parse(price.ToString());
            }
            else return;
        }
        upgradeLevel++;
        UpdatePrice();
    }

    public void BuyABuff()
    {
        if (DataController.GetInstance().m_Gem > price)
        {
            DataController.GetInstance().m_Gem -= int.Parse(price.ToString());
        }
    }

    public void UpdatePrice()
    {
        int tmplevel = upgradeLevel - 1;
        if (isPowerPrice) price = double.Parse((startPrice * Mathf.Pow(priceIncreasement, tmplevel)).ToString().Split('.')[0] + '.' + (((startPrice * Mathf.Pow(priceIncreasement, tmplevel)) * 10) % 10).ToString().Split('.')[0]);
        else price = (int)(startPrice * Mathf.Pow(priceIncreasement, tmplevel) + 1);
        if (isPlusContent) content = StartContent + ContentIncreasement * (upgradeLevel - 1);
        else content = double.Parse((StartContent * Mathf.Pow(ContentIncreasement, upgradeLevel - 1)).ToString().Split('.')[0] + '.' + ((StartContent * Mathf.Pow(ContentIncreasement, upgradeLevel - 1) * 10) % 10).ToString().Split('.')[0]);
        this.transform.Find("Button").GetChild(0).Find("Text").GetComponent<Text>().text = DataController.GetInstance().convertResource(price);
        this.transform.Find("Button").GetChild(1).Find("Text").GetComponent<Text>().text = DataController.GetInstance().convertResource(content);
    }

    IEnumerator ButtonActiveLoop()
    {
        while (true)
        {
            double tmp;
            if (isPowerPrice)
            {
                tmp = DataController.GetInstance().m_power;
            }
            else
            {
                tmp = DataController.GetInstance().m_Gem;
            }
            if (tmp >= price) this.transform.Find("Button").GetComponent<Button>().interactable = true;
            else this.transform.Find("Button").GetComponent<Button>().interactable = false;
            yield return new WaitForSeconds(0.1f);
        }
    }

    void OnEnable()
    {
        StartCoroutine("ButtonActiveLoop");
    }

    void OnDisable()
    {
        StopCoroutine("ButtonActiveLoop");
    }



    //버프 얻는거
}
