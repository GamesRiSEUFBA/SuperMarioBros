using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetCoin : MonoBehaviour {

	private Rigidbody2D rb;
	private Animator anim;
	private Vector2 initPos;
	private scr_GameController gC;
	void Start () {
		rb = GetComponent<Rigidbody2D> ();
		anim = GetComponent<Animator> ();

		initPos = rb.transform.position;
		gC = GameObject.Find("global_controller").GetComponent<scr_GameController>();
		rb.AddForce(new Vector2 (0, 20), ForceMode2D.Impulse);
	}

	void Update () {
		if (anim.GetCurrentAnimatorStateInfo (0).normalizedTime > 0.4 && !anim.IsInTransition (0)) {
			rb.velocity = new Vector2 (0, 0);
		}
		if (anim.GetCurrentAnimatorStateInfo (0).normalizedTime > 1 && !anim.IsInTransition (0)) {
			gC.add_coin();
			Destroy (this.gameObject);
		}
	}
}
