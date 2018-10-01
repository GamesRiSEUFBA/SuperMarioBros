using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour {

	private Rigidbody2D rb;
	private int velX = 0, velY = 0;
	private Vector2 pos;

	private float distX;
	private float distY;

	void Start () {
		rb = GetComponent<Rigidbody2D> ();
		pos = rb.transform.position;
		if (distX != 0)
			velX = 2;
		if (distY != 0)
			velY = 2;
	}

	void Update () {
		if (distX != 0) {
			if (velX > 0 && rb.transform.position.x >= pos.x + distX) {
				velX = -2;
			} else if (velX < 0 && rb.transform.position.x <= pos.x) {
				velX = 2;
			}
		}
		if (distY != 0) {
			if (rb.transform.position.y >= pos.y + distY) {
				velY = -2;
			} else if (velY < 0 && rb.transform.position.y <= pos.y) {
				velY = 2;
			}
		}
		
		rb.velocity = new Vector2 (velX, velY);
	}
}
