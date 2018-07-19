using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour {

    public static BoardManager boardManager;
    public List<Sprite> symbols = new List<Sprite>();
    public GameObject tile;
    public int xSize, ySize;

    private GameObject[,] tiles;

    public bool IsMoving { get; set; }

    // Use this for initialization
    void Start () {
        boardManager = GetComponent<BoardManager>();

        Vector2 offset = tile.GetComponent<BoxCollider2D>().size;

        MakeBoard(offset.x, offset.y);
	}
	
    private void MakeBoard(float xOffset, float yOffset)
    {
        tiles = new GameObject[xSize, ySize];

        float originX = transform.position.x;
        float originY = transform.position.y;

        Sprite[] previousLeftList = new Sprite[ySize];
        Sprite previousBelow = null;

        for (int x = 0; x < xSize; x++)
        {
            for (int y = 0; y < ySize; y++)
            {
                GameObject newTile = Instantiate(tile, new Vector3(originX + (xOffset * x), originY + (yOffset * y), 0), tile.transform.rotation);
                newTile.name = " Tile at " + x + " " + y;

                tiles[x, y] = newTile;
                newTile.transform.parent = transform;

                List<Sprite> possibleSymbols = new List<Sprite>();
                possibleSymbols.AddRange(symbols);
                possibleSymbols.Remove(previousLeftList[y]);
                possibleSymbols.Remove(previousBelow);

                Sprite newSprite = possibleSymbols[Random.Range(0, possibleSymbols.Count)];
                newTile.GetComponent<SpriteRenderer>().sprite = newSprite;
                previousLeftList[y] = newSprite;
                previousBelow = newSprite;
            }
        }
    }

	// Update is called once per frame
	void Update () {
		
	}
}
