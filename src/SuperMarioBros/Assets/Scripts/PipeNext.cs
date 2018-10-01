using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PipeNext : MonoBehaviour {

	private Animator animMario;
	private Rigidbody2D rbMario;
	private Transform transPipe;
	private Vector2 posMario;

	private bool onPipeLeft = false;

	private int nextScene;
	private int actualScene;
	private Vector2 outPosition;
	private Vector2 outPosition2;

	private Collider2D cPipe;
	private Collider2D cMario;

	void Start () {
		animMario = GameObject.Find("Player").GetComponent<Animator>();
		rbMario = GameObject.Find("Player").GetComponent<Rigidbody2D>();
		transPipe = GetComponent<Transform> ();
	}

	void Update () {
		if (onPipeLeft) {
			rbMario.velocity = new Vector2 (1.5f, 0);
			if (rbMario.position.x >= posMario.x + 1) {
				SceneManager.LoadScene (nextScene, LoadSceneMode.Single);
				Physics2D.IgnoreCollision (cMario, cPipe, false);

				rbMario.transform.position = outPosition;

				GameObject.Find ("Player").GetComponent<MarioLuigi> ().setOutPosition2(outPosition2);
				GameObject.Find ("Player").GetComponent<MarioLuigi> ().setLeavingPipe(true);
				onPipeLeft = false;
				animMario.SetBool ("Walking", false);
			}
		}


	}

	void OnCollisionStay2D(Collision2D coll) {
		Vector2 collPos = coll.gameObject.transform.position;
		Vector2 transPos = transPipe.position;

		if (coll.gameObject.tag == "player") {
			cPipe = coll.collider;
			cMario = coll.otherCollider;

			if (Input.GetKeyDown (KeyCode.RightArrow) && collPos.y <= transPos.y + 0.2f) {
				animMario.SetBool ("Walking", true);
				rbMario.velocity = new Vector2 (1.5f, 0);
				posMario = collPos;
				onPipeLeft = true;
				Physics2D.IgnoreCollision (cMario, cPipe, true);
			}
		}
	}
}
