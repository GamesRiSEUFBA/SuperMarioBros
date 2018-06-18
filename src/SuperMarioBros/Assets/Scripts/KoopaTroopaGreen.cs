using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KoopaTroopaGreen : MonoBehaviour 
{

	public enum KoopaState
	{
		NORMAL,
		SHELL_STANDBY,
		SHELL_SLIDING
	};

	private Rigidbody2D rigidKoopaGreen;
	private Transform transKoopaGreen;
	private Animator animKoopaGreen;
	private BoxCollider2D boxKoopaGreen;
	private SpriteRenderer sprite;

	private bool fall = false;

	public float velX = 3;
	private float impulseY = 8;
	private float impulseX = 4;

	private int time_counter = 0;
	private int time_to_revive = 360;
	private scr_GameController gC;
	public KoopaState current_state = KoopaState.NORMAL;
	private int combo_counter = 0;
	//private int current_state = 0;

	void Start () 
	{
		rigidKoopaGreen = GetComponent<Rigidbody2D>();
		transKoopaGreen = GetComponent<Transform>();
		animKoopaGreen = GetComponent<Animator>();
		boxKoopaGreen = GetComponent<BoxCollider2D> ();
		sprite = GetComponent<SpriteRenderer> ();

		velX = -Mathf.Abs(velX);
		KoopaState current_state = KoopaState.NORMAL;
		gC = GameObject.Find("global_controller").GetComponent<scr_GameController>();
	}

	void Update () 
	{
		Vector3 screenPoint = Camera.main.WorldToViewportPoint (rigidKoopaGreen.position);
		if (screenPoint.z > 0 && screenPoint.x > -0.5 && screenPoint.x < 1.5 && screenPoint.y > 0 && screenPoint.y < 1) {	
			rigidKoopaGreen.velocity = new Vector2 (velX, rigidKoopaGreen.velocity.y);
		}

		if (current_state == KoopaState.SHELL_STANDBY)
		{
			time_counter++;
			if (time_counter > time_to_revive)
			{
				switch_state(KoopaState.NORMAL);
			}
		}

		if (rigidKoopaGreen.velocity.x > 0)
			sprite.flipX = true;
		else
			sprite.flipX = false;
	}

	public void Fall() {
		if (velX < 0)
			impulseX = -Mathf.Abs (impulseX);
		else
			impulseX = Mathf.Abs (impulseX);
			
		rigidKoopaGreen.AddForce (new Vector2 (impulseX, impulseY), ForceMode2D.Impulse);
		fall = (fall == false) ? true : false;
		boxKoopaGreen.size = new Vector2 (0, 0);
	}

	public void switch_state(KoopaState state_switching_to)
	{
		switch(state_switching_to)
		{
			case KoopaState.NORMAL:
				velX = -Mathf.Abs(velX);
				time_counter = 0;
				break;

			case KoopaState.SHELL_STANDBY:
				velX = 0;
				velX = -Mathf.Abs(velX);
				time_counter = 0;
				animKoopaGreen.SetBool ("Stop", true);
				break;

			case KoopaState.SHELL_SLIDING:
				animKoopaGreen.SetBool ("Stop", false);
				time_counter = 0;
				break;
		}
		combo_counter = 0;
		current_state = state_switching_to;
	}

	void OnBecameInvisible() {
		if (fall) {
			Destroy (this.gameObject);
		}
	}

	void OnCollisionStay2D(Collision2D coll) {
		Vector2 collPos = coll.gameObject.transform.position;
		Vector2 rbPos = rigidKoopaGreen.position;

		if (current_state == KoopaState.SHELL_SLIDING)
		{
			if (coll.gameObject.tag == "goomba")
			{
				coll.gameObject.GetComponent<Goomba> ().Fall ();
				add_combo();
				Physics2D.IgnoreCollision (coll.collider, coll.otherCollider);
			}

			else if (coll.gameObject.tag == "KoopaTroopaGreen")
			{
				coll.gameObject.GetComponent<KoopaTroopaGreen> ().Fall ();
				add_combo();
				Physics2D.IgnoreCollision (coll.collider, coll.otherCollider);
			}

			else if (coll.gameObject.tag != "player")
			foreach (ContactPoint2D hitPos in coll.contacts)
			{
				Debug.Log (hitPos.normal);

				if (hitPos.normal.x > 0)
				{
					velX = Mathf.Abs (velX);
					scr_GameController.play_sound(scr_GameController.Sound.BUMP);
				}
				else if (hitPos.normal.x < 0)
				{
					velX = -Mathf.Abs (velX);
					scr_GameController.play_sound(scr_GameController.Sound.BUMP);				
				}
			}
		}
		else
			if (coll.gameObject.tag == "block1" || coll.gameObject.tag.Contains("pipe") || coll.gameObject.tag == "block2" || coll.gameObject.tag == "blocksurp" || coll.gameObject.tag == "goomba") {
				if (rbPos.y < collPos.y + 0.5 && rbPos.y > collPos.y - 0.5) {
					if (coll.gameObject.transform.position.x > transKoopaGreen.position.x) {
						velX = -Mathf.Abs (velX);
					} else {
						velX = Mathf.Abs (velX);
					}
				}
		} else if (coll.gameObject.tag != "floor" && coll.gameObject.tag != "player") {
			Physics2D.IgnoreCollision (coll.collider, coll.otherCollider);
		}
	}

	public void add_combo()
	{
		combo_counter++;
		switch(combo_counter)
		{
			case 1:
				gC.score += 200;
				break;

			case 2:
				gC.score += 400;
				break;

			case 3:
				gC.score += 800;
				break;

			case 4:
				gC.score += 1000;
				break;

			case 5:
				gC.score += 2000;
				break;

			case 6:
				gC.score += 4000;
				break;

			case 7:
				gC.score += 8000;
				break;

			default:
				gC.add_life();
				break;
		}
	}
}
