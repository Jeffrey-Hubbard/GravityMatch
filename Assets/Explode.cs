using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explode : MonoBehaviour {

    public Animation anim;
    public int x, y;
    public Tile TileToDestroy;


    // Use this for initialization
    //   IEnumerator Start () {
    //       anim = GetComponent<Animation>();
    //       anim.Play();
    //       yield return new WaitForSeconds(anim.clip.length);
    //}

    // Update is called once per frame
    void Update()
    {

    }

    void DestroyTile()
    {
        Destroy(TileToDestroy.gameObject);
    }

    void DestroyGameObject()
    {
        Destroy(gameObject);
    }
}
