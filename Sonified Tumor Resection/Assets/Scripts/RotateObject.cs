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

    private float velocity = 5;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(pressLeft))
        {
            transform.Rotate(0.0f, -5 * velocity * Time.deltaTime, 0.0f);
        }
        if (Input.GetKey(pressRight))
        {
            transform.Rotate(0.0f, 5  * velocity * Time.deltaTime, 0.0f);
        }
        if (Input.GetKey(pressForward))
        {
            transform.Rotate(5 * velocity * Time.deltaTime, 0.0f, 0.0f);
        }
        if (Input.GetKey(pressBackward))
        {
            transform.Rotate(-5 * velocity * Time.deltaTime, 0.0f, 0.0f);
        }
        if (Input.GetKey(pressUp))
        {
            transform.Rotate(0.0f, 0.0f, 5 * velocity * Time.deltaTime);
        }
        if (Input.GetKey(pressDown))
        {
            transform.Rotate(0.0f, 0.0f, -5 * velocity * Time.deltaTime);
        }
    }
}
