using UnityEngine;
using System;
using System.IO;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
	private ObjectBuilder builder;
	private FileInfo[] levelsList;
	private int currentLevel;

	void Start () {
		builder = GetComponent<ObjectBuilder> ();
		builder.Initialize ();
		currentLevel = 0;

		//Get the levels list
		DirectoryInfo directoryInfo = new DirectoryInfo (Application.dataPath + "/Levels/");
		levelsList = directoryInfo.GetFiles("lvl???.csv");
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
			builder.BuildLevel (levelsList [currentLevel].FullName);
			currentLevel++;
		} else {
			FinishGame ();
		}
	}

	private void FinishGame() {
		SceneManager.LoadScene(2); //end scene
	}
}
