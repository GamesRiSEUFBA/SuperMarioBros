using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : MonoBehaviour {

	private Rigidbody2D rb;
	private int count = 0;
	private Vector2 initPos;
	private Rigidbody2D pRigid;

	public float vel;
	public int countMax;

	void Start () {
		rb = GetComponent<Rigidbody2D> ();	
		initPos = rb.transform.position;
		rb.velocity = new Vector2 (0, 2);

		pRigid = GameObject.Find("Player").GetComponent<Rigidbody2D>();
	}

	void Update () {
		if (rb.transform.position.y >= initPos.y + 1.9 && rb.velocity.y > 0) {
			rb.velocity = new Vector2 (0, 0);
			count = -countMax;
		} else if (rb.transform.position.y <= initPos.y && rb.velocity.y < 0) {
			rb.velocity = new Vector2 (0, 0);
			count = -countMax;
		}

		if (count == 0) {
			if (rb.transform.position.y >= initPos.y + 1.9)
				rb.velocity = new Vector2 (0, -vel);
			else if (rb.transform.position.y <= initPos.y) {
				if (pRigid.position.x >= rb.position.x - 3 && pRigid.position.x <= rb.position.x + 3)
					rb.velocity = new Vector2 (0, 0);
				else
					rb.velocity = new Vector2 (0, vel);

			}
		} else if (count == countMax)
			count = 0;
		else {
			count++;
		}
	}

	void OnCollisionEnter2D(Collision2D coll) {
		if (coll.gameObject.tag != "player" || coll.gameObject.tag.Contains("pipe")) {
			Physics2D.IgnoreCollision (coll.collider, coll.otherCollider);
		}
	}

	void OnCollisionStay2D(Collision2D coll) {
		if (coll.gameObject.tag != "player" || coll.gameObject.tag.Contains("pipe")) {
			Physics2D.IgnoreCollision (coll.collider, coll.otherCollider);
		}
	}
}
