using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class actions : MonoBehaviour {


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		if( Input.touchCount > 0 )
		{
			Touch touch = Input.GetTouch(0);
			if(touch.phase == TouchPhase.Moved)
			{
				var delta = touch.deltaPosition;
				transform.Rotate(Vector3.up, delta.x);
			}
		}
		
	}
}
