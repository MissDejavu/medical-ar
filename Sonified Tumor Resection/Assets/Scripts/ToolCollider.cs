using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

//This script handles everything with playing sounds and displaying texts regarding the distance between tool tip and tumor
public class ToolCollider : MonoBehaviour
{
    public GameObject tumor;
    public GameObject toolTip;
    public Text tumorDistance;
    public Text vesselDistance;
    public Text alertText;
    public Text currentArea;
    public AudioManager audioManager;

    //For Raycast
    public float maxRayDistance = 50;

    void Start()
    {
        tumor = GameObject.Find("Tumor");
        toolTip = GameObject.Find("ToolTip");
        tumorDistance.text = "";
        alertText.text = "";
        currentArea.text = "";
        audioManager = FindObjectOfType<AudioManager>();

    }

    //For raycasting
    void Update()
    {
        Ray rayDown = new Ray(transform.position, Vector3.down);
        Ray rayUp = new Ray(transform.position, Vector3.up);
        Ray rayLeft = new Ray(transform.position, Vector3.left);
        Ray rayRight = new Ray(transform.position, Vector3.right);
        Ray rayForw = new Ray(transform.position, Vector3.forward);
        Ray rayBack = new Ray(transform.position, Vector3.back);
        List<Ray> rays = new List<Ray>();
        rays.Add(rayDown); rays.Add(rayUp); rays.Add(rayLeft); rays.Add(rayRight); rays.Add(rayForw); rays.Add(rayBack);

        RaycastHit hit;
        List<float> distancesTumor = new List<float>();
        List<float> distancesVessel = new List<float>();

        foreach (Ray ray in rays)
        {
            Debug.Log("New ray");
            if (Physics.Raycast(ray, out hit, maxRayDistance))
            {
                Debug.Log("Distance to: " + hit.collider.name + " is: " + hit.distance);
                if (hit.collider.name == "Tumor")
                {
                    distancesTumor.Add(hit.distance);
                }
            }
        }

        foreach (Ray ray in rays)
        {
            Debug.Log("New ray");
            if (Physics.Raycast(ray, out hit, maxRayDistance))
            {
                Debug.Log("Distance to: " + hit.collider.name + " is: " + hit.distance);
                if (hit.collider.name == "BloodVessel")
                {
                    distancesVessel.Add(hit.distance);
                }
            }
        }

        if (distancesTumor.Count > 0)
        {
            float minDistance = distancesTumor[0];

            foreach (float distance in distancesTumor)
            {
                if (distance < minDistance)
                {
                    minDistance = distance;
                }
            }
            Debug.Log("Min Distance: " + minDistance);
            tumorDistance.text = "Distance to tumor: " + minDistance;
        }
        else
        {
            Debug.Log("No valid min distance. Your scalpel is not well positioned towards the tumor or the vessel.");
            tumorDistance.text = "Distance to tumor cannot be measured. Change position of your scapel.";
        }


        if (distancesVessel.Count > 0)
        {
            float minDistance = distancesVessel[0];

            foreach (float distance in distancesVessel)
            {
                if (distance < minDistance)
                {
                    minDistance = distance;
                }
            }
            Debug.Log("Min Distance to vessel: " + minDistance);
            vesselDistance.text = "Distance to vessel: " + minDistance;
        }
        else
        {
            Debug.Log("No valid min distance. Your scalpel is not well positioned towards the tumor or the vessel.");
            vesselDistance.text = "Distance to vessel cannot be measured. Change position of your scapel.";
        }
    }

    void OnTriggerEnter(Collider col)
    {
        audioManager.StopAll();
        // start playing sound depending on the area that was entered
        if (col.gameObject.CompareTag("OuterErrorMargin"))
        {
            audioManager.Play("OuterErrorMargin");
            currentArea.color = Color.red;
            currentArea.text = "Current area: outer error margin of tumor";
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
            currentArea.text = "Current area: inner error margin of tumor";
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
            currentArea.text = "No area for cutting";
        }
        else if (col.gameObject.CompareTag("ResectionArea"))
        {
            audioManager.Stop("ResectionArea");
            audioManager.Play("OuterErrorMargin");
            alertText.color = Color.red;
            alertText.text = "Do not cut here! You are too far away from the tumor";
            currentArea.color = Color.red;
            currentArea.text = "Current area: outer error margin of tumor";
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
            currentArea.text = "Current area: inner error margin of tumor";
        }

    }
}
