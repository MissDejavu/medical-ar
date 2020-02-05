using UnityEngine;

//Reset button for getting an object back to its start and rotation position
public class ResetPosition : MonoBehaviour
{
    public Vector3 startPosition;
    public Quaternion startRotation;
    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;
        startRotation = transform.rotation;
    }

    // Update is called once per frame
    void OnGUI()
    {
        if (GUI.Button (new Rect (925, 10, 100, 25), "Reset position"))
        {
            transform.position = startPosition;
            transform.rotation = startRotation;
        }
    }
}
