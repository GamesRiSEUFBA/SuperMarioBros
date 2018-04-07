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

	private bool fall = false;

	public float velX = 3;
	private float impulseY = 8;
	private float impulseX = 4;

	private int time_counter = 0;
	private int time_to_revive = 360;

	public KoopaState current_state = KoopaState.NORMAL;
	//private int current_state = 0;

	void Start () 
	{
		rigidKoopaGreen = GetComponent<Rigidbody2D>();
		transKoopaGreen = GetComponent<Transform>();
		animKoopaGreen = GetComponent<Animator>();
		boxKoopaGreen = GetComponent<BoxCollider2D> ();

		velX = -Mathf.Abs(velX);
		KoopaState current_state = KoopaState.NORMAL;
	}

	void Update () 
	{
		rigidKoopaGreen.velocity = new Vector2 (velX, rigidKoopaGreen.velocity.y);
		if (current_state == KoopaState.SHELL_STANDBY)
		{
			time_counter++;
			if (time_counter > time_to_revive)
			{
				switch_state(KoopaState.NORMAL);
			}
		}
	}

	void Fall() {
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

			break;

			case KoopaState.SHELL_SLIDING:
				time_counter = 0;
			break;
		}

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
		if (coll.gameObject.tag == "block1" || coll.gameObject.tag == "pipe" || coll.gameObject.tag == "block2" || coll.gameObject.tag == "blocksurp" || coll.gameObject.tag == "goomba") {
			if (rbPos.y < collPos.y + 0.5 && rbPos.y > collPos.y - 0.5) {
				if (coll.gameObject.transform.position.x > transKoopaGreen.position.x)
					velX = -Mathf.Abs (velX);
				else
					velX = Mathf.Abs (velX);
			}
		} else if (coll.gameObject.tag != "floor" && coll.gameObject.tag != "player") {
			Physics2D.IgnoreCollision (coll.collider, coll.otherCollider);
		}
	}
}
