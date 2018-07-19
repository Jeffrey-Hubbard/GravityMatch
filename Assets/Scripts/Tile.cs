using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour {
    private static Tile previousTile = null;
    public Color selectedColor = new Color(.5f, .5f, .5f, 1.0f);

    private SpriteRenderer render;
    private bool isSelected = false;

    private Vector2[] adjacentCells = new Vector2[] { Vector2.up, Vector2.down, Vector2.left, Vector2.right };

    public enum matchColor { blue, green, black, orange, brown, grey}

	// Use this for initialization
	void Awake ()
    {
        render = GetComponent<SpriteRenderer>();
	}

    private void Select()
    {
        isSelected = true;
        render.color = selectedColor;
        previousTile = gameObject.GetComponent<Tile>();
        // Play a sound?
    }

    private void Deselect()
    {
        isSelected = false;
        render.color = Color.white;
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
}
