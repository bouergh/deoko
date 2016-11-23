using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    public BoardCreator boardScript;
    private int level = 1;
    public static GameManager instance = null;

    // Use this for initialization
    void Start () {

        //Check if instance already exists
        if (instance == null)

            //if not, set instance to this
            instance = this;

        //If instance already exists and it's not this:
        else if (instance != this)

            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);

        //Sets this to not be destroyed when reloading scene
        DontDestroyOnLoad(gameObject);
        boardScript = GetComponent<BoardCreator>();
        
        InitGame(level);
    }

    public void InitGame(int level)
    {
        if (level == 1) FirstLevel();
        else OtherLevels(level);
    }

    public void FirstLevel()
    {

        boardScript.numRooms = new IntRange(2,2);
        boardScript.roomHeight = new IntRange(10, 10);
        boardScript.roomWidth = new IntRange(15, 15);
        boardScript.SetScene();
    }

    public void OtherLevels(int level)
    {

    }

    // Update is called once per frame
    void Update () {
	
	}
}
