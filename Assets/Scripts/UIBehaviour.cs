using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using UnityEngine.Android;

public class UIBehaviour : MonoBehaviour
{	
	public CoreARTracking CoreTracker;
	public Button Back;
	public GameObject prevContainer;

	[Header("Home")]
	public GameObject HomeContainer;
	public Button PreConstruction;
	public Button PostConstruction;
	
	[Header("GPS")]
	public Text LatText;
	public Text LonText;
	public Text LatText1;
	public Text LonText1;
	public Text LatText2;
	public Text LonText2;
	
	[Header("Pre-Coonstruction")]
	public GameObject PreConstructionContainer;
	public Button AddObject;
	public Button AddMeasurement;
	
	[Header("3D Object")]
	public GameObject TDObjectContainer;
	public Button AddNewObject;
	public GameObject ObjectScrollBar;
	public Button CloseScrollBar;
	public Button DeleteObject;
	
	[Header("MeasurementTape")]
	public Button AddMeasurementTape;
	
	private bool gpsON = false;
	

    // Start is called before the first frame update
	
     void Start()
    {	
		#if UNITY_ANDROID
			if (!Permission.HasUserAuthorizedPermission(Permission.FineLocation))
			{
				Permission.RequestUserPermission(Permission.FineLocation);
			}
		#endif
		
		StartCoroutine(StartLocationService());
    }
	
	IEnumerator StartLocationService()
	{
		// First, check if user has location service enabled
        if (!Input.location.isEnabledByUser)
            yield break;

        // Start service before querying location
        Input.location.Start();

        // Wait until service initializes
        int maxWait = 20;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            yield return new WaitForSeconds(1);
            maxWait--;
        }

        // Service didn't initialize in 20 seconds
        if (maxWait < 1)
        {
            print("Timed out");
            yield break;
        }

        // Connection has failed
        if (Input.location.status == LocationServiceStatus.Failed)
        {
            print("Unable to determine device location");
            yield break;
        }
        else
        {	
			gpsON = true;
	
            // Access granted and location value could be retrieved
            print("Location: " + Input.location.lastData.latitude + " " + Input.location.lastData.longitude );
			LatText.text = Input.location.lastData.latitude.ToString();
			LonText.text = Input.location.lastData.longitude.ToString();
			LatText1.text = Input.location.lastData.latitude.ToString();
			LonText1.text = Input.location.lastData.longitude.ToString();
			LatText2.text = Input.location.lastData.latitude.ToString();
			LonText2.text = Input.location.lastData.longitude.ToString();
		}

        // Stop service if there is no need to query location updates continuously
        // Input.location.Stop();
	}
	
	void Update()
	{
		if(gpsON)
        {
            // Access granted and location value could be retrieved
            //print("Location: " + Input.location.lastData.latitude + " " + Input.location.lastData.longitude);
			//LatText.text = Input.location.lastData.latitude.ToString();
			//LonText.text = Input.location.lastData.longitude.ToString();
        }
	}
	
		
	private void GoBack()
	{	
		Back.gameObject.SetActive(false);
		prevContainer.SetActive(false);
		HomeContainer.SetActive(true);
	}
	
	
	public void DeleteObjectSelected()
	{
		if(CoreTracker.RemovePawnObject)
			Destroy(CoreTracker.RemovePawnObject);
	}
	
}