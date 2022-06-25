using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Sound : MonoBehaviour
{
    private static Sound instance;

    public static Sound GetInstance()
    {
        if (instance == null)
        {
            instance = FindObjectOfType<Sound>();
            if (instance == null)
            {
                GameObject container = new GameObject("Sound");

                instance = container.GetComponent<Sound>();
            }
        }
        return instance;
    }

    private AudioSource BGM_Main;

    private AudioSource Effect_Sound;

    public GameObject BGM_VolumeScroll;

    public GameObject Effect_VolumeScroll;

    //[HideInInspector]
    //public int BGM_Volume;
    //[HideInInspector]
    //public int Effect_Volume;

    // Start is called before the first frame update
    void Start()
    {
        BGM_Main = DataBase.GetInstance().Sound.transform.GetChild(0).GetChild(0).GetComponent<AudioSource>();
        Effect_Sound = DataBase.GetInstance().Sound.transform.GetChild(1).GetChild(0).GetComponent<AudioSource>();
        StartCoroutine("BGM_Management", BGM_Main);
        StartCoroutine("EffectManagement", Effect_Sound);
    }

    // Update is called once per frame
    void Update()
    {
    }

    IEnumerator BGM_Management(AudioSource bgm)
    {
        while (true)
        {
            //BGM 볼륨 조절
            if(BGM_VolumeScroll.GetComponent<Slider>().value != (int)(bgm.volume * 4)) bgm.volume = BGM_VolumeScroll.GetComponent<Slider>().value / 4;
            yield return new WaitForSeconds(0.01f);

            //BGM 재생, 정지 조절
            if (!Interface.GetInstance().GetSome.activeInHierarchy && !Interface.GetInstance().SpaceShip.activeInHierarchy && bgm.volume != 0)
            {
                if(!bgm.isPlaying) bgm.Play();
            }
            else
            {
                bgm.Stop();
            }
        }
    }

    IEnumerator EffectManagement(AudioSource effect)
    { 
        while (true)
        {
            //효과음 볼륨 조절
            if (Effect_VolumeScroll.GetComponent<Slider>().value != (int)(effect.volume * 4))
            {
                effect.volume = Effect_VolumeScroll.GetComponent<Slider>().value / 4;
                DataBase.GetInstance().TmpEffect.volume = Effect_VolumeScroll.GetComponent<Slider>().value / 4;
            }
            yield return new WaitForSeconds(0.01f);
        }
    }

    IEnumerator EffectSound(AudioSource effect)
    {
        effect.Play();
        yield return new WaitForSeconds(0.01f);
    }

    public void EffectSoundStart(AudioSource effect)
    {
        StartCoroutine("EffectSound", effect);
    }

    //public void VolumeChange()
    //{
    //    BGM_Volume = (int)BGM_VolumeScroll.GetComponent<Slider>().value;
    //    Effect_Volume = (int)Effect_VolumeScroll.GetComponent<Slider>().value;
    //}

}
