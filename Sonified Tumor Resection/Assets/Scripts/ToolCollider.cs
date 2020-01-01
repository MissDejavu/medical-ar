using UnityEngine;
using UnityEngine.UI;

//This script handles everything with playing sounds and displaying texts regarding the distance between tool tip and tumor
public class ToolCollider : MonoBehaviour
{
    public GameObject Tumor;
    public GameObject ToolTip;
    private float _Distance;
    private float TumorRadius;
    public Text TumorDistance;
    public Text AlertText;
    public Text CurrentArea;

    void Start()
    {
        TumorRadius = Tumor.GetComponent<SphereCollider>().radius;
        TumorDistance.text = "";
        AlertText.text = "";
        CurrentArea.text = "";
    }

    void Update()
    {
        _Distance = Vector3.Distance(Tumor.transform.position, ToolTip.transform.position);
        _Distance -= TumorRadius;
        TumorDistance.text = "Distance to tumor: " + _Distance;
    }

    void OnTriggerEnter(Collider col)
    {
        FindObjectOfType<AudioManager>().StopAll();
        // start playing sound depending on the area that was entered
        if (col.gameObject.CompareTag("OuterErrorMargin"))
        {
            FindObjectOfType<AudioManager>().Play("OuterErrorMargin");
            CurrentArea.color = Color.red;
            CurrentArea.text = "Current area: outer error margin";
            AlertText.color = Color.red;
            AlertText.text = "Do not cut here! You are too far away from the tumor";
        }
        else if (col.gameObject.CompareTag("ResectionArea"))
        {
            FindObjectOfType<AudioManager>().Play("ResectionArea");
            CurrentArea.color = Color.green;
            CurrentArea.text = "Current area: resection area";
            AlertText.color = Color.green;
            AlertText.text = "You can cut here!";
        }
        else if (col.gameObject.CompareTag("InnerErrorMargin"))
        {
            FindObjectOfType<AudioManager>().Play("InnerErrorMargin");
            CurrentArea.color = Color.red;
            CurrentArea.text = "Current area: inner error margin";
            AlertText.color = Color.red;
            AlertText.text = "Do not cut here! You are too close to the tumor";
        }
        else if (col.gameObject.CompareTag("TumorArea"))
        {
            FindObjectOfType<AudioManager>().LoopStart("TumorArea");
            FindObjectOfType<AudioManager>().Play("TumorArea");
            CurrentArea.color = Color.red;
            CurrentArea.text = "Current area: tumor area";
            AlertText.color = Color.red;
            AlertText.text = "Attention! You touched the tumor!";
        }

    }

    void OnTriggerExit(Collider col)
    {

        // stop sound depending on the area that was exited
        if (col.gameObject.CompareTag("OuterErrorMargin"))
        {
            FindObjectOfType<AudioManager>().Stop("OuterErrorMargin");
            FindObjectOfType<AudioManager>().Play("NoArea");
            AlertText.color = Color.red;
            AlertText.text = "Do not cut here! You are too far away from the tumor";
            CurrentArea.color = Color.red;
            CurrentArea.text = "No area";
        }
        else if (col.gameObject.CompareTag("ResectionArea"))
        {
            FindObjectOfType<AudioManager>().Stop("ResectionArea");
            FindObjectOfType<AudioManager>().Play("OuterErrorMargin");
            AlertText.color = Color.red;
            AlertText.text = "Do not cut here! You are too far away from the tumor";
            CurrentArea.color = Color.red;
            CurrentArea.text = "Current area: outer error margin";
        }
        else if (col.gameObject.CompareTag("InnerErrorMargin"))
        {
            FindObjectOfType<AudioManager>().Stop("InnerErrorMargin");
            FindObjectOfType<AudioManager>().Play("ResectionArea");
            AlertText.color = Color.green;
            AlertText.text = "You can cut here!";
            CurrentArea.color = Color.green;
            CurrentArea.text = "Current area: resection area";
        }
        else if (col.gameObject.CompareTag("TumorArea"))
        {
            
            FindObjectOfType<AudioManager>().LoopStop("TumorArea");
            FindObjectOfType<AudioManager>().Stop("TumorArea");
            FindObjectOfType<AudioManager>().Play("InnerErrorMargin");
            AlertText.color = Color.red;
            AlertText.text = "Do not cut here! You are too close to the tumor";
            CurrentArea.color = Color.red;
            CurrentArea.text = "Current area: inner error margin";
        }

    }
}
