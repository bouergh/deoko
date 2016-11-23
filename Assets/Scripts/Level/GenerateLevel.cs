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
	public GameObject Floor;
	public GameObject Wall;
	public GameObject OuterWall;
	public GameObject Player;
	public GameObject Exit;
	public EnnemyClass Ennemies;
}


public class GenerateLevel : MonoBehaviour{
	[SerializeField] private TextAsset levelToLoad;
	[SerializeField] private TileClass prefabs;

	private Transform levelContainer;
	private Transform powerUpsContainer;
	private Transform EnnemiesContainer;

	private Dictionary<string, GameObject> relation;


	//appelée au lancement
	void Start() {
		relation = new Dictionary<string, GameObject>() 
		{
			{ "", null},
			{ "G", prefabs.Floor},
			{ "W", prefabs.Wall},
			{ "B", prefabs.OuterWall},
			{ "P", prefabs.Player},
			{ "F", prefabs.Exit},
			{ "E1", prefabs.Ennemies.Basic},
			{ "E2", prefabs.Ennemies.Toxic},
		};

		levelContainer = new GameObject("Level").transform;
		powerUpsContainer = new GameObject("Power-ups").transform;
		EnnemiesContainer = new GameObject("Ennemies").transform;

		MakeLevel ();
	}

	public void MakeLevel() {
		string[][] level = CSVReader.SplitCsvGrid (levelToLoad.text);
		for (int i = 0; i < level.Length; ++i) {
			string[] row = level [i];
			for (int j = 0; j < row.Length; ++j) {
				try {
					GameObject toInstanciate = relation [level [i] [j]];
					GameObject instance = null;

					if (toInstanciate != null) {
						instance = (GameObject)Instantiate (toInstanciate, new Vector3 (j, -i, 0f), Quaternion.identity);
						instance.transform.parent = levelContainer;
					}
				}
				catch(KeyNotFoundException) {
					Debug.Log ("clé non trouvée : " + level [i] [j]);
				}
			}
		}
	}
}
