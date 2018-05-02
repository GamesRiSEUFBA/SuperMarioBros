using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform2 : MonoBehaviour {

	private Rigidbody2D rb;
	private bool down = false;

	public Vector2 initPos;
	public Vector2 endPos;
	public float vel = 3;

	void Start () {
		rb = GetComponent<Rigidbody2D> ();
		rb.velocity = new Vector2 (0, vel);

		if (initPos.y >= endPos.y) {
			down = true;
		}
	}

	void Update () {
		rb.velocity = new Vector2 (0, vel);

		if (down) {
			if (rb.position.y <= endPos.y) {
				Instantiate (this.gameObject, initPos, rb.transform.localRotation);
				Destroy (this.gameObject);
			}
		} else {
			if (rb.position.y >= endPos.y) {
				Instantiate (this.gameObject, initPos, rb.transform.localRotation);
				Destroy (this.gameObject);
			}
		}
	}
}
