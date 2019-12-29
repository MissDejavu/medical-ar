using UnityEngine;

//Rotate an object with keys you like
public class RotateObject : MonoBehaviour
{
    public KeyCode pressUp;
    public KeyCode pressDown;
    public KeyCode pressLeft;
    public KeyCode pressRight;
    public KeyCode pressForward;
    public KeyCode pressBackward;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(pressLeft))
        {
            transform.Rotate(0.0f, -10, 0.0f);
        }
        if (Input.GetKeyDown(pressRight))
        {
            transform.Rotate(0.0f, +10, 0.0f);
        }
        if (Input.GetKeyDown(pressForward))
        {
            transform.Rotate(+10, 0.0f, 0.0f);
        }
        if (Input.GetKeyDown(pressBackward))
        {
            transform.Rotate(-10, 0.0f, 0.0f);
        }
        if (Input.GetKeyDown(pressUp))
        {
            transform.Rotate(0.0f, 0.0f, +5);
        }
        if (Input.GetKeyDown(pressDown))
        {
            transform.Rotate(0.0f, 0.0f, -5);
        }
    }
}
