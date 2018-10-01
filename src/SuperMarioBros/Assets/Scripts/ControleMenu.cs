using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class ControleMenu : MonoBehaviour {

	private int controller_count = 0;	
	private GameObject GameController;

	void OnGUI(){
		if(Input.GetKeyDown(KeyCode.Space)){
			if (controller_count == 0)
			{
				GameObject controller = Instantiate(GameController, transform.position, transform.rotation);
				controller.name = "global_controller";
				controller_count++;
				Application.LoadLevel ("Map1-1");
			}
		}
	}
}
