using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mushroom : MonoBehaviour {

	private Rigidbody2D rb;

	private float initPos;
	private float velX = 0, velY = 1;
	private bool arising = false;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody2D> ();

		initPos = rb.position.y;
		rb.velocity = new Vector2 (velX, velY);
	}

	// Update is called once per frame
	void Update () {
		if (rb.position.y >= initPos + 0.95 && !arising) {
			velX = 2;
			velY = 0;
			rb.velocity = new Vector2 (2, 0);
			rb.bodyType = RigidbodyType2D.Dynamic;
			arising = true;
		}

		rb.velocity = new Vector2 (velX, rb.velocity.y);
	}

	void OnCollisionStay2D(Collision2D coll) {
		if (coll.gameObject.tag == "block1" || coll.gameObject.tag == "pipe" || coll.gameObject.tag == "block2" || coll.gameObject.tag == "blocksurp") {
			if (coll.gameObject.transform.position.x > rb.position.x)
				velX = -Mathf.Abs(velX);
			else
				velX = Mathf.Abs(velX);
		} else if (coll.gameObject.tag != "floor") {
			Physics2D.IgnoreCollision (coll.collider, coll.otherCollider);
		}
	}

	void OnBecameInvisible() {
		//if (rb.position.x < playerRb.position.x || rb.position.y < playerRb.position.y) {
			Destroy (this.gameObject);
		//}
	}
}
