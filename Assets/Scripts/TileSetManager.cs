using UnityEngine;
using System.Collections;

public class TileSetManager : MonoBehaviour {

    public Grabbable[] tiles;

	// Use this for initialization
	void Start () {
	    for(int i = 0; i < tiles.Length; i++)
        {
            tiles[i].SetDupMode(true);
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
