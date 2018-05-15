using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireUpgrade : MonoBehaviour {

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
		if (rb.position.y >= initPos + 0.94 && !arising) {
			velX = 0;
			velY = 0;
			rb.velocity = new Vector2 (0, 0);
			rb.bodyType = RigidbodyType2D.Dynamic;
			arising = true;
		}

		rb.velocity = new Vector2 (velX, rb.velocity.y);
	}

	void OnBecameInvisible() {
		Destroy (this.gameObject);
	}


	void OnCollisionEnter2D(Collision2D coll) {
		if (coll.gameObject.tag != "player") {
			Physics2D.IgnoreCollision (coll.collider, coll.otherCollider);
		}
	}
}
