using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour {
    private static Tile previousTile = null;

    private SpriteRenderer render;
    private bool isSelected = false;

    private Vector2[] adjacentCells = new Vector2[] { Vector2.up, Vector2.down, Vector2.left, Vector2.right };

    //public enum MatchColor { blue, green, black, orange, brown, grey, none, random}
    //public enum ShipType { fighter, bomber, transport, meteor, bonus, none, random}

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

    public void Initialize( string matchColor )
    {
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
        render.sprite = Resources.Load<Sprite>(spriteName);
        transform.Rotate(0.0f, 0.0f, UnityEngine.Random.Range(0, 3) * 90);
    }

    //public void Initialize(string matchColor = "random", string shipType = "random")
    //{
    //    if (shipType == "random")
    //    {
    //        shipType = GetRandomShipType();
    //    }
    //    if (matchColor == "random")
    //    {
    //        matchColor = GetRandomMatchColor();
    //    }


    //    string spriteName = shipType + "_" + matchColor;
    //    render.sprite = Resources.Load<Sprite>(spriteName);
    //}

    void Awake ()
    {
        render = GetComponent<SpriteRenderer>();
	}

    private void Select()
    {
        isSelected = true;
        gameObject.transform.GetChild(0).GetComponent<Renderer>().enabled = true;
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
        if (render.sprite == null || BoardManager.boardManager.IsMoving) { return; }

        if (isSelected)
        {
            Deselect();
        } else
        {
            if (previousTile == null)
            {
                Select();
            } else
            {
                if (GetAllAdjacentTiles().Contains(previousTile.gameObject))
                {
                    SwapTiles(previousTile.render);
                    previousTile.Deselect();
                } else
                {
                    previousTile.GetComponent<Tile>().Deselect();
                    Select();
                }

            }
        }
    }

    public void SwapTiles (SpriteRenderer renderer)
    {
        if (render.sprite == renderer.sprite) { return;  }

        Sprite tempSprite = renderer.sprite;
        renderer.sprite = render.sprite;
        render.sprite = tempSprite;
    }

    private GameObject GetAdjacent(Vector2 castDirection)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, castDirection);
        if (hit.collider != null)
        {
            return hit.collider.gameObject;
        }
        return null;
    }

    private List<GameObject> GetAllAdjacentTiles()
    {
        List<GameObject> adjacentTiles = new List<GameObject>();
        for (int i = 0; i < adjacentCells.Length; i++)
        {
            adjacentTiles.Add(GetAdjacent(adjacentCells[i]));
        }
        return adjacentTiles;
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
