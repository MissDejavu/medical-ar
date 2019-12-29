using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolCollider : MonoBehaviour
{
    void OnTriggerEnter(Collider col)
    {
        FindObjectOfType<AudioManager>().StopAll();
        // start playing sound depending on the area that was entered
        if (col.gameObject.CompareTag("OuterErrorMargin"))
        {
            FindObjectOfType<AudioManager>().Play("OuterErrorMargin");
        }
        else if (col.gameObject.CompareTag("ResectionArea"))
        {
            FindObjectOfType<AudioManager>().Play("ResectionArea");
        }
        else if (col.gameObject.CompareTag("InnerErrorMargin"))
        {
            FindObjectOfType<AudioManager>().Play("InnerErrorMargin");
        }
        else if (col.gameObject.CompareTag("TumorArea"))
        {
            FindObjectOfType<AudioManager>().Play("TumorArea");
        }

    }

    void OnTriggerExit(Collider col)
    {

        // stop sound depending on the area that was exited
        if (col.gameObject.CompareTag("OuterErrorMargin"))
        {
            FindObjectOfType<AudioManager>().Stop("OuterErrorMargin");
            FindObjectOfType<AudioManager>().Play("NoArea");
        }
        else if (col.gameObject.CompareTag("ResectionArea"))
        {
            FindObjectOfType<AudioManager>().Stop("ResectionArea");
            FindObjectOfType<AudioManager>().Play("OuterErrorMargin");
        }
        else if (col.gameObject.CompareTag("InnerErrorMargin"))
        {
            FindObjectOfType<AudioManager>().Stop("InnerErrorMargin");
            FindObjectOfType<AudioManager>().Play("ResectionArea");
        }
        else if (col.gameObject.CompareTag("TumorArea"))
        {
            FindObjectOfType<AudioManager>().Stop("TumorArea");
            FindObjectOfType<AudioManager>().Play("InnerErrorMargin");
        }

    }
}
