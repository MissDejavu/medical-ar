using UnityEngine;
using UnityEngine.UI;

//This script handles everything with playing sounds and displaying texts regarding the distance between tool tip and tumor
public class ToolCollider : MonoBehaviour
{
    public GameObject tumor;
    public GameObject toolTip;
    private float _distance;
    private float tumorRadius;
    public Text tumorDistance;
    public Text alertText;
    public Text currentArea;
    public AudioManager audioManager;

    void Start()
    {
        tumor = GameObject.Find("Tumor");
        toolTip = GameObject.Find("ToolTip");
        tumorRadius = tumor.GetComponent<SphereCollider>().radius;
        tumorDistance.text = "";
        alertText.text = "";
        currentArea.text = "";
        audioManager = FindObjectOfType<AudioManager>();
    }

    void Update()
    {
        _distance = Vector3.Distance(tumor.transform.position, gameObject.transform.position);
        _distance -= tumorRadius;
        tumorDistance.text = "Distance to tumor: " + _distance;
    }

    void OnTriggerEnter(Collider col)
    {
        audioManager.StopAll();
        // start playing sound depending on the area that was entered
        if (col.gameObject.CompareTag("OuterErrorMargin"))
        {
            audioManager.Play("OuterErrorMargin");
            currentArea.color = Color.red;
            currentArea.text = "Current area: outer error margin";
            alertText.color = Color.red;
            alertText.text = "Do not cut here! You are too far away from the tumor";
        }
        else if (col.gameObject.CompareTag("ResectionArea"))
        {
            audioManager.Play("ResectionArea");
            currentArea.color = Color.green;
            currentArea.text = "Current area: resection area";
            alertText.color = Color.green;
            alertText.text = "You can cut here!";
        }
        else if (col.gameObject.CompareTag("InnerErrorMargin"))
        {
            audioManager.Play("InnerErrorMargin");
            currentArea.color = Color.red;
            currentArea.text = "Current area: inner error margin";
            alertText.color = Color.red;
            alertText.text = "Do not cut here! You are too close to the tumor";
        }
        else if (col.gameObject.CompareTag("Tumor"))
        {
            audioManager.LoopStart("Tumor");
            FindObjectOfType<AudioManager>().Play("Tumor");
            currentArea.color = Color.red;
            currentArea.text = "Current area: tumor area";
            alertText.color = Color.red;
            alertText.text = "Attention! You touched the tumor!";
        }

    }

    void OnTriggerExit(Collider col)
    {

        // stop sound depending on the area that was exited
        if (col.gameObject.CompareTag("OuterErrorMargin"))
        {
            audioManager.Stop("OuterErrorMargin");
            audioManager.Play("NoArea");
            alertText.color = Color.red;
            alertText.text = "Do not cut here! You are too far away from the tumor";
            currentArea.color = Color.red;
            currentArea.text = "No area";
        }
        else if (col.gameObject.CompareTag("ResectionArea"))
        {
            audioManager.Stop("ResectionArea");
            audioManager.Play("OuterErrorMargin");
            alertText.color = Color.red;
            alertText.text = "Do not cut here! You are too far away from the tumor";
            currentArea.color = Color.red;
            currentArea.text = "Current area: outer error margin";
        }
        else if (col.gameObject.CompareTag("InnerErrorMargin"))
        {
            audioManager.Stop("InnerErrorMargin");
            audioManager.Play("ResectionArea");
            alertText.color = Color.green;
            alertText.text = "You can cut here!";
            currentArea.color = Color.green;
            currentArea.text = "Current area: resection area";
        }
        else if (col.gameObject.CompareTag("Tumor"))
        {

            audioManager.LoopStop("Tumor");
            audioManager.Stop("Tumor");
            audioManager.Play("InnerErrorMargin");
            alertText.color = Color.red;
            alertText.text = "Do not cut here! You are too close to the tumor";
            currentArea.color = Color.red;
            currentArea.text = "Current area: inner error margin";
        }

    }
}
