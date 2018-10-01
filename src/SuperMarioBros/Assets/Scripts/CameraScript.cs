using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour {

	private Vector2 oldCamPos;
	private Rigidbody2D rbMario;
	private MarioLuigi csMario;

	private float xEndMap;

	void Start () {
		rbMario = GameObject.Find ("Player").GetComponent<Rigidbody2D> ();
		csMario = GameObject.Find ("Player").GetComponent<MarioLuigi> ();
		oldCamPos = Camera.main.transform.position;
	}

	void Update () {
		if (Camera.main.transform.position.x == 16 && Camera.main.transform.position.y == -32) {
			Camera.main.transform.position = new Vector2 (16, -32);
		}

		if (Camera.main.transform.position.x >= xEndMap) {
			Camera.main.transform.position = new Vector3 (Camera.main.transform.position.x, Camera.main.transform.position.y, Camera.main.transform.position.z);
		}
		else if (!csMario.isDead() && oldCamPos.x < rbMario.transform.position.x) {
			Camera.main.transform.position = new Vector3 (rbMario.transform.position.x, Camera.main.transform.position.y, -10);
			oldCamPos = Camera.main.transform.position;
		}
	}
}
