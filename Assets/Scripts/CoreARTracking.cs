using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleARCore;
using GoogleARCore.Examples.ObjectManipulation;
using UnityEngine.UI;

public class CoreARTracking : MonoBehaviour
{	
	public bool trackMode = false;
	private bool TogglePostConstruction = true;
	public PawnManipulator PawnSpawner;
	
	public GameObject RemovePawnObject;
	private GameObject PostConstructionObject;
	
	public GameObject ShadowPlane;
	
	public LayerMask IgnoreShadow;
	
	//public TextMesh MeasureText;
	public Text MeasureText;
	public float UnitsFactor = 100f;

	void Start()
	{
		//ToggleOffPlaneDetection();
	}
	
	void Update()
	{	
		//ShadowPlane.transform.position = new Vector3(ShadowPlane.transform.position.x, 0f, ShadowPlane.transform.position.z);
		/*
		if ( input.getmousebuttondown (0))
		{ 
		   raycasthit hit; 
		   ray ray = camera.main.screenpointtoray(input.mouseposition); 
		   if ( physics.raycast (ray,out hit,100.0f)) {
			 // // // // debug.log("you selected the " + hit.transform.tag); // ensure you picked right object
			 if (raycastHit.transform.tag == "Pawn")
			{
				RemovePawnObject = hit.transform.gameobject;
			}
		  }
		}
		*/
	
		 if ((Input.touchCount > 0) && (Input.GetTouch(0).phase == TouchPhase.Began))
		{
			Ray raycast = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
			RaycastHit raycastHit;
			if (Physics.Raycast(raycast, out raycastHit ))
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
	
	public void ExitMeasurement()
	{
		Destroy(GameObject.FindGameObjectWithTag("ShadowPlane"));
		PawnManipulator.canShadowSpawn = false;
	}
	
	public void EnterMeasurement()
	{ 
		PawnManipulator.canShadowSpawn = true;
	}
	
	public void ChangeUnits(float _val)
	{	
		float oldUnits = float.Parse( MeasureText.text ) / UnitsFactor;
		UnitsFactor = _val;
		MeasureText.text = (oldUnits * UnitsFactor).ToString();
		
	}
	
	public void ChangeMeasureValue( Vector3 lpt0, Vector3 lpt1 )
	{
		float dist = Vector3.Distance( lpt0,lpt1 )* UnitsFactor;
		
		if(dist<0.5f)
		{
			MeasureText.transform.gameObject.SetActive(false);
		}
		else
		{
			MeasureText.transform.gameObject.SetActive(true);
		}
		
		MeasureText.text = dist.ToString();
		
		//Vector3 _mp = new Vector3( (lpt0.x + lpt1.x)/2, (lpt0.y + lpt1.y)/2, (lpt0.z + lpt1.z)/2 );
		//MeasureText.transform.gameObject.transform.position = _mp;
	}
	
}