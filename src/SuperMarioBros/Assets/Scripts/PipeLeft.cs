using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeLeft : MonoBehaviour {

	void OnCollisionEnter2D(Collision2D coll) {
		if (coll.gameObject.tag == "player" && coll.rigidbody.velocity.y == 1.5f && coll.rigidbody.velocity.x == 0) {
			Physics2D.IgnoreCollision (coll.collider, coll.otherCollider, true);
		}
	}

	void OnCollisionStay2D(Collision2D coll) {
		if (coll.gameObject.tag == "player" && coll.rigidbody.velocity.y == 1.5f && coll.rigidbody.velocity.x == 0) {
			Physics2D.IgnoreCollision (coll.collider, coll.otherCollider, true);
		}
	}

	void OnCollisionExit2D(Collision2D coll) {
		if (coll.gameObject.tag == "player") {
			Physics2D.IgnoreCollision (coll.collider, coll.otherCollider, false);
		}
	}

}
