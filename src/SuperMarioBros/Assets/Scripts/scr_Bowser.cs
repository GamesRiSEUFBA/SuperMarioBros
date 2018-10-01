using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_Bowser : MonoBehaviour {

	private Rigidbody2D rigidBowser;
	private Transform transBowser;
	private Animator animBowser;
	private BoxCollider2D boxBowser;
	private bool fall = false;
	private float velX = 2;
	private bool var_right = false;
	private int counter_move = 0;
	private int counter_move_max = 140;
	private int counter_jump = 0;
	private int counter_jump_time = 0;
	private int counter_jump_max = 360;
	private int jump_dir = 0;
	private bool dead = false;
	private int counter_fireball = 0;
	private int counter_fireball_max = 200;
	private scr_GameController gC;
	private GameObject Fireball;

	private int hp = 8;

	// Use this for initialization
	void Start () {
		rigidBowser = GetComponent<Rigidbody2D>();
		gC = GameObject.Find("global_controller").GetComponent<scr_GameController>();
		velX = -Mathf.Abs(velX);
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (dead == false)
		{
			counter_move++;
			counter_jump++;
			counter_fireball++;

			if (counter_move >= counter_move_max)
			{
				velX = -velX;
				counter_move = 0;
			}

			if (counter_jump >= counter_jump_max)
			{
				rigidBowser.AddForce (new Vector2 (0, 5), ForceMode2D.Impulse);
				counter_jump = 0;
				Debug.Log ("Bowser jumped!");
			}

			if (counter_fireball >= counter_fireball_max)
			{
				//create firaball
				Instantiate(Fireball, transform.position, transform.rotation);
				Debug.Log ("Bowser threw a fireball!");
				scr_GameController.play_sound(scr_GameController.Sound.BOWSERFIREBALL);
				counter_fireball = 0;
			}
		}
		else
		{
			rigidBowser.AddForce (new Vector2 (0, -3), ForceMode2D.Impulse);
		}

		rigidBowser.velocity = new Vector2 (velX, rigidBowser.velocity.y);
	}

	void OnCollisionEnter2D(Collision2D coll)
	{
		if (coll.gameObject.tag == "lava") 
		{
			dead = true;
			Destroy (this.gameObject);
		}

		if (coll.gameObject.tag == "MariosFireball")
		{
			//Physics2D.IgnoreCollision (coll.collider, coll.otherCollider);
			hp--;
			Debug.Log ("Bowser HP was reduced!");
			if (hp <= 0)
			{
				dead = true;
				Debug.Log ("Bowser HP hit 0, it must die!");
				Destroy (this.gameObject);
			}
			gC.subFireCount(1);
			Destroy (coll.gameObject);
		}
	}
}