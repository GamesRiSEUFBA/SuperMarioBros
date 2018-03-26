using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePhysics : MonoBehaviour {

	private Rigidbody2D rb;
	private bool onFloor = false;

	private float gravity = 25;

	void Start () {
		rb = GetComponent<Rigidbody2D> ();
	}

	void Update () {
		rb.AddForce (new Vector2 (0, -gravity), ForceMode2D.Force);
	}
}
