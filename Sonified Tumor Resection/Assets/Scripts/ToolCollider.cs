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
        //2D
        vectorList.Add(Vector3.down); vectorList.Add(Vector3.up); vectorList.Add(Vector3.left); vectorList.Add(Vector3.right); vectorList.Add(Vector3.forward); vectorList.Add(Vector3.back);
        vectorList.Add(new Vector3(1, 1, 0)); vectorList.Add(new Vector3(-1, 1, 0)); vectorList.Add(new Vector3(-1, -1, 0)); vectorList.Add(new Vector3(1, -1, 0));
        vectorList.Add(new Vector3(1, 0, 1)); vectorList.Add(new Vector3(-1, 0, 1)); vectorList.Add(new Vector3(-1, 0, -1)); vectorList.Add(new Vector3(1, 0, -1));
        vectorList.Add(new Vector3(0, 1, 1)); vectorList.Add(new Vector3(0, -1, 1)); vectorList.Add(new Vector3(0, -1, -1)); vectorList.Add(new Vector3(0, 1, -1));
        vectorList.Add(new Vector3(1, 0.5f, 0)); vectorList.Add(new Vector3(0.5f, 1, 0)); vectorList.Add(new Vector3(-0.5f, 1, 0)); vectorList.Add(new Vector3(-1, 0.5f, 0));
        vectorList.Add(new Vector3(-0.5f, -1, 0)); vectorList.Add(new Vector3(-1, -0.5f, 0)); vectorList.Add(new Vector3(0.5f, -1, 0)); vectorList.Add(new Vector3(1, -0.5f, 0));
        vectorList.Add(new Vector3(0.5f, 0, 1)); vectorList.Add(new Vector3(1, 0, 0.5f)); vectorList.Add(new Vector3(-0.5f, 0, 1)); vectorList.Add(new Vector3(-1f, 0, 0.5f));
        vectorList.Add(new Vector3(-0.5f, 0, -1)); vectorList.Add(new Vector3(-1, 0, -0.5f)); vectorList.Add(new Vector3(0.5f, 0, -1)); vectorList.Add(new Vector3(1, 0, -0.5f));
        vectorList.Add(new Vector3(0, 0.5f, 1)); vectorList.Add(new Vector3(0, 1, 0.5f)); vectorList.Add(new Vector3(0, -0.5f, 1)); vectorList.Add(new Vector3(0, -1, 0.5f));
        //3D
        vectorList.Add(new Vector3(0, -0.5f, -1)); vectorList.Add(new Vector3(0, -1, -0.5f)); vectorList.Add(new Vector3(0, 0.5f, -1)); vectorList.Add(new Vector3(0, 1, -0.5f));
        vectorList.Add(new Vector3(1, 1, 1)); vectorList.Add(new Vector3(-1, 1, 1)); vectorList.Add(new Vector3(-1, -1, 1)); vectorList.Add(new Vector3(1, -1, 1));
        vectorList.Add(new Vector3(-1, 1, -1)); vectorList.Add(new Vector3(1, 1, -1));vectorList.Add(new Vector3(1, -1, -1)); vectorList.Add(new Vector3(1, 1, -1));
        vectorList.Add(new Vector3(1, 0.5f, 1)); vectorList.Add(new Vector3(0.5f, 1, 1)); vectorList.Add(new Vector3(-0.5f, 1, 1)); vectorList.Add(new Vector3(-1, 0.5f, 1));
        vectorList.Add(new Vector3(-0.5f, -1, 1)); vectorList.Add(new Vector3(-1, -0.5f, 1)); vectorList.Add(new Vector3(0.5f, -1, 1)); vectorList.Add(new Vector3(1, -0.5f, 1));
        vectorList.Add(new Vector3(1, 1, 0.5f));  vectorList.Add(new Vector3(-1f, 1, 0.5f)); vectorList.Add(new Vector3(-0.5f, 1, -1)); vectorList.Add(new Vector3(-1, 1, -0.5f));
        vectorList.Add(new Vector3(1, -1, 0.5f)); vectorList.Add(new Vector3(1, -0.5f, -1)); vectorList.Add(new Vector3(1, -1, -0.5f)); vectorList.Add(new Vector3(1, 0.5f, -1)); 
        vectorList.Add(new Vector3(-1, -1, -1)); vectorList.Add(new Vector3(-1, 0.5f, -1)); vectorList.Add(new Vector3(-0.5f, -1, -1));  vectorList.Add(new Vector3(0.5f, -1, -1)); vectorList.Add(new Vector3(-1f, -1, 0.5f));
    }

    void Start()
    {
        tumor = GameObject.Find("Tumor");
        toolTip = GameObject.Find("ToolTip");
        tumorDistance.text = "";
        alertText.text = "";
        currentArea.text = "";
        audioManager = FindObjectOfType<AudioManager>();
  
        // todo check where the tool is and start the correct sound (would probably always be outside the cutting area in the "too far" section)
        audioManager.PlayPrimary("NoArea");
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
                audioManager.SetPitchPrimary(Constants.MaxPitch - scaledValue);
                Debug.Log("Pitch:" + (Constants.MaxPitch - scaledValue));
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
            vesselDistance.text = "Distance to vessel: " + minDistanceVessel;
            //update sound
            if (minDistanceVessel <= Constants.MaxObstacleDistance)
            {
                float scaledValue = Scaled(minDistanceVessel, 0, Constants.MaxObstacleDistance, Constants.MinVolume, Constants.MaxVolume);
                audioManager.SetVolumePrimary(Constants.MaxVolume - scaledValue);
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
            audioManager.PlayPrimary(Constants.MarginsSound);
            UpdateCanvas(color: Color.red, area: "No area for cutting", alert: "Do not cut here! You are too far away from the tumor");
        }
        else if (col.gameObject.CompareTag("ResectionArea"))
        {
            UpdateCanvas(color: Color.green, area: "Current area: resection area", alert: "You can cut here!");
        }
        else if (col.gameObject.CompareTag("InnerErrorMargin"))
        {
            UpdateCanvas(color: Color.red, area: "Current area: inner error margin of tumor", alert: "Do not cut here! You are too close to the tumor");
        }
        else if (col.gameObject.CompareTag("Tumor"))
        {
            audioManager.PlayPrimary(Constants.TumorSound);
            FindObjectOfType<AudioManager>().Play("Tumor");
            UpdateCanvas(color: Color.red, area: "Current area: tumor area", alert: "Attention! You touched the tumor!");
        }
    }

    void OnTriggerExit(Collider col)
    {
        // stop sound depending on the area that was exited
        if (col.gameObject.CompareTag("OuterErrorMargin"))
        {
            audioManager.PlayPrimary(Constants.OuterAreaSound);
            UpdateCanvas(color: Color.red, area: "No area for cutting", alert: "Do not cut here! You are too far away from the tumor");
        }
        else if (col.gameObject.CompareTag("ResectionArea"))
        {
            UpdateCanvas(color: Color.red, area: "Current area: outer error margin of tumor", alert: "Do not cut here! You are too far away from the tumor");
        }
        else if (col.gameObject.CompareTag("InnerErrorMargin"))
        {
            UpdateCanvas(color: Color.green, area: "Current area: resection area", alert: "You can cut here!");
        }
        else if (col.gameObject.CompareTag("Tumor"))
        {
            audioManager.PlayPrimary(Constants.MarginsSound);
            UpdateCanvas(color: Color.red, area: "Current area: inner error margin of tumor", alert: "Do not cut here! You are too close to the tumor");
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
            if (Physics.Raycast(ray, out hit, maxRayDistance))
            {
                //Debug.Log("Distance to: " + hit.collider.name + " is: " + hit.distance);
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
            Debug.DrawRay(transform.position, ray.direction, Color.blue, 1000, false);
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

    public void UpdateCanvas(Color color, string area, string alert)
    {
        currentArea.color = color;
        currentArea.text = area;
        alertText.color = color;
        alertText.text = alert;
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
