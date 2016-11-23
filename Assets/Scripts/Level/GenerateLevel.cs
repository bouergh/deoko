using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class EnnemyClass 
{
	public GameObject Basic;
	public GameObject Toxic;
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
	public EnnemyClass Ennemies;
}


public class GenerateLevel : MonoBehaviour{
	[SerializeField] private TextAsset levelToLoad;
	[SerializeField] private TileClass prefabs;

	private Transform levelContainer;
	private Transform objectsContainer;
	private Transform ennemiesContainer;

	/* Dictionnaire des relations string -> préfab.
	 * Exemple d'une entrée du dictionnaire : "W" -> (levelContainer, prefabs.Floor)
	 * - le "W" est la lettre présente dans le fichier CSV
	 * - le "levelContainer est le container du futur objet généré (pour regrouper les éléments semblables dans l'éditeur Unity)
	 * - le "prefabs.Floor" est un lien vers le préfab de l'objet */
	private Dictionary<string, KeyValuePair<Transform, GameObject>> relation;

	//appelée au lancement
	void Start() {
		levelContainer = new GameObject("Level").transform;
		objectsContainer = new GameObject("Power-ups").transform;
		ennemiesContainer = new GameObject("Ennemies").transform;

		relation = new Dictionary<string, KeyValuePair<Transform, GameObject>>() 
		{
			// void
			{ "", new KeyValuePair<Transform, GameObject>(null, null)},

			// level
			{ "G", new KeyValuePair<Transform, GameObject>(levelContainer, prefabs.Floor)},
			{ "W", new KeyValuePair<Transform, GameObject>(levelContainer, prefabs.Wall)},
			{ "B", new KeyValuePair<Transform, GameObject>(levelContainer, prefabs.OuterWall)},
			{ "F", new KeyValuePair<Transform, GameObject>(levelContainer, prefabs.Exit)},
			{ "L", new KeyValuePair<Transform, GameObject>(levelContainer, prefabs.Lock)},

			// objects (including player and power-ups)
			{ "P", new KeyValuePair<Transform, GameObject>(objectsContainer, prefabs.Player)},
			{ "K", new KeyValuePair<Transform, GameObject>(objectsContainer, prefabs.Key)},

			// ennemies
			{ "E1", new KeyValuePair<Transform, GameObject>(ennemiesContainer, prefabs.Ennemies.Basic)},
			{ "E2", new KeyValuePair<Transform, GameObject>(ennemiesContainer, prefabs.Ennemies.Toxic)},
		};

		MakeLevel ();
	}

	public void MakeLevel() {
		string[][] level = CSVReader.SplitCsvGrid (levelToLoad.text);
		for (int i = 0; i < level.Length; ++i) {
			string[] row = level [i];
			for (int j = 0; j < row.Length; ++j) {
				try {
					GameObject instance = null;
					KeyValuePair<Transform, GameObject> toInstanciate = relation [level [i] [j]];

					//si l'objet à instancier est un objet ou un ennemi, on ajoute une tuile de sol par dessous
					if (toInstanciate.Key == objectsContainer || toInstanciate.Key == ennemiesContainer) {
						instance = (GameObject)Instantiate (prefabs.Floor, new Vector3 (j, -i, 0f), Quaternion.identity);
						instance.transform.parent = levelContainer;
					}

					//ajout de l'objet sur la scène
					if (toInstanciate.Value != null) { //si on doit instancier quelque chose
						instance = (GameObject)Instantiate (toInstanciate.Value, new Vector3 (j, -i, 0f), Quaternion.identity);
						instance.transform.parent = relation [level [i] [j]].Key;
					}
				}
				catch(KeyNotFoundException) {
					Debug.Log ("L'objet suivant n'existe pas dans le code : " + level [i] [j]);
				}
			}
		}
	}
}
