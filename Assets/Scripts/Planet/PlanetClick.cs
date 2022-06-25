using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlanetClick : MonoBehaviour
{
    public AudioSource PlanetTouchSound;

    public AudioSource FeverTouchSound;

    public Fever fever;

    public bool ActiveEffect
    {
        get
        {
            return bool.Parse(PlayerPrefs.GetString("ActiveEffect", "True"));
        }
        set
        {
            PlayerPrefs.SetString("ActiveEffect", value.ToString());
        }
    }
    public GameObject FeverEffect;

    public Slider PlanetHealthSlider;

    public Slider PlanetFeverSlider;

    public Slider FeverGaugeSlider;

    public Text planetHealthText;

    [HideInInspector]
    public int active = 0;
    [HideInInspector]
    public bool change = false;

    //public GameObject planetbutton;

    public Vector2 center;

    //피버시에는 체력이 닳는 것이 보이지 않는다.
    [HideInInspector]
    public bool feverOn;

    public void Start()
    {
        //행성 정보 생성
        if (!PlayerPrefs.HasKey("PlanetMaxHealth")) PlayerPrefs.SetString("PlanetMaxHealth", "1000");
        if (!PlayerPrefs.HasKey("PlanetHealth")) PlayerPrefs.SetString("PlanetHealth", PlayerPrefs.GetString("PlanetMaxHealth"));
    
        this.GetComponent<Image>().sprite = DataBase.GetInstance().planet[DataBase.GetInstance().Theme].getPlanetImage();
        PlanetHealthSlider.maxValue = 100;
        PlanetHealthSlider.value = float.Parse(PlayerPrefs.GetString("PlanetHealth")) / float.Parse(PlayerPrefs.GetString("PlanetMaxHealth")) * PlanetHealthSlider.maxValue;
        center = this.transform.position;
    }

    public void OnClick()
    {
        //critical
        if (Random.Range(1, (1/DataController.GetInstance().m_critical * 100)) == 1) ClickPlanet();
        //normal
        ClickPlanet();

        /*if (DataBase.GetInstance().partner != null) */   //DataBase.GetInstance().partner.SoundEffect.Play();

        //fever
        fever.GetComponent<Fever>().AddFever(fever.GetComponent<Fever>().gaugeIncreasement);

        //행성 변화
        if (double.Parse(PlayerPrefs.GetString("PlanetHealth")) <= 0)
        {
            StartCoroutine("ChangeButton");
        }

        //touch 업적 달성
        PlayerPrefs.SetString("TouchPlanet", (double.Parse(PlayerPrefs.GetString("TouchPlanet", "0")) + 1).ToString());

        //Effect
        if (this.GetComponent<Animation>().isPlaying) this.GetComponent<Animation>().Stop();
        this.GetComponent<Animation>().Play();

        //FeverEffect
        if (feverOn) StartCoroutine("FeverEffectActive");

        //SoundEffect
        if (!feverOn) PlanetTouchSound.Play();
    }

    private void ClickPlanet()
    {
        //행성 데미지 계산
        DataController.GetInstance().m_power += DataController.GetInstance().m_damagePerClick * DataController.GetInstance().m_powerPerDamage;
        if (double.Parse(PlayerPrefs.GetString("PlanetHealth")) - DataController.GetInstance().m_damagePerClick <= 0) PlayerPrefs.SetString("PlanetHealth", "0");
        else PlayerPrefs.SetString("PlanetHealth", (double.Parse(PlayerPrefs.GetString("PlanetHealth")) - DataController.GetInstance().m_damagePerClick).ToString());

        PlanetHealthSlider.maxValue = PlanetFeverSlider.maxValue = 100;
        float sldierValue = float.Parse(PlayerPrefs.GetString("PlanetHealth")) / float.Parse(PlayerPrefs.GetString("PlanetMaxHealth")) * PlanetHealthSlider.maxValue;
        if (!feverOn) PlanetHealthSlider.value = PlanetFeverSlider.value = sldierValue;
        //else PlanetHealthSlider.value = sldierValue;
    }

    IEnumerator FeverEffectActive()
    {
        FeverEffect.SetActive(true);
        FeverTouchSound.Play();
        yield return new WaitForSeconds(0.05f);
        FeverEffect.SetActive(false);
    }

    IEnumerator ChangeButton()
    {
        change = true;
        PlayerPrefs.SetString("PlanetHealth", "0");
        this.GetComponent<Button>().interactable = false;
        if (feverOn)
        {
            fever.setFeverDuration(0.01f);
            while (true)
            {
                if (FeverGaugeSlider.value == 0) break;
                yield return new WaitForSeconds(0.01f);
            }
        }
        fever.ResetFever();


        for (int i = 1; i >10; i++)
        {
            this.GetComponent<Image>().color = new Color(1 / i, 1 / i, 1 / i);
            yield return new WaitForSeconds(0.5f);
        }

        this.GetComponent<Button>().interactable = false;
        yield return new WaitForSeconds(3.0f);
        this.GetComponent<Button>().interactable = true;

        Interface.GetInstance().GetSome.GetComponent<GetSomething>().getSomething(DataBase.GetInstance().Theme);

        //ending
        if (DataBase.GetInstance().Theme == 9)
        {
            Debug.Log(DataBase.GetInstance().Theme);
            DataBase.GetInstance().Theme = 0;                                                                                          
            MovePlanetTheme();
            Interface.GetInstance().Clean();
            DataBase.GetInstance().MapActive = true;
        }

        this.GetComponent<Button>().interactable = true;
        this.GetComponent<Image>().sprite = DataBase.GetInstance().planet[DataBase.GetInstance().Theme].getPlanetImage();
        PlayerPrefs.SetString("PlanetMaxHealth", (double.Parse(PlayerPrefs.GetString("PlanetMaxHealth")) + Mathf.Pow(10000, (DataBase.GetInstance().planetDistroy + 1) * 0.055f + 0.75f)).ToString());                   //여기서 행성 체력 관리
        PlayerPrefs.SetString("PlanetHealth", (double.Parse(PlayerPrefs.GetString("PlanetMaxHealth"))).ToString());

        //Add PlanetDistory
        PlayerPrefs.SetInt("PlanetDistroy", (PlayerPrefs.GetInt("PlanetDistroy", 0) + 1));
        change = false;
    }

    void Update()
    {
        planetHealthText.text = (Mathf.Ceil(float.Parse(PlayerPrefs.GetString("PlanetHealth")) * 10) / 10).ToString();
    }

    //행성 테마 변환
    public void MovePlanetTheme()
    {
        this.GetComponent<Image>().sprite = DataBase.GetInstance().planet[DataBase.GetInstance().Theme - 1].getPlanetImage();
    }

    public void EffectActivation(GameObject text)
    {
        if (ActiveEffect)
        {
            ActiveEffect = false;
            //effect.GetComponent<Image>().color = new Color(0, 0, 0, 0);
            text.GetComponent<Text>().text = "Effect Active";
        }
        else
        {
            ActiveEffect = true;
            //effect.GetComponent<Image>().color = new Color(255, 255, 255, 255);
            text.GetComponent<Text>().text = "Effect Disable";
        }
    }
}