using System;
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
    public float maxRayDistance = 0.2f; // 20cm 

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
        // get all distances from rays
        distancesTumor = MeasureDistanceToTumor(vectorList);
        distancesVessel = MeasureDistanceToVessel(vectorList);

        // -------- DISTANCE TUMOR -------------
        if (distancesTumor.Count > 0)
        {
            float minDistanceTumor = FindMinDistance(distancesTumor); // get smallest distance
            HandleTumorDistance(minDistanceTumor); 
        }
        else
        {
            //Debug.Log("Could not calculate any distance to tumor.");
            tumorDistance.text = "Distance cannot be measured.";
        }
        //--------------------------------------

        //-----------DISTANCE OBSTACLES---------
        if (distancesVessel.Count > 0)
        {
            float minDistanceVessel = FindMinDistance(distancesVessel); // get smallest distance
            HandleObstacleDistance(minDistanceVessel);
        }
        else
        {
            //Debug.Log("No valid min distance. Your scalpel is not well positioned towards or the vessel.");
            //vesselDistance.text = "Distance to vessel cannot be measured. Change position of your scapel.";  // TODO don't show?
        }

        //reset distances
        distancesTumor.Clear();
        distancesVessel.Clear();
    }

    // change sound in relation to tumor distance and update canvas
    void HandleTumorDistance(float distance)
    {
        tumorDistance.text = "Distance to tumor: " + System.Math.Round(distance*1000) + " mm";  // update distance on canvas

        // tumor area
        if(distance <= 0.004)  // TODO handle touching the tumor -> problem, no distance from rays
        {
            audioManager.Stop(Constants.MarginsSound);
            audioManager.Play(Constants.TumorSound);
            UpdateCanvas(color: Color.red, "Attention! You touched the tumor!");
        }
        // margins
        else if (distance > 0.004 && distance <= Constants.TotalMaxDistance)
        {
            float scaledValue = Scaled(distance, 0, Constants.TotalMaxDistance, Constants.MinPitch, Constants.MaxPitch);
            audioManager.SetPitch(Constants.MarginsSound, Constants.MinPitch + scaledValue);
            audioManager.Stop(Constants.TumorSound); // TODO try to avoid unnecessary updates?
            audioManager.Play(Constants.MarginsSound); // TODO try to avoid unnecessary updates?
            // inner error margin area
            if (distance >= 0.004 && distance < Constants.ErrorMarginSize)
            {
                UpdateCanvas(color: Color.magenta, "You are near the tumor!");
            }
            // cutting area 
            else if (distance >= Constants.ErrorMarginSize && distance < Constants.ErrorMarginSize + Constants.CuttingAreaSize)
            {
                UpdateCanvas(color: Color.green, "Resection area");
            }
            // outer error margin
            else
            {
                UpdateCanvas(color: Color.cyan, "Too far away from the tumor");
            }         
        }
        // outer area
        else
        {
            audioManager.Stop(Constants.MarginsSound);
            UpdateCanvas(color: Color.blue, "Outside the surgical field!");
        }
    }
    // change sound in relation to obstacle distance and update canvas
    void HandleObstacleDistance(float distance)
    {
        // do sth only when smallest distance in smaller than threshold
        if (distance <= Constants.MaxObstacleDistance)
        {
            // New sound with pitch changing
            float scaledValueFreq = Scaled(distance, 0, Constants.MaxObstacleDistance, Constants.MinFrequency, Constants.MaxFrequency);
            audioManager.SetLowPassFrequency(Constants.MarginsSound, scaledValueFreq + 2500);
            float scaledValueDist = Scaled(distance, 0, Constants.MaxObstacleDistance, Constants.MaxDistortionLevel, Constants.MinDistortionLevel);
            audioManager.SetDistortionLevel(Constants.MarginsSound, scaledValueDist);
            audioManager.SetVolume(Constants.MarginsSound, 0.5f);
            //Debug.Log("scaledValue: " + scaledValue);
            vesselDistance.text = "Close to vessel! (Distance:  " + System.Math.Round(distance*1000) + " mm" + ")";
        }
        else
        {
            audioManager.SetHighPassFrequency(Constants.MarginsSound, Constants.MeanHighCutFrequency);  // TODO try to avoid unnecessary updates?
            audioManager.SetLowPassFrequency(Constants.MarginsSound, Constants.MeanLowCutFrequency);
            audioManager.SetDistortionLevel(Constants.MarginsSound, Constants.MeanDistortionLevel);
            audioManager.SetVolume(Constants.MarginsSound, 1.0f);
            vesselDistance.text = "";
        }
    }

    /*
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
    */

    /*
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
    */

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
            Debug.DrawRay(transform.position, ray.direction, Color.green, 0.1f, false);
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
            return 0;       // TODO throw error?
        }
        return ((value - originMin) / (originMax - originMin)) * (targetMax - targetMin) + targetMin;
    }
}
