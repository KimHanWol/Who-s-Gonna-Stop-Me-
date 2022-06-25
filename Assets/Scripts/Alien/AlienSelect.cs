using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AlienSelect : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void SelectAlien()
    {
        DataBase.GetInstance().partner = DataBase.GetInstance().alien[this.transform.GetSiblingIndex() + Interface.GetInstance().bottomAlien* 5];
        Debug.Log(DataBase.GetInstance().partner.alienName);
        if(DataBase.GetInstance().partner.AlienFace != null) Interface.GetInstance().AlienSelect.transform.GetChild(0).GetChild(1).GetComponent<Image>().sprite = DataBase.GetInstance().partner.AlienFace;
        else Interface.GetInstance().AlienSelect.transform.GetChild(0).GetChild(1).GetComponent<Image>().sprite = DataBase.GetInstance().partner.image;
        Interface.GetInstance().BottomClick();
    }
}
