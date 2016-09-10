using UnityEngine;
using System.Collections;

public class Grabbable : MonoBehaviour {

    public MouseHandler mouseHandler;
    public bool mySnapTo = true;
    public bool dupMode = false;
    public GameObject AnchorPrefab;

    public static float highestZDepth = 0f;

    public GameObject duplicatorPos;

    BoxCollider2D myCollider;

    public bool customAnchors = false;

    public int numAnchorPoints = 9;
    public float snapRadius = 0.2f;

    public GameObject[] anchorPoints;
    GameObject[] customAnchorPointsSaved;

    bool grabbed = false;

    void Awake()
    {
        myCollider = GetComponent<BoxCollider2D>();

        if (!customAnchors)
        {
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
        }
        Debug.Log("ANCHOR POINTS: " + anchorPoints.Length);
    }

	// Use this for initialization
	void Start () {
        mouseHandler = FindObjectOfType<MouseHandler>();
    }
	
	// Update is called once per frame
	void Update () {
        if (grabbed)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector3(mousePos.x,mousePos.y,transform.position.z);
        }
	}

    public Grabbable Grabbed(GameObject grabber)
    {
        if (!dupMode)
        {
            highestZDepth -= 0.001f;
            if (highestZDepth == -9f)
                highestZDepth = 0f;
            gameObject.transform.position = new Vector3(transform.position.x, transform.position.y, highestZDepth);
        }

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
        if (mouseHandler.globalSnapTo == true && mySnapTo == true)
        {
            bool snapped = false;
            for (int i = 0; i < anchorPoints.Length; i++)
            {
                int saveLayer = anchorPoints[i].layer;
                for (int j = 0; j < anchorPoints.Length; j++) {
                    anchorPoints[j].layer = 0;
                }
                Collider2D col = Physics2D.OverlapCircle(anchorPoints[i].transform.position, snapRadius, LayerMask.GetMask("Anchor"));
                for (int j = 0; j < anchorPoints.Length; j++)
                {
                    anchorPoints[j].layer = saveLayer;
                }

                if (col)
                {
                    float oldZ = gameObject.transform.position.z;
                    snapped = true;
                    anchorPoints[i].transform.SetParent(null);
                    gameObject.transform.SetParent(anchorPoints[i].transform);

                    anchorPoints[i].transform.position = col.gameObject.transform.position;

                    gameObject.transform.SetParent(null);
                    anchorPoints[i].transform.SetParent(gameObject.transform);
                    transform.position = new Vector3(transform.position.x, transform.position.y, oldZ);
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
            DisableAnchors();
        }
        else
        {
            EnableAnchors();
        }
    }

    public void DisableAnchors()
    {
        for (int i = 0; i < anchorPoints.Length; i++)
        {
            anchorPoints[i].SetActive(false);
        }
    }

    public void EnableAnchors()
    {
        for (int i = 0; i < anchorPoints.Length; i++)
        {
            anchorPoints[i].SetActive(true);
        }
    }

    public Grabbable CopyClean()
    {
        GameObject copy;

        if (!customAnchors)
        {
            for (int i = 0; i < anchorPoints.Length; i++)
            {
                anchorPoints[i].transform.SetParent(null);
            }
        }
        bool wasGrabbed = grabbed;
        if (wasGrabbed)
            grabbed = false;

        bool wasDupMode = dupMode;
        if (wasDupMode)
            SetDupMode(false);

        copy = Instantiate(gameObject) as GameObject;

        if (wasDupMode)
            SetDupMode(true);
        if (wasGrabbed)
            grabbed = true;

        if (!customAnchors)
        {
            for (int i = 0; i < anchorPoints.Length; i++)
            {
                anchorPoints[i].transform.SetParent(gameObject.transform);
            }
        }
        return copy.GetComponent<Grabbable>();

    }
}
