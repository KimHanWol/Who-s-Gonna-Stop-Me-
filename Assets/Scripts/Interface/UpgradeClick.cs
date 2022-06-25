using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeClick : MonoBehaviour
{
    public CanvasGroup canvasGroup;

    public Text upgradeDisplayer;
    public Text costDisplayer;

    public string upgradeName;

    [HideInInspector]
    public int powerByUpgrade;

    public int startPowerByUpgrade = 1;

    [HideInInspector]
    public long currentCost = 1;

    public long startCurrentCost = 1;

    [HideInInspector]
    public int level = 1;

    public float upgradePow = 3.14f;

    public float costPow = 3.14f;

    void Start()
    {
        currentCost = startCurrentCost;
        level = DataController.GetInstance().getClickUpgrade();
        powerByUpgrade = startPowerByUpgrade;
        UpdateUpgrade();
    }

    public void perchaseUpgrade()
    {
        if(DataController.GetInstance().m_power >= currentCost)
        {
            DataController.GetInstance().m_power -= currentCost;
            DataController.GetInstance().m_damagePerClick += powerByUpgrade;
            level++;
            UpdateUpgrade();
            DataController.GetInstance().setClickUpgrade(level);
        }
    }

    public void UpdateUpgrade()
    {
        powerByUpgrade = startPowerByUpgrade * (int)Mathf.Pow(upgradePow, level);
        currentCost = startCurrentCost * (int)Mathf.Pow(costPow, level);
        UpgradeText();
        UpgradeButton(); 
        //if (!dataController.canUseButton[dataController.planetLevel - 1])
        //{
        //    canvasGroup.alpha = 0.3f;
        //    canvasGroup.interactable = false;
        //}

        //else
        //{
            if (DataController.GetInstance().m_power >= currentCost)
            {
                canvasGroup.alpha = 1;
                canvasGroup.interactable = true;
            }
            else
            {
                canvasGroup.alpha = 0.5f;
                canvasGroup.interactable = false;
            }
        //}
    }

    public void UpgradeText()
    {
        upgradeDisplayer.text = upgradeName + " level" + level;
    }

    public void UpgradeButton()
    {
        costDisplayer.text = currentCost + "힘";
    }

    void Update()
    {
        UpdateUpgrade();
    }
}
