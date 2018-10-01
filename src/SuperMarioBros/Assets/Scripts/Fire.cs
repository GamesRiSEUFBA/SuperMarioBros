using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour {

	private Transform rb;

	private bool left = true;

	void Start () {
		rb = GetComponent<Transform> ();	
	}

	void Update () {
		if (left)
			rb.Rotate(0, 0, 1.3f);	
		else
			rb.Rotate(0, 0, -1.3f);	
	}
}
