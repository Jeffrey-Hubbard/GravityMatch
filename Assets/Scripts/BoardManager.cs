using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BoardManager : MonoBehaviour
{

    public static BoardManager boardManager;
    public List<Sprite> symbols = new List<Sprite>();
    public Tile tile;
    public static int xSize = 9;
    public static int ySize = 9;

    public Tile[,] tiles;

    public bool IsMoving { get; set; }

    public List<string> MatchColor = new List<string> { "blue", "green", "black", "orange", "brown", "grey"};
    public List<string> ShipType = new List<string> { "fighter", "bomber", "transport", "asteroid", "bonus"};

    // Use this for initialization
    void Start()
    {
        boardManager = GetComponent<BoardManager>();

        Vector2 offset = tile.GetComponent<BoxCollider2D>().size;

        MakeBoard(offset.x, offset.y);
    }

    private void MakeBoard(float xOffset, float yOffset)
    {
        tiles = new Tile[xSize, ySize];

        float originX = transform.position.x;
        float originY = transform.position.y;

        string[] previousLeftList = new string[ySize];
        string previousBelow = null;

        for (int x = 0; x < xSize; x++)
        {
            for (int y = 0; y < ySize; y++)
            {
                Tile newTile = Instantiate(tile, new Vector3(originX + (xOffset * x), originY + (yOffset * y), 0), tile.transform.rotation);
                newTile.name = " Tile at " + x + " " + y;

                tiles[x, y] = newTile;
                newTile.transform.parent = transform;

                List<string> possibleColors = new List<string>();
                possibleColors.AddRange(MatchColor);
                possibleColors.Remove("bonus");
                possibleColors.Remove("random");
                possibleColors.Remove("none");
                possibleColors.Remove(previousLeftList[y]);
                possibleColors.Remove(previousBelow);

                //Sprite newSprite = possibleSymbols[Random.Range(0, possibleSymbols.Count)];
                string newColor = possibleColors[UnityEngine.Random.Range(0, possibleColors.Count)];
                Debug.Log("Setting sprite at " + x + ", " + y + " to " + newColor);
                newTile.Initialize(matchColor : newColor);


                previousLeftList[y] = newColor;
                previousBelow = newColor;
            }
        }
    }

    private void ClearMatches()
    {
        bool FoundAnyMatches = false;
        boardManager.IsMoving = true;
        Tile[,] tempTiles = tiles;
        Tile[,] matchedTiles = new Tile[xSize, ySize];

        //begin a loop through the tempTiles
        foreach (Tile tile in tempTiles)
        {
            int howManyMatches = 0;
            // is tile a valid matched tile? (reuse code from movement check)
                
            // if yes, then set FoundAnyMatches to true, then
            // mark all matched tiles in matchedTiles and remove from temptiles

            // based on value of howManyMatches either replace tile with a bonus tile at tile location...

            // ... or remove tile from temptiles

        }

        // If the program FoundAnyMatches then
        // Call function to fill in empty spaces

        //else return control to player
        

    }
}

