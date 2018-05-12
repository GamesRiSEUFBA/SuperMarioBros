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

	public enum Sound
	{
		JUMP,
		BOWSERFIREBALL,
		STOMP,
		ONEUP,
		COIN,
		FIREBALL,
		POWERUP,
		POWERDOWN,
		PLAYERDIED
	}


	public MarioState mario_state = MarioState.SMALL;

	public Sound snd;

	public int lives = 3;
	public int score = 0;
	public int current_course = 1;
	public int current_world = 1;
	public int coins = 0;

	public int damage_time_counter = -1;
	public int damage_time = 300;
	private MarioLuigi mario_object;
	//public controller_object = FindObjectOfType<obj_GameController>();

	public static AudioClip snd_stomp, snd_coin, snd_jump, snd_bowser_fireball, snd_1up, snd_powerup, snd_powerdown, snd_dead;
	static AudioSource audio_src;

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
					play_sound(Sound.POWERDOWN);
				}
				break;

				case MarioState.FIRE:
				{
					mario_switch_state(MarioState.BIG);
					damage_time_counter = damage_time;
					play_sound(Sound.POWERDOWN);
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

		snd_coin = Resources.Load<AudioClip> ("Coin");
		snd_stomp = Resources.Load<AudioClip> ("Stomp");
		snd_1up = Resources.Load<AudioClip> ("1up");
		snd_bowser_fireball = Resources.Load<AudioClip> ("Bowser_Fireball");
		snd_jump = Resources.Load<AudioClip> ("Jump");
		snd_powerup = Resources.Load<AudioClip> ("PowerUp");
		snd_powerdown = Resources.Load<AudioClip> ("PowerDown");
		snd_dead = Resources.Load<AudioClip> ("PlayerDead");
		
		audio_src = GetComponent<AudioSource> ();

	}

	public static void play_sound (Sound snd_to_play)
	{
		switch(snd_to_play)
		{
			case Sound.JUMP:
				Debug.Log ("Played jump sound!");
				audio_src.PlayOneShot (snd_jump);
			break;

			case Sound.STOMP:
				Debug.Log ("Played stomp sound!");
				audio_src.PlayOneShot (snd_stomp);
			break;

			case Sound.ONEUP:
				Debug.Log ("Played 1-Up sound!");
				audio_src.PlayOneShot (snd_1up);
			break;

			case Sound.BOWSERFIREBALL:
				Debug.Log ("Played Bowser's fireball sound!");
				audio_src.PlayOneShot (snd_bowser_fireball);
			break;

			case Sound.COIN:
				Debug.Log ("Played coin sound!");
				audio_src.PlayOneShot (snd_coin);
			break;

			case Sound.POWERUP:
				Debug.Log ("Played power up sound!");
				audio_src.PlayOneShot (snd_powerup);
			break;

			case Sound.POWERDOWN:
				Debug.Log ("Played power down sound!");
				audio_src.PlayOneShot (snd_powerdown);
			break;

			case Sound.PLAYERDIED:
				Debug.Log ("Played dead sound!");
				audio_src.PlayOneShot (snd_dead);
			break;
		}
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

	public void add_coin ()
	{
		coins++;
		score += 200;
		//play sound here
		if (coins >= 100)
		{
			lives++;
			coins = 0;
			play_sound(Sound.ONEUP);
		}
		else
		{
			play_sound(Sound.COIN);
		}
	}
}
