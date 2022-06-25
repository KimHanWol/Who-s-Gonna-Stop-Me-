using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boss : MonoBehaviour
{
    [HideInInspector]
    public GameObject Count;

    private DummyMomTraining dummyMomTraining;

    private PartTimeJob partTimeJob;

    public bool animationWorking;

    // Start is called before the first frame update
    void Start()
    {
        Count = this.transform.Find("Count").gameObject;
        dummyMomTraining = this.transform.Find("MomTraining").GetComponent<DummyMomTraining>();
        partTimeJob = this.transform.Find("PartTimeJob").GetComponent<PartTimeJob>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator CountAnimation()
    {
        animationWorking = true;
        Count.SetActive(true);
        Text count = Count.transform.GetChild(0).GetComponent<Text>();
        float timer = 0;
        count.fontSize = 200;
        while (true)
        {
            timer += Time.deltaTime;
            count.fontSize = (int)((timer % 1) * 200);
            count.text = (3 - (int)timer).ToString();
            yield return new WaitForSeconds(0.01f);
            if (timer > 3) break;
        }
        count.fontSize = 200;
        count.text = "Game Start!";
        yield return new WaitForSeconds(1);
        Count.SetActive(false);
        animationWorking = false;
    }

    public void MomTrainingClick()
    {
        if (DataController.GetInstance().MomTrainingTicket == 0) return;
        DataController.GetInstance().MomTrainingTicket--;
        PlayerPrefs.SetString("UseTickets", (int.Parse(PlayerPrefs.GetString("UseTickets", "0")) + 1).ToString());
        this.transform.Find("MomTraining").gameObject.SetActive(true);
        StartCoroutine("CountAnimation");
        //엄마전투 시작
        dummyMomTraining.StartGame();
    }
    
    public void PartTimeJobClick()
    {
        if (DataController.GetInstance().PartTimeJobTicket == 0) return;
        DataController.GetInstance().PartTimeJobTicket--;
        PlayerPrefs.SetString("UseTickets", (int.Parse(PlayerPrefs.GetString("UseTickets", "0")) + 1).ToString());
        this.transform.Find("PartTimeJob").gameObject.SetActive(true);
        StartCoroutine("CountAnimation");
        //아르바이트 시작
        partTimeJob.StartGame();
    }
}
