using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurpriseBlock : MonoBehaviour {

	private GameObject obj;
	private int qnt = 1;
	private bool invisible = false;

	private Transform trans;
	private Rigidbody2D rb;
	private Animator anim;

	private Vector2 initPos;
	private int count = 0;
	private bool locked = false;
	private scr_GameController gC;

	void Start () {
		trans = GetComponent<Transform> ();
		rb = GetComponent<Rigidbody2D> ();
		anim = GetComponent<Animator> ();
		gC = GameObject.Find("global_controller").GetComponent<scr_GameController>();
		initPos = trans.position;

		if (invisible) {
			anim.SetBool ("Invisible", true);
		}
	}

	void Open() {
		Instantiate (obj, initPos, trans.localRotation);
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
		if (trans.position.y >= initPos.y + 0.5) {
			rb.velocity = new Vector2 (0, -6);
		}
		if (!locked && rb.velocity.y < 0 && trans.position.y <= initPos.y) {
			rb.velocity = new Vector2 (0, 0);
			Open ();
		}
	}

	void OnCollisionEnter2D(Collision2D coll) {
		Vector2 collPos = coll.gameObject.transform.position;
		Vector2 transPos = trans.position;

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
