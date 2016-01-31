using UnityEngine;
using System.Collections;

public class LightFliker : MonoBehaviour {

    Light myLight;
    
	// Use this for initialization
	void Start ()
    {
        myLight = this.GetComponent<Light>();
        StartCoroutine(Fliker());
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public IEnumerator Fliker()
    {
        while(true)
        {
            myLight.intensity = Random.Range(3f ,5f);
            yield return new WaitForSeconds(0.1f);
        }
    }
}
