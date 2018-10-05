using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickBlock : Block {

	private GameObject particle;
	private bool willBreak = true;
    
    
	private int count = 0;
	private bool locked = false;
	private int sizeMario;

	void Start () {
        GetReferences();
	}

	void Open() {
		if (obj != null) {
			Instantiate (obj, initPos, transform.localRotation);
			count++;
			if (count == qnt) {
				Lock ();
			}
		}
	}

	void Lock() {
		anim.SetBool ("Locked", true);
		locked = true;
	}

	void Break() {
		Instantiate (particle, initPos, transform.localRotation);
		scr_GameController.play_sound(scr_GameController.Sound.BRICKSHATTER);
		Destroy (this.gameObject);
	}

	void Update () {
		if (transform.position.y >= initPos.y + 0.5) {
			scr_GameController.play_sound(scr_GameController.Sound.BUMP);	
			if (willBreak && sizeMario > 0)
				Break ();
			rb.velocity = new Vector2 (0, -6);
		}
		if (!locked && rb.velocity.y < 0 && transform.position.y <= initPos.y) {
			rb.velocity = new Vector2 (0, 0);
			if (!willBreak)
				Open ();
		}
	}

	void OnCollisionEnter2D(Collision2D coll) {
		Vector2 collPos = coll.gameObject.transform.position;
		Vector2 transPos = transform.position;

		foreach (ContactPoint2D hitPos in coll.contacts)
			{
				Debug.Log (hitPos.normal);

				if (hitPos.normal.y > 0)
				{
					if (!locked && coll.gameObject.tag == "player")
					{
						sizeMario = coll.gameObject.GetComponent<Animator> ().GetInteger("MarioSize");
						float alt = (sizeMario == 0) ? 1.1f : 1.5f;
						if (transPos.y >= collPos.y + alt) {
							rb.velocity = new Vector2 (0, 6);
					}
				}
			}
		}
	}
}
