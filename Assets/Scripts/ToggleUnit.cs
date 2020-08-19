using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleUnit : MonoBehaviour
{	
	public Sprite background1;
	public Sprite background2;
	
	public Toggle theToggle;
	
	public string Units;

    // Start is called before the first frame update
    void Start()
    {
		theToggle = GetComponent<Toggle>();
    }

    public void ChangeBackground()
	{
			if (theToggle.isOn) {
				theToggle.image.sprite = background1;
			}
			else
			{
				theToggle.image.sprite = background2;
			}        
	}
}
