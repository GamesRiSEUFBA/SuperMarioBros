using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goomba : MonoBehaviour {

	private Rigidbody2D rigidGoomba;
	private Transform transGoomba;
	private Animator animGoomba;
	private BoxCollider2D boxGoomba;
	private SpriteRenderer spriteGoomba;

	private bool stomped = false;
	private bool fall = false;

	private float velX = 3;
	private float impulseY = 8;
	private float impulseX = 4;
	private float sprite_alpha = 1f;

	void Start () {
		rigidGoomba = GetComponent<Rigidbody2D>();
		transGoomba = GetComponent<Transform>();
		animGoomba = GetComponent<Animator>();
		boxGoomba = GetComponent<BoxCollider2D> ();
		spriteGoomba = GetComponent<SpriteRenderer> ();

		velX = -Mathf.Abs(velX);
	}

	void Update () {
		//if (Input.GetMouseButtonDown(0)) {
		//	Fall ();
		//}

		if (animGoomba.GetCurrentAnimatorStateInfo (0).normalizedTime > 0.5f &&
			animGoomba.GetCurrentAnimatorStateInfo (0).IsName("GoombaDead") 
			&& !animGoomba.IsInTransition (0) && stomped) {
			sprite_alpha -= 0.05f;
			spriteGoomba.color = new Color (1f, 1f, 1f, sprite_alpha);
		}

		if (animGoomba.GetCurrentAnimatorStateInfo (0).normalizedTime > 0.3f &&
			animGoomba.GetCurrentAnimatorStateInfo (0).IsName("GoombaFall") 
			&& !animGoomba.IsInTransition (0) && fall) {
			sprite_alpha -= 0.01f;
			spriteGoomba.color = new Color (1f, 1f, 1f, sprite_alpha);
		}


		if (animGoomba.GetCurrentAnimatorStateInfo (0).normalizedTime > 1.5f &&
			animGoomba.GetCurrentAnimatorStateInfo (0).IsName("GoombaDead") 
			&& !animGoomba.IsInTransition (0) && stomped) {
				
			Destroy (this.gameObject);
		}

		Vector3 screenPoint = Camera.main.WorldToViewportPoint (rigidGoomba.position);
		if (screenPoint.z > 0 && screenPoint.x > -0.5 && screenPoint.x < 1.5 && screenPoint.y > 0 && screenPoint.y < 1) {	
			rigidGoomba.velocity = new Vector2 (velX, rigidGoomba.velocity.y);
		}
	}

	public void Fall() {
		if (velX < 0)
			impulseX = -Mathf.Abs (impulseX);
		else
			impulseX = Mathf.Abs (impulseX);
			
		rigidGoomba.AddForce (new Vector2 (impulseX, impulseY), ForceMode2D.Impulse);
		fall = (fall == false) ? true : false;
		animGoomba.SetBool ("GoombaFall", fall);
		boxGoomba.size = new Vector2 (0, 0);
	}

	public void Stomped() {
		stomped = (stomped == false) ? true : false;
		animGoomba.SetBool ("GoombaDead", stomped);
		rigidGoomba.bodyType = RigidbodyType2D.Static;
		boxGoomba.size = new Vector2 (0, 0);
	}

	void OnBecameInvisible() {
		if (fall) {
			Destroy (this.gameObject);
		}
	}

	void OnCollisionStay2D(Collision2D coll) {
		Vector2 collPos = coll.gameObject.transform.position;
		Vector2 rbPos = rigidGoomba.position;
		//if (coll.gameObject.tag == "block1" || coll.gameObject.tag == "pipe" || coll.gameObject.tag == "block2" || coll.gameObject.tag == "blocksurp" || coll.gameObject.tag == "goomba") {
			/*if (rbPos.y < collPos.y + 0.5 && rbPos.y > collPos.y - 0.5) {
				if (coll.gameObject.transform.position.x > transGoomba.position.x)
					velX = -Mathf.Abs (velX);
				else
					velX = Mathf.Abs (velX);
			}*/
		/*} else if (coll.gameObject.tag != "floor" && coll.gameObject.tag != "player") {
			//Physics2D.IgnoreCollision (coll.collider, coll.otherCollider);
		}*/

		foreach (ContactPoint2D hitPos in coll.contacts)
		{
			Debug.Log (hitPos.normal);

			if (hitPos.normal.x > 0)
				velX = Mathf.Abs (velX);


			if (hitPos.normal.x < 0)
				velX = -Mathf.Abs (velX);				
		}
	}
}
