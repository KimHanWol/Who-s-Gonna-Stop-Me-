using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    /*
     * 1. Damage Per Click
     * 2. Power Per Damage
     * 3. Auto Attack
     * 4. Critical Attack Probability
     * 5. Fever Duration
     * 6. Fever Gauge Increasement
     * 7. Damage Per Click
     * 8. Buy A Buff
     */



    //return true when item can bought with Gem
    public bool isGemItem;

    public GetSomething GetSomething;

    public Achievements achievements;

    [HideInInspector]
    public int level
    {
        get
        {
            return PlayerPrefs.GetInt(upgradeName, 0);
        }
        set
        {
            PlayerPrefs.SetInt(upgradeName, value);
        }
    }

    [HideInInspector]
    public Text upgradeDisplayer;
    [HideInInspector]
    public Text costDisplayer;

    public string upgradeName;

    [HideInInspector]
    public double currentCost = 1;
    public double startCurrentCost = 1;

    [HideInInspector]
    public double damageByClick;
    public double startDamageByClick;

    [HideInInspector]
    public double powerPerDamage;
    public double startPowerPerDamage;


    public double powerPerSec;
    public double startPowerPerSec = 1;

    public float critical;

    public float upgradePower;

    public float costPower;

    // Start is called before the first frame update
    void Start()
    {
        damageByClick = 0;
        powerPerDamage = 0;
        powerPerSec = 0;
        upgradeDisplayer = this.transform.GetChild(0).GetComponent<Text>();
        costDisplayer = this.transform.GetChild(1).GetChild(0).GetComponent<Text>();
        upgradeDisplayer.text = upgradeName;
        costDisplayer.text = DataController.GetInstance().convertResource(double.Parse(string.Format("{0:0.0}", currentCost)));
    }

    public void perchase()
    {
        if ((!isGemItem && DataController.GetInstance().m_power >= currentCost))
        {
            DataController.GetInstance().m_power -= double.Parse(string.Format("{0:0.0}", currentCost));
            PlayerPrefs.SetString("SpendPower", (double.Parse(PlayerPrefs.GetString("SpendPower", "0")) + double.Parse(string.Format("{0:0.0}", currentCost))).ToString());
            level++;
            updateShop();
            DataController.GetInstance().m_damagePerClick += damageByClick;
            DataController.GetInstance().m_powerPerDamage += powerPerDamage;
            DataController.GetInstance().m_powerPerSec += powerPerSec;
            DataController.GetInstance().m_critical += critical;
        }
        else if (isGemItem && DataController.GetInstance().m_Gem >= currentCost)
        {
            DataController.GetInstance().m_Gem -= int.Parse(currentCost.ToString());
            PlayerPrefs.SetString("SpendGem", (double.Parse(PlayerPrefs.GetString("SpendGem", "0")) + int.Parse(currentCost.ToString())).ToString());
            GetSomething.getSomething(0);
        }
        //achievements.upgrade++;
        costDisplayer.text = DataController.GetInstance().convertResource(double.Parse(string.Format("{0:0.0}", currentCost)));
    }

    public void updateShop()
    {
        currentCost = startCurrentCost * Mathf.Pow(costPower, level);
        damageByClick = startDamageByClick * Mathf.Pow(upgradePower, level); 
        powerPerDamage = startPowerPerDamage * Mathf.Pow(upgradePower, level);
        powerPerSec = startPowerPerSec * Mathf.Pow(upgradePower, level);
    }

    // Update is called once per frame
    void Update()
    {
        CanBuy();
    }

    public void CanBuy()
    {
        bool canBuy;
        if ((!isGemItem && DataController.GetInstance().m_power >= currentCost) || (isGemItem && DataController.GetInstance().m_Gem >= currentCost)) canBuy = true;
        else canBuy = false;

        if (canBuy)
        {
            this.transform.GetChild(1).GetComponent<CanvasGroup>().alpha = 1;
            this.transform.GetChild(1).GetComponent<CanvasGroup>().interactable = true;
        }
        else
        {
            this.transform.GetChild(1).GetComponent<CanvasGroup>().alpha = 0.5f;
            this.transform.GetChild(1).GetComponent<CanvasGroup>().interactable = false;
        }
    }
}
