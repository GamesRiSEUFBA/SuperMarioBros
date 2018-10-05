using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurpriseBlock : Block {
    
	private bool invisible = false;


	private int count = 0;
	private bool locked = false;

    

	void Start () {
        GetReferences();

		if (invisible)
			anim.SetBool ("Invisible", true);
		
	}

	void Open() {
        if (obj == null)
            return;
		Instantiate (obj, initPos, transform.localRotation);
		count++;
		if (count == qnt) {
			Lock();
		}
	}

	void Lock() {
		anim.SetBool ("Locked", true);
		locked = true;
	}

	void Update () {
		if (transform.position.y >= initPos.y + 0.5) {
			rb.velocity = new Vector2 (0, -6);
		}
		if (!locked && rb.velocity.y < 0 && transform.position.y <= initPos.y) {
			rb.velocity = new Vector2 (0, 0);
			Open ();
		}
	}

	void OnCollisionEnter2D(Collision2D coll) {
		Vector2 collPos = coll.gameObject.transform.position;
		Vector2 transPos = transform.position;

		if (!locked && coll.gameObject.tag == "player") {
			if (transPos.y - 0.5 >= collPos.y) {
				if (invisible) {
					anim.SetBool ("Invisible", false);
				}
				rb.velocity = new Vector2 (0, 6);
				scr_GameController.play_sound(scr_GameController.Sound.BUMP);
			}
		}
	}
}
