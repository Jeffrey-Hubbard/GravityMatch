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
    public Tile[,] tempTiles;
    public int howManyMatches;

    List<Tile> matchedTiles = new List<Tile>();

    public bool IsMoving = false;

    public List<string> MatchColor = new List<string> { "blue", "green", "black", "orange", "brown", "grey"};
    public List<string> ShipType = new List<string> { "fighter", "bomber", "transport", "asteroid", "bonus"};

    // Use this for initialization
    void Start()
    {
        boardManager = GetComponent<BoardManager>();

        Vector2 offset = tile.GetComponent<BoxCollider2D>().size;

        MakeBoard(offset.x, offset.y);
    }

    private void Update()
    {
        for (int x = 0; x < xSize; x++)
        {
            for (int y = 0; y < ySize; y++)
            {
                Tile eachTile = tiles[x, y];
                if (eachTile != null)
                {
                    eachTile.x = x;
                    eachTile.y = y;
                }
            }
        }
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
                newTile.x = x;
                newTile.y = y;

                tiles[x, y] = newTile;
                newTile.transform.parent = transform;

                List<string> possibleColors = new List<string>();
                possibleColors.AddRange(MatchColor);
                possibleColors.Remove("bonus");
                possibleColors.Remove("random");
                possibleColors.Remove("none");
                possibleColors.Remove(previousLeftList[y]);
                possibleColors.Remove(previousBelow);

                string newColor = possibleColors[UnityEngine.Random.Range(0, possibleColors.Count)];
                Debug.Log("Setting sprite at " + x + ", " + y + " to " + newColor);
                newTile.Initialize(assignedColor : newColor);


                previousLeftList[y] = newColor;
                previousBelow = newColor;
            }
        }
    }


    public void ClearMatches()
    {
        bool FoundAnyMatches = false;
        boardManager.IsMoving = true;
        tempTiles = tiles;
        matchedTiles = new List<Tile>();

        //begin a loop through the tempTiles
        foreach (Tile tile in tempTiles)
        {
            howManyMatches = 0;
            if (tile != null)
            {
                // is tile a valid matched tile? (reuse code from movement check)
                if (ConfirmMatch(tile))
                {
                    // if yes, then set FoundAnyMatches to true, then
                    // mark all matched tiles in matchedTiles and remove from temptiles
                    FoundAnyMatches = true;
                    DestroyTileAndMatches(tile);
                    if (howManyMatches == 3)
                    {
                        // create minor bonus at tile location, d
                    }
                    else if (howManyMatches > 3)
                    {
                        //create major bonus at tile location, remove from matched and destroy
                    }
                    else
                    {

                    }
                }
            }



            // based on value of howManyMatches either replace tile with a bonus tile at tile location...

            // ... or remove tile from temptiles

        }

        // If the program FoundAnyMatches then
        // Call function to fill in empty spaces
        tiles = tempTiles;
        if (FoundAnyMatches == true)
        {
            ClearMatches();
        } else
        {
            IsMoving = false;
        }
        //else return control to player

        
    }

    private void DestroyTileAndMatches(Tile tile)
    {
        matchedTiles.Add(tile);
        tempTiles[tile.x, tile.y] = null;
        howManyMatches += 1;
        List<Tile> nextMatches = new List<Tile>();
        nextMatches = tile.GetTempAdjacentMatches();
        foreach (Tile newTile in nextMatches)
        {
            if (newTile != null)
            {
                DestroyTileAndMatches(newTile);
            }
            
        }
        foreach (Tile destroyThisTile in matchedTiles)
        {
            Debug.Log("Attempting to destroy the tile " + destroyThisTile.name + " at " + destroyThisTile.x + ", " + destroyThisTile.y);
            Destroy(destroyThisTile);
        }
    }

    public bool IsValidMove(Tile tile1, Tile tile2)
    {

        return true;
    }

    public bool ConfirmMatch(Tile tile)
    {
        List<Tile> possibleMatchList = tile.GetExtendedMatches();
        bool confirmedMatch = false;
        for (int a = 0; a < possibleMatchList.Count; a++)
        {
            for (int b = 0; b < possibleMatchList.Count; b++)
            {
                if (a != b)
                {
                    if ((possibleMatchList[a].x == possibleMatchList[b].x && Math.Abs(possibleMatchList[a].y - possibleMatchList[b].y) <= 2) ||
                        (possibleMatchList[a].y == possibleMatchList[b].y && Math.Abs(possibleMatchList[a].x - possibleMatchList[b].x) <= 2))
                    {
                        confirmedMatch = true;
                    }
                }
            }
        }
        return confirmedMatch;
    }

}

