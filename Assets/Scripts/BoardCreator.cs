using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardCreator : MonoBehaviour
{
    // The type of tile that will be laid in a specific position.
    public enum TileType
    {
        Wall, Floor,
    }


    public int columns = 100;                                 // The number of columns on the board (how wide it will be).
    public int rows = 100;                                    // The number of rows on the board (how tall it will be).
    public IntRange numRooms = new IntRange(15, 20);         // The range of the number of rooms there can be.
    public IntRange roomWidth = new IntRange(3, 10);         // The range of widths rooms can have.
    public IntRange roomHeight = new IntRange(3, 10);        // The range of heights rooms can have.
    public IntRange corridorLength = new IntRange(6, 10);    // The range of lengths corridors between rooms can have.
    public GameObject[] floorTiles;                           // An array of floor tile prefabs.
    public GameObject[] wallTiles;                            // An array of wall tile prefabs.
    public GameObject[] outerWallTiles;                       // An array of outer wall tile prefabs.
    public GameObject[] ennemyTiles;
    public GameObject exit;                                         //Prefab to spawn for exit.
    public GameObject player;

    private TileType[][] tiles;                               // A jagged array of tile types representing the board, like a grid.
    private Room[] rooms;                                     // All the rooms that are created for this board.
    private Corridor[] corridors;                             // All the corridors that connect the rooms.
    private GameObject boardHolder;                           // GameObject that acts as a container for all other tiles.
    private List<Vector3> gridPositions = new List<Vector3>();   //A list of possible locations to place tiles.
    private List<Vector3> forbiddenPosition = new List<Vector3>(); //A list of position to avoid if we want to instantiate objects.



    public void SetScene()
    {
        
        boardHolder = new GameObject("BoardHolder");

        SetupTilesArray();

        CreateRoomsAndCorridors();

        SetTilesValuesForRooms();
        SetTilesValuesForCorridors();

        InstantiateTiles();
        InstantiateOuterWalls();

 
    }




    void SetupTilesArray()
    {
        // Set the tiles jagged array to the correct width.
        tiles = new TileType[columns][];

        // Go through all the tile arrays...
        for (int i = 0; i < tiles.Length; i++)
        {
            // ... and set each tile array is the correct height.
            tiles[i] = new TileType[rows];
        }
    }


    void CreateRoomsAndCorridors()
    {
        // Create the rooms array with a random size.
        rooms = new Room[numRooms.Random];

        // There should be one less corridor than there is rooms.
        corridors = new Corridor[rooms.Length - 1];

        // Create the first room and corridor.
        rooms[0] = new Room();
        corridors[0] = new Corridor();

        // Setup the first room, there is no previous corridor so we do not use one.
        rooms[0].SetupRoom(new IntRange(4, 4), new IntRange(4, 4), columns, rows);

        // Setup the first corridor using the first room.
        corridors[0].SetupCorridor(rooms[0], corridorLength, roomWidth, roomHeight, columns, rows, true);

        Vector3 playerPos = new Vector3(rooms[0].xPos, rooms[0].yPos, 0);
        Instantiate(player, playerPos, Quaternion.identity);

        for (int i = 1; i < rooms.Length; i++)
        {
            // Create a room.
            rooms[i] = new Room();

            // Setup the room based on the previous corridor.
            rooms[i].SetupRoom(roomWidth, roomHeight, columns, rows, corridors[i - 1]);

            // If we haven't reached the end of the corridors array...
            if (i < corridors.Length)
            {
                // ... create a corridor.
                corridors[i] = new Corridor();

                // Setup the corridor based on the room that was just created.
                corridors[i].SetupCorridor(rooms[i], corridorLength, roomWidth, roomHeight, columns, rows, false);
            }


            InitialiseList(rooms[i].roomWidth, rooms[i].roomHeight, rooms[i].xPos, rooms[i].yPos);
            RemoveFromGrid();

            LayoutObjectAtRandom(ennemyTiles, 2, 2);
            LayoutObjectAtRandom(wallTiles, 10, 10);

            //Instantiate the exit tile in the upper right hand corner of the game board
            Instantiate(exit, new Vector3(rooms[i].xPos + rooms[i].roomWidth - 1, rooms[i].yPos + rooms[i].roomHeight - 1, 0f), Quaternion.identity);
            Instantiate(exit, new Vector3(corridors[i-1].EndPositionX, corridors[i-1].EndPositionY, 0f), Quaternion.identity);

        }

    }


    void SetTilesValuesForRooms()
    {
        // Go through all the rooms...
        for (int i = 0; i < rooms.Length; i++)
        {
            Room currentRoom = rooms[i];

            // ... and for each room go through it's width.
            for (int j = 0; j < currentRoom.roomWidth; j++)
            {
                int xCoord = currentRoom.xPos + j;

                // For each horizontal tile, go up vertically through the room's height.
                for (int k = 0; k < currentRoom.roomHeight; k++)
                {
                    int yCoord = currentRoom.yPos + k;

                    // The coordinates in the jagged array are based on the room's position and it's width and height.
                    tiles[xCoord][yCoord] = TileType.Floor;
                }
            }
        }
    }


    void SetTilesValuesForCorridors()
    {
        // Go through every corridor...
        for (int i = 0; i < corridors.Length; i++)
        {
            Corridor currentCorridor = corridors[i];

            // and go through it's length.
            for (int j = 0; j < currentCorridor.corridorLength; j++)
            {
                // Start the coordinates at the start of the corridor.
                int xCoord = currentCorridor.startXPos;
                int yCoord = currentCorridor.startYPos;

                // Depending on the direction, add or subtract from the appropriate
                // coordinate based on how far through the length the loop is.
                switch (currentCorridor.direction)
                {
                    case Direction.North:
                        yCoord += j;
                        break;
                    case Direction.East:
                        xCoord += j;
                        break;
                    case Direction.South:
                        yCoord -= j;
                        break;
                    case Direction.West:
                        xCoord -= j;
                        break;
                }

                // Set the tile at these coordinates to Floor.
                tiles[xCoord][yCoord] = TileType.Floor;
            }
        }
    }


    void InstantiateTiles()
    {
        // Go through all the tiles in the jagged array...
        for (int i = 0; i < tiles.Length; i++)
        {
            for (int j = 0; j < tiles[i].Length; j++)
            {
                // ... and instantiate a floor tile for it.
                InstantiateFromArray(floorTiles, i, j);

                // If the tile type is Wall...
                if (tiles[i][j] == TileType.Wall)
                {
                    // ... instantiate a wall over the top.
                    InstantiateFromArray(wallTiles, i, j);
                }
            }
        }
    }


    void InstantiateOuterWalls()
    {
        // The outer walls are one unit left, right, up and down from the board.
        float leftEdgeX = -1f;
        float rightEdgeX = columns + 0f;
        float bottomEdgeY = -1f;
        float topEdgeY = rows + 0f;

        // Instantiate both vertical walls (one on each side).
        InstantiateVerticalOuterWall(leftEdgeX, bottomEdgeY, topEdgeY);
        InstantiateVerticalOuterWall(rightEdgeX, bottomEdgeY, topEdgeY);

        // Instantiate both horizontal walls, these are one in left and right from the outer walls.
        InstantiateHorizontalOuterWall(leftEdgeX + 1f, rightEdgeX - 1f, bottomEdgeY);
        InstantiateHorizontalOuterWall(leftEdgeX + 1f, rightEdgeX - 1f, topEdgeY);
    }


    void InstantiateVerticalOuterWall(float xCoord, float startingY, float endingY)
    {
        // Start the loop at the starting value for Y.
        float currentY = startingY;

        // While the value for Y is less than the end value...
        while (currentY <= endingY)
        {
            // ... instantiate an outer wall tile at the x coordinate and the current y coordinate.
            InstantiateFromArray(outerWallTiles, xCoord, currentY);

            currentY++;
        }
    }


    void InstantiateHorizontalOuterWall(float startingX, float endingX, float yCoord)
    {
        // Start the loop at the starting value for X.
        float currentX = startingX;

        // While the value for X is less than the end value...
        while (currentX <= endingX)
        {
            // ... instantiate an outer wall tile at the y coordinate and the current x coordinate.
            InstantiateFromArray(outerWallTiles, currentX, yCoord);

            currentX++;
        }
    }


    void InstantiateFromArray(GameObject[] prefabs, float xCoord, float yCoord)
    {
        // Create a random index for the array.
        int randomIndex = Random.Range(0, prefabs.Length);

        // The position to be instantiated at is based on the coordinates.
        Vector3 position = new Vector3(xCoord, yCoord, 0f);

        // Create an instance of the prefab from the random index of the array.
        GameObject tileInstance = Instantiate(prefabs[randomIndex], position, Quaternion.identity) as GameObject;

        // Set the tile's parent to the board holder.
        tileInstance.transform.parent = boardHolder.transform;
    }


    //RandomPosition returns a random position from our list gridPositions.
    Vector3 RandomPosition()
    {
        //Declare an integer randomIndex, set it's value to a random number between 0 and the count of items in our List gridPositions.
        int randomIndex = Random.Range(0, gridPositions.Count);

        //Declare a variable of type Vector3 called randomPosition, set it's value to the entry at randomIndex from our List gridPositions.
        Vector3 randomPosition = gridPositions[randomIndex];

        //Remove the entry at randomIndex from the list so that it can't be re-used.
        gridPositions.RemoveAt(randomIndex);

        //Return the randomly selected Vector3 position.
        return randomPosition;
    }


    //LayoutObjectAtRandom accepts an array of game objects to choose from along with a minimum and maximum range for the number of objects to create.
    void LayoutObjectAtRandom(GameObject[] tileArray, int minimum, int maximum)
    {
        //Choose a random number of objects to instantiate within the minimum and maximum limits
        int objectCount = Random.Range(minimum, maximum + 1);

        //Instantiate objects until the randomly chosen limit objectCount is reached
        for (int i = 0; i < objectCount; i++)
        {
            //Choose a position for randomPosition by getting a random position from our list of available Vector3s stored in gridPosition
            Vector3 randomPosition = RandomPosition();

            //Choose a random tile from tileArray and assign it to tileChoice
            GameObject tileChoice = tileArray[Random.Range(0, tileArray.Length)];

            //Instantiate tileChoice at the position returned by RandomPosition with no change in rotation
            Instantiate(tileChoice, randomPosition, Quaternion.identity);
        }
    }


    //Clears our list gridPositions and prepares it to generate a new board.
    void InitialiseList(int columns, int rows, int xLowerLeft, int yLowerLeft)
    {
        //Clear our list gridPositions.
        gridPositions.Clear();

        //Loop through x axis (columns).
        for (int x = xLowerLeft; x < xLowerLeft + columns - 1; x++)
        {
            //Within each column, loop through y axis (rows).
            for (int y = yLowerLeft; y < yLowerLeft + rows - 1; y++)
            {
                //At each index add a new Vector3 to our list with the x and y coordinates of that position.
                if( (x != xLowerLeft + columns -1 || y != yLowerLeft + rows - 1 )) //Delete the position of the EXIT from the grid
                gridPositions.Add(new Vector3(x, y, 0f)); //&& (x != corridors[i - 1].EndPositionX || y != corridors[i - 1].EndPositionY)
            }
        }

    }

    void RemoveFromGrid()
    {
        //gridPositions.Remove(new Vector3(corridors[i - 1].EndPositionX, corridors[i - 1].EndPositionY, 0f));
        //gridPositions.Remove(new Vector3(xLowerLeft + columns - 1))
    }
}