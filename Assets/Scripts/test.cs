using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    public Fever fever;

    // Start is called before the first frame update
    void Start()
    {


    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ButtonClick()
    {
        DataController.GetInstance().gainGem(10);
        DataController.GetInstance().m_power = 10000;
        PlayerPrefs.SetString("PlanetHealth", 5.ToString());
        fever.setFever(99);
    }
}
