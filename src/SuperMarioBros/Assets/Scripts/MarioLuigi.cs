using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarioLuigi : MonoBehaviour {

	private Rigidbody2D rb;
	private BoxCollider2D box;
	public Animator anim;
	private SpriteRenderer sprite;
	private bool rightKeyPressed = false, leftKeyPressed = false, jumpPressed = false, firePressed = false, turnTime = false, onFloor = false, onAxe = false;
	public bool dead = false;
	private float maxVel = 6;
	private float spd = 0;
	private float spd_dir = 0;
	private bool col_dir = false;
	private float spd_max = 0.1f;
	private float accel = 0.01f;
	private bool dir_right = true;
	private bool col_right = false;
	private bool col_left = false;
	private bool col_up = false;
	private bool col_bottom = false;

	public int colliders = 0;
	public MarioLuigi mario_object;
	private Vector2 oldCamPos;

	private scr_GameController gC;

	void Start () {
		rb = GetComponent<Rigidbody2D> ();
		box = GetComponent<BoxCollider2D> ();
		anim = GetComponent<Animator> ();
		sprite = GetComponent<SpriteRenderer> ();

		gC = GameObject.Find("obj_GameController").GetComponent<scr_GameController>();
	}

	public int getSizeMario() {
		return anim.GetInteger ("MarioSize");
	}

	void checkInputs() {
		if (Input.GetKeyUp (KeyCode.C)) {
			jumpPressed = false;
		}

		if (Input.GetKeyUp (KeyCode.LeftArrow)) {
			leftKeyPressed = false;
			//anim.SetBool ("Turn", false);
		} else if (Input.GetKeyUp (KeyCode.RightArrow)) {
			rightKeyPressed = false;
			//anim.SetBool ("Turn", false);
		}

		if (Input.GetKeyDown (KeyCode.C)) {
			if (rb.velocity.y == 0 && !jumpPressed) {
				jumpPressed = true;
				rb.AddForce (new Vector2 (0, 16), ForceMode2D.Impulse);
			}
		}

		if (Input.GetKeyDown (KeyCode.LeftArrow)) {
			leftKeyPressed = true;
		} else if (Input.GetKeyDown (KeyCode.RightArrow)) {

			rightKeyPressed = true;
		}
		/*else
		{
			rightKeyPressed = false;
			leftKeyPressed = false;
		}*/
	}
	void updateMaxSpeed() {
		maxVel = (onFloor) ? 6 : 5;

		if (Mathf.Abs (rb.velocity.x) > maxVel) {
			rb.velocity = (rb.velocity.x < 0) ? new Vector2 (-maxVel, rb.velocity.y) : rb.velocity = new Vector2 (maxVel, rb.velocity.y);
		}
	}

	void checkAxe() {
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

	void Update () {

		if (!dead) {
			updateMaxSpeed ();
			checkAxe ();
			checkInputs ();

			if (jumpPressed) {
				anim.SetBool ("Jump", true);
				if (onFloor)
					jumpPressed = false;
			} else {
				if (rb.velocity.y == 0 && onFloor) {
					anim.SetBool ("Jump", false);
				}
			}

			if (leftKeyPressed)
			{
				if (col_left == true)
				{
					Debug.Log ("Can't move, there's a colllision on the left side!");
				}
				else
				{
					if (onFloor)
						sprite.flipX = true;
					//if (rb.velocity.x == 0)
						//turnTime = false;
					/*
					if (rb.velocity.x > 0 && !turnTime)
						rb.velocity = new Vector2 (-rb.velocity.x, rb.velocity.y);
					if (Mathf.Abs (rb.velocity.x) < maxVel)
						rb.AddForce (new Vector2 (-16, 0), ForceMode2D.Force);
						*/

					dir_right = false;
					if (Mathf.Abs(spd) < spd_max)
						spd -= accel;
					else if (spd > 0)
						spd -= accel * 0.5f;
					var tempPosition = transform.position;
					tempPosition.x = tempPosition.x + spd;

					transform.position = tempPosition;

					if (onFloor == true)
					{
						if (spd > 0 && turnTime == false)
						{
							turnTime = true;
							anim.SetBool ("Turn", true);
							//rb.AddForce (new Vector2 (1.2f, 0), ForceMode2D.Impulse);
						}
						else if (spd < 0)
						{
							turnTime = false;
							anim.SetBool ("Turn", false);
							anim.SetBool ("Walking", true);
						}
					}
				}
			}
			else if (rightKeyPressed)
			{
				//anim.SetBool ("Walking", true);
				if (col_right == true)
				{
					Debug.Log ("Can't move, there's a colllision on the right side!");
				}
				else
				{
					if (onFloor)
						sprite.flipX = false;
					//if (rb.velocity.x == 0)
						//turnTime = false;
					/*
					if (rb.velocity.x < 0 && !turnTime)
						rb.velocity = new Vector2 (-rb.velocity.x, rb.velocity.y);
					if (Mathf.Abs (rb.velocity.x) < maxVel)
						rb.AddForce (new Vector2 (16, 0), ForceMode2D.Force);
					*/

					dir_right = true;
					if (spd < 0)
						spd += accel * 0.5f;
					else if (spd < spd_max)
						spd += accel;

					var tempPosition = transform.position;
					tempPosition.x = tempPosition.x + spd;

					transform.position = tempPosition;

					if (onFloor == true)
					{
						if (spd < 0 && turnTime == false)
						{
							turnTime = true;
							anim.SetBool ("Turn", true);
							//rb.AddForce (new Vector2 (1.2f, 0), ForceMode2D.Impulse);
						}
						else if (spd > 0)
						{
							turnTime = false;
							anim.SetBool ("Turn", false);
							anim.SetBool ("Walking", true);
						}
					}
				}
			} else {
				anim.SetBool ("Walking", false);
				/*if (rb.velocity.x != 0)
					rb.AddForce (new Vector2 ((rb.velocity.x / Mathf.Abs (rb.velocity.x)) * (-15), 0), ForceMode2D.Force);*/
				if (spd != 0)
				{
					if (dir_right == true)
					{
						col_dir = col_right;
					}
					else
					{
						col_dir = col_left;
					}

					if (col_dir == false)
					{
						var tempPosition = transform.position;
						tempPosition.x = tempPosition.x + spd;

						transform.position = tempPosition;

						if (spd < 0)
						{
							spd += 0.005f;
							if (spd > 0)
								spd = 0;
						} else if (spd > 0)
						{
							spd -= 0.005f;
							if (spd < 0)
								spd = 0;
						}
					}
				}
			}
		}
	}

	public void KillMario() {
		if (getSizeMario () > 0) {
			anim.SetInteger ("MarioSize", getSizeMario () - 1);
			if (getSizeMario () == 0) {
				box.size = new Vector2 (0.12f, 0.15f);
				box.offset = new Vector2 (0, 0.075f);
			}
		}
		else {
			dead = true;
			anim.SetBool ("Dead", true);
			box.size = new Vector2 (0, 0);
			rb.AddForce (new Vector2 (0, 20), ForceMode2D.Impulse);
		}
	}

	void OnBecameInvisible() {
		if (dead && rb.transform.position.y <= Camera.main.transform.position.y) {
			Destroy (this.gameObject);
			print ("Restart Game");
		}
	}

	void OnCollisionEnter2D(Collision2D coll) {
		if (dead)
			return;

		colliders++;
		//Debug.Log ("i am the bone of my peru");
		Vector2 collPos = coll.gameObject.transform.position;
		Vector2 rbPos = rb.position;
		Debug.Log (rbPos);
		if (coll.gameObject.tag == "axe") {
			onAxe = true;
			Destroy (coll.gameObject);
		}

		if (coll.gameObject.tag == "goomba") {
			Debug.Log ("Colided with Goomba!");
			if (rbPos.y >= collPos.y + 0.5) {
				rb.AddForce (new Vector2 (0, 14), ForceMode2D.Impulse);
				coll.gameObject.GetComponent<Goomba> ().Stomped ();
			} else {
				//FindObjectOfType<global_controller>().damage_mario();
				//controller.damage_mario ();
				//otherGameObject.GetComponent.DoSomething();
				//var controller_aaa = GameObject.Find("obj_GameController").GetComponent<scr_GameController>().damage_mario();

				gC.damage_mario();
				Debug.Log ("Collided with Goomba, not sure if damaged!");
			}
		}

		if (coll.gameObject.tag == "KoopaTroopa")
		{
			var other_koopa = coll.gameObject.GetComponent<KoopaTroopaGreen> ();
			if (rbPos.y >= collPos.y + 0.5)
			{
				rb.AddForce (new Vector2 (0, 14), ForceMode2D.Impulse);

				if (other_koopa.current_state == KoopaTroopaGreen.KoopaState.NORMAL || other_koopa.current_state == KoopaTroopaGreen.KoopaState.SHELL_SLIDING)
				{
					other_koopa.switch_state (KoopaTroopaGreen.KoopaState.SHELL_STANDBY);
				}
				else
				{
					if (dir_right == true)
					{
						other_koopa.velX = 9;
					}
					else
					{
						other_koopa.velX = -9;
					}
					other_koopa.switch_state (KoopaTroopaGreen.KoopaState.SHELL_SLIDING);
				}
			}
			else
			{
				if (other_koopa.current_state == KoopaTroopaGreen.KoopaState.NORMAL || other_koopa.current_state == KoopaTroopaGreen.KoopaState.SHELL_SLIDING)
				{
					gC.damage_mario();
				}
				else
				{
					if (dir_right == true)
					{
						other_koopa.velX = 9;
					}
					else
					{
						other_koopa.velX = -9;
					}
					other_koopa.switch_state (KoopaTroopaGreen.KoopaState.SHELL_SLIDING);
				}
			}
		}

		if (coll.gameObject.tag == "lava") {
			KillMario ();
		}

		if (coll.gameObject.tag == "plant") {
			gC.damage_mario();
		}

		if (coll.gameObject.tag == "upgrade") {
			anim.SetInteger ("MarioSize", 1);
			Destroy (coll.gameObject);
			box.size = new Vector2 (0.14f, 0.3f);
			box.offset = new Vector2 (0, 0.15f);
			gC.mario_switch_state(scr_GameController.MarioState.BIG);
			//controller_object.mario_switch_state(scr_GameController.MarioState.BIG);
		}

		if (coll.gameObject.tag == "fire") {
			anim.SetInteger ("MarioSize", 2);
			Destroy (coll.gameObject);
			gC.mario_switch_state(scr_GameController.MarioState.FIRE);
		}

		//if (coll.gameObject.tag == "floor" || coll.gameObject.tag == "pipe" || coll.gameObject.tag == "block2" || coll.gameObject.tag == "blocksurp" || coll.gameObject.tag == "floor")
		{
			//rb.velocity = new Vector2 (rb.velocity.x, 0);

			foreach (ContactPoint2D hitPos in coll.contacts)
			{
				Debug.Log (hitPos.normal);

				if (hitPos.normal.y < 0)
					col_bottom = true;
				else if (hitPos.normal.y > 0)
				{
					//do something
				}
				//else
				{
					if (hitPos.normal.x > 0)
						col_left = true;
					else
						col_left = false;

					if (hitPos.normal.x < 0)
						col_right = true;
					else
						col_right = false;

					if (dir_right == true)
						col_dir = col_right;
					else
						col_dir = col_left;

					if (spd != 0 && col_dir == true)
					{
						spd = 0;
					}
				}
			}
		}

	}

	void OnCollisionStay2D(Collision2D coll) {
		if (dead)
			return;

		if (coll.gameObject.tag == "block1" || coll.gameObject.tag == "pipe"|| coll.gameObject.tag == "pipein" || coll.gameObject.tag == "block2" || coll.gameObject.tag == "blocksurp" || coll.gameObject.tag == "floor") {
			onFloor = true;
			col_bottom = true;
		} else {
			onFloor = false;
			col_bottom = false;
		}
		/*
		foreach (ContactPoint2D hitPos in coll.contacts)
		{
			Debug.Log (hitPos.normal);

			if (hitPos.normal.y < 0)
				col_bottom = true;
			if (hitPos.normal.x < 0)
				col_left = true;
			else
				col_left = false;

			if (hitPos.normal.x > 0)
				col_right = true;
			else
				col_right = false;
		}
		*/
	}

	void OnCollisionExit2D(Collision2D coll) {
		if (dead)
			return;

		colliders--;

		//if (coll.gameObject.tag == "block1" || coll.gameObject.tag == "pipe" || coll.gameObject.tag == "block2" || coll.gameObject.tag == "blocksurp" || coll.gameObject.tag == "floor")
		{
			//onFloor = false;
			//col_bottom = false;

			//foreach (ContactPoint2D hitPos in coll.contacts)
			{
				//Debug.Log (hitPos.normal);
				//if (hitPos.normal.x < 0)
					col_right = false;
				//else if (hitPos.normal.x > 0)
					col_left = false;

				//if (hitPos.normal.y > 0)
				//{
					col_bottom = false;
					onFloor = false;
				//}
			}
		}
	}
}
