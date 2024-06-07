using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class paddlemove : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //testing purposes to move the first paddle
        if (Input.GetKey("w"))
        {
            transform.position = new Vector3( transform.position.x, transform.position.y + 0.1f, 0);
        }
        
        if (Input.GetKey("s"))
        {
            transform.position = new Vector3( transform.position.x, transform.position.y - 0.1f, 0);
        }
        //locks the paddle to the top and bottom of the screen
        if (transform.position.y >= 9.3)
        {
            transform.position = new Vector3( transform.position.x, 9.3f, 0);
        }
        if (transform.position.y <= 1.5)
        {
            transform.position = new Vector3( transform.position.x, 1.5f, 0);
        }

    }
}
