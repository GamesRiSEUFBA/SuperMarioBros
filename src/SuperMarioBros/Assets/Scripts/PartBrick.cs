using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartBrick : MonoBehaviour {

	private Rigidbody2D topL;
	private Rigidbody2D topR;
	private Rigidbody2D downL;
	private Rigidbody2D downR;

	void Start () {
		topL = GameObject.Find ("PartBrickTopL").GetComponent<Rigidbody2D> ();
		topR = GameObject.Find ("PartBrickTopR").GetComponent<Rigidbody2D> ();
		downL = GameObject.Find ("PartBrickDownL").GetComponent<Rigidbody2D> ();
		downR = GameObject.Find ("PartBrickDownR").GetComponent<Rigidbody2D> ();

		topL.AddForce (new Vector2 (-3, 9), ForceMode2D.Impulse);
		topR.AddForce (new Vector2 (3, 9), ForceMode2D.Impulse);
		downL.AddForce (new Vector2 (-2, 5), ForceMode2D.Impulse);
		downR.AddForce (new Vector2 (2, 5), ForceMode2D.Impulse);
	}

	void OnBecameInvisible() {
		Destroy (GameObject.Find ("PartBrickTopL"));
		Destroy (GameObject.Find ("PartBrickTopR"));
		Destroy (GameObject.Find ("PartBrickDownL"));
		Destroy (GameObject.Find ("PartBrickDownR"));
		Destroy (this.gameObject);
	}
}
