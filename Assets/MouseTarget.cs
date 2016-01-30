using UnityEngine;
using System.Collections;

public class MouseTarget : MonoBehaviour {
    RaycastHit hit;
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        Physics.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Camera.main.transform.forward, out hit, 1<<LayerMask.NameToLayer("Ground"));
        this.transform.position = hit.point;
        this.transform.rotation = Quaternion.Euler(hit.normal);
    }
}
