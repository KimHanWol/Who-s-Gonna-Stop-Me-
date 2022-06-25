using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Information : MonoBehaviour
{
    private Text InformationText;

    private Text EndingInformationText;

    public Fever fever;

    // Start is called before the first frame update
    void Start()
    {
        InformationText = this.transform.GetChild(0).GetChild(1).GetChild(0).GetChild(0).GetComponent<Text>();
        EndingInformationText = this.transform.GetChild(0).GetChild(1).GetChild(0).GetChild(1).GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        InformationText.text = "DamagePerClick : " + DataController.GetInstance().m_damagePerClick + "\n"
                             + "PowerPerClick : " + DataController.GetInstance().m_powerPerClick + "\n"
                             + "PowerPerSec : " + DataController.GetInstance().m_powerPerSec + "\n"
                             + "Critical : " + DataController.GetInstance().m_critical + "%\n"
                             + "FeverDuration : " + fever.feverDuration + "s\n"
                             + "FeverDamage : " + fever.feverDamagePerClick * DataController.GetInstance().m_damagePerClick + "\n"
                             + "GaugeIncreasement : " + fever.gaugeIncreasement + "\n"
                             + "AlienFriends : " + DataBase.GetInstance().howMuchAlien + "\n";
        EndingInformationText.text = "You Need " + "<color=#ff0000>" + (50 - DataBase.GetInstance().howMuchAlien) + "</color>" + " Aliens \nFor Ending!";
    }
}
