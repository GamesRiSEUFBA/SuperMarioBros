using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickBlock : MonoBehaviour {

	public GameObject obj;
	public GameObject particle;
	public int qnt = 1;
	public bool willBreak = true;

	private Transform trans;
	private Rigidbody2D rb;
	private Animator anim;

	private Vector2 initPos;
	private int count = 0;
	private bool locked = false;
	private int sizeMario;

	void Start () {
		trans = GetComponent<Transform> ();
		rb = GetComponent<Rigidbody2D> ();
		anim = GetComponent<Animator> ();

		initPos = trans.position;
	}

	void Open() {
		if (obj != null) {
			Instantiate (obj, initPos, trans.localRotation);
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
		//Instantiate (particle, initPos, trans.localRotation);
		Destroy (this.gameObject);
	}

	void Update () {
		if (trans.position.y >= initPos.y + 0.5) {
			if (willBreak && sizeMario > 0)
				Break ();
			rb.velocity = new Vector2 (0, -6);
		}
		if (!locked && rb.velocity.y < 0 && trans.position.y <= initPos.y) {
			rb.velocity = new Vector2 (0, 0);
			if (!willBreak)
				Open ();
		}
	}

	void OnCollisionEnter2D(Collision2D coll) {
		Vector2 collPos = coll.gameObject.transform.position;
		Vector2 transPos = trans.position;

		if (!locked && coll.gameObject.tag == "player") {
			sizeMario = coll.gameObject.GetComponent<Animator> ().GetInteger("MarioSize");
			float alt = (sizeMario == 0) ? 1.1f : 1.5f;
			if (transPos.y >= collPos.y + alt) {
				rb.velocity = new Vector2 (0, 6);
			}
		}
	}
}
