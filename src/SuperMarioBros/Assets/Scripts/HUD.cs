using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour {

	private Text txtScore;
	private Text txtLife;
	private Text txtCoin;
	private scr_GameController gC;

	void Start () {
		gC = GameObject.Find("global_controller").GetComponent<scr_GameController>();

		txtScore = GameObject.Find ("TxtMarioQnt").GetComponent<Text> ();
		txtLife = GameObject.Find ("TxtLifeQnt").GetComponent<Text> ();
		txtCoin = GameObject.Find ("TxtCoinQnt").GetComponent<Text> ();
	}

	void Update () {
		txtScore.text = gC.score + "";
		txtLife.text = gC.lives + "";
		txtCoin.text = gC.coins + "";
	}
}
