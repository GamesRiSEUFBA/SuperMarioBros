using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireMario : MonoBehaviour {
private float velX = 4;
private Rigidbody2D rigid_fire;
private bool col_bottom = false;
	// Use this for initialization
	void Start () {
		rigid_fire = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
		if (col_bottom)
		{
			rigid_fire.AddForce (new Vector2 (0, 5), ForceMode2D.Impulse);
			col_bottom = false;
		}

		rigid_fire.velocity = new Vector2 (velX, rigid_fire.velocity.y);
	}

	void OnCollisionStay2D(Collision2D coll) {
		Vector2 collPos = coll.gameObject.transform.position;
		Vector2 rbPos = rigid_fire.position;
		if (coll.gameObject.tag == "block1" || coll.gameObject.tag == "pipe" || coll.gameObject.tag == "block2" || coll.gameObject.tag == "blocksurp") {
			if (rbPos.y < collPos.y + 0.5 && rbPos.y > collPos.y - 0.5) {
				Destroy (this.gameObject);
			}
		}

		foreach (ContactPoint2D hitPos in coll.contacts)
			{
				if (hitPos.normal.y < 0)
					col_bottom = true;					
			}

		}
}
