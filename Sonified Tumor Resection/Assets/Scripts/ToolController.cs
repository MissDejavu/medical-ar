using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ToolController : MonoBehaviour
{
    //public Text tumorDistance;
    //public Text currentArea;

    [SerializeField]
    private float speed = 1;

    // Start is called before the first frame update
    void Start()
    {
        //tumorDistance.text = "Distance to tumor: ...";
        //currentArea.text = "Current area: ...";
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(horizontal, vertical);
        transform.position += movement * Time.deltaTime * speed; // make movement framerate independent

        // update values in user interface
        //tumorDistance.text = "updated tumor distance..";
        //currentArea.text = "updated area";

    }
}
