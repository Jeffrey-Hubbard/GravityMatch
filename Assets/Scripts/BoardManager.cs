using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BoardManager : MonoBehaviour
{

    public static BoardManager boardManager;
    public HealthBar scoreBoard;
    public List<Sprite> symbols = new List<Sprite>();
    public Tile tile;
    public static int xSize = 9;
    public static int ySize = 9;
    public Warp warp;
    public Explode explode;


    public Tile[,] tiles;
    public Tile[,] tempTiles;
    public int howManyMatches;

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
                if (tiles[x,y] != null)
                {
                    Tile eachTile = tiles[x, y];
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

    private Tile[,] CopyBoard(Tile[,] tiles)
    {
        Tile[,] copyTiles = new Tile[xSize,ySize];
        for (int x = 0; x < xSize; x++)
        {
            for (int y = 0; y < ySize; y++)
            {
                copyTiles[x, y] = tiles[x, y];
            }
        }

        return copyTiles;
    }


    public void ClearMatches()
    {
        GameManager.ChangeState(GameManager.GameState.Matching);
        bool FoundAnyMatches = false;
        boardManager.IsMoving = true;
        tempTiles = CopyBoard(tiles);
        List<Tile> matchedTiles = new List<Tile>();

        //begin a loop through the tempTiles
        for (int x = 0; x < xSize; x++)
        {
            for (int y = 0; y < ySize; y++)
            {
                
                tile = tempTiles[x, y];
                if ( tile != null)
                {
                    // is tile a valid matched tile? (reuse code from movement check)
                    if (ConfirmMatch(tile))
                    {
                        matchedTiles.Add(tile);
                        // if yes, then set FoundAnyMatches to true, then
                        // mark all matched tiles in matchedTiles and remove from temptiles
                        FoundAnyMatches = true;
                        Debug.Log("Tile at " + tile.x + ", " + tile.y + " Is a match!");
                        Debug.Log("matched " + howManyMatches + " tiles");

                    }
                }
            }
        }

        if (matchedTiles.Count > 0)
        {
            StartCoroutine(DestroyMatchedTiles(matchedTiles));
        }
        

        


        //// If the program FoundAnyMatches then
        //// Call function to fill in empty spaces

        //if (FoundAnyMatches == true)
        //{
        //    Debug.Log("Tested FoundAnyMatches as true");
        //    FillTiles();
        //}
        else
        {
            Debug.Log("Tested FoundAnyMatches as false. Move along.");
            IsMoving = false;
            GameManager.ChangeState(GameManager.GameState.PlayerMove);
        }




    }

    private IEnumerator DestroyMatchedTiles(List<Tile> matchedTiles)
    {
        foreach (Tile matchedTile in matchedTiles)
        {
            tiles[matchedTile.x, matchedTile.y] = null;
            if (matchedTile.matchColor == "green" || matchedTile.matchColor == "blue")
            {
                Warp warpGO = Instantiate(warp, new Vector3(matchedTile.x, matchedTile.y, 0), warp.transform.rotation);
                warpGO.TileToDestroy = matchedTile;
            } else
            {
                Explode explodeGO = Instantiate(explode, new Vector3(matchedTile.x, matchedTile.y, 0), explode.transform.rotation);
                explodeGO.TileToDestroy = matchedTile;
            }
            

        }
        yield return new WaitForSeconds(2);
        scoreBoard.ChangeScore(10);
        FillTiles();
    }

    private void FillTiles()
    {
        Debug.Log("starting fill tiles");
        GameManager.ChangeState(GameManager.GameState.Filling);
        int firstEmpty;

        for (int x = 0; x < xSize; x++)
        {
            firstEmpty = -1;
            for (int y = 0; y < ySize; y++)
            {
                if (tiles[x,y] == null && firstEmpty < 0)
                {
                    firstEmpty = y;
                    Debug.Log("Found empty space at " + x + ", " + y);
                } else
                {
                    if (firstEmpty >= 0 && tiles[x,y] != null)
                    {
                        tiles[x, firstEmpty] = tiles[x, y];
                        tiles[x, y] = null;
                        firstEmpty++;
                    }
                }

            }
        }

        AddNewTiles();

    }

    private void AddNewTiles()
    {
        Debug.Log("Starting AddNewTiles");
        for (int x = 0; x < xSize; x++)
        {
            for (int y = 0; y < ySize; y++)
            {
                //if there is an empty space
                if (tiles[x,y] == null)
                {

                    Debug.Log("found a hole at " + x + ", " + y);
                    //create a new tile at x, y + ySize + 1
                    CreateRandomTile(x, y);
                    //add tile to tiles at x, y (this will force animation)
                }
            }
        }

        StartCoroutine(RecheckTilesDelay());
    }

    private void CreateRandomTile(int x, int y)
    {
        Debug.Log("creating a random tile at " + x + ", " + (y + ySize));
        Tile newTile = Instantiate(tile, new Vector3(x, y + ySize, 0), tile.transform.rotation);
          
        newTile.transform.parent = transform;

        List<string> possibleColors = new List<string>();
        possibleColors.AddRange(MatchColor);
        possibleColors.Remove("bonus");
        possibleColors.Remove("random");
        possibleColors.Remove("none");

        string newColor = possibleColors[UnityEngine.Random.Range(0, possibleColors.Count)];
        Debug.Log("Setting sprite at " + x + ", " + y + " to " + newColor);
        newTile.Initialize(assignedColor: newColor);
        tiles[x, y] = newTile;
        Debug.Log("tile at " + x + ", " + y + " is " + tiles[x, y].name);
    }

    private IEnumerator RecheckTilesDelay()
    {
        yield return new WaitForSeconds(1.2f);
        Debug.Log("running match check again");
        ClearMatches();
    }

    private List<Tile> GetContiguousMatches(Tile tile)
    {
        var matchList = new List<Tile>();
        matchList.Add(tile);
        howManyMatches += 1;
        matchList.AddRange(tile.GetAdjacentMatches());
        foreach (Tile forTile in matchList)
        {
            if (forTile != tile)
            {
                List<Tile> tempList = GetContiguousMatches(forTile);
                matchList.AddRange(tempList);
            }
        }

        return matchList;
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

