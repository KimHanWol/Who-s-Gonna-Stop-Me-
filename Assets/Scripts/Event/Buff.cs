using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Buff : MonoBehaviour
{
    //버프를 4개 만들고 버프 컨트롤러를 없애고 SHOP 스크립트에서 직접 뽑을 수 있게 만들자...

    public string BuffName;

    public bool nameToggle;

    public Sprite buffImage;

    public float Effect;

    [HideInInspector]
    public Transform buffPosition;

    [HideInInspector]
    public bool Touched;

    /*
     * 1. 터치 시 힘 ~배
     * 2. 터치 시 데미지 ~배
     * 3. 크리티컬 2배
     * 4. 피버 증가량 ~배
     */

    [HideInInspector]
    public bool active;

    public float buffDuration {
        get
        {
            return PlayerPrefs.GetFloat(BuffName + "BuffDuration", 0);
        }
        set
        {
            PlayerPrefs.SetFloat(BuffName + "BuffDuration", value);
        }
    }

    public double getEffect {
        get
        {
            return double.Parse(PlayerPrefs.GetString(BuffName, "0"));
        }
        set
        {
            PlayerPrefs.GetString(BuffName, value.ToString());
        }
    }

    void Start()
    {
        buffDuration = 0;
    }

    IEnumerator BuffDuration()
    {
        while (true)
        {
            if (buffDuration > 0) buffPosition.GetChild(0).GetComponent<Text>().text = DataBase.GetInstance().TimeController(buffDuration);
            else
            {
                active = false;
                buffPosition.GetComponent<CanvasGroup>().alpha = 0;
                buffPosition.GetComponent<Image>().sprite = null;

                getEffect /= Effect;
                break;
            }
            yield return new WaitForSeconds(0.1f);
        }
        buffDuration = 0;
    }

    void Update()
    {
        if (buffDuration > 0) buffDuration -= Time.deltaTime * 60;
    }

    public void getBuffPosition()
    {
        int tmpBuffActive = 0;
        for (int i = 0; i < 4; i++)
        {
            if (DataBase.GetInstance().buff[i].active == true) tmpBuffActive++;
        }
        buffPosition = Interface.GetInstance().buffPanel.transform.GetChild(3 - tmpBuffActive);
    }

    public void getBuff()
    {
        if (DataController.GetInstance().m_Gem >= 1)
        {
            DataController.GetInstance().m_Gem--;
            //PlayerPrefs.SetString("Buff", (int.Parse(PlayerPrefs.GetString("Buff", "0")) + 1).ToString());
            if (!active)
            {
                getBuffPosition();
                active = true;
            }
            StartCoroutine("BuffDuration");
            buffPosition.GetComponent<CanvasGroup>().alpha = 1;
            buffPosition.GetComponent<Image>().sprite = buffImage;
        }
    }
}
