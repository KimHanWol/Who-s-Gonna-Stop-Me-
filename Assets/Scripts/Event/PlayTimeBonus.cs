using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayTimeBonus : MonoBehaviour
{
    private string PlayTime;

    // Start is called before the first frame update
    void Start()
    {
        this.transform.GetChild(0).GetChild(1).GetChild(0).Find("Slider").GetComponent<Slider>().maxValue = 10800;
    }


    // Update is called once per frame
    void Update()
    {
        PlayTime = PlayerPrefs.GetString("PlayTime", "0");
        this.transform.GetChild(0).GetChild(1).GetChild(0).Find("Slider").Find("Text").GetComponent<Text>().text = DataBase.GetInstance().TimeController(float.Parse(PlayTime) * 60 );
        Debug.Log(float.Parse(PlayTime));
        this.transform.GetChild(0).GetChild(1).GetChild(0).Find("Slider").GetComponent<Slider>().value = (float)(double.Parse(PlayTime));
        SliderController();
        ButtonActive();
    }

    public void SliderController()
    {
        this.transform.GetChild(0).GetChild(1).GetChild(0).Find("Slider").GetComponent<Slider>().value = float.Parse(PlayTime);
    }

    //IEnumerator ButtonActive()
    //{
    //    Transform tmp = this.transform.GetChild(0).GetChild(1).GetChild(0).GetChild(1);
    //    if (double.Parse(PlayTime) > 600)
    //    {
    //        tmp.GetChild(1).GetComponent<Button>().interactable = true;
    //        if(double.Parse(PlayTime) > )
    //    }
    //    this.transform.GetChild(0).GetChild(1).GetChild(0).GetChild(1).GetChild(i).GetComponent<Button>().interactable = 
    //    yield return new WaitForSeconds(0.01f);
    //}

    public void ButtonActive()
    {
        Transform tmp = this.transform.GetChild(0).GetChild(1).GetChild(0).GetChild(1);
        if (double.Parse(PlayTime) > 600 && !bool.Parse(PlayerPrefs.GetString("PlayTime1End", "false")))
        {
            tmp.GetChild(0).GetComponent<Button>().interactable = true;
            if (double.Parse(PlayTime) > 3600 && !bool.Parse(PlayerPrefs.GetString("PlayTime2End", "false")))
            {
                tmp.GetChild(1).GetComponent<Button>().interactable = true;
                if (double.Parse(PlayTime) > 10800 && !bool.Parse(PlayerPrefs.GetString("PlayTime3End", "false")))
                {
                    tmp.GetChild(2).GetComponent<Button>().interactable = true;
                }
            }
        }
    }

    public void TenMinClick()
    {
        if (double.Parse(PlayTime) > 600) DataController.GetInstance().gainGem(3);
        this.transform.GetChild(0).GetChild(1).GetChild(0).GetChild(1).GetChild(0).GetChild(0).Find("Text").GetComponent<Text>().text = "Complete";
        PlayerPrefs.SetString("PlayTime1End", "true");
    }

    public void OneHourClick()
    {
        if (double.Parse(PlayTime) > 3600) DataController.GetInstance().gainGem(5);
        this.transform.GetChild(0).GetChild(1).GetChild(0).GetChild(1).GetChild(1).GetChild(0).Find("Text").GetComponent<Text>().text = "Complete";
        PlayerPrefs.SetString("PlayTime2End", "true");
    }

    public void ThreeHourClick()
    {
        if (double.Parse(PlayTime) > 10800) DataController.GetInstance().gainGem(10);
        this.transform.GetChild(0).GetChild(1).GetChild(0).GetChild(1).GetChild(2).GetChild(0).Find("Text").GetComponent<Text>().text = "Complete";
        PlayerPrefs.SetString("PlayTime3End", "true");
    }
}
