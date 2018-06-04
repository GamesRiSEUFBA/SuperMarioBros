using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mushroom : MonoBehaviour {

	private Rigidbody2D rb;

	private float initPos;
	private float velX = 0, velY = 1;
	private bool arising = false;
	private scr_GameController gC;
	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody2D> ();

		initPos = rb.position.y;
		rb.velocity = new Vector2 (velX, velY);
		gC = GameObject.Find("global_controller").GetComponent<scr_GameController>();
		scr_GameController.play_sound(scr_GameController.Sound.SPROUT);
	}

	// Update is called once per frame
	void Update () {
		if (rb.position.y >= initPos + 1.05 && !arising) {
			velX = 2.5f;
			velY = 0;
			rb.velocity = new Vector2 (2, 0);
			rb.bodyType = RigidbodyType2D.Dynamic;
			arising = true;
		}

		rb.velocity = new Vector2 (velX, rb.velocity.y);
	}

	void OnCollisionStay2D(Collision2D coll) {
		Vector2 collPos = coll.gameObject.transform.position;
		Vector2 rbPos = rb.position;
		if (coll.gameObject.tag == "block1" || coll.gameObject.tag.Contains("pipe") || coll.gameObject.tag == "block2" || coll.gameObject.tag == "blocksurp") {
			if (rbPos.y < collPos.y + 0.5 && rbPos.y > collPos.y - 0.5) {
				if (collPos.x > rb.position.x)
					velX = -Mathf.Abs(velX);
				else
					velX = Mathf.Abs(velX);
			}
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
