using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class scr_end_course_1 : MonoBehaviour {

	private BoxCollider2D box;

	void Start () {
		box = GetComponent<BoxCollider2D> ();
	}

	void OnTriggerStay2D(Collider2D coll)
	{
		if (coll.gameObject.tag == "player")
		{
			if (Input.GetKeyDown (KeyCode.UpArrow))
			{
				SceneManager.LoadScene (2, LoadSceneMode.Single);
			}
		} 
	}
}
