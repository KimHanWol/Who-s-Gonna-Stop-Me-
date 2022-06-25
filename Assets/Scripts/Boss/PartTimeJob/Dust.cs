using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dust : MonoBehaviour
{
    [HideInInspector]
    private int Scale;

    // Start is called before the first frame update
    void Start()
    {
        this.transform.position = new Vector2(Random.Range(-250, 250), Random.Range(-350, 350));
        Scale = Random.Range(1, 5);
        this.transform.localScale = new Vector2(Scale, Scale);
        ScaleSetting();
        StartCoroutine("GetBigger");
    }

    public void ScaleSetting()
    {
        if (Scale == 0) Destroy(this.gameObject);
        this.transform.localScale = new Vector2(0.2f * Scale, 0.2f * Scale);
    }

    IEnumerator GetBigger()
    {
        while (true)
        {
            if (Scale < 5) Scale++;
            ScaleSetting();
            yield return new WaitForSeconds(Random.Range(1, 3));
        }
    }

    public void DustClicked()
    {
        Scale--;
        ScaleSetting();
    }
}
