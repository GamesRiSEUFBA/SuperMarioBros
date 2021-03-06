using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]

public class MarioLuigi : MonoBehaviour {

	private Rigidbody2D rb;
	private BoxCollider2D box;
	private Animator anim;
	private SpriteRenderer sprite;
	private bool rightKeyPressed = false, leftKeyPressed = false, jumpPressed = false, firePressed = false, turnTime = false, onFloor = false, onAxe = false, onPipe = false, onPlatform = false;
	private bool dead = false;
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
	private int combo_counter = 0;
	private int star_combo_counter = 0;
	private Vector2 outPosition2;
	private bool leavingPipe = false;
	private GameObject FireMario;

	private GameObject toad;

	private int colliders = 0;
	private MarioLuigi mario_object;
	private Vector2 oldCamPos;

    private scr_GameController gC { get { return scr_GameController.instance; } }

    private BoxCollider2D m_ObjectCollider;

	public void Start () {
		rb = GetComponent<Rigidbody2D> ();
		box = GetComponent<BoxCollider2D> ();
		anim = GetComponent<Animator> ();
		sprite = GetComponent<SpriteRenderer> ();
        

        //gC = GameObject.Find("global_controller").GetComponent<scr_GameController>();
        gC.setMarioObject(GameObject.Find("Player").GetComponent<MarioLuigi>());
		gC.setFireCount(0);
		//gC.Start();
		gC.mario_update_according_to_state();

		if (gC.getPipeCourse() == true)
		{
			transform.position = new Vector2(98.88f, -4.1f);//-2.88f);
			leavingPipe = true;
			anim.SetBool ("Walking", false);
			gC.setPipeCourse(false);
			outPosition2 = new Vector2(98.88f, -2.88f);//-2.88f)
			scr_GameController.play_sound(scr_GameController.Sound.POWERDOWN);
		}
	}

	/*void Awake ()
	{
		gC.mario_object = GameObject.Find("Player").GetComponent<MarioLuigi>();
	}*/

	public int getSizeMario() {
		return anim.GetInteger ("MarioSize");
	}

	void checkInputs() {
		if (Input.GetKeyUp (KeyCode.C)) {
			jumpPressed = false;
			if (rb.velocity.y > 0)
			{
				rb.AddForce (new Vector2 (0, -6), ForceMode2D.Impulse);
			}

		}

		if (Input.GetKeyUp (KeyCode.LeftArrow)) {
			leftKeyPressed = false;
			//anim.SetBool ("Turn", false);
		} else if (Input.GetKeyUp (KeyCode.RightArrow)) {
			rightKeyPressed = false;
			//anim.SetBool ("Turn", false);
		}

		if (Input.GetKeyDown (KeyCode.C)) {
			if ((rb.velocity.y == 0 || onPlatform) && !jumpPressed) {
				jumpPressed = true;
				scr_GameController.play_sound(scr_GameController.Sound.JUMP);
				rb.AddForce (new Vector2 (0, 16), ForceMode2D.Impulse);
			}
		}

		if (Input.GetKey (KeyCode.X)) {
			if (rb.velocity.y == 0 && !jumpPressed) {
				spd_max = 0.135f;
			}
		}
		else
		{
			spd_max = 0.1f;
		}

		if (Input.GetKeyDown (KeyCode.X))
		{
			//Fireball code goes here
			if (gC.getFireCount() < 2 && gC.getMarioState() == scr_GameController.MarioState.FIRE)
			{
				gC.setFireCount(gC.getFireCount() + 1);
				scr_GameController.play_sound(scr_GameController.Sound.FIREBALL);
				Instantiate(FireMario, transform.position, transform.rotation);
				Debug.Log ("Mario threw a fireball!");
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
		if (!dead && !onPipe && !leavingPipe) {
			updateMaxSpeed ();
			checkAxe ();
			checkInputs ();
				
			if (jumpPressed) {
				anim.SetBool ("Jump", true);
				if (onFloor)
					jumpPressed = false;
			} else {
				if ((rb.velocity.y == 0 && onFloor) || onPlatform) {
					anim.SetBool ("Jump", false);
				}
			}

			if (leftKeyPressed) {
				if (col_left == true) {
					Debug.Log ("Can't move, there's a colllision on the left side!");
				} else {
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
					if (Mathf.Abs (spd) < spd_max)
						spd -= accel;
					else if (spd > 0)
						spd -= accel * 0.5f;
					var tempPosition = transform.position;
					tempPosition.x = tempPosition.x + spd;

					transform.position = tempPosition;

					if (onFloor == true) {
						if (spd > 0 && turnTime == false) {
							turnTime = true;
							anim.SetBool ("Turn", true);
							//rb.AddForce (new Vector2 (1.2f, 0), ForceMode2D.Impulse);
						} else if (spd < 0) {
							turnTime = false;
							anim.SetBool ("Turn", false);
							anim.SetBool ("Walking", true);
						}
					}
				}
			} else if (rightKeyPressed) {
				//anim.SetBool ("Walking", true);
				if (col_right == true) {
					Debug.Log ("Can't move, there's a colllision on the right side!");
				} else {
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

					if (onFloor == true) {
						if (spd < 0 && turnTime == false) {
							turnTime = true;
							anim.SetBool ("Turn", true);
							//rb.AddForce (new Vector2 (1.2f, 0), ForceMode2D.Impulse);
						} else if (spd > 0) {
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
				if (spd != 0) {
					if (dir_right == true) {
						col_dir = col_right;
					} else {
						col_dir = col_left;
					}

					if (col_dir == false) {
						var tempPosition = transform.position;
						tempPosition.x = tempPosition.x + spd;

						transform.position = tempPosition;

						if (spd < 0) {
							spd += 0.005f;
							if (spd > 0)
								spd = 0;
						} else if (spd > 0) {
							spd -= 0.005f;
							if (spd < 0)
								spd = 0;
						}
					}
				}
			}
		} else if (leavingPipe) {
			print ("Leave");
			rb.velocity = new Vector2 (0, 1.5f);
			if (rb.position.y >= outPosition2.y) {
				onPipe = false;
				rb.velocity = new Vector2 (0, 0);
				leavingPipe = false;
				print ("Parou");
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
			scr_GameController.play_sound(scr_GameController.Sound.PLAYERDIED);
			m_ObjectCollider = GetComponent<BoxCollider2D>();
            if (m_ObjectCollider) m_ObjectCollider.isTrigger = true;
            else Debug.LogError("Player Collider equals NULL");
		}
	}

	void OnBecameInvisible() {
		if (dead && rb.transform.position.y <= Camera.main.transform.position.y) {
			Destroy (this.gameObject);
			print ("Restart Game");
		}
		if (rb.transform.position.y <= -8.3) {
			if (dead == false)
				scr_GameController.play_sound(scr_GameController.Sound.PLAYERDIED);
			dead = true;
			print ("Restart Game");

			if (gC.getLives() <= 0 && rb.transform.position.y <= -8.3f)
			{
				Debug.Log ("Game Over!");
				gC.game_over();
			}
			else if (gC.getLives() > 0 && rb.transform.position.y <= -8.3f)
			{
				//gC.restart_course();
				gC.setDeathCounter(150);
				Debug.Log ("Course must restart!");
			}
		}
	}

	void OnCollisionEnter2D(Collision2D coll) {
		if (dead)
		{
			Debug.Log ("I'm dead, I can't colide!");
			return;
		}

		colliders++;

		Vector2 collPos = coll.gameObject.transform.position;
		Vector2 rbPos = rb.position;
		Debug.Log (rbPos);
		if (coll.gameObject.tag == "axe") {
			onAxe = true;
			Destroy (coll.gameObject);
			Instantiate (toad, new Vector3 (137, -5.78f, 0), rb.transform.rotation);
		}

		if (coll.gameObject.tag == "goomba") {
			Debug.Log ("Colided with Goomba!");
			if (rbPos.y >= collPos.y + 0.5) {
				rb.AddForce (new Vector2 (0, 14), ForceMode2D.Impulse);
				coll.gameObject.GetComponent<Goomba> ().Stomped ();
				scr_GameController.play_sound(scr_GameController.Sound.STOMP);
				add_combo();
			} else {
				//FindObjectOfType<global_controller>().damage_mario();
				//controller.damage_mario ();
				//otherGameObject.GetComponent.DoSomething();
				//var controller_aaa = GameObject.Find("obj_GameController").GetComponent<scr_GameController>().damage_mario();

				gC.damage_mario();
				Debug.Log ("Collided with Goomba, not sure if damaged!");
			}
		}

		if (coll.gameObject.tag == "DangerousFire")
			gC.damage_mario();

		if (coll.gameObject.tag == "KoopaTroopa")
		{
			var other_koopa = coll.gameObject.GetComponent<KoopaTroopaGreen> ();
			if (rbPos.y >= collPos.y + 0.1)
			{
				rb.AddForce (new Vector2 (0, 14), ForceMode2D.Impulse);
				add_combo();
				if (other_koopa.current_state == KoopaTroopaGreen.KoopaState.NORMAL || other_koopa.current_state == KoopaTroopaGreen.KoopaState.SHELL_SLIDING)
				{
					other_koopa.switch_state (KoopaTroopaGreen.KoopaState.SHELL_STANDBY);
					scr_GameController.play_sound(scr_GameController.Sound.STOMP);
				}
				else
				{
					if (dir_right == true)
					{
						other_koopa.setVelX(9);
					}
					else
					{
						other_koopa.setVelX(-9);
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
						other_koopa.setVelX(9);
					}
					else
					{
						other_koopa.setVelX(-9);
					}
					other_koopa.switch_state (KoopaTroopaGreen.KoopaState.SHELL_SLIDING);
				}
			}
		}

		if (coll.gameObject.tag == "lava") {
			KillMario ();
		}

		if ((coll.gameObject.tag == "plant") || (coll.gameObject.tag == "Bowser")) {
			gC.damage_mario();
		}

		if (coll.gameObject.tag == "bowser") {
			gC.damage_mario();
		}

		if ((coll.gameObject.name == "Fireball") || (coll.gameObject.tag == "fireball")){
			Debug.Log ("Collided with fireball!");
			gC.damage_mario();
			Destroy (coll.gameObject);
		}

		if (coll.gameObject.tag == "upgrade") {
			scr_GameController.play_sound(scr_GameController.Sound.POWERUP);
			Destroy (coll.gameObject);

			if (gC.getMarioState() == scr_GameController.MarioState.SMALL)
			{
				anim.SetInteger ("MarioSize", 1);
				box.size = new Vector2 (0.14f, 0.3f);
				box.offset = new Vector2 (0, 0.15f);
				gC.mario_switch_state(scr_GameController.MarioState.BIG);
			}
			//controller_object.mario_switch_state(scr_GameController.MarioState.BIG);
		}

		if (coll.gameObject.tag == "fire") {
			anim.SetInteger ("MarioSize", 2);
			Destroy (coll.gameObject);
			gC.mario_switch_state(scr_GameController.MarioState.FIRE);
			scr_GameController.play_sound(scr_GameController.Sound.POWERUP);
		}

		if (coll.gameObject.tag == "life") {
		Destroy (coll.gameObject);
		gC.add_life();
		}

		//if (coll.gameObject.tag == "floor" || coll.gameObject.tag == "pipe" || coll.gameObject.tag == "block2" || coll.gameObject.tag == "blocksurp" || coll.gameObject.tag == "floor")
		{
			//rb.velocity = new Vector2 (rb.velocity.x, 0);
			if (coll.gameObject.tag != "coin" && coll.gameObject.tag != "upgrade")
			foreach (ContactPoint2D hitPos in coll.contacts)
			{
				//Debug.Log (hitPos.normal);

				if (hitPos.normal.y > 0)
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

	void OnTriggerStay2D(Collider2D coll)
	{
		if (coll.gameObject.tag == "coin")
		{
			Destroy (coll.gameObject);
			Debug.Log ("Colided with coin!");
			gC.add_coin();
		}
	}

	void OnCollisionStay2D(Collision2D coll) {
		if (dead)
			return;

		if (coll.gameObject.tag == "block1" || coll.gameObject.tag == "pipe"|| coll.gameObject.tag == "pipeindown"|| coll.gameObject.tag == "pipeinleft" || coll.gameObject.tag == "block2" || coll.gameObject.tag == "blocksurp" || coll.gameObject.tag == "floor") {
			onFloor = true;
			col_bottom = true;
			combo_counter = 0;
		} else {
			onFloor = false;
			col_bottom = false;
		}

		/*if (coll.gameObject.tag.Contains ("pipe")) {
			coll.gameObject.GetComponent<BoxCollider2D> ().isTrigger = onPipe;
			
			if (coll.gameObject.tag.Contains ("pipein")) {
				if (coll.gameObject.GetComponent<Pipe> ().onPipeDown || coll.gameObject.GetComponent<Pipe> ().onPipeLeft) {
					onPipe = true;
				}
			}
		}*/

		if (coll.gameObject.tag == "platform") {
			onPlatform = true;
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

		if (coll.gameObject.tag == "platform") {
			onPlatform = false;
		}

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

	void add_combo()
	{
		combo_counter++;
		switch(combo_counter)
		{
			case 1:
				gC.addScore(200);
				break;

			case 2:
				gC.addScore(400);
				break;

			case 3:
				gC.addScore(800);
				break;

			case 4:
				gC.addScore(1000);
				break;

			case 5:
				gC.addScore(2000);
				break;

			case 6:
				gC.addScore(4000);
				break;

			case 7:
				gC.addScore(8000);
				break;

			default:
				gC.setLives(gC.getLives() + 1);
				break;
		}
	}

	public bool isDead() {
		return this.dead;
	}

	public void setDead(bool x) {
		this.dead = x;
	}

	public bool isDirRight() {
		return this.dir_right;
	}

	public void setLeavingPipe(bool x) {
		this.leavingPipe = x;
	}

	public void setOutPosition2(Vector2 x) {
		this.outPosition2 = x;
	}

	public Animator getAnim() {
		return this.anim;
	}

	public BoxCollider2D getBox() {
		return this.box;
	}

	public SpriteRenderer getSprite() {
		return this.sprite;
	}
}
