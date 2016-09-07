using UnityEngine;
using System.Collections;

public class TrashCan : MonoBehaviour {

	// Use this for initialization
	void Start () {

        float worldToPixels = ((Screen.height / 2.0f) / Camera.main.orthographicSize);
        Vector3 trashScreenPos = new Vector3(Camera.main.pixelWidth - GetComponent<BoxCollider2D>().bounds.extents.x * worldToPixels, GetComponent<BoxCollider2D>().bounds.extents.y * worldToPixels, 10f);
        transform.position = Camera.main.ScreenToWorldPoint(trashScreenPos);
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
