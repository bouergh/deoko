using UnityEngine;
using System;
using System.IO;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
	[SerializeField] private GameObject deathScreen;

	private ObjectBuilder builder;
	private string[] levelsList;
	private int currentLevel;
	private bool playerDead;


	void Start () {
		builder = GetComponent<ObjectBuilder> ();
		builder.Initialize ();
		currentLevel = 0;
		playerDead = false;

		//Get the levels list
		DirectoryInfo directoryInfo = new DirectoryInfo (Application.dataPath + "/Levels/");
		FileInfo[] fileInfo = directoryInfo.GetFiles("lvl???.csv");
		levelsList = new string[fileInfo.Length];
		for (int i = 0; i < fileInfo.Length; i++) {
			levelsList [i] = fileInfo [i].FullName;
		}
		Array.Sort (levelsList);

		Debug.Log ("found " + levelsList.Length + " levels !");

		if (levelsList.Length != 0) {
			//Load base stuff and the first level
			builder.InstantiateUICanavs ();
			builder.InstantiatePlayer ();
			LoadNextLevel ();
		} else {
			Debug.LogErrorFormat ("Error : no level to load.");
		}
	}
	
	public void LoadNextLevel() {
		if (currentLevel < levelsList.Length) {
			builder.DestroyCurrentLevel ();
			builder.BuildLevel (levelsList [currentLevel]);
			currentLevel++;
		} else {
			FinishGame ();
		}
	}

	public void ShowDeathScreen() {
		StartCoroutine (DeathScreenRoutine ());
	}

	IEnumerator DeathScreenRoutine() {
		playerDead = true;
		yield return new WaitForSeconds (1);
		deathScreen.SetActive (true);
	}

	public void RestartGame() {
		SceneManager.LoadScene(1); //reload current scene to restart from the beginning
	}

	private void FinishGame() {
		SceneManager.LoadScene(2); //end screen
	}

	public void MainMenu() {
		SceneManager.LoadScene(0); //start screen
	}

	public bool isPlayerDead() {
		return playerDead;
	}
}
