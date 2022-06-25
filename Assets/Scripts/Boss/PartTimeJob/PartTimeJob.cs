using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PartTimeJob : MonoBehaviour
{                                                               //먼지가 랜덤으로 화면에 나타나고 일정시간 이내에 먼지를 연속으로 닦으면
                                                                //추가 점수를 얻는 걸 추가하자.
    private float timer;

    public GameObject dust;

    private float Score;

    public void StartGame()
    {
        StartCoroutine("TimerLoop");
        this.transform.GetChild(0).gameObject.SetActive(true);
    }

    IEnumerator TimerLoop()
    {
        while (this.transform.parent.GetComponent<Boss>().animationWorking)
        {
            yield return new WaitForSeconds(0.1f);
        }
        timer = 30;
        StartCoroutine("MakeDust");
        StartCoroutine("DustRemoved");
        StartCoroutine("Cleaning");
        while (timer > 0)
        {
            timer -= Time.deltaTime * 6;
            yield return new WaitForSeconds(0.1f);
            this.transform.GetChild(0).Find("TimeDisplayer").GetComponent<Text>().text = (Mathf.Ceil(timer * 10) / 10).ToString();
        }

        this.transform.GetChild(0).gameObject.SetActive(false);
        this.transform.GetChild(1).gameObject.SetActive(true);

        Transform End = this.transform.GetChild(1);
        End.Find("EndButton").GetChild(0).GetComponent<Text>().text = "Get" + (int)(Score / 10) + "Gems";
        if ((int)(Score / 10) < 1) End.Find("EndButton").GetChild(0).GetComponent<Text>().text = "End Game";
        End.Find("Score").GetComponent<Text>().text = (Mathf.Ceil(Score * 10) / 10).ToString();
    }

    IEnumerator Cleaning()
    {
        Vector2 startPos;
        while (timer > 0)
        {
            if (!this.transform.GetChild(0).Find("DustPanel").gameObject.activeInHierarchy)
            {
                if (Input.GetMouseButton(0) || Input.GetMouseButtonDown(0)) {
                    startPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
                    yield return new WaitForSeconds(0.01f);
                    Score += Mathf.Sqrt(Mathf.Pow(Mathf.Abs(startPos.x - Input.mousePosition.x), 2) + Mathf.Pow(Mathf.Abs(startPos.y - Input.mousePosition.x), 2)) / 1000;
                }
                this.transform.GetChild(0).Find("ScoreDisplayer").GetComponent<Text>().text =  (Mathf.Ceil(Score * 10) / 10).ToString();
            }
            yield return new WaitForSeconds(0.01f);
        }
    }

    IEnumerator MakeDust()
    {
        while (timer > 0)
        {
            GameObject tmp;
            tmp = Instantiate(dust, new Vector2(Random.Range(-360, 360), Random.Range(-640, 640)), transform.rotation);
            tmp.transform.SetParent(this.transform.GetChild(0).Find("DustPanel"));
            this.transform.GetChild(0).Find("DustPanel").gameObject.SetActive(true);
            yield return new WaitForSeconds(Random.Range(2, 5));
        }
    }

    IEnumerator DustRemoved()
    {
        while (timer > 0)
        {
            if (this.transform.GetChild(0).Find("DustPanel").childCount == 0) this.transform.GetChild(0).Find("DustPanel").gameObject.SetActive(false);
            yield return new WaitForSeconds(0.1f);
        }
    }

    public void EndGame()
    {
        DataController.GetInstance().gainGem((int)(Score / 10));
        this.transform.GetChild(1).gameObject.SetActive(false);
        this.transform.gameObject.SetActive(false);
        this.transform.GetChild(0).gameObject.SetActive(true);
    }
} 
