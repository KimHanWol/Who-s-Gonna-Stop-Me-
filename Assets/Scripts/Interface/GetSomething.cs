using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetSomething : MonoBehaviour
{
    public GameObject getSomeObject;

    public GameObject BadgeDisplayer;

    public GameObject BadgeViewer;
    [HideInInspector]
    public Text whatName;

    public bool isBuff;

    [HideInInspector]
    private int alienNum;

    //public GameObject AlienPanel;

    public void getSomething(int theme)
    {
        getSomeObject.SetActive(true);
        int alien = -1;
        //외계인 획득
        for (int i = 0; i < 5; i++)
        {
            if (!DataBase.GetInstance().alien[theme * 5 + i].exist)
            {
                getSomeObject.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = DataBase.GetInstance().alien[theme * 5 + i].image;
                whatName.text = DataBase.GetInstance().alien[theme * 5 + i].alienName;
                alienNum = theme * 5 + i;
                DataBase.GetInstance().alien[theme * 5 + i].exist = true;
                alien = i;
                if (Interface.GetInstance().bottomActive) Interface.GetInstance().BottomClick();
                break;
            }
        }
        Debug.Log(alien);
        if (alien == -1)
        {
            //뱃지 획득
            if (!DataBase.GetInstance().HaveBadge[theme])
            {
                getSomeObject.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = DataBase.GetInstance().BadgeImage[theme];
                Debug.Log(DataBase.GetInstance().BadgeImage[theme]);
                whatName.text = DataBase.GetInstance().planet[theme].planetName + "의 뱃지";
                DataBase.GetInstance().HaveBadge[theme] = true;
                Color tmp = new Color(255, 255, 255);
                tmp.a = 255f;
                BadgeDisplayer.transform.GetChild(0).GetChild((theme) / 5).GetChild((theme) % 5).GetComponent<Image>().color = tmp;
                BadgeViewer.transform.GetChild(0).GetChild((theme) / 5).GetChild((theme) % 5).GetComponent<Image>().color = tmp;
                if (theme < 10)
                {
                    theme = ++DataBase.GetInstance().Theme;
                    DataBase.GetInstance().planetClick.MovePlanetTheme();
                }
                else
                {
                    theme = 0;
                    DataBase.GetInstance().planetClick.MovePlanetTheme();
                }
            }
        }
        //DataBase.GetInstance().isBuff = false;
    }


    public void getABuff()
    {
        getSomeObject.SetActive(true);
        int whatBuff = Random.Range(0, 4);
        //지속시간 5분
        DataBase.GetInstance().buff[whatBuff].buffDuration += 3600 * 5;
        getSomeObject.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = DataBase.GetInstance().buff[whatBuff].buffImage;
        DataBase.GetInstance().buff[whatBuff].getBuff();
    }

    public void getSomethingBreak()
    {
        //if (!DataBase.GetInstance().isBuff)
        //{
        //    DataBase.GetInstance().alien[alienNum].exist = true;
        //    getSomeObject.SetActive(false);
        //}
        //else
        //{
        //    getSomeObject.SetActive(false);
        //}
        getSomeObject.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        getSomeObject.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = getSomeObject.transform.GetChild(0).Find("What").GetComponent<Image>().sprite;
        whatName = getSomeObject.transform.GetChild(0).Find("WhatName").GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
    }
}
