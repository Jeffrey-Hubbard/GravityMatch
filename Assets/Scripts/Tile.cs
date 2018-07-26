using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour {
    private static Tile previousTile = null;

    private SpriteRenderer render;
    private bool isSelected = false;
    public float TileMovementTime = 0.2f;

    public int x, y;
    public static float speed = 9.0f;

    public List<string> MatchColors = new List<string> { "blue", "green", "black", "orange", "brown", "grey" };
    public List<string> ShipTypes = new List<string> { "fighter", "bomber", "transport", "asteroid" };

    public string matchColor;
    public string shipType;

    // Use this for initialization

    public void Initialize()
    {
         matchColor = GetRandomMatchColor();
        // All grey and brown colored icons are asteroids
        if (matchColor == "grey" || matchColor == "brown")
        {
            shipType = "asteroid";
        }
        // Black and Orange icons are enemies (pick randomly from ship types)
        if (matchColor == "black" || matchColor == "orange")
        {
            shipType = ShipTypes[UnityEngine.Random.Range(0, ShipTypes.Count - 1)];
        }
        // Green and Blue icons are friendly ships (pick randomly from ship types)
        if (matchColor == "blue" || matchColor == "green")
        {
            shipType = ShipTypes[UnityEngine.Random.Range(0, ShipTypes.Count - 1)];
        }


        string spriteName = shipType + "_" + matchColor;
        render.sprite = Resources.Load<Sprite>(spriteName);
        transform.Rotate(0.0f, 0.0f, UnityEngine.Random.Range(0, 3) * 90);

    }

    public void Initialize( string assignedColor )
    {
        matchColor = assignedColor;
        if (matchColor == "random")
        {
            matchColor = GetRandomMatchColor();
        }

        // All grey and brown colored icons are asteroids
        if (matchColor == "grey" || matchColor == "brown")
        {
            shipType = "asteroid";
        }
        // Black and Orange icons are enemies (pick randomly from ship types)
        if (matchColor == "black" || matchColor == "orange")
        {
            shipType = ShipTypes[UnityEngine.Random.Range(0, ShipTypes.Count - 1)];
        }
        // Green and Blue icons are friendly ships (pick randomly from ship types)
        if (matchColor == "blue" || matchColor == "green")
        {
            shipType = ShipTypes[UnityEngine.Random.Range(0, ShipTypes.Count - 1)];
        }


        string spriteName = shipType + "_" + matchColor;
        this.name = spriteName;
        render.sprite = Resources.Load<Sprite>(spriteName);
        transform.Rotate(0.0f, 0.0f, UnityEngine.Random.Range(0, 3) * 90);
    }

    void Awake ()
    {
        render = GetComponent<SpriteRenderer>();
	}

    private void Update()
    {
        if ((transform.position.x != x || transform.position.y != y))
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(x, y, 0), speed * Time.deltaTime);
        }
    }

    private void Select()
    {
        isSelected = true;
        this.gameObject.transform.GetChild(0).GetComponent<Renderer>().enabled = true;
        previousTile = gameObject.GetComponent<Tile>();
        // Play a sound?
    }

    private void Deselect()
    {
        isSelected = false;
        gameObject.transform.GetChild(0).GetComponent<Renderer>().enabled = false;
        previousTile = null;
    }

    public void OnMouseDown()
    {
        //BoardManager.boardManager.IsMoving = false;
        if (render.sprite == null || BoardManager.boardManager.IsMoving) { return; }
        if (isSelected)
        {
            Debug.Log("calling deselect");
            Deselect();
        } else
        {
            if (previousTile == null)
            {
                Debug.Log("calling select");
                Select();
            } else
            {
                if (GetAllAdjacentTiles().Contains(previousTile))
                {
                    Debug.Log("touched tile is adjacent, moving tiles");
                    BoardManager.boardManager.IsMoving = true;
                    SwapTiles(previousTile, this);
                    Debug.Log("Ran swap tiles");
                    previousTile.Deselect();
                    //BoardManager.boardManager.IsMoving = false;
                } else
                {
                    previousTile.GetComponent<Tile>().Deselect();
                    Select();
                }

            }
        }
    }

    public void SwapTiles (Tile previousTile, Tile touchedTile)
    {
        Debug.Log("Made it into swap tiles");

            
        int previousX = previousTile.x;
        int previousY = previousTile.y;
        BoardManager.boardManager.tiles[touchedTile.x, touchedTile.y] = previousTile;
        BoardManager.boardManager.tiles[previousX, previousY] = touchedTile;

        // give the tiles time to move
        Debug.Log("start coroutine");
        StartCoroutine(TileMovementDelay(previousTile, touchedTile));
        Debug.Log("end coroutine");
        
    }

    private IEnumerator TileMovementDelay (Tile previousTile, Tile touchedTile)
    {
        yield return new WaitForSeconds(TileMovementTime);
        bool previousMatch = previousTile.ConfirmMatch(previousTile.x, previousTile.y);
        bool touchedMatch = touchedTile.ConfirmMatch(touchedTile.x, touchedTile.y);
        if (previousMatch || touchedMatch)
        {
            // match! Clear matches!
            Debug.Log("valid match");
            BoardManager.boardManager.ClearMatches();
        } else
        {
            int previousX = previousTile.x;
            int previousY = previousTile.y;
            BoardManager.boardManager.tiles[touchedTile.x, touchedTile.y] = previousTile;
            BoardManager.boardManager.tiles[previousX, previousY] = touchedTile;
        }
        yield return new WaitForSeconds(0.5f);
        BoardManager.boardManager.IsMoving = false;
    }

    private List<Tile> GetAllAdjacentTiles()
    {
        List<Tile> adjacentTiles = new List<Tile>();

        for (int ix = -1; ix <= 1; ix++)
        {
            for (int iy = -1; iy <= 1; iy++)
            {
                if (x + ix >= 0 && x + ix < BoardManager.xSize && y + iy >= 0 && y + iy < BoardManager.ySize)
                {
                    if ((ix == 0 || iy == 0) && !(ix == 0 && iy == 0))
                    {
                        Tile provisionalTile = BoardManager.boardManager.tiles[x + ix, y + iy];
                        if (provisionalTile != null)
                        {
                            adjacentTiles.Add(provisionalTile);
                        }
                    }
                }
            }
        }
        return adjacentTiles;
    }

    private List<Tile> GetAllTempAdjacentTiles()
    {
        List<Tile> adjacentTiles = new List<Tile>();

        for (int ix = -1; ix <= 1; ix++)
        {
            for (int iy = -1; iy <= 1; iy++)
            {
                if (x + ix >= 0 && x + ix < BoardManager.xSize && y + iy >= 0 && y + iy < BoardManager.ySize)
                {
                    if ((ix == 0 || iy == 0) && !(ix == 0 && iy == 0))
                    {
                        Tile provisionalTile = BoardManager.boardManager.tempTiles[x + ix, y + iy];
                        if (provisionalTile != null)
                        {
                            adjacentTiles.Add(provisionalTile);
                        }
                    }
                }
            }
        }
        return adjacentTiles;
    }

    private List<Tile> GetAllExtendedTiles()
    {
        List<Tile> adjacentTiles = new List<Tile>();

        for (int ix = -2; ix <= 2; ix++)
        {
            for (int iy = -2; iy <= 2; iy++)
            {
                if (x + ix >= 0 && x + ix < BoardManager.xSize && y + iy >= 0 && y + iy < BoardManager.ySize)
                {
                    if ((ix == 0 || iy == 0) && !(ix == 0 && iy == 0))
                    {
                        Tile provisionalTile = BoardManager.boardManager.tiles[x + ix, y + iy];
                        if (provisionalTile != null)
                        {
                            adjacentTiles.Add(provisionalTile);
                        }
                        
                    }
                }
            }
        }
        return adjacentTiles;
    }

    public bool ConfirmMatch( int xPos, int yPos )
    {
        List<Tile> possibleMatchList = this.GetExtendedMatches();
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

    public List<Tile> GetAdjacentMatches()
    {
        List<Tile> adjacentTiles = GetAllAdjacentTiles();
        List<Tile> adjacentMatches = new List<Tile>();
        foreach (Tile tile in adjacentTiles)
        {
            if ((tile != this) && (tile.matchColor == this.matchColor))
            {
                adjacentMatches.Add(tile);
            }
        }
        return adjacentMatches;
    }

    public List<Tile> GetTempAdjacentMatches()
    {
        List<Tile> adjacentTiles = GetAllTempAdjacentTiles();
        List<Tile> adjacentMatches = new List<Tile>();
        foreach (Tile tile in adjacentTiles)
        {
            if ((tile != this) && (tile.matchColor == this.matchColor))
            {
                adjacentMatches.Add(tile);
            }
        }
        return adjacentMatches;
    }

    public List<Tile> GetExtendedMatches()
    {
        List<Tile> adjacentTiles = GetAllExtendedTiles();
        List<Tile> adjacentMatches = new List<Tile>();
        foreach (Tile tile in adjacentTiles)
        {
            if (tile.matchColor == this.matchColor)
            {
                adjacentMatches.Add(tile);
            }
        }
        return adjacentMatches;
    }

    public List<Tile> GetMatches(List<Tile> tileList)
    {
        List<Tile> returnMatches = new List<Tile>(); ;
        return returnMatches;
    }

    private string GetRandomShipType()
    {
        try
        {
            string shipType = ShipTypes[UnityEngine.Random.Range(0, ShipTypes.Count)];
            return shipType;
        }
        catch (Exception ex)
        {
            //error handling here
            Debug.Log("Caught error " + ex.Message);
            return "none";
        }
    }

    private string GetRandomMatchColor ()
    {
        try
        {
            string matchColor = MatchColors[UnityEngine.Random.Range(0, MatchColors.Count)];
            return matchColor;
        }
        catch (Exception ex)
        {
            //error handling here
            Debug.Log("Caught error " + ex.Message);
            return "none";
        }
    }

    public GameObject GetRight(int count = 1)
    {
        //GameObject returnGameObject = BoardManager.boardManager.tiles[];

        GameObject returnGameObject = new GameObject();

        return returnGameObject;
    }
}
