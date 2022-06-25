using System.Collections;
using System.Reflection;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Achievements : MonoBehaviour
{
    /*
     * 티켓 모두 소진
     * 행성 터치 1만회
     * 피버타임 3회
     * 상점에서 버프 뽑기 3회
     * 일일 미션 모두 완료
     * 행성~번 파괴
     * 힘~모으기
     * 보석~모으기
     * 힘~사용하기
     * 보석~사용하기
     * 
     * 
     * 업적 달성하면 바로바로 보상 획득해주는 기능도 매뉴 기능으로 넣으면 좋겠다.
     * 
     * 
     * 접속 시간 보상은 30분, 1시간, 3시간 간격으로 하루마다 초기화 되고 보상은 1개 3개 10개 이렇게 늘려가면서 할 수 있게 해보자
     */
    public string AchievementName;

    public string Unit;

    private int AchievementLevel
    {
        get
        {
            return PlayerPrefs.GetInt(AchievementName + "Level", 1);
        }
        set
        {
            PlayerPrefs.SetInt(AchievementName + "Level", value);
        }
    }

    //achievement
    public double[] achievement;

    //reword
    public int[] reword;


    [HideInInspector]
    public bool[] AchieveComplete = new bool[10];

    public double Content {
        get
        {
            return double.Parse(PlayerPrefs.GetString(AchievementName, "0"));
        }
        set
        {
            PlayerPrefs.SetString(AchievementName, value.ToString());
        }
    }
    

    // Start is called before the first frame update
    void Start()
    {
    }

    public void RewordClick()
    {
        if (Content >= achievement[AchievementLevel - 1])
        {
            DataController.GetInstance().gainGem(reword[AchievementLevel - 1]);
            AchievementLevel++;
            if (this.transform.GetSiblingIndex() <= 3) PlayerPrefs.SetString("DailyAchievements", (double.Parse(PlayerPrefs.GetString("DailyAchievements", "0") + 1).ToString()));

            //Achievement Complete
            if (AchievementLevel > achievement.Length)
            {
                this.transform.Find("Button").Find("Achievement").GetComponent<Text>().text = "Achievement";
                this.transform.Find("Button").Find("Reword").GetComponent<Text>().text = "Complete";
                this.transform.Find("Button").GetComponent<Button>().interactable = false;
                StopCoroutine("RewordButtonActiveLoop");
                return;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
    }

    IEnumerator RewordButtonActiveLoop()
    {
        //버튼 활성화
        while (true)
        {
            if (Content >= achievement[AchievementLevel - 1]) this.transform.Find("Button").GetComponent<Button>().interactable = true;
            else this.transform.Find("Button").GetComponent<Button>().interactable = false;
            yield return new WaitForSeconds(0.1f);

            //텍스트 업데이트
            if (AchievementLevel <= achievement.Length) TextUpdate();
        }
    }

    public void TextUpdate()
    {
        //버튼 텍스트 변환
        this.transform.Find("Button").Find("Achievement").GetComponent<Text>().text = "(" + Content + "/" + achievement[AchievementLevel - 1] + ")";
        this.transform.Find("Button").Find("Reword").GetComponent<Text>().text = reword[AchievementLevel - 1].ToString();
    }

    void OnEnable()
    {
        if (AchievementLevel <= achievement.Length) StartCoroutine("RewordButtonActiveLoop");
    }

    void OnDisable()
    {
        StopCoroutine("RewordButtonActiveLoop");
    }
}
