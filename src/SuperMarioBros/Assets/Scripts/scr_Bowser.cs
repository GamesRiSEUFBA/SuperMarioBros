using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_Bowser : MonoBehaviour {

	private bool fall = false;
	private float velX = 3;
	private bool var_right = false;
	private int counter_move = 0;
	private int counter_move_max = 180;
	private int counter_jump = 0;
	private int counter_jump_time = 0;
	private int counter_jump_max = 360;
	private int jump_dir = 0;

	private Rigidbody2D rigidBowser;

	// Use this for initialization
	void Start () {
		rigidBowser = GetComponent<Rigidbody2D>();
		velX = -Mathf.Abs(velX);
	}
	
	// Update is called once per frame
	void Update () {
		counter_move++;
		counter_jump++;

		if (counter_move >= counter_move_max)
		{
			velX = -velX;
			counter_move = 0;
		}

		rigidBowser.velocity = new Vector2 (velX, rigidBowser.velocity.y);
	}
}
