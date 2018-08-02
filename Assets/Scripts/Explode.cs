using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explode : MonoBehaviour {

    public Animation anim;
    public int x, y;
    public Tile TileToDestroy;

    void DestroyTile()
    {
        Destroy(TileToDestroy.gameObject);
    }

    void DestroyGameObject()
    {
        Destroy(gameObject);
    }
}
