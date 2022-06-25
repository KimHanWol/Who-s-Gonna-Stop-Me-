using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Event : MonoBehaviour
{
    public GameObject AttendancePanel;

    public int attendanceDays {
        get
        {
            return PlayerPrefs.GetInt("AttendanceDays", 0);
        }
        set
        {
            PlayerPrefs.SetInt("AttendanceDays", value);
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Attendance()
    {
        DataController.GetInstance().gainGem(1);
    }

    public void AttendanceButtonClick()
    {
        for (int i = 0; i < 6; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                AttendancePanel.transform.GetChild(0).GetChild(1).GetChild(0).GetChild(0).GetChild(i).GetChild(j).GetChild(0).GetComponent<Text>().text = (j + 1 + i * 5).ToString();
                if(j + 1 + i * 5 > attendanceDays)
                {
                    AttendancePanel.transform.GetChild(0).GetChild(1).GetChild(0).GetChild(0).GetChild(i).GetChild(j).GetComponent<CanvasGroup>().interactable = false;
                    AttendancePanel.transform.GetChild(0).GetChild(1).GetChild(0).GetChild(0).GetChild(i).GetChild(j).GetComponent<CanvasGroup>().alpha = 0.5f;
                }
            }
        }
        AttendancePanel.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
