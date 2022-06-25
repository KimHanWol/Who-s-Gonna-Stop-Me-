using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DummyMomTraining : MonoBehaviour
{
    //Red light Green light
    private float timer;

    private bool foot;

    private int step;

    private int MaxStep;

    private float stepLong;

    private GameObject MomImage;

    private GameObject MyImage;

    public Sprite MomGreen;

    public Sprite MomYellow;

    public Sprite MomRed;

    //0 green
    //1 yellow
    //2 red
    private int MomState;

    void Start()
    {
        step = 0;
        MaxStep = 10; //여기서 걸음 수치 정해야함
        MomImage = this.transform.GetChild(0).Find("DummyMom").gameObject;
        MyImage = this.transform.GetChild(0).Find("Me").gameObject;
        stepLong = (Mathf.Abs(MomImage.transform.position.y) + Mathf.Abs(MyImage.transform.position.y)) / (float)MaxStep;
        this.transform.GetChild(0).Find("Left").GetComponent<Button>().interactable = true;
        this.transform.GetChild(0).Find("Right").GetComponent<Button>().interactable = false;
        MomState = 0;
    }

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
        StartCoroutine("MomMood");
        timer = 60;
        while (timer > 0)
        {
            timer -= Time.deltaTime * 6;
            yield return new WaitForSeconds(0.1f);
            this.transform.GetChild(0).Find("TimeDisplayer").GetComponent<Text>().text = (Mathf.Ceil(timer*10) / 10).ToString();
            if (step == MaxStep) break;
        }
        EndGame();
    }

    IEnumerator MomMood()
    {
        while (true)
        {
            switch (MomState) {
                case 0:
                    MomImage.GetComponent<Image>().sprite = MomYellow;
                    MomState++;
                    break;
                case 1:
                    MomImage.GetComponent<Image>().sprite = MomRed;
                    MomState++;
                    break;
                case 2:
                    MomImage.GetComponent<Image>().sprite = MomGreen;
                    MomState = 0;
                    break;
            }
            yield return new WaitForSeconds(Random.Range(0.3f, 3));
        }
    }

    public void EndGame()
    {
        this.transform.GetChild(0).gameObject.SetActive(false);
        this.transform.GetChild(1).gameObject.SetActive(true);
        if (step == MaxStep)
        {
            this.transform.GetChild(1).Find("Text").GetComponent<Text>().text = "Success";
            this.transform.GetChild(1).Find("EndButton").GetChild(0).GetComponent<Text>().text = "Get 10 Gems";
        }
        else
        {
            this.transform.GetChild(1).Find("Text").GetComponent<Text>().text = "Fail...";
            this.transform.GetChild(1).Find("EndButton").GetChild(0).GetComponent<Text>().text = "End Game";
        }
    }

    public void EndGameButtonClick()
    {
        if (step == MaxStep)
        {
            DataController.GetInstance().gainGem(10);
        }
        this.transform.GetChild(1).gameObject.SetActive(false);
        this.gameObject.SetActive(false);
    }
    

    //false left
    //true  right
    public void walk()
    {
        // Red Light
        if (MomState == 2) timer = 0;

        // else
        step++;
        if (foot)
        {
            this.transform.GetChild(0).Find("Left").GetComponent<Button>().interactable = true;
            this.transform.GetChild(0).Find("Right").GetComponent<Button>().interactable = false;
            foot = false;
        }
        else
        {
            this.transform.GetChild(0).Find("Right").GetComponent<Button>().interactable = true;
            this.transform.GetChild(0).Find("Left").GetComponent<Button>().interactable = false;
            foot = true;
        }
        MyImage.transform.position = new Vector2(MyImage.transform.position.x, MyImage.transform.position.y + stepLong);
        Debug.Log(step);
    }

}
