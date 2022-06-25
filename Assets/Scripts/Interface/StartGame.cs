using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartGame : MonoBehaviour
{
    public GameObject Logo;

    public GameObject StartPanel;

    public GameObject music_Start;

    public void touch()
    {
        music_Start.GetComponent<AudioSource>().Stop();
        SceneManager.LoadScene(1);
    }

    void Awake()
    {
        Screen.SetResolution(720, 1280, true);
        Screen.fullScreen = !Screen.fullScreen;
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        //480, 800 9:16
    }


    void Start()
    {
        //Logo.transform.Find("LINK").GetComponent<Animation>().Play();          
        //Logo.transform.Find("IN").GetComponent<Animation>().Play();
        //Logo.transform.parent.GetComponent<Animation>().Play("LogoOut");

        StartCoroutine("SoundPlay");
    }

    IEnumerator SoundPlay()
    {
        while (true)
        {
            if (!Logo.activeInHierarchy) break;
            yield return new WaitForSeconds(0.01f);
        }
        StartPanel.SetActive(true);
        StartPanel.GetComponent<Animation>().PlayQueued("Title_Title");
        StartPanel.GetComponent<Animation>().PlayQueued("TouchPlzFadeInOut");
        while (true)
        {
            if (StartPanel.transform.GetChild(2).gameObject.activeInHierarchy) break;
            yield return new WaitForSeconds(0.01f);
        }
        music_Start.GetComponent<AudioSource>().Play();
    }
}

