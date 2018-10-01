using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
		PLAYERDIED,
		BUMP,
		SPROUT,
		BRICKSHATTER
	}


	private MarioState mario_state = MarioState.SMALL;

	private Sound snd;

	private int lives = 3;
	private int score = 0;
	private static int current_course = 1;
	private int current_world = 1;
	private int coins = 0;
	private int fire_count = 0;
	private int death_counter = -1;

	private static int damage_time_counter = -1;
	private static int damage_time = 300;
	private MarioLuigi mario_object;
	//public controller_object = FindObjectOfType<obj_GameController>();

	private static AudioClip snd_stomp, snd_coin, snd_jump, snd_bowser_fireball, snd_1up, snd_powerup,
	snd_powerdown, snd_dead, snd_bump, snd_sprout, snd_brickshatter, snd_mario_fireball;
	private static AudioSource audio_src;

	//pipes
	private bool pipe_course2 = false;

	public int getLives() {
		return lives;
	}

	public void mario_switch_state(MarioState state_switching_to)
	{
		mario_state = state_switching_to;
	}

	public void mario_update_according_to_state()
	{
		Debug.Log ("Checking Mario's current state...");
		switch (mario_state)
		{
			case MarioState.BIG:
			{
				mario_object.getAnim().SetInteger ("MarioSize", 1);
				mario_object.getBox().size = new Vector2 (0.14f, 0.3f);
				mario_object.getBox().offset = new Vector2 (0, 0.15f);
			}
			break;

			case MarioState.FIRE:
			{
				mario_object.getAnim().SetInteger ("MarioSize", 1);
				mario_object.getBox().size = new Vector2 (0.14f, 0.3f);
				mario_object.getBox().offset = new Vector2 (0, 0.15f);
			}
			break;
		}
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
					//lives--;
	        		mario_object.KillMario();
					//var mario_object = FindObjectOfType<player>();
					//mario_object.KillMario();
				}
				break;

				case MarioState.BIG:
				{
					mario_switch_state(MarioState.SMALL);
					mario_object.getAnim().SetInteger ("MarioSize", 0);
					Debug.Log ("Mario became smaller, and invulnerable for some time!");
					damage_time_counter = damage_time;
					play_sound(Sound.POWERDOWN);
				}
				break;

				case MarioState.FIRE:
				{
					mario_switch_state(MarioState.BIG);
					mario_object.getAnim().SetInteger ("MarioSize", 1);
					Debug.Log ("Fire Mario became Big Mario!");
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
	public void Start () {
		DontDestroyOnLoad(gameObject);
		Debug.Log ("I exist!");
		mario_object = GameObject.Find("Player").GetComponent<MarioLuigi>();

		snd_coin = Resources.Load<AudioClip> ("Coin");
		snd_stomp = Resources.Load<AudioClip> ("Stomp");
		snd_1up = Resources.Load<AudioClip> ("1up");
		snd_bowser_fireball = Resources.Load<AudioClip> ("Bowser_Fireball");
		snd_mario_fireball = Resources.Load<AudioClip> ("Fireball");
		snd_jump = Resources.Load<AudioClip> ("Jump");
		snd_powerup = Resources.Load<AudioClip> ("PowerUp");
		snd_powerdown = Resources.Load<AudioClip> ("PowerDown");
		snd_dead = Resources.Load<AudioClip> ("PlayerDead");
		snd_bump = Resources.Load<AudioClip> ("Bump");
		snd_sprout = Resources.Load<AudioClip> ("Sprout");
		snd_brickshatter = Resources.Load<AudioClip> ("BrickShatter");		
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

			case Sound.FIREBALL:
				Debug.Log ("Played Mario's fireball sound!");
				audio_src.PlayOneShot (snd_mario_fireball);
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

			case Sound.BUMP:
				Debug.Log ("Played bump sound!");
				audio_src.PlayOneShot (snd_bump);
			break;

			case Sound.SPROUT:
				Debug.Log ("Played sprout sound!");
				audio_src.PlayOneShot (snd_sprout);
			break;

			case Sound.BRICKSHATTER:
				Debug.Log ("Played brick shatter sound!");
				audio_src.PlayOneShot (snd_brickshatter);
			break;
		}
	}

	// Update is called once per frame
	void Update () {
		if (damage_time_counter > 0)
		{
			mario_object.getSprite().color = new Color(1f,1f,1f,.65f);
			damage_time_counter--;
			if (damage_time_counter <= 0)
			{
				damage_time_counter = -1;
				mario_object.getSprite().color = new Color(1f,1f,1f,1f);
			}
		}

		if (death_counter > 0)
		{
			death_counter--;
			if (death_counter == 0)
				restart_course();
		}
	}

	public void game_over ()
	{
		SceneManager.LoadScene (0, LoadSceneMode.Single);
		Destroy (this.gameObject);
		/*
		lives = 3;
		current_course = 1;
		damage_time_counter = -1;
		damage_time = 300;
		*/
	}

	public void restart_course ()
	{
		death_counter = -1;
		damage_time_counter = -1;
		mario_object.setDead(false);
		mario_state = MarioState.SMALL;
		lives--;
		SceneManager.LoadScene (SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
		mario_object.Start();
	}

	public void add_coin ()
	{
		coins++;
		score += 200;
		//play sound here
		if (coins >= 100)
		{
			coins = 0;
			add_life();
		}
		else
		{
			play_sound(Sound.COIN);
		}
	}

	public void add_life ()
	{
		if (lives < 99)
		{
			lives++;
		}
		play_sound(Sound.ONEUP);
	}

	public void addScore(int x) {
		this.score += x;
	}

	public void subFireCount(int x) {
		this.fire_count -= x;
	}

	public void setFireCount(int x) {
		this.fire_count = x;
	}

	public int getFireCount() {
		return this.fire_count;
	}

	public int getScore() {
		return this.score;
	}

	public int getCoins() {
		return this.coins;
	}

	public MarioLuigi getMarioObject() {
		return this.mario_object;
	}

	public void setMarioObject(MarioLuigi x) {
		this.mario_object = x;
	}


	public bool getPipeCourse() {
		return this.pipe_course2;
	}

	public void setPipeCourse(bool x) {
		this.pipe_course2 = x;
	}

	public MarioState getMarioState() {
		return this.mario_state;
	}

	public void setDeathCounter(int x) {
		this.death_counter = x;
	}

	public void setLives(int x) {
		this.lives = x;
	}
}
