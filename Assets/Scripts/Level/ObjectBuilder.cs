using UnityEngine;
using UnityEngine.Windows;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class EnemyClass 
{
    public GameObject RunGun;
	public GameObject Toxic;
    public GameObject AimTurret;
    public GameObject Sniper;
    public GameObject Tazman;
}

[System.Serializable]
public class TileClass 
{
	public GameObject Player;
	public GameObject Floor;
	public GameObject Wall;
	public GameObject OuterWall;
	public GameObject Exit;
	public GameObject Key;
	public GameObject Lock;
    public GameObject PowerUp;
	public EnemyClass Enemies;
}


public class ObjectBuilder : MonoBehaviour{
	public string PlayerGameObjectName = "Player";
	public string UICanvasGameObjectName = "UI canvas";

	[SerializeField] private GameObject UICanvas;
	[SerializeField] private TileClass prefabs;

	private Transform levelContainer;
	private Transform objectsContainer;
	private Transform enemiesContainer;

	/* Dictionnaire des relations string -> préfab.
	 * Exemple d'une entrée du dictionnaire : "W" -> (levelContainer, prefabs.Floor)
	 * - le "W" est la lettre présente dans le fichier CSV
	 * - le "levelContainer est le container du futur objet généré (pour regrouper les éléments semblables dans l'éditeur Unity)
	 * - le "prefabs.Floor" est un lien vers le préfab de l'objet */
	private Dictionary<string, KeyValuePair<Transform, GameObject>> objectCodes;
	private string playerCode;

	//appelée au lancement
	public void Initialize() {
		levelContainer = new GameObject("Level").transform;
		objectsContainer = new GameObject("Power-ups").transform;
		enemiesContainer = new GameObject("Enemies").transform;

		objectCodes = new Dictionary<string, KeyValuePair<Transform, GameObject>>() 
		{
			// void
			{ "", new KeyValuePair<Transform, GameObject>(null, null)},
			{ "P", new KeyValuePair<Transform, GameObject>(null, null)},

			// level
			{ "G", new KeyValuePair<Transform, GameObject>(levelContainer, prefabs.Floor)},
			{ "W", new KeyValuePair<Transform, GameObject>(levelContainer, prefabs.Wall)},
			{ "B", new KeyValuePair<Transform, GameObject>(levelContainer, prefabs.OuterWall)},
			{ "F", new KeyValuePair<Transform, GameObject>(levelContainer, prefabs.Exit)},

			// objects
			{ "L", new KeyValuePair<Transform, GameObject>(objectsContainer, prefabs.Lock)},
			{ "K", new KeyValuePair<Transform, GameObject>(objectsContainer, prefabs.Key)},
            { "U", new KeyValuePair<Transform, GameObject>(objectsContainer, prefabs.PowerUp) },

			// ennemies
			{ "E1", new KeyValuePair<Transform, GameObject>(enemiesContainer, prefabs.Enemies.RunGun)},
			{ "E2", new KeyValuePair<Transform, GameObject>(enemiesContainer, prefabs.Enemies.Toxic)},
            { "E3", new KeyValuePair<Transform, GameObject>(enemiesContainer, prefabs.Enemies.AimTurret)},
            { "E4", new KeyValuePair<Transform, GameObject>(enemiesContainer, prefabs.Enemies.Sniper)},
            { "E5", new KeyValuePair<Transform, GameObject>(enemiesContainer, prefabs.Enemies.Tazman)},
        };
		playerCode = "P";
	}


	public void InstantiateUICanavs() {
		GameObject canvas = (GameObject)Instantiate (UICanvas, new Vector3 (0f, 0f, 0f), Quaternion.identity);
		canvas.name = UICanvasGameObjectName;
	}

	public void InstantiatePlayer() {
		GameObject player = (GameObject)Instantiate (prefabs.Player, new Vector3 (0f, 0f, 0f), Quaternion.identity);
		player.name = PlayerGameObjectName;
	}

	public void BuildLevel(string fileName) {
		string[][] level = CSVReader.SplitCsvGrid (fileName);
		for (int i = 0; i < level.Length; ++i) {
			string[] row = level [i];
			for (int j = 0; j < row.Length; ++j) {
				try {
					GameObject instance = null;
					KeyValuePair<Transform, GameObject> toInstantiate = objectCodes [level [i] [j]];;

					//si l'objet à instancier n'est pas un élément de décors, on ajoute une tuile de sol par dessous
					if (toInstantiate.Key != levelContainer && toInstantiate.Key != null) {
						instance = (GameObject)Instantiate (prefabs.Floor, new Vector3 (j, -i, 0f), Quaternion.identity);
						instance.transform.parent = levelContainer;
					}

					//ajout de l'objet sur la scène
					if (toInstantiate.Value != null) { //si on doit instancier quelque chose
						instance = (GameObject)Instantiate (toInstantiate.Value, new Vector3 (j, -i, 0f), Quaternion.identity);
						instance.transform.parent = objectCodes [level [i] [j]].Key;
					}

					//cas spécial du joueur : on le place aux coordonnées correspondantes (le gameobject du joueur existe déjà)
					if (level [i] [j] == playerCode) {
						//tuile de sol à la position courante
						instance = (GameObject)Instantiate (prefabs.Floor, new Vector3 (j, -i, 0f), Quaternion.identity);
						instance.transform.parent = levelContainer;
						//déplacement du joueur
						GameObject.Find (PlayerGameObjectName).transform.position = new Vector3(j, -i, 0f);
					}

				} catch (KeyNotFoundException) {
					Debug.Log ("L'objet suivant n'existe pas dans le code : " + level [i] [j]);
				}
			}
		}
	}

	public void DestroyCurrentLevel() {
		foreach (Transform child in levelContainer) {
			Destroy (child.gameObject);
		}
		foreach (Transform child in objectsContainer) {
			Destroy (child.gameObject);
		}
		foreach (Transform child in enemiesContainer) {
			Destroy (child.gameObject);
		}
	}
}
