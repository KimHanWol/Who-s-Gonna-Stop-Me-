using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DataBase : MonoBehaviour
{
    private static DataBase instance;
    //public Achievements achievement;

    public Event eventScript;
    public GameObject Tutorial;

    public bool MapActive
    {
        set
        {
            PlayerPrefs.SetString("MapActive", value.ToString());
        }
        get
        {
            return bool.Parse(PlayerPrefs.GetString("MapActive", "false"));
        }
    }

    public static DataBase GetInstance()
    {
        if (instance == null)
        {
            instance = FindObjectOfType<DataBase>();
            if (instance == null)
            {
                GameObject container = new GameObject("DataBase");
                container.AddComponent<DataBase>();

                instance = container.GetComponent<DataBase>();
            }
        }
        return instance;
    }

    //playing
    [HideInInspector]
    public bool playing;
    
    //time
    public string date
    {
        get
        {
            return PlayerPrefs.GetString("Date", "0");
        }
        set
        {
            PlayerPrefs.SetString("Date", value);
        }
    }

    //alien
    private static int AlienNum = 50;
    //public GameObject AlienSetting;
    [HideInInspector]
    public Alien[] alien = new Alien[AlienNum];

    public int howMuchAlien
    {
        get
        {
            for(int i = 0;i< AlienNum; i++)
            {
                if (!alien[i].exist) return i;
            }
            return -1;
        }
    }

    //Icon
    [HideInInspector]
    public Sprite PowerIcon;
    [HideInInspector]
    public Sprite GemIcon;

    //buff
    [HideInInspector]
    public Buff[] buff = new Buff[4];
    [HideInInspector]
    public GameObject buffPanel;
    //[HideInInspector]
    //public bool isBuff;
    [HideInInspector]
    public int whatBuff;



    //planet
    [HideInInspector]
    public Planet[] planet = new Planet[10];

    public int Theme
    {
        get
        {
            return PlayerPrefs.GetInt("Theme", 0);
        }
        set
        {
            PlayerPrefs.SetInt("Theme", value);
        }
    }

    public PlanetClick planetClick;

    //badge
    [HideInInspector]
    public Sprite[] BadgeImage = new Sprite[10];
    [HideInInspector]
    public bool[] HaveBadge = new bool[10];

    public int planetDistroy
    {
        get
        {
            return PlayerPrefs.GetInt("PlanetDistroy", 0);
        }
        set
        {
            PlayerPrefs.SetInt("PlanetDistroy", value);
        }
    }


    //public Sprite tmp;

    public Sprite effect;

    [HideInInspector]
    public Alien partner;
    [HideInInspector]
    public bool alienSettingMoving = false;

    //Sound
    [HideInInspector]
    public GameObject Sound;
    public AudioSource TmpEffect;

    //SpaceShip
    [HideInInspector]
    public GameObject SpaceShip;

    //Font
    [HideInInspector]
    public Font malgunbd;

    void OnEnable()
    {
        alienLoading();
        planetLoading();
        buffLoading();
        soundLoading();
        iconLoading();
        badgeLoading();
        dataBaseLoading();
    }
    public void Start()
    {
        StartCoroutine("checkPlayTime");

        //로딩
        //alienLoading();
        //planetLoading();
        //buffLoading();
        //soundLoading();
        //iconLoading();
        //badgeLoading();
        //dataBaseLoading();

        //행성 세팅
        setPlanet();

        for(int i = 0; i < 150; i++)
        {
            //alien[i].effect = effect;
        }

        timeChecker();

        //Story.SetActive(true);
        //Tutorial.SetActive(true);

        //tutorial
        Tutorial.SetActive(true);
    }

    //외계인 로딩
    public void alienLoading()
    {
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                alien[i * 5 + j] = this.gameObject.transform.Find("Alien").GetChild(i).GetChild(j).GetComponent<Alien>();
                Debug.Log(this.gameObject.transform.Find("Alien").GetChild(i).GetChild(j).GetComponent<Alien>().alienName);
            }
        }
    }

    //행성 로딩
    public void planetLoading()
    {
        for (int i = 0; i < 10; i++)
        {
            planet[i] = DataBase.GetInstance().gameObject.transform.Find("Planet").GetChild(i).GetComponent<Planet>();
        }
    }

    //버프 로딩
    public void buffLoading()
    {
        for (int i = 0; i < 4; i++)
        {
            buff[i] = transform.Find("Buff").GetChild(i).GetComponent<Buff>();
        }
    }

    //사운드 로딩
    public void soundLoading()
    {
        Sound = DataBase.GetInstance().gameObject.transform.Find("Sound").gameObject;
    }

    //아이콘 로딩
    public void iconLoading()
    {
        PowerIcon = GetInstance().gameObject.transform.Find("Icon").GetChild(0).GetComponent<Sprite>();
        GemIcon = GetInstance().gameObject.transform.Find("Icon").GetChild(1).GetComponent<Sprite>();
    }

    //뱃지 이미지 로딩
    public void badgeLoading()
    {
        for (int i = 0; i < 10; i++)
        {
            BadgeImage[i] = this.gameObject.transform.Find("Badge").GetChild(i).GetComponent<Image>().sprite;
        }
    }

    public void dataBaseLoading()
    {
        //버프 패널 로딩
        buffPanel = Interface.GetInstance().buffPanel;
        SpaceShip = transform.Find("SpaceShip").gameObject;
        malgunbd = GetInstance().transform.Find("Font").Find("Malgunbd").GetComponent<Text>().font;
    }

    //행성 세팅
    public void setPlanet()
    {
        for(int i = 0; i < 10; i++)
        {
            planet[i] = this.transform.Find("Planet").GetChild(i).GetComponent<Planet>();
        }
    }


    IEnumerator checkPlayTime()
    {
        while (true)
        {
            PlayerPrefs.SetString("PlayTime", (double.Parse(PlayerPrefs.GetString("PlayTime", "0")) + Time.deltaTime).ToString());
            yield return new WaitForSeconds(0.01f);
        }
    }

    //시간 측정
    public void timeChecker()
    {
        if (date != System.DateTime.Now.ToString("yyyy-MM-dd"))
        {
            if (date != "0" && int.Parse(System.DateTime.Now.ToString("yyyy-MM-dd").Split('-')[2]) > int.Parse(date.Split('-')[2]) + 1) eventScript.attendanceDays = 0;
            date = System.DateTime.Now.ToString("yyyy-MM-dd");
            DataController.GetInstance().MomTrainingTicket = 3;
            DataController.GetInstance().PartTimeJobTicket = 3;
            eventScript.attendanceDays++;
        }
    }

    void OnApplicationQuit()
    {
        playing = false;
    }

    public void OnApplicationStart()
    {
        playing = true;
    }

    //시간 변환
    public string TimeController(float time)
    {
        if (time > 216000)
        {
            return (int)(time / 216000) + "h" +
                (int)(time % 216000 / 3600) + "m" +
                (int)(time % 3600 / 60) + "s";
        }
        else if (time > 3600)
        {
            return (int)(time % 216000 / 3600) + "m" +
                (int)(time % 3600 / 60) + "s";
        }
        else
        {
            return (int)(time % 3600 / 60) + "s";
        }
    }


}
