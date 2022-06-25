using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Transparency : MonoBehaviour
{
    void Start()
    {
        //sprite 이미지 read/write 체크, 메쉬타입 풀렉 으로 하면 됨 메쉬타입은 안해두 되는듯??
        this.GetComponent<Image>().alphaHitTestMinimumThreshold = 0.1f;
    }
}
   