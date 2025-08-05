using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class beatscroll : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x > 0)
        {
            transform.position -= new Vector3(2 * Time.deltaTime, 0f, 0f);
        }
        else if (transform.position.x < 0)
        {
            transform.position -= new Vector3(-2 * Time.deltaTime, 0f, 0f);
        }
        else if (transform.position.y > 0)
        {
            transform.position -= new Vector3(0f, 2 * Time.deltaTime, 0f);
        }
        else if (transform.position.y < 0)
        {
            transform.position -= new Vector3(0f, -2 * Time.deltaTime, 0f);
        }

        if(transform.position.x == 0&& transform.position.y == 0 && transform.position.z == 0)
        {
            Destroy(gameObject);
        }
    }
}
