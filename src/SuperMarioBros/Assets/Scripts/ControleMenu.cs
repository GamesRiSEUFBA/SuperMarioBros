using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class ControleMenu : MonoBehaviour {
	

	void OnGUI(){
		if(Input.GetKeyDown(KeyCode.Space)){
			Application.LoadLevel ("Map1-2");
		}
	}
}
