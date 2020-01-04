using UnityEngine;

public class ToolController : MonoBehaviour
{

    [SerializeField]
    private float speed = 1;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(horizontal, vertical);
        transform.position += movement * Time.deltaTime * speed; // make movement framerate independent

    }
}
