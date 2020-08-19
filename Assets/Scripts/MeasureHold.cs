using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;


[RequireComponent(typeof(Button))]
public class MeasureHold : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
{
    public UnityEvent onPointerDown;
    public UnityEvent onPointerUp;

    // gets invoked every frame while pointer is down
    public UnityEvent whilePointerPressed;

    private Button _button;
	
	public GameObject ScaleMarker;
	public Material linemat;
	
	private LineRenderer lr;
	private Camera camera;
	
	private Vector3 intersectionPoint;

	public CoreARTracking ArTracker;
	

    private void Awake()
    {
        _button = GetComponent<Button>();
    }
	
	private void Start()
	{	
		camera= Camera.main;
	}	

    private IEnumerator WhilePressed()
    {
        // this looks strange but is okay in a Coroutine
        // as long as you yield somewhere
        while(true)
        {	
	
            whilePointerPressed?.Invoke();
            yield return null;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // ignore if button not interactable
        if(!_button.interactable) return;

        // just to be sure kill all current routines
        // (although there should be none)
        StopAllCoroutines();
        StartCoroutine(WhilePressed());
		
		Debug.Log("button down");
		GetPlaneIntersection();
		DrawLine(intersectionPoint);
		lr.SetPosition(1, intersectionPoint);

        onPointerDown?.Invoke();
    }

    public void OnPointerUp(PointerEventData eventData)
    {	
		Debug.Log("button up");
	
        StopAllCoroutines();
		EndLine();
        onPointerUp?.Invoke();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        StopAllCoroutines();
        onPointerUp?.Invoke();
    }
		
	void DrawLine(Vector3 start)
	{
		GameObject myLine = new GameObject();
		myLine.tag = "MeasureLine";
		myLine.transform.position = start;
		myLine.AddComponent<LineRenderer>();
		lr = myLine.GetComponent<LineRenderer>();
		lr.material = linemat;
		lr.startWidth =  0.025f;
		lr.endWidth = 0.025f;
		lr.startColor = Color.cyan;
		lr.endColor = Color.blue;
		lr.SetPosition(0, start);
		lr.SetPosition(1, start);
		
		GameObject measureObj = Instantiate(ScaleMarker, start, Quaternion.identity);
		measureObj.transform.parent =  myLine.transform;
		
	}
	
	void EndLine()
	{
		GameObject measureObj = Instantiate(ScaleMarker, lr.GetPosition(1), Quaternion.identity);
		measureObj.transform.parent =  GameObject.FindGameObjectWithTag("MeasureLine").transform;
	}
	
	public void HoldDraw()
	{
		GetPlaneIntersection();
		if(lr)
		{
			lr.SetPosition(1, intersectionPoint);
			ArTracker.ChangeMeasureValue( lr.GetPosition(0), lr.GetPosition(1) );
		}
		Debug.Log(intersectionPoint);
	}
	
	private void GetPlaneIntersection()
	{	
		Transform cameraTransform = Camera.main.transform;
		
		Ray RayOrigin;
		RaycastHit HitInfo;
 
		if(Physics.Raycast(cameraTransform.position,cameraTransform.forward, out HitInfo, 100.0f, LayerMask.GetMask("Shadow") ))
		{ 
			if(HitInfo.transform.tag == "ShadowPlane")	//ShadowPlane	//DetectedPlane
				intersectionPoint = HitInfo.point;	//new Vector3(HitInfo.point.x, 0.05f, HitInfo.point.z);
		}	
		
		 // Ray ray = Camera.main.ScreenPointToRay(new Vector3( Screen.width/2, Screen.height/2, 0));
	   // // create a plane at 0,0,0 whose normal points to +Y:
	    // Plane hPlane = new Plane(Vector3.up, Vector3.zero);
	    // //Plane.Raycast stores the distance from ray.origin to the hit point in this variable:
	    // float distance = 0; 
	    // // if the ray hits the plane...
	    // if (hPlane.Raycast(ray, out distance)){
		// // get the hit point:
		// //temp.transform.position = ray.GetPoint(distance);
		// intersectionPoint = ray.GetPoint(distance);
	   // }
	}
}