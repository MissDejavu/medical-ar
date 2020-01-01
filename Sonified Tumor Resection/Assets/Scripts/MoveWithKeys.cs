using UnityEngine;

//use this script to move an object with keys you like (only translation)
public class MoveWithKeys : MonoBehaviour
{
    //y axis
    public KeyCode pressUp; 
    public KeyCode pressDown;
    //x axis
    public KeyCode pressLeft;
    public KeyCode pressRight;
    //z axis
    public KeyCode pressBackward;
    public KeyCode pressForward;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
     if (Input.GetKey(pressUp))
        {
            Vector3 objectPosition = transform.position;
            objectPosition.y += 0.01f;
            transform.position = objectPosition;
        }
        if (Input.GetKey(pressDown))
        {
            Vector3 objectPosition = transform.position;
            objectPosition.y -= 0.01f;
            transform.position = objectPosition;
        }
        if (Input.GetKey(pressLeft))
        {
            Vector3 objectPosition = transform.position;
            objectPosition.x -= 0.01f;
            transform.position = objectPosition;
        }
        if (Input.GetKey(pressRight))
        {
            Vector3 objectPosition = transform.position;
            objectPosition.x += 0.01f;
            transform.position = objectPosition;
        }
        if (Input.GetKey(pressForward))
        {
            Vector3 objectPosition = transform.position;
            objectPosition.z -= 0.01f;
            transform.position = objectPosition;
        }
        if (Input.GetKey(pressBackward))
        {
            Vector3 objectPosition = transform.position;
            objectPosition.z += 0.01f;
            transform.position = objectPosition;
        }
    }
}
