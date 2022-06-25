using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fever : MonoBehaviour
{
    public Text tmp;

    public GameObject planetButton;

    //피버중 일때는 -1;
    private float fever;

    private int MaxFever;

    private double TmpDamagePerClick;

    public GameObject PartnerEffect;

    public ParticleSystem FlowerEffect;

    public ParticleSystem HeartRain;

    public ParticleSystem MoneyExposion;

    public float feverDuration {
        get
        {
            return float.Parse(PlayerPrefs.GetString("FeverDuration", "10"));
        }
        set
        {
            PlayerPrefs.SetString("FeverDuration", value.ToString());
        }
    }

    public float feverDamagePerClick {
        get
        {
            return float.Parse(PlayerPrefs.GetString("FeverDamagePerClick", "1.5"));
        }
        set
        {
            PlayerPrefs.SetString("FeverDamagePerClick", value.ToString());
        }
    }

    public float gaugeIncreasement {
        get
        {
            return float.Parse(PlayerPrefs.GetString("GaugeIncreasement", "0.5"));
        }
        set
        {
            PlayerPrefs.SetString("GaugeIncreasement", value.ToString());
        }
    }


    public float getFever()
    {
        return fever;
    }

    public void setFever(int tmp)
    {
        fever = tmp;
    }

    public void AddFever(float newFever)
    {
        if (fever == MaxFever) return;
        //피버중일때
        if (fever == -1) return;
        fever += newFever;
    }

    public void ResetFever()
    {
        fever = 0;
    }

    public int getMaxFever()
    {
        return MaxFever;
    }

    public void setMaxFever(int newMaxFever)
    {
        MaxFever = newMaxFever;
        this.gameObject.GetComponent<Slider>().maxValue = MaxFever;
    }

    public void setFeverDuration(float newFeverDuration)
    {
        feverDuration = newFeverDuration;
    }

    // Start is called before the first frame update
    void Start()
    {
        fever = 0;
        feverDamagePerClick = 1.5f;
        setMaxFever(10);
    }

    // Update is called once per frame
    void Update()
    {
        this.gameObject.GetComponent<Slider>().value = fever;
        if (fever >= MaxFever) FeverTimeStart();
        tmp.text = fever.ToString();
    }

    public void FeverTimeStart()
    {
        Debug.Log("FeverTIme!!");
        planetButton.GetComponent<PlanetClick>().feverOn = true;
        PlayerPrefs.SetInt("FeverTime", PlayerPrefs.GetInt("FeverTime", 0) + 1);
        fever = -1;
        planetButton.GetComponent<Button>().interactable = false;
        //planetButton.GetComponent<Animation>().Play();
        StartCoroutine("Vibration");
    }
    IEnumerator Vibration()
    {
        float time = 0;
        for (int i = 1; i < 100; i++)
        {
            Vector2 planetPosition = new Vector2(planetButton.transform.position.x, planetButton.transform.position.y);
            planetButton.transform.position = new Vector2(planetButton.transform.position.x + Random.Range(-i/2, i/2), planetButton.transform.position.y + Random.Range(-i/2, i/2));
            yield return new WaitForSeconds(0.01f);
            planetButton.transform.position = planetPosition;
            time += Time.deltaTime;
            if (time > 2) break;
        }
        planetButton.GetComponent<Button>().interactable = true;
        FeverTime();
    }

    public void FeverTime()
    {
        feverDuration = 10;
        planetButton.GetComponent<PlanetClick>().feverOn = true;
        double tmp = DataController.GetInstance().m_damagePerClick;
        TmpDamagePerClick = DataController.GetInstance().m_damagePerClick;
        DataController.GetInstance().m_damagePerClick = feverDamagePerClick * DataController.GetInstance().m_damagePerClick;
        //planetButton.GetComponent<PlanetClick>().PlanetHealthSlider.value = float.Parse(PlayerPrefs.GetString("PlanetHealth"));
        //float feverDamage = planetButton.GetComponent<PlanetClick>().PlanetFeverSlider.value - planetButton.GetComponent<PlanetClick>().PlanetHealthSlider.value;

        StartCoroutine("FeverDuration");
    }


    public void FeverEnd()
    {
        StartCoroutine("Damaged", planetButton);
        StartCoroutine("Damaged", planetButton.GetComponent<PlanetClick>().PlanetHealthSlider.gameObject);
        StartCoroutine("FeverDamageCul");
        DataController.GetInstance().m_damagePerClick = TmpDamagePerClick;
        planetButton.GetComponent<PlanetClick>().feverOn = false;
        ResetFever();
    }

    IEnumerator WaitAnimation()
    {
        if (PartnerEffect.GetComponent<Animation>().IsPlaying("PartnerMotion"))
        {
            yield return new WaitForSeconds(2f);
            while (PartnerEffect.GetComponent<Animation>().IsPlaying(DataBase.GetInstance().partner.whatAnimation)) yield return new WaitForSeconds(0.01f);
            for (int i = 0; i < 5; i++) PartnerEffect.transform.GetChild(i + 1).gameObject.SetActive(false);
            PartnerEffect.SetActive(false);
        }
        FeverEnd();
    }

    IEnumerator FeverDamageCul()
    {
        planetButton.GetComponent<Button>().interactable = false;
        planetButton.GetComponent<PlanetClick>().PlanetHealthSlider.value = float.Parse(PlayerPrefs.GetString("PlanetHealth")) / float.Parse(PlayerPrefs.GetString("PlanetMaxHealth")) * 100;
        yield return new WaitForSeconds(1);
        double damage = planetButton.GetComponent<PlanetClick>().PlanetFeverSlider.value - planetButton.GetComponent<PlanetClick>().PlanetHealthSlider.value;
        while (true)
        {
            planetButton.GetComponent<PlanetClick>().PlanetFeverSlider.value -= (float)damage / 100;
            if (planetButton.GetComponent<PlanetClick>().PlanetFeverSlider.value < planetButton.GetComponent<PlanetClick>().PlanetHealthSlider.value)
            {
                planetButton.GetComponent<PlanetClick>().PlanetFeverSlider.value = planetButton.GetComponent<PlanetClick>().PlanetHealthSlider.value;
                break;
            }
            yield return new WaitForSeconds(0.01f);
        }
        if (planetButton.GetComponent<PlanetClick>().PlanetHealthSlider.value != 0) planetButton.GetComponent<Button>().interactable = true;
    }

    IEnumerator FeverDuration()
    {
        while (feverDuration > 0)
        {
            yield return new WaitForSeconds(0.01f);
            feverDuration -= Time.deltaTime;
        }
        FeverEffect();
    }

    public void FeverEffect()
    {
        planetButton.GetComponent<Button>().interactable = false;
        if (DataBase.GetInstance().partner != null)
        {
            PartnerEffect.SetActive(true);
            PartnerEffect.transform.GetChild(0).Find("Partner").GetComponent<Image>().sprite = DataBase.GetInstance().partner.image;
            PartnerEffect.GetComponent<Animation>().Play("PartnerMotion");

            for(int i = 0; i < 5; i++) PartnerEffect.transform.GetChild(i + 1).GetComponent<Image>().sprite = DataBase.GetInstance().partner.PartnerEffectImage[i];         //null이 정상적으로 들어가는지 확인해야함.
            PartnerEffect.GetComponent<Animation>().PlayQueued(DataBase.GetInstance().partner.whatAnimation);
        }
        StartCoroutine("WaitAnimation");
    }

    IEnumerator Damaged(GameObject gameObject)
    {
        for (int i = 1; i < 10; i++)
        {
            Vector2 ObjectPosition = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y);
            gameObject.transform.position = new Vector2(gameObject.transform.position.x + Random.Range(-30, 30), gameObject.transform.position.y + Random.Range(-30, 30));
            yield return new WaitForSeconds(0.01f);
            gameObject.transform.position = ObjectPosition;
        }

    }

}
