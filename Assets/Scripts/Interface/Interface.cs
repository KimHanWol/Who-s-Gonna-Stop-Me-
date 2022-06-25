using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Interface : MonoBehaviour
{
    private static Interface instance;

    public static Interface GetInstance()
    {
        if (instance == null)
        {
            instance = FindObjectOfType<Interface>();
            if (instance == null)
            {
                GameObject container = new GameObject("Interface");

                instance = container.GetComponent<Interface>();
            }
        }
        return instance;
    }
    

    public GameObject Menu;
    public GameObject Achievements;
    public GameObject Shop;
    public GameObject Boss;
    public GameObject GetSome;
    public GameObject SpaceShip;
    public GameObject Information;
    public GameObject AlienSelect;
    public GameObject Badge;
    public GameObject Attendance;
    public GameObject EvolutionPanel;
    public GameObject PlayTimeBonus;

    //ButtonPanel

    public GameObject BottomPanel;
    public bool bottomActive;
    public int bottomAlien;
    GameObject BottomAlienPanel;
    GameObject BottomUpDownPanel;
    Text BottomTheme;

    //public GameObject Main;

    public GameObject buffPanel;

    public GameObject SleepMode;


    public GameObject AchievementButton;
    public GameObject SpaceshipButton;
    public GameObject InformationButton;
    public GameObject ShopButton;

    public Text PowerDisplayer;
    public Text GemDisplayer;
    public Text MomTicketDisplayer;
    public Text CleanTicketDisplayer;

    public void Awake()
    {
        BottomAlienPanel = BottomPanel.transform.GetChild(1).GetChild(0).GetChild(0).gameObject;
        BottomUpDownPanel = BottomPanel.transform.GetChild(1).GetChild(0).GetChild(1).gameObject;
        BottomTheme = BottomPanel.transform.GetChild(1).GetChild(0).GetChild(1).GetChild(0).GetComponent<Text>();
    }

    public void Start()
    {

        Menu.transform.GetChild(0).Translate(-400.0f, 0, 0);
        Clean();
    }

    public void buttonClick(GameObject GameOb)
    {
        Clean();
        GameOb.SetActive(true);
        ButtonStateChange(false);
    }

    public void Clean()
    {
        if (Menu.transform.GetChild(0).gameObject.activeInHierarchy) MenuClick();
        Menu.SetActive(false);
        Achievements.SetActive(false);
        Shop.SetActive(false);
        Badge.SetActive(false);
        Attendance.SetActive(false);
        //임시
        Boss.SetActive(false);
        GetSome.SetActive(false);
        SpaceShip.SetActive(false);
        Information.SetActive(false);
        PlayTimeBonus.SetActive(false);
        ButtonStateChange(true);
        if (bottomActive) BottomClick();
    }

    public void ButtonStateChange(bool state)
    {
        AchievementButton.GetComponent<Button>().interactable = state;
        SpaceshipButton.GetComponent<Button>().interactable = state;
        InformationButton.GetComponent<Button>().interactable = state;
        ShopButton.GetComponent<Button>().interactable = state;
        //BottomPanel.transform.GetChild(0).GetChild(0).GetComponent<Button>().interactable = state;
    }

    public void disable()
    {
        Clean();
    }

    public void MenuClick()
    {
        if (!Menu.GetComponent<Animation>().isPlaying)
        {
            if (Menu.activeInHierarchy)
            {
                Menu.GetComponent<Animation>().Play("Menu_GetOut");
                StartCoroutine("MenuDisable");                                                          //여기 코루틴으로 만들었어.
                if (!Achievements.activeInHierarchy && !SpaceShip.activeInHierarchy && !Shop.activeInHierarchy) ButtonStateChange(true);
                BottomTheme.text = (bottomAlien + 1).ToString();
            }
            else
            {
                if (bottomActive) BottomClick();
                Menu.SetActive(true);
                Menu.GetComponent<Animation>().Play("Menu_GetIn");
                ButtonStateChange(false);
            }
        }
    }

    public void BottomClick()
    {
        if (!bottomActive)
        {
            SetBottomAlien();
            BottomPanel.GetComponent<Animation>().Play("Bottom_GetIn");
            BottomPanel.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetComponent<Text>().text = "▼";
            bottomActive = true;
        }
        else
        {
            BottomPanel.GetComponent<Animation>().Play("Bottom_GetOut");
            BottomPanel.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetComponent<Text>().text = "▲";
            bottomActive = false;
        }
        Debug.Log(DataBase.GetInstance().alien[bottomAlien * 5 + 0].exist);
    }

    public void SetBottomAlien()
    {
        GameObject UpButton = BottomUpDownPanel.transform.GetChild(1).gameObject;
        GameObject DownButton = BottomUpDownPanel.transform.GetChild(2).gameObject;
        GameObject AlienPanel = BottomAlienPanel.transform.GetChild(0).gameObject;

        if (bottomAlien == 0)
        {
            UpButton.transform.GetChild(0).GetComponent<Text>().text = "";
            UpButton.GetComponent<Button>().interactable = false;
        }
        else
        {
            UpButton.transform.GetChild(0).GetComponent<Text>().text = "▲";
            UpButton.GetComponent<Button>().interactable = true;
        }
        if (bottomAlien == 9)
        {
            DownButton.transform.GetChild(0).GetComponent<Text>().text = "";
            DownButton.GetComponent<Button>().interactable = false;
        }
        else
        {
            DownButton.transform.GetChild(0).GetComponent<Text>().text = "▼";
            DownButton.GetComponent<Button>().interactable = true;
        }

        for (int i = 0; i < 5; i++)
        {
            AlienPanel.transform.GetChild(i).GetComponent<Image>().sprite = DataBase.GetInstance().alien[bottomAlien * 5 + i].image;
            if (!DataBase.GetInstance().alien[bottomAlien * 5 + i].exist)
            {
                AlienPanel.transform.GetChild(i).GetComponent<Image>().color = Color.white;
                AlienPanel.transform.GetChild(i).GetComponent<Image>().material = DataBase.GetInstance().malgunbd.material;
                AlienPanel.transform.GetChild(i).GetComponent<Button>().interactable = false;
            }
            else
            {
                AlienPanel.transform.GetChild(i).GetComponent<Image>().material = null;
                AlienPanel.transform.GetChild(i).GetComponent<Button>().interactable = true;
            }
        }
    }

    public void BottomUp()
    {
        bottomAlien--;
        SetBottomAlien();
        BottomTheme.text = (bottomAlien + 1).ToString();
    }

    public void BottomDown()
    {
        bottomAlien++;
        SetBottomAlien();
        BottomTheme.text = (bottomAlien + 1).ToString();
    }

    IEnumerator MenuDisable()
    {
        yield return new WaitForSeconds(1);
        Menu.SetActive(false);
    }

    public void AchieveMentsClick()
    {
        buttonClick(Achievements);
    }

    public void ShopsClick()
    {
        buttonClick(Shop);
    }

    public void BossClick()
    {
        Boss.transform.GetChild(0).GetChild(0).GetChild(0).Find("Text").GetComponent<Text>().text = "DummyMom\nTraining\n(" + DataController.GetInstance().MomTrainingTicket + "/3)";
        Boss.transform.GetChild(0).GetChild(0).GetChild(1).Find("Text").GetComponent<Text>().text = "PartTimeJob\n(" + DataController.GetInstance().PartTimeJobTicket + "/3)";
        if (DataController.GetInstance().MomTrainingTicket == 0) Boss.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Button>().interactable = false;
        if (DataController.GetInstance().PartTimeJobTicket == 0) Boss.transform.GetChild(0).GetChild(0).GetChild(1).GetComponent<Button>().interactable = false;
        buttonClick(Boss);
    }

    public void SpaceShipClick()
    {
        buttonClick(SpaceShip);
        SpaceShip.transform.GetChild(0).gameObject.SetActive(true);
        SpaceShip.transform.GetChild(1).gameObject.SetActive(false);
    }

    public void InformationClick()
    {
        buttonClick(Information);
    }

    public void BadgeClick()
    {
        buttonClick(Badge);
    }

    public void PlayTimeClick()
    {
        buttonClick(PlayTimeBonus);
    }

    public void AttendanceButtonClick()
    {
        DataBase.GetInstance().eventScript.AttendanceButtonClick();
    }


    void Update()
    {
        PowerDisplayer.text = DataController.GetInstance().convertResource(DataController.GetInstance().m_power) + "(" + DataController.GetInstance().convertResource(double.Parse(string.Format("{0:0.0}", DataController.GetInstance().m_powerPerSec))) + "/s)";
        GemDisplayer.text = DataController.GetInstance().m_Gem.ToString();
    }
                                        //랜덤으로 뽑기 되는 형식으로 업그레이드 하게 만들자
                                        //한 3x3 형식으로 만들고 그중에서 랜덤으로 뽑혀서 레벨업하는형식?
    public void SleepModeActive()
    {
        Clean();
        SleepMode.SetActive(true);
    }

    public void SleepModeDistory()
    {
        SleepMode.SetActive(false);
    }

    public void Reset()
    {
        PlayerPrefs.DeleteAll();
    }
}
