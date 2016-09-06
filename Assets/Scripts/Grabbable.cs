using UnityEngine;
using System.Collections;

public class Grabbable : MonoBehaviour {
    public static bool globalSnapTo = true;
    public bool mySnapTo = true;
    public bool dupMode = false;
    public GameObject AnchorPrefab;

    public GameObject duplicatorPos;

    BoxCollider2D myCollider;

    public int numAnchorPoints = 9;
    public float snapRadius = 0.2f;

    GameObject[] anchorPoints;

    bool grabbed = false;

    void Awake()
    {
        myCollider = GetComponent<BoxCollider2D>();

        anchorPoints = new GameObject[numAnchorPoints];

        for (int i = 0; i < anchorPoints.Length; i++)
        {
            anchorPoints[i] = Instantiate(AnchorPrefab);
            anchorPoints[i].transform.SetParent(transform);
        }

        anchorPoints[0].transform.localPosition = new Vector2(0f, 0f);
        anchorPoints[1].transform.localPosition = new Vector2(-myCollider.bounds.extents.x, 0f);
        anchorPoints[2].transform.localPosition = new Vector2(-myCollider.bounds.extents.x, -myCollider.bounds.extents.y);
        anchorPoints[3].transform.localPosition = new Vector2(0f, -myCollider.bounds.extents.y);
        anchorPoints[4].transform.localPosition = new Vector2(myCollider.bounds.extents.x, -myCollider.bounds.extents.y);
        anchorPoints[5].transform.localPosition = new Vector2(myCollider.bounds.extents.x, 0f);
        anchorPoints[6].transform.localPosition = new Vector2(myCollider.bounds.extents.x, myCollider.bounds.extents.y);
        anchorPoints[7].transform.localPosition = new Vector2(0f, myCollider.bounds.extents.y);
        anchorPoints[8].transform.localPosition = new Vector2(-myCollider.bounds.extents.x, myCollider.bounds.extents.y);

        Debug.Log("ANCHOR POINTS: " + anchorPoints.Length);
    }

	// Use this for initialization
	void Start () {
    }
	
	// Update is called once per frame
	void Update () {
        if (grabbed)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = mousePos;
        }
	}

    public Grabbable Grabbed(GameObject grabber)
    {

        if (dupMode)
        {
            return CopyClean().Grabbed(grabber);
        }
        else
        {
            grabbed = true;
            return this;
        }
        
    }

    public void Dropped(GameObject dropper)
    {
        grabbed = false;
        if (globalSnapTo == true && mySnapTo == true)
        {
            bool snapped = false;
            for (int i = 0; i < anchorPoints.Length; i++)
            {
                int saveLayer = anchorPoints[i].layer;
                anchorPoints[i].layer = 0;
                Collider2D col = Physics2D.OverlapCircle(anchorPoints[i].transform.position, snapRadius, LayerMask.GetMask("Anchor"));
                anchorPoints[i].layer = saveLayer;
                if (col)
                {
                    snapped = true;
                    anchorPoints[i].transform.SetParent(null);
                    gameObject.transform.SetParent(anchorPoints[i].transform);

                    anchorPoints[i].transform.position = col.gameObject.transform.position;

                    gameObject.transform.SetParent(null);
                    anchorPoints[i].transform.SetParent(gameObject.transform);
                }
                if (snapped)
                    break;
            }
        }
    }

    public void SetDupMode(bool state)
    {
        dupMode = state;
        if(dupMode == true)
        {
            for(int i = 0; i < anchorPoints.Length; i++)
            {
                anchorPoints[i].SetActive(false);
            }
        }
        else
        {
            for (int i = 0; i < anchorPoints.Length; i++)
            {
                anchorPoints[i].SetActive(true);
            }
        }
    }

    public Grabbable CopyClean()
    {
        GameObject copy;
            for (int i = 0; i < anchorPoints.Length; i++)
            {
                anchorPoints[i].transform.SetParent(null);
            }
            bool wasGrabbed = grabbed;
            if(wasGrabbed)
                grabbed = false;

        bool wasDupMode = dupMode;
        if (wasDupMode)
            SetDupMode(false);

            copy = Instantiate(gameObject) as GameObject;

        if (wasDupMode)
            SetDupMode(true);
            if (wasGrabbed)
                grabbed = true;
            
            for (int i = 0; i < anchorPoints.Length; i++)
            {
                anchorPoints[i].transform.SetParent(gameObject.transform);
            }

            return copy.GetComponent<Grabbable>();
        
    }

}
