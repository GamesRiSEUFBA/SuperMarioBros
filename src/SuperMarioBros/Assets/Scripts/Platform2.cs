using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform2 : MonoBehaviour {

	private Rigidbody2D rb;
	private Vector2 initPos;

	public float vel = 3;

	void Start () {
		rb = GetComponent<Rigidbody2D> ();

		initPos = new Vector2 (123.84f, -8.16f);
		rb.velocity = new Vector2 (0, vel);
	}

	void OnBecameInvisible() {
		Instantiate (this.gameObject, initPos, rb.transform.localRotation);
		Destroy (this.gameObject);
	}

	void Update () {
		rb.velocity = new Vector2 (0, vel);
	}
}
