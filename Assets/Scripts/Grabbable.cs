using UnityEngine;
using System.Collections;

public class Grabbable : MonoBehaviour {
    public static bool snapTo = true;
    public GameObject AnchorPrefab;

    BoxCollider2D myCollider;

    public int numAnchorPoints = 5;
    public float snapRadius = 0.25f;

    GameObject[] anchorPoints;

    bool grabbed = false;

	// Use this for initialization
	void Start () {
        myCollider = GetComponent<BoxCollider2D>();

        anchorPoints = new GameObject[numAnchorPoints];

        for(int i = 0; i < anchorPoints.Length; i++)
        {
            anchorPoints[i] = Instantiate(AnchorPrefab);
            anchorPoints[i].transform.SetParent(transform);
        }
        
        anchorPoints[0].transform.localPosition = new Vector2(0f, 0f);
        anchorPoints[0].tag = "AnchorCenter";
        anchorPoints[1].transform.localPosition = new Vector2(-myCollider.bounds.extents.x, 0f);
        anchorPoints[1].tag = "AnchorLeft";
        anchorPoints[2].transform.localPosition = new Vector2(0f, -myCollider.bounds.extents.y);
        anchorPoints[2].tag = "AnchorBottom";
        anchorPoints[3].transform.localPosition = new Vector2(myCollider.bounds.extents.x, 0f);
        anchorPoints[3].tag = "AnchorRight";
        anchorPoints[4].transform.localPosition = new Vector2(0f, myCollider.bounds.extents.y);
        anchorPoints[4].tag = "AnchorTop";

        Debug.Log("ANCHOR POINTS: " + anchorPoints.Length);
        
    }
	
	// Update is called once per frame
	void Update () {
        if (grabbed)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Debug.Log(mousePos);
            transform.position = mousePos;
        }
	}

    public void Grabbed(GameObject grabber)
    {
        grabbed = true;
    }

    public void Dropped(GameObject dropper) {
        grabbed = false;
        if (snapTo == true) {
            bool snapped = false;
            for (int i = 0; i < anchorPoints.Length; i++)
            {
                int saveLayer = gameObject.layer;
                gameObject.layer = 0;
                Collider2D col = Physics2D.OverlapCircle(anchorPoints[i].transform.position, snapRadius, LayerMask.GetMask("Grabbable"));
                gameObject.layer = saveLayer;
                if (col)
                {
                    snapped = true;
                    if (anchorPoints[i].CompareTag("AnchorCenter"))
                        transform.position = col.gameObject.transform.position;
                    else if (anchorPoints[i].CompareTag("AnchorLeft"))
                        transform.position = new Vector2(col.gameObject.transform.position.x + (col.gameObject.GetComponent<BoxCollider2D>().bounds.extents.x + myCollider.bounds.extents.x), col.gameObject.transform.position.y);
                    else if (anchorPoints[i].CompareTag("AnchorBottom"))
                        transform.position = new Vector2(col.gameObject.transform.position.x, col.gameObject.transform.position.y + (col.gameObject.GetComponent<BoxCollider2D>().bounds.extents.y + myCollider.bounds.extents.y));
                    else if (anchorPoints[i].CompareTag("AnchorRight"))
                        transform.position = new Vector2(col.gameObject.transform.position.x - (col.gameObject.GetComponent<BoxCollider2D>().bounds.extents.x + myCollider.bounds.extents.x), col.gameObject.transform.position.y);
                    else if (anchorPoints[i].CompareTag("AnchorTop"))
                        transform.position = new Vector2(col.gameObject.transform.position.x, col.gameObject.transform.position.y - (col.gameObject.GetComponent<BoxCollider2D>().bounds.extents.y + myCollider.bounds.extents.y));
                }
                if (snapped)
                    break;
            }
        }
    }

    public Grabbable DupSelf()
    {
        return (Instantiate(gameObject) as GameObject).GetComponent<Grabbable>();
    }
}
