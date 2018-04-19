using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_GameController : MonoBehaviour {

	GameObject referenceObject;

	public enum MarioState
	{
		SMALL,
		BIG,
		FIRE,
		STAR,
		DEAD
	};

	public MarioState mario_state = MarioState.SMALL;

	public int lives = 3;
	public int score = 0;
	public int current_course = 1;
	public int current_world = 1;

	public int damage_time_counter = -1;
	public int damage_time = 300;
	private MarioLuigi mario_object;
	//public controller_object = FindObjectOfType<obj_GameController>();

	public void mario_switch_state(MarioState state_switching_to)
	{
		mario_state = state_switching_to;
	}

	public void damage_mario()
	{
		if (damage_time_counter == -1)
		{
			Debug.Log ("Mario was damaged!");
			switch (mario_state)
			{
				case MarioState.SMALL:
				{
					Debug.Log ("Mario must die!");
	        mario_object.KillMario();
					lives --;

					if (lives <= 0)
					{
						Debug.Log ("Game Over!");
						game_over();
					}
					else
					{
						restart_course();
						Debug.Log ("Course must restart!");
					}
					//var mario_object = FindObjectOfType<player>();
					//mario_object.KillMario();
				}
				break;

				case MarioState.BIG:
				{
					mario_switch_state(MarioState.SMALL);
					mario_object.anim.SetInteger ("MarioSize", 0);
					Debug.Log ("Mario became smaller, and invulnerable for some time!");
					damage_time_counter = damage_time;
				}
				break;

				case MarioState.FIRE:
				{
					mario_switch_state(MarioState.BIG);
					damage_time_counter = damage_time;
				}
				break;

				case MarioState.STAR:
				case MarioState.DEAD:
				{
					//do nothing
					break;
				}
			}
		}
		else
		{
			Debug.Log ("Mario can't be damaged right now!");
		}
	}
	// Use this for initialization
	void Start () {
		DontDestroyOnLoad(gameObject);
		Debug.Log ("I exist!");
		mario_object = GameObject.Find("Player").GetComponent<MarioLuigi>();
	}

	// Update is called once per frame
	void Update () {
		if (damage_time_counter > 0)
		{
			damage_time_counter--;
			if (damage_time_counter <= 0)
			{
				damage_time_counter = -1;
			}
		}
	}

	public void game_over ()
	{
		//do game over thing
	}

	public void restart_course ()
	{
		//restart current course
	}
}
