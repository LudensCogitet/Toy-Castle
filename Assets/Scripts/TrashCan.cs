using UnityEngine;
using System.Collections;

public class TrashCan : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonUp(0))
        {
            Collider2D col = Physics2D.OverlapCircle(transform.position, 1f, LayerMask.GetMask("Grabbable"));
            if (col)
            {
                Destroy(col.gameObject);
            }
        }
	}

}
