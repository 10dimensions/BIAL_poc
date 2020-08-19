using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraVel : MonoBehaviour
{	
	private int speed = 10;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 Movement = new Vector3 (Input.GetAxis("Horizontal"),Input.GetAxis("Vertical"),  0f);
		transform.position += Movement * speed * Time.deltaTime;
		Debug.Log(transform.position);
    }
}



 