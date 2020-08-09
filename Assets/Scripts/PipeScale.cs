using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeScale : MonoBehaviour
{	
	public bool expand = false;
	public GameObject PipeFab;
	
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(expand == false)
		{
			float cam_z = GameObject.FindWithTag("MainCamera").transform.position.z;
			float measure_z = transform.position.z - cam_z;
			
			Debug.Log(measure_z);
			
			if(measure_z < -1.5f)
			{
				expand = true;
				SpawnAdjacentPipe();
			}
		}
    }
	
	private void SpawnAdjacentPipe()
	{
		GameObject _clone = Instantiate(PipeFab, new Vector3(0, 0, transform.position.z + 3.95f), Quaternion.identity );		
		_clone.SetActive(true);
		_clone.GetComponent<PipeScale>().expand = false;
	}
}
