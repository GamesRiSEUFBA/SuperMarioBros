using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarioLuigi : MonoBehaviour {

	private Rigidbody2D rb;
	private BoxCollider2D box;
	private Animator anim;
	private SpriteRenderer sprite;
	private bool rightKeyPressed = false, leftKeyPressed = false, jumpPressed = false, firePressed = false, turnTime = false, onFloor = false, onAxe = false;
	private float maxVel = 6;

	void Start () {
		rb = GetComponent<Rigidbody2D> ();
		box = GetComponent<BoxCollider2D> ();
		anim = GetComponent<Animator> ();
		sprite = GetComponent<SpriteRenderer> ();
	}

	void Update () {
		if (Input.GetKeyDown (KeyCode.C)) {
			if (rb.velocity.y == 0 && !jumpPressed) {
				jumpPressed = true;
				rb.AddForce (new Vector2 (0, 10), ForceMode2D.Impulse);
			}
		}

		if (Input.GetKeyUp (KeyCode.C)) {
			jumpPressed = false;
		}

		if (Input.GetKeyUp (KeyCode.LeftArrow)) {
			leftKeyPressed = false;
			anim.SetBool ("Turn", false);
		} else if (Input.GetKeyUp (KeyCode.RightArrow)) {
			rightKeyPressed = false;
			anim.SetBool ("Turn", false);
		}

		if (Input.GetKeyDown (KeyCode.LeftArrow)) {
			leftKeyPressed = true;
			if (Mathf.Abs(rb.velocity.x) > 4) {
				turnTime = true;
				anim.SetBool ("Turn", true);
				rb.AddForce (new Vector2 (1.2f, 0), ForceMode2D.Impulse);
			}
		} else if (Input.GetKeyDown (KeyCode.RightArrow)) {
			rightKeyPressed = true;
			if (Mathf.Abs(rb.velocity.x) > 4) {
				turnTime = true;
				anim.SetBool ("Turn", true);
				rb.AddForce (new Vector2 (-1.2f, 0), ForceMode2D.Impulse);
			}
		}

		if (!onFloor) {
			maxVel = 3;
		} else {
			maxVel = 6;
		}

		if (Mathf.Abs (rb.velocity.x) > maxVel) {
			if (rb.velocity.x < 0)
				rb.velocity = new Vector2 (-maxVel, rb.velocity.y);
			else
				rb.velocity = new Vector2 (maxVel, rb.velocity.y);
		}

		if (jumpPressed) {
			anim.SetBool ("Jump", true);
			if (Mathf.Abs(rb.velocity.y) < 5 && rb.velocity.y > 0)
				rb.AddForce (new Vector2 (0, 10), ForceMode2D.Impulse);
			if (onFloor)
				jumpPressed = false;
		} else {
			if (rb.velocity.y == 0 && onFloor) {
				anim.SetBool ("Jump", false);
			}
		}

		if (leftKeyPressed) {
			anim.SetBool ("Walking", true);
			if (onFloor)
				sprite.flipX = true;
			if (rb.velocity.x == 0)
				turnTime = false;
			if (rb.velocity.x > 0 && !turnTime)
				rb.velocity = new Vector2 (-rb.velocity.x, rb.velocity.y);
			if (Mathf.Abs(rb.velocity.x) < maxVel)
				rb.AddForce(new Vector2(-16, 0), ForceMode2D.Force);
		} else if (rightKeyPressed) {
			anim.SetBool ("Walking", true);
			if (onFloor)
				sprite.flipX = false;
			if (rb.velocity.x == 0)
				turnTime = false;
			if (rb.velocity.x < 0 && !turnTime)
				rb.velocity = new Vector2 (-rb.velocity.x, rb.velocity.y);
			if (Mathf.Abs(rb.velocity.x) < maxVel)
				rb.AddForce(new Vector2(16, 0), ForceMode2D.Force);
		} else {
			anim.SetBool ("Walking", false);
			if (rb.velocity.x != 0)
				rb.AddForce(new Vector2((rb.velocity.x/Mathf.Abs(rb.velocity.x))*(-15), 0), ForceMode2D.Force);
		}

		if (onAxe) {
			uint count = 0;
			for (int i = 1; i <= 13 && count < 1000; count++) {				
				Animator anim = GameObject.Find("BridgeCastle" + i).GetComponent<Animator> ();
				anim.SetBool ("BridgeFall", true);
				GameObject.Find ("BridgeCastle" + i).GetComponent<BoxCollider2D> ().size = new Vector2 (0, 0);
				if (anim.GetCurrentAnimatorStateInfo (0).normalizedTime > 0.1 && anim.GetCurrentAnimatorStateInfo (0).IsName("BridgeFall") && !anim.IsInTransition(0)) {
					i++;
				}
			}
		}
	}

	void OnCollisionEnter2D(Collision2D coll) {
		if (coll.gameObject.tag == "axe") {
			onAxe = true;
			Destroy (coll.gameObject);
		}
	}

	void OnCollisionStay2D(Collision2D coll) {
		if (coll.gameObject.tag == "block1" || coll.gameObject.tag == "pipe" || coll.gameObject.tag == "block2" || coll.gameObject.tag == "blocksurp" || coll.gameObject.tag == "floor") {
			onFloor = true;
		} else {
			onFloor = false;
		}
	}

	void OnCollisionExit2D(Collision2D coll) {
		if (coll.gameObject.tag == "block1" || coll.gameObject.tag == "pipe" || coll.gameObject.tag == "block2" || coll.gameObject.tag == "blocksurp" || coll.gameObject.tag == "floor") {
			onFloor = false;
		}
	}
}
