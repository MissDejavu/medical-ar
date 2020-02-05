using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TumorKeywords : MonoBehaviour
{
    public GameObject tumor;
    // Start is called before the first frame update
    void Awake()
    {
        tumor = GameObject.Find("Tumor");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowTumor()
    {
        var col = tumor.GetComponent<Renderer>().material.color;
        col.a = 1.0f;
        for (int a = 0; a < transform.childCount; a++)
        {
            col = transform.GetChild(a).gameObject.GetComponent<Renderer>().material.color;
            col.a = 1.0f;
        }
             
    }

    public void HideTumor()
    {
        var col2 = tumor.GetComponent<Renderer>().material.color;
        col2.a = 0.0f;
        for (int a = 0; a < transform.childCount; a++)
        {
            col2 = transform.GetChild(a).gameObject.GetComponent<Renderer>().material.color;
            col2.a = 0.0f;
        }
    }
}
