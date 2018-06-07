using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireMario : MonoBehaviour {
private float velX = 11;
private Rigidbody2D rigid_fire;
private bool col_bottom = false;
private scr_GameController gC;
public MarioLuigi mario_object;
	// Use this for initialization
	void Start () {
		rigid_fire = GetComponent<Rigidbody2D>();
		gC = GameObject.Find("global_controller").GetComponent<scr_GameController>();
		mario_object = GameObject.Find("Player").GetComponent<MarioLuigi>();

		if (mario_object.dir_right == false)
		{
			velX = -velX;
		}

		rigid_fire.AddForce (new Vector2 (0, 2), ForceMode2D.Impulse);
	}
	
	// Update is called once per frame
	void Update () {
		if (col_bottom)
		{
			rigid_fire.AddForce (new Vector2 (0, 5), ForceMode2D.Impulse);
			col_bottom = false;
		}
		else
		{
			rigid_fire.AddForce (new Vector2 (0, -0.15f), ForceMode2D.Impulse);
		}
		rigid_fire.velocity = new Vector2 (velX, rigid_fire.velocity.y);
	}

	void OnCollisionStay2D(Collision2D coll) 
	{
		Vector2 collPos = coll.gameObject.transform.position;
		Vector2 rbPos = rigid_fire.position;


		if (coll.gameObject.tag == "goomba" || coll.gameObject.tag == "KoopaTroopa")
		{
			gC.add_coin();
			gC.score += 200;
			gC.fire_count--;
			Destroy (coll.gameObject);
			Destroy (this.gameObject);
		}
		foreach (ContactPoint2D hitPos in coll.contacts)
		{
			if (hitPos.normal.y > 0)
				col_bottom = true;

			if (hitPos.normal.x != 0)
			{
				gC.fire_count--;
				Destroy (this.gameObject);
			}					
		}

	}
}
