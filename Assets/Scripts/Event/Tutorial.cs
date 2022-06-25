using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    private bool showTutorial {
        get
        {
            return bool.Parse(PlayerPrefs.GetString("ShowTutorial", "True"));
        }
        set
        {
            PlayerPrefs.SetString("ShowTutorial", value.ToString());
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if (showTutorial) ShowTutorial();
        else DataBase.GetInstance().Tutorial.SetActive(false);
    }

    public void NextButtonClick()
    {
        int page = -1;
        for(int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).gameObject.activeInHierarchy)
            {
                page = i;
                break;
            }
        }
        if (page == transform.childCount - 1)
        {
            transform.GetChild(transform.childCount - 1).gameObject.SetActive(false);
            this.gameObject.SetActive(false);
            showTutorial = false;
        }
        else
        {
            transform.GetChild(page).gameObject.SetActive(false);
            transform.GetChild(page + 1).gameObject.SetActive(true);
        }
    }

    public void BackButtonClick()
    {
        int page = -1;
        for (int i = 1; i < 6; i++)
        {
            if (transform.GetChild(i).gameObject.activeInHierarchy)
            {
                page = i;
                break;
            }
        }
        transform.GetChild(page).gameObject.SetActive(false);
        transform.GetChild(page - 1).gameObject.SetActive(true);
    }

    public void ShowTutorial()
    {
        this.gameObject.SetActive(true);
        transform.GetChild(0).gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
