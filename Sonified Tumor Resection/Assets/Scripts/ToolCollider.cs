using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//This script handles everything with playing sounds and displaying texts regarding the distance between tool tip and tumor
public class ToolCollider : MonoBehaviour
{
    public GameObject tumor;
    public GameObject toolTip;
    public Text tumorDistance;
    public Text vesselDistance;
    public Text currentText;
    public AudioManager audioManager;

    List<float> distancesTumor = new List<float>();
    List<float> distancesVessel = new List<float>();
    readonly List<Vector3> vectorList = new List<Vector3>();

    //For raycasting
    public float maxRayDistance = 0.5f; // 50cm

    void Awake()
    {
        //2D
        vectorList.Add(Vector3.down); vectorList.Add(Vector3.left); vectorList.Add(Vector3.right); vectorList.Add(Vector3.forward); vectorList.Add(Vector3.back);
        vectorList.Add(new Vector3(-1, -1, 0)); vectorList.Add(new Vector3(1, -1, 0)); vectorList.Add(new Vector3(1, 0, 1)); vectorList.Add(new Vector3(-1, 0, 1)); 
        vectorList.Add(new Vector3(-1, 0, -1)); vectorList.Add(new Vector3(1, 0, -1)); vectorList.Add(new Vector3(0, -1, 1)); vectorList.Add(new Vector3(0, -1, -1));
        vectorList.Add(new Vector3(-0.5f, -1, 0)); vectorList.Add(new Vector3(-1, -0.5f, 0)); vectorList.Add(new Vector3(0.5f, -1, 0)); vectorList.Add(new Vector3(1, -0.5f, 0));
        vectorList.Add(new Vector3(0.5f, 0, 1)); vectorList.Add(new Vector3(1, 0, 0.5f)); vectorList.Add(new Vector3(-0.5f, 0, 1)); vectorList.Add(new Vector3(-1f, 0, 0.5f));
        vectorList.Add(new Vector3(-0.5f, 0, -1)); vectorList.Add(new Vector3(-1, 0, -0.5f)); vectorList.Add(new Vector3(0.5f, 0, -1)); vectorList.Add(new Vector3(1, 0, -0.5f));
        vectorList.Add(new Vector3(0, -0.5f, 1)); vectorList.Add(new Vector3(0, -1, 0.5f)); vectorList.Add(new Vector3(0, -0.5f, -1)); vectorList.Add(new Vector3(0, -1, -0.5f)); 
        vectorList.Add(new Vector3(-1, -1, 1)); vectorList.Add(new Vector3(1, -1, 1)); vectorList.Add(new Vector3(1, -1, -1)); vectorList.Add(new Vector3(-0.5f, -1, 1)); 
        vectorList.Add(new Vector3(-1, -0.5f, 1)); vectorList.Add(new Vector3(0.5f, -1, 1)); vectorList.Add(new Vector3(1, -0.5f, 1)); vectorList.Add(new Vector3(1, -1, 0.5f)); 
        vectorList.Add(new Vector3(1, -0.5f, -1)); vectorList.Add(new Vector3(1, -1, -0.5f)); vectorList.Add(new Vector3(-1, -1, -1)); vectorList.Add(new Vector3(-0.5f, -1, -1));  
        vectorList.Add(new Vector3(0.5f, -1, -1)); vectorList.Add(new Vector3(-1f, -1, 0.5f));

        vectorList.Add(new Vector3(-0.25f, -1, 0)); vectorList.Add(new Vector3(-1, -0.25f, 0)); vectorList.Add(new Vector3(0.25f, -1, 0)); vectorList.Add(new Vector3(1, -0.25f, 0));
        vectorList.Add(new Vector3(0.25f, 0, 1)); vectorList.Add(new Vector3(1, 0, 0.25f)); vectorList.Add(new Vector3(-0.25f, 0, 1)); vectorList.Add(new Vector3(-1f, 0, 0.25f));
        vectorList.Add(new Vector3(-0.25f, 0, -1)); vectorList.Add(new Vector3(-1, 0, -0.25f)); vectorList.Add(new Vector3(0.25f, 0, -1)); vectorList.Add(new Vector3(1, 0, -0.25f));
        vectorList.Add(new Vector3(0, -0.25f, 1)); vectorList.Add(new Vector3(0, -1, 0.25f)); vectorList.Add(new Vector3(0, -0.25f, -1)); vectorList.Add(new Vector3(0, -1, -0.25f));
        vectorList.Add(new Vector3(-1, -1, 1)); vectorList.Add(new Vector3(1, -1, 1)); vectorList.Add(new Vector3(1, -1, -1)); vectorList.Add(new Vector3(-0.25f, -1, 1));
        vectorList.Add(new Vector3(-1, -0.25f, 1)); vectorList.Add(new Vector3(0.25f, -1, 1)); vectorList.Add(new Vector3(1, -0.25f, 1)); vectorList.Add(new Vector3(1, -1, 0.25f));
        vectorList.Add(new Vector3(1, -0.25f, -1)); vectorList.Add(new Vector3(1, -1, -0.25f)); vectorList.Add(new Vector3(-1, -1, -1)); vectorList.Add(new Vector3(-0.25f, -1, -1));
        vectorList.Add(new Vector3(0.25f, -1, -1)); vectorList.Add(new Vector3(-1f, -1, 0.25f));

        vectorList.Add(new Vector3(-0.75f, -1, 0)); vectorList.Add(new Vector3(-1, -0.75f, 0)); vectorList.Add(new Vector3(0.75f, -1, 0)); vectorList.Add(new Vector3(1, -0.75f, 0));
        vectorList.Add(new Vector3(0.75f, 0, 1)); vectorList.Add(new Vector3(1, 0, 0.75f)); vectorList.Add(new Vector3(-0.75f, 0, 1)); vectorList.Add(new Vector3(-1f, 0, 0.75f));
        vectorList.Add(new Vector3(-0.75f, 0, -1)); vectorList.Add(new Vector3(-1, 0, -0.75f)); vectorList.Add(new Vector3(0.75f, 0, -1)); vectorList.Add(new Vector3(1, 0, -0.75f));
        vectorList.Add(new Vector3(0, -0.75f, 1)); vectorList.Add(new Vector3(0, -1, 0.75f)); vectorList.Add(new Vector3(0, -0.75f, -1)); vectorList.Add(new Vector3(0, -1, -0.75f));
        vectorList.Add(new Vector3(-1, -1, 1)); vectorList.Add(new Vector3(1, -1, 1)); vectorList.Add(new Vector3(1, -1, -1)); vectorList.Add(new Vector3(-0.75f, -1, 1));
        vectorList.Add(new Vector3(-1, -0.75f, 1)); vectorList.Add(new Vector3(0.75f, -1, 1)); vectorList.Add(new Vector3(1, -0.75f, 1)); vectorList.Add(new Vector3(1, -1, 0.75f));
        vectorList.Add(new Vector3(1, -0.75f, -1)); vectorList.Add(new Vector3(1, -1, -0.75f)); vectorList.Add(new Vector3(-1, -1, -1)); vectorList.Add(new Vector3(-0.75f, -1, -1));
        vectorList.Add(new Vector3(0.75f, -1, -1)); vectorList.Add(new Vector3(-1f, -1, 0.75f));


    }

    void Start()
    {
        tumor = GameObject.Find("Tumor");
        toolTip = GameObject.Find("ToolTip");
        tumorDistance.text = "";
        currentText.text = "";
        audioManager = FindObjectOfType<AudioManager>();
    }

    void Update()
    {
        distancesTumor = MeasureDistanceToTumor(vectorList);
        distancesVessel = MeasureDistanceToVessel(vectorList);

        if (distancesTumor.Count > 0)
        {
            float minDistanceTumor = FindMinDistance(distancesTumor);
            if (Constants.DebugLogAll)
            {
                //Debug.Log("Min distance to tumor: " + minDistanceTumor);
            }
            tumorDistance.text = "Distance to tumor: " + minDistanceTumor;

            //update sound
            if (minDistanceTumor <= Constants.TotalMaxDistance && minDistanceTumor >= 0)
            {
                float scaledValue = Scaled(minDistanceTumor, 0, Constants.TotalMaxDistance, Constants.MinPitch, Constants.MaxPitch);
                audioManager.SetPitch(Constants.MarginsSound, Constants.MinPitch + scaledValue);
                //Debug.Log("Pitch:" + (Constants.MaxPitch - scaledValue));
            }
        }
        else
        {
            //Debug.Log("No valid min distance. Your scalpel is not well positioned towards the tumor.");
            tumorDistance.text = "Distance to tumor cannot be measured. Change position of your scapel.";
        }

        if (distancesVessel.Count > 0)
        {
            float minDistanceVessel = FindMinDistance(distancesVessel);
            if (Constants.DebugLogAll)
            {
                //Debug.Log("Min distance to vessel: " + minDistanceVessel);
            }
            
            //update sound
            if (minDistanceVessel <= Constants.MaxObstacleDistance)
            {
                // Same sound with volume changing
                //float scaledValue = Scaled(minDistanceVessel, 0, Constants.MaxObstacleDistance, Constants.MinVolume, Constants.MaxVolume);
                //audioManager.SetVolume(Constants.MarginsSound, Constants.MaxVolume - scaledValue);
                //Debug.Log("Volume:" + (Constants.MaxVolume - scaledValue));

                // New sound with pitch changing
                float scaledValue = Scaled(minDistanceVessel, 0, Constants.MaxObstacleDistance, Constants.MinFrequency, Constants.MaxFrequency);
                audioManager.SetHighPassFrequency(scaledValue+1000);
                Debug.Log("scaledValue: " + scaledValue);
                vesselDistance.text = "You are nearing a blood vessel! (Distance:  " + minDistanceVessel + ")";
            }
            else
            {
                audioManager.SetHighPassFrequency(Constants.MeanFrequency);
                vesselDistance.text = "";
            }
        }
        else
        {
            //Debug.Log("No valid min distance. Your scalpel is not well positioned towards or the vessel.");
            vesselDistance.text = "Distance to vessel cannot be measured. Change position of your scapel.";
        }
        distancesTumor.Clear();
        distancesVessel.Clear();
    }

    void OnTriggerEnter(Collider col)
    {           
        // play sound and update text depending on the area
        if (col.gameObject.CompareTag("OuterErrorMargin"))
        {
            audioManager.Play(Constants.MarginsSound);
            UpdateCanvas(color: Color.cyan, "You are too far away from the tumor");
        }
        else if (col.gameObject.CompareTag("ResectionArea"))
        {
            UpdateCanvas(color: Color.green,"Resection area");
        }
        else if (col.gameObject.CompareTag("InnerErrorMargin"))
        {
            UpdateCanvas(color: Color.magenta, "You are near the tumor!");
        }
        else if (col.gameObject.CompareTag("Tumor"))
        {
            audioManager.Stop(Constants.MarginsSound);
            audioManager.Play(Constants.TumorSound);
            UpdateCanvas(color: Color.red, "Attention! You touched the tumor!");
        }
    }

    void OnTriggerExit(Collider col)
    {
        // stop sound depending on the area that was exited
        if (col.gameObject.CompareTag("OuterErrorMargin"))
        {
            audioManager.Stop(Constants.MarginsSound);
            UpdateCanvas(color: Color.blue, "Outside the surgical field!");
        }
        else if (col.gameObject.CompareTag("ResectionArea"))
        {
            UpdateCanvas(color: Color.cyan, "You are too far away from the tumor");
        }
        else if (col.gameObject.CompareTag("InnerErrorMargin"))
        {
            UpdateCanvas(color: Color.green, "Resection area");
        }
        else if (col.gameObject.CompareTag("Tumor"))
        {
            audioManager.Stop(Constants.TumorSound);
            audioManager.Play(Constants.MarginsSound);
            UpdateCanvas(color: Color.magenta, "You are near the tumor!");
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
            Debug.DrawRay(transform.position, ray.direction, Color.green, 0.5f, false);
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
            //Debug.DrawRay(transform.position, ray.direction, Color.blue, 1000, false);
            if (Physics.Raycast(ray, out hit, maxRayDistance))
            {
                //Debug.Log("Distance to: " + hit.collider.name + " is: " + hit.distance);
                if (hit.collider.name == "BloodVessel")
                {
                    distances.Add(hit.distance);
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

    public void UpdateCanvas(Color color, string text)
    {
        currentText.color = color;
        currentText.text = text;
    }

    // scale a value (value) from the range [originMin, originMax] to a new range [targetMin, targetMax]
    public float Scaled(float value, float originMin, float originMax, float targetMin, float targetMax)
    {
        if (value > originMax || value < originMin)
        {
            Debug.LogWarning("Can't scale. Value not in range.");
            return 0;
        }
        return ((value - originMin) / (originMax - originMin)) * (targetMax - targetMin) + targetMin;
    }
}
