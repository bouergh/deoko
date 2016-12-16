using UnityEngine;
using System.Collections;

public class InputController : MonoBehaviour {
	private GameManager gameManager;


	void Awake() {
		gameManager = GameObject.Find ("BoardManager").GetComponent<GameManager> ();
	}

	void Update () {
		if (Input.GetButton ("Restart")) {
			gameManager.RestartGame ();
		}
		if (Input.GetButton ("MainMenu")) {
			gameManager.MainMenu ();
		}
	}
}
