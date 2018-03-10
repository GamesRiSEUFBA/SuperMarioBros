using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToadBag : MonoBehaviour {

	private Animator anim;
	private Transform trans;
	private bool isLock = false;

	public GameObject toadPrefab;
	public Vector3 pos;

	void Start () {
		anim = GetComponent<Animator> ();
		trans = GetComponent<Transform> ();

		pos = trans.position;
	}

	void Update () {
		if (anim.GetCurrentAnimatorStateInfo (0).normalizedTime >= 0.9 && !isLock) {
			Instantiate (toadPrefab, pos, transform.localRotation);
			isLock = true;
		}
	}
}
