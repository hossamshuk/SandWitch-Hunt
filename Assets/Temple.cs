using UnityEngine;
using System.Collections;

public class Temple : MonoBehaviour {
    public float ritualMeter;
	public GUIStyle SliderStyle, ThumbStyle;
	
	// Use this for initialization
	void Start ()
    {

	}
	
	// Update is called once per frame
	void Update ()
    {
    
    }

    public void OnTriggerStay(Collider other)
    {
		if(!GameManager.Instance || !GameManager.Instance.bMatchActive)
			return;
        if(other.gameObject.tag.Contains("Worshiper"))
        {
            if(ritualMeter < 100)
				ritualMeter += 0.01f;
        }
    }
	
	void OnGUI()
	{
		Vector3 ScreenPos = Camera.main.WorldToScreenPoint(transform.position);
		Rect rect = new Rect(ScreenPos.x - 50, ScreenPos.y, 100, 5);
		GUI.HorizontalSlider(rect, ritualMeter, 0.0f, 100.0f);//, SliderStyle, SliderStyle);
	}
}
