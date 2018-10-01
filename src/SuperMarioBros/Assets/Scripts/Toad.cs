using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Toad : MonoBehaviour {

	private bool onFloor = false;
	private Rigidbody2D rbToad;
	private Animator animToad;
	private int counter = 0;

	void Start () {
		rbToad = GetComponent<Rigidbody2D> ();
		animToad = GetComponent<Animator> ();
		
	}
	
	// Update is called once per frame
	void Update () {
		if (onFloor && rbToad.velocity.y == 0) {
			if (counter == 1) {
				rbToad.AddForce (new Vector2 (0, 10), ForceMode2D.Impulse);
				animToad.SetBool ("ToadJump", true);
				counter = 0;
			} else
				counter += 1;
		}

		if (onFloor) {
			animToad.SetBool ("ToadJump", false);
		} else {
			animToad.SetBool ("ToadJump", true);
		}

	}

	void OnCollisionEnter2D(Collision2D coll) {
		if (coll.gameObject.tag == "floor") {
			onFloor = true;
			//animToad.SetBool ("ToadJump", false);
		}
	}

	void OnCollisionExit2D(Collision2D coll) {
		if (coll.gameObject.tag == "floor") {
			onFloor = false;
			//animToad.SetBool ("ToadJump", false);
		}
	}

	void OnCollisionStay2D(Collision2D coll) {
		if (coll.gameObject.tag == "floor") {
			onFloor = true;
			//animToad.SetBool ("ToadJump", false);
		} else {
			//onFloor = false;
		}
	}
}
