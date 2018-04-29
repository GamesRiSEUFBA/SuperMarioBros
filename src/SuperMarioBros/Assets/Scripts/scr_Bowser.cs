using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_Bowser : MonoBehaviour {

	private Rigidbody2D rigidBowser;
	private Transform transBowser;
	private Animator animBowser;
	private BoxCollider2D boxBowser;
	private bool fall = false;
	private float velX = 3;
	private bool var_right = false;
	private int counter_move = 0;
	private int counter_move_max = 180;
	private int counter_jump = 0;
	private int counter_jump_time = 0;
	private int counter_jump_max = 360;
	private int jump_dir = 0;

	private int counter_fireball = 0;
	private int counter_fireball_max = 200;

	public GameObject Fireball;

	// Use this for initialization
	void Start () {
		rigidBowser = GetComponent<Rigidbody2D>();
		velX = -Mathf.Abs(velX);
	}
	
	// Update is called once per frame
	void Update () {
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
			/*
			if (counter_jump <= counter_jump_max + 60)
			{
				var tempPosition = transform.position;
				tempPosition.y = tempPosition.y + counter_jump - counter_jump_max;
				transform.position = tempPosition;
			}

			if (counter_jump <= counter_jump_max + 120)
			{
				var tempPosition = transform.position;
				tempPosition.y = tempPosition.y + 60 - counter_jump - counter_jump_max;
				transform.position = tempPosition;
			}
			else
			{
				counter_jump = 0;
			}
			*/
			rigidBowser.AddForce (new Vector2 (0, 5), ForceMode2D.Impulse);
			counter_jump = 0;
			Debug.Log ("Bowser jumped!");
		}

		if (counter_fireball >= counter_fireball_max)
		{
			//create firaball
			Instantiate(Fireball, transform.position, transform.rotation);
			Debug.Log ("Bowser threw a fireball!");
			counter_fireball = 0;
		}

		rigidBowser.velocity = new Vector2 (velX, rigidBowser.velocity.y);
	}
}
