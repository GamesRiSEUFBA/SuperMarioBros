using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Pipe : MonoBehaviour {

	private Animator animMario;
	private Rigidbody2D rbMario;
	private Transform transPipe;
	private Vector2 posMario;

	private bool loaded = false;

	public bool onPipeDown = false;
	public bool onPipeLeft = false;

	public int nextScene;
	public int actualScene;
	public Vector2 triggerPos;
	public Vector2 outPosition;
	public Vector2 outPosition2;

	private Collider2D cPipe;
	private Collider2D cMario;

	void Start () {
		animMario = GameObject.Find("Player").GetComponent<Animator>();
		rbMario = GameObject.Find("Player").GetComponent<Rigidbody2D>();
		transPipe = GetComponent<Transform> ();
	}

	void Update () {
		if (onPipeDown) {
			rbMario.velocity = new Vector2 (0, -1.5f);

			if (rbMario.position.y <= posMario.y - 2 && !loaded) {
				SceneManager.LoadScene (nextScene, LoadSceneMode.Additive);
				rbMario.transform.position = new Vector2(10, 120);
				Physics2D.IgnoreCollision (cMario, cPipe, false);

				loaded = true;
				onPipeDown = false;
				onPipeLeft = false;
				animMario.SetBool ("Pipe", false);
			}
		}

		if (onPipeLeft) {
			rbMario.velocity = new Vector2 (1.5f, 0);
			if (rbMario.position.x >= posMario.x + 1 && !loaded) {
				SceneManager.UnloadSceneAsync (actualScene);
				Physics2D.IgnoreCollision (cMario, cPipe, false);

				rbMario.transform.position = outPosition;

				GameObject.Find ("Player").GetComponent<MarioLuigi> ().outPosition2 = outPosition2;
				GameObject.Find ("Player").GetComponent<MarioLuigi> ().leavingPipe = true;
				loaded = true;
				onPipeDown = false;
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

			if (Input.GetKeyDown (KeyCode.DownArrow) && (collPos.x <= transPos.x + 0.2f && collPos.x >= transPos.x - 0.2f)) {
				animMario.SetBool ("Pipe", true);
				rbMario.velocity = new Vector2 (0, -2);
				posMario = collPos;
				loaded = false;
				onPipeDown = true;
				onPipeLeft = false;
				Physics2D.IgnoreCollision (cMario, cPipe, true);
			} 

			if (Input.GetKeyDown (KeyCode.RightArrow) && collPos.y <= transPos.y + 0.2f) {
				animMario.SetBool ("Walking", true);
				rbMario.velocity = new Vector2 (1.5f, 0);
				posMario = collPos;
				loaded = false;
				onPipeDown = false;
				onPipeLeft = true;
				Physics2D.IgnoreCollision (cMario, cPipe, true);
			}
		}
	}
}
