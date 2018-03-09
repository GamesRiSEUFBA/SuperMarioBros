using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour {

	private Rigidbody2D rb;

	public bool left = true;

	void Start () {
		rb = GetComponent<Rigidbody2D> ();	
	}

	void Update () {
		if (left)
			rb.rotation += 1.3f;	
		else
			rb.rotation += -1.3f;	

		if (rb.rotation == 360)
			rb.rotation = 0;

		if (rb.rotation == -360)
			rb.rotation = 0;
	}
}
