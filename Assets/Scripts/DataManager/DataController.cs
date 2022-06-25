using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataController : MonoBehaviour
{

    public GameObject gemIcon;

    public GameObject main;

    private static DataController instance;

    public static DataController GetInstance()
    {
        if(instance == null)
        {
            instance = FindObjectOfType<DataController>();
            if (instance == null)
            {
                GameObject container = new GameObject("DataController");

                instance = container.AddComponent<DataController>();
            }
        }
        return instance;
    }

    public double m_power {
        get
        {
            return double.Parse(PlayerPrefs.GetString("Power", "0"));
        }
        set
        {
            PlayerPrefs.SetString("Power", value.ToString());
        }
    }
    public int m_Gem
    {
        get
        {
            return PlayerPrefs.GetInt("Gem");
        }
        set
        {
            PlayerPrefs.SetInt("Gem", value);
        }

    }

    //터치 시 행성 데미지
    public double m_damagePerClick
    {
        get
        {
            return double.Parse(PlayerPrefs.GetString("DamagePerClick", "1"));
        }
        set
        {
            PlayerPrefs.SetString("DamagePerClick", value.ToString());
        }
    }

    //터치 시 획득 힘
    public double m_powerPerClick
    {
        get
        {
            PlayerPrefs.SetString("PowerPerClick", (m_damagePerClick * m_powerPerDamage).ToString());
            return double.Parse(PlayerPrefs.GetString("PowerPerClick"));
        }
    }

    //터치 시 데미지 당 얻는 힘
    public double m_powerPerDamage {
        get
        {
            return double.Parse(PlayerPrefs.GetString("PowerPerDamage", "1"));
        }
        set
        {
            PlayerPrefs.SetString("PowerPerDamage", value.ToString());
        }
    }

    //터치 시 힘 획득 2배 데미지 2배 확률
    public float m_critical
    {
        get
        {
            return PlayerPrefs.GetFloat("Critical", 0.1f);
        }
        set
        {
            PlayerPrefs.SetFloat("Critical", value);
        }
    }

    //초 당 힘 획득
    public double m_powerPerSec
    {
        get
        {
            return double.Parse(PlayerPrefs.GetString("PowerPerSec", "0"));
        }
        set
        {
            PlayerPrefs.SetString("PowerPerSec", value.ToString());
        }
    }

    public int MomTrainingTicket {
        get
        {
            return PlayerPrefs.GetInt("MomTrainingTicket", 3);
        }
        set
        {
            PlayerPrefs.SetInt("MomTrainingTicket", value);
        }
    }
    public int PartTimeJobTicket
    {
        get
        {
            return PlayerPrefs.GetInt("PartTimeJobTicket", 3);
        }
        set
        {
            PlayerPrefs.SetInt("PartTimeJobTicket", value);
        }
    }

    [HideInInspector]
    public string[] unit;

    private int m_clickLevel = 1;

    void Awake()
    {
        StartCoroutine("AddPowerLoop");
        unit = new string[12];
        unit[0] = "K";
        unit[1] = "M";
        unit[2] = "B";
        unit[3] = "T";
        unit[4] = "aa";
        unit[5] = "bb";
        unit[6] = "cc";
        unit[7] = "dd";
        unit[8] = "ee";
        unit[9] = "ff";
        unit[10] = "gg";
        unit[11] = "hh";

        //시작 알림
        DataBase.GetInstance().OnApplicationStart();
    }

    IEnumerator AddPowerLoop()
    {
        while (true)
        {
            //m_power += double.Parse(m_powerPerSec.ToString().Split('.')[0] + '.' + ((m_powerPerSec * 10f) % 10f).ToString().Split('.')[0]);
            m_power += double.Parse(string.Format("{0:0.0}", m_powerPerSec));
            yield return new WaitForSeconds(1.0f);
        }
    }

    public string convertResource(double newResource)
    {
        if (newResource > 999)
        {
            newResource = newResource / 1000;
            for (int i = 0; i < 12; i++)
            {
                if (newResource > 999)
                {
                    newResource = newResource / 1000;
                }
                else
                {
                    return string.Format("{0:0.0}", newResource) + unit[i];   
                }
            }
            return "OverFlow";
        }
        else
        {
            if (newResource < 0) return "0";
            return newResource.ToString().Split('.')[0] + '.' + ((newResource * 10) % 10).ToString().Split('.')[0];
        }
    }


    //set(PlayerPrefs)

    public void setClickUpgrade(int newClickupgrade)
    {
        m_clickLevel = newClickupgrade;
        PlayerPrefs.SetInt("ClickLevel", m_clickLevel);
    }

    public void setPowerPerSecUpgrade(int level, int newAlienUpgrade)
    {
        PlayerPrefs.SetInt(level + "PowerPerSec", newAlienUpgrade);
    }

    public void setPowerPerSec(int newPowerPerSec)
    {
        m_damagePerClick = newPowerPerSec;
    }

    //Add/Sub

    public void AddPowerPerSec(int level)
    {
        setPowerPerSecUpgrade(level, getAlienLevel(level) + 1);
    }

    public void gainGem(int howMuch)
    {
        if (howMuch <= 0) return;
        PlayerPrefs.SetString("GetGem", (double.Parse(PlayerPrefs.GetString("GetGem", "0")) + howMuch).ToString());
        StartCoroutine("GetGemAnimation", howMuch);
        //else m_Gem += howMuch;
    }

    IEnumerator GetGemAnimation(int howMuch)
    {
        int tmp = howMuch;
        if (howMuch > 10) howMuch = 10;
        Vector2 vector = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
        GameObject[] gem = new GameObject[howMuch];
        Vector2[] gemPosition = new Vector2[howMuch];
        for (int i = 0; i < howMuch; i++)
        {
            float gemMove = 0;
            if (howMuch > 3) gemMove = 40f;
            gem[i] = Instantiate(gemIcon, new Vector2(vector.x + Random.Range(-gemMove, gemMove), vector.y + Random.Range(-gemMove, gemMove)), transform.rotation);
            gem[i].transform.SetParent(main.transform.parent);
            float scale = 1;
            if (howMuch > 2) scale = Random.Range(0.3f, 1.0f);
            gem[i].transform.localScale = new Vector2(scale, scale);
            gemPosition[i] = new Vector2(gem[i].transform.position.x, gem[i].transform.position.y);
        }

        yield return new WaitForSeconds(0.5f);

        while (Mathf.Abs(gemIcon.transform.position.x - gem[0].transform.position.x) > 0.5f || Mathf.Abs(gemIcon.transform.position.y - gem[0].transform.position.y) > 0.5f)
        {
            for (int i = 0; i < howMuch; i++)
            {
                gem[i].transform.Translate(new Vector2((gemIcon.transform.position.x - gemPosition[i].x) / 20, (gemIcon.transform.position.y - gemPosition[i].y) / 20));
            }
            yield return new WaitForSeconds(0.02f);
        }

        for(int i = 0; i < howMuch; i++)
        {
            Destroy(gem[i]);
        }

        m_Gem += tmp;
    }

    //get
    public int getClickUpgrade()
    {
        return m_clickLevel;
    }

    public int getAlienLevel(int level)
    {
        return PlayerPrefs.GetInt(level + "PowerPerSec");
    }
}
