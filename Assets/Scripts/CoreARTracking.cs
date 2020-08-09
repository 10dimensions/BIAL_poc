using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleARCore;
using GoogleARCore.Examples.ObjectManipulation;

public class CoreARTracking : MonoBehaviour
{	
	public bool trackMode = false;
	private bool TogglePostConstruction = true;
	public PawnManipulator PawnSpawner;
	
	public GameObject RemovePawnObject;
	private GameObject PostConstructionObject;

	void Start()
	{
		//ToggleOffPlaneDetection();
	}
	
	void Update()
	{	
		if ( Input.GetMouseButtonDown (0))
		{ 
		   RaycastHit hit; 
		   Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); 
		   if ( Physics.Raycast (ray,out hit,100.0f)) {
			 Debug.Log("You selected the " + hit.transform.tag); // ensure you picked right object
			 RemovePawnObject = hit.transform.gameObject;
		  }
		 }
	
		 if ((Input.touchCount > 0) && (Input.GetTouch(0).phase == TouchPhase.Began))
		{
			Ray raycast = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
			RaycastHit raycastHit;
			if (Physics.Raycast(raycast, out raycastHit))
			{
				if (raycastHit.transform.tag == "Pawn")
				{
					//Destroy(raycastHit.transform.gameObject);
					RemovePawnObject = raycastHit.transform.gameObject;
				}
			}
		}
		
	}

	
	public void TogglePlaneTracking()
	{	
		if(trackMode)
		{
			trackMode = false;
			PawnManipulator.canSpawn = false;
			
			//RemovePlanes();
		}
		else
		{	
			StartCoroutine(TrackerTrue()); 
		}
	}
	
	IEnumerator TrackerTrue()
	{	
		yield return new WaitForSeconds(1f);
		
		trackMode = true;
		PawnManipulator.canSpawn = true;
	}
	
	private void RemovePlanes()
	{
		foreach (GameObject plane in GameObject.FindGameObjectsWithTag ("DetectedPlane")) 
		{
            Destroy(plane);
        }
	}
		
	public void ToggleOnMeasurement()
	{
		trackMode = false;
		PawnManipulator.canSpawn = false;
		FindObjectOfType<ARCoreSession>().SessionConfig.PlaneFindingMode = DetectedPlaneFindingMode.Horizontal;
	}
	
		
	public void ToggleOffPlaneDetection()
	{
		trackMode = false;
		PawnManipulator.canSpawn = false;
		FindObjectOfType<ARCoreSession>().SessionConfig.PlaneFindingMode = DetectedPlaneFindingMode.Disabled;
	}
	
	public void ToggleOnPlaneDetection()
	{
		StartCoroutine(TrackerTrue());
		FindObjectOfType<ARCoreSession>().SessionConfig.PlaneFindingMode = DetectedPlaneFindingMode.Horizontal;
	}
	
	public void ToggleARDetetection()
	{
		FindObjectOfType<ARCoreSession>().SessionConfig.PlaneFindingMode = DetectedPlaneFindingMode.Horizontal;
	}
	
	public void TogglePostConstructionObject()
	{
		TogglePostConstruction = !TogglePostConstruction;
		if(GameObject.FindWithTag("Pipe"))
		{	
			PostConstructionObject = GameObject.FindWithTag("Pipe");
			PostConstructionObject.SetActive(TogglePostConstruction);
		}
		else
		{	
			if(PostConstructionObject)
				PostConstructionObject.SetActive(TogglePostConstruction);
		}
	}
	
	public void DeletePostConstructionObject()
	{
		if(GameObject.FindWithTag("Pipe"))
		{	
			PostConstructionObject = GameObject.FindWithTag("Pipe");
			Destroy(PostConstructionObject);
		}
		else
		{
			if(PostConstructionObject)
				Destroy(PostConstructionObject);
		}
	}
	
	public void DeletePreConstructionObjects()
	{
		GameObject[] PreConstructionObjects = GameObject.FindGameObjectsWithTag("Pawn");
		
		foreach(GameObject obj in PreConstructionObjects)
		  GameObject.Destroy(obj);
	}
	
	public void DeleteMeasurement()
	{ 
		Destroy(GameObject.FindGameObjectWithTag("MeasureLine"));
	}
	
}