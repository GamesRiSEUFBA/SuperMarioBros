using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour {

	private Vector2 oldCamPos;
	private Rigidbody2D rbMario;

	void Start () {
		rbMario = GameObject.Find ("Player").GetComponent<Rigidbody2D> ();
		oldCamPos = Camera.main.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		if (oldCamPos.x < rbMario.transform.position.x) {
			Camera.main.transform.position = new Vector3 (rbMario.transform.position.x, Camera.main.transform.position.y, -10);
			oldCamPos = Camera.main.transform.position;
		}
		
	}
}
