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

    List<float> distancesTumor = new List<float>();
    List<float> distancesVessel = new List<float>();
    List<Vector3> vectorList = new List<Vector3>();

    //For raycasting
    public float maxRayDistance = Mathf.Infinity;

    void Awake()
    {
        vectorList.Add(Vector3.down); vectorList.Add(Vector3.up); vectorList.Add(Vector3.left); vectorList.Add(Vector3.right); vectorList.Add(Vector3.forward); vectorList.Add(Vector3.back);
        vectorList.Add(new Vector3(1, 1, 0)); vectorList.Add(new Vector3(-1, 1, 0)); vectorList.Add(new Vector3(-1, -1, 0)); vectorList.Add(new Vector3(1, -1, 0));
        vectorList.Add(new Vector3(1, 0, 1)); vectorList.Add(new Vector3(-1, 0, 1)); vectorList.Add(new Vector3(-1, 0, -1)); vectorList.Add(new Vector3(1, 0, -1));
        vectorList.Add(new Vector3(0, 1, 1)); vectorList.Add(new Vector3(0, -1, 1)); vectorList.Add(new Vector3(0, -1, -1)); vectorList.Add(new Vector3(0, 1, -1));
        vectorList.Add(new Vector3(1, 0.5f, 0)); vectorList.Add(new Vector3(0.5f, 1, 0)); vectorList.Add(new Vector3(-0.5f, 1, 0)); vectorList.Add(new Vector3(-1, 0.5f, 0));
        vectorList.Add(new Vector3(-0.5f, -1, 0)); vectorList.Add(new Vector3(-1, -0.5f, 0)); vectorList.Add(new Vector3(0.5f, -1, 0)); vectorList.Add(new Vector3(1, -0.5f, 0));
        vectorList.Add(new Vector3(0.5f, 0, 1)); vectorList.Add(new Vector3(1, 0, 0.5f)); vectorList.Add(new Vector3(-0.5f, 0, 1)); vectorList.Add(new Vector3(-1f, 0, 0.5f));
        vectorList.Add(new Vector3(-0.5f, 0, -1)); vectorList.Add(new Vector3(-1, 0, -0.5f)); vectorList.Add(new Vector3(0.5f, 0, -1)); vectorList.Add(new Vector3(1, 0, -0.5f));
        vectorList.Add(new Vector3(0, 0.5f, 1)); vectorList.Add(new Vector3(0, 1, 0.5f)); vectorList.Add(new Vector3(0, -0.5f, 1)); vectorList.Add(new Vector3(0, -1, 0.5f));
        vectorList.Add(new Vector3(0, -0.5f, -1)); vectorList.Add(new Vector3(0, -1, -0.5f)); vectorList.Add(new Vector3(0, 0.5f, -1)); vectorList.Add(new Vector3(0, 1, -0.5f));
    }

    void Start()
    {
        tumor = GameObject.Find("Tumor");
        toolTip = GameObject.Find("ToolTip");
        tumorDistance.text = "";
        alertText.text = "";
        currentArea.text = "";
        audioManager = FindObjectOfType<AudioManager>();
    }

    void Update()
    {
        distancesTumor = MeasureDistanceToTumor(vectorList);
        distancesVessel = MeasureDistanceToVessel(vectorList);

        if (distancesTumor.Count > 0)
        {
            float minDistanceTumor = FindMinDistance(distancesTumor);
            Debug.Log("Min distance to tumor: " + minDistanceTumor);
            tumorDistance.text = "Distance to tumor: " + minDistanceTumor;
        }
        else
        {
            Debug.Log("No valid min distance. Your scalpel is not well positioned towards the tumor.");
            tumorDistance.text = "Distance to tumor cannot be measured. Change position of your scapel.";
        }

        if (distancesVessel.Count > 0)
        {
            float minDistanceVessel = FindMinDistance(distancesVessel);
            Debug.Log("Min distance to vessel: " + minDistanceVessel);
            vesselDistance.text = "Distance to vessel: " + minDistanceVessel;
        }
        else
        {
            Debug.Log("No valid min distance. Your scalpel is not well positioned towards or the vessel.");
            vesselDistance.text = "Distance to vessel cannot be measured. Change position of your scapel.";
        }
        distancesTumor.Clear();
        distancesVessel.Clear();
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

    List<float> MeasureDistanceToTumor(List<Vector3> vector3List)
    {
        RaycastHit hit;
        List<float> distances = new List<float>();
        List<Ray> rays = new List<Ray>();

        foreach (Vector3 vector in vector3List)
        {
            rays.Add(new Ray(transform.position, transform.TransformDirection(vector)));
        }

        foreach (Ray ray in rays)
        {
            Debug.DrawRay(transform.position, ray.direction, Color.green, 1000, false);
            Debug.Log("New ray");
            if (Physics.Raycast(ray, out hit, maxRayDistance))
            {
                Debug.Log("Distance to: " + hit.collider.name + " is: " + hit.distance);
                if (hit.collider.name == "Tumor")
                {
                    distances.Add(hit.distance);
                }
            }
        }
        return (distances);
    }

    List<float> MeasureDistanceToVessel(List<Vector3> vector3List)
    {
        RaycastHit hit;
        List<float> distances = new List<float>();
        List<Ray> rays = new List<Ray>();

        foreach (Vector3 vector in vector3List)
        {
            rays.Add(new Ray(transform.position, transform.TransformDirection(vector)));
        }

        foreach (Ray ray in rays)
        {
            Debug.DrawRay(transform.position, ray.direction, Color.green, 1000, false);
            Debug.Log("New ray");
            if (Physics.Raycast(ray, out hit, maxRayDistance))
            {
                Debug.Log("New ray");
                if (Physics.Raycast(ray, out hit, maxRayDistance))
                {
                    Debug.Log("Distance to: " + hit.collider.name + " is: " + hit.distance);
                    if (hit.collider.name == "BloodVessel")
                    {
                        distances.Add(hit.distance);
                    }
                }
            }
        }
        return (distances);
    }

    float FindMinDistance(List<float> distances)
    {
        float minDistance = distances[0];
        if (distances.Count > 0)
        {
            foreach (float distance in distances)
            {
                if (distance < minDistance)
                {
                    minDistance = distance;
                }
            }
        }
        return (minDistance);
    }
}
