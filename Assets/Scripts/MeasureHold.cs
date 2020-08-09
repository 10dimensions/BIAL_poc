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
		lr.startWidth =  0.3f;
		lr.endWidth = 0.3f;
		lr.startColor = Color.cyan;
		lr.endColor = Color.blue;
		lr.SetPosition(0, start);
		lr.SetPosition(1, start);
	}
	
	public void HoldDraw()
	{
		GetPlaneIntersection();
		if(lr)
			lr.SetPosition(1, intersectionPoint);
		Debug.Log(intersectionPoint);
	}
	
	private void GetPlaneIntersection()
	{	
		Transform cameraTransform = Camera.main.transform;
		
		Ray RayOrigin;
		RaycastHit HitInfo;
 
		if(Physics.Raycast(cameraTransform.position,cameraTransform.forward, out HitInfo, 100.0f))
		{
			if(HitInfo.transform.tag == "ShadowPlane")
				intersectionPoint = HitInfo.point;
		}	
	}
}