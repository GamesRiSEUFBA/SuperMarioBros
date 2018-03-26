using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurpriseBlock : MonoBehaviour {

	public GameObject obj;
	private Transform trans;

	void Start () {
		trans = GetComponent<Transform> ();
	}

	void Update () {
		
	}

	void OnCollisionEnter2D(Collision2D coll) {
		Vector2 collPos = coll.gameObject.transform.position;
		Vector2 transPos = trans.position;

		if (coll.gameObject.tag == "player") {
			print ("A");
			if (transPos.y - 0.5 >= collPos.y) {
				print ("B");
				switch (obj.tag) {
					case "upgrade":
						print ("C");	
					Instantiate (obj, trans.position, Quaternion.identity);
						break;
				}
			}
		}
	}
}
