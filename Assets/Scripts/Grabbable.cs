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

    public bool topLeftAnchor;
    public bool topCenterAnchor;
    public bool topRightAnchor;
    public bool midLeftAnchor;
    public bool centerAnchor;
    public bool midRightAnchor;
    public bool botLeftAnchor;
    public bool botCenterAnchor;
    public bool botRightAnchor;


    public float anchorOffset;
    public float snapRadius = 0.2f;

    int numAnchorPoints;
    public GameObject[] anchorPoints;

    public GameObject[] customAnchorPoints;

    bool grabbed = false;
    int anchorLayer;

    void Awake()
    {
        myCollider = GetComponent<BoxCollider2D>();

        if (topLeftAnchor)
            numAnchorPoints++;
        if (topCenterAnchor)
            numAnchorPoints++;
        if (topRightAnchor)
            numAnchorPoints++;
        if (midLeftAnchor)
            numAnchorPoints++;
        if (centerAnchor)
            numAnchorPoints++;
        if (midRightAnchor)
            numAnchorPoints++;
        if (botLeftAnchor)
            numAnchorPoints++;
        if (botCenterAnchor)
            numAnchorPoints++;
        if (botRightAnchor)
            numAnchorPoints++;

        if (numAnchorPoints > 0)
        {
            anchorPoints = new GameObject[numAnchorPoints];
            Debug.Log(anchorPoints.Length);

            for (int i = 0; i < anchorPoints.Length; i++)
            {
                anchorPoints[i] = Instantiate(AnchorPrefab);
                anchorPoints[i].transform.SetParent(transform);
            }

            int x = 0;
            if (topLeftAnchor)
            {
                anchorPoints[x].transform.localPosition = new Vector2(-myCollider.bounds.extents.x + anchorOffset, myCollider.bounds.extents.y - anchorOffset);
                x++;
            }

            if (topCenterAnchor)
            {
                anchorPoints[x].transform.localPosition = new Vector2(0f, myCollider.bounds.extents.y - anchorOffset);
                x++;
            }

            if (topRightAnchor)
            {
                anchorPoints[x].transform.localPosition = new Vector2(myCollider.bounds.extents.x - anchorOffset, myCollider.bounds.extents.y - anchorOffset);
                x++;
            }

            if (midLeftAnchor)
            {
                anchorPoints[x].transform.localPosition = new Vector2(-myCollider.bounds.extents.x + anchorOffset, 0f);
                x++;
            }

            if (centerAnchor)
            {
                anchorPoints[x].transform.localPosition = new Vector2(0f, 0f);
                x++;
            }

            if (midRightAnchor)
            {
                anchorPoints[x].transform.localPosition = new Vector2(myCollider.bounds.extents.x - anchorOffset, 0f);
                x++;
            }

            if (botLeftAnchor)
            {
                anchorPoints[x].transform.localPosition = new Vector2(-myCollider.bounds.extents.x + anchorOffset, -myCollider.bounds.extents.y + anchorOffset);
                x++;
            }

            if (botCenterAnchor)
            {
                anchorPoints[x].transform.localPosition = new Vector2(0f, -myCollider.bounds.extents.y + anchorOffset);
                x++;
            }

            if (botRightAnchor)
            {
                anchorPoints[x].transform.localPosition = new Vector2(myCollider.bounds.extents.x - anchorOffset, -myCollider.bounds.extents.y + anchorOffset);
                x++;
            }
            anchorLayer = anchorPoints[0].layer;
        }
        else
        {
            anchorLayer = customAnchorPoints[0].layer;
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

    void CloakAnchors()
    {
        if (anchorPoints != null)
        {
            for (int j = 0; j < anchorPoints.Length; j++)
            {
                anchorPoints[j].layer = 0;
            }
        }
        if (customAnchorPoints != null)
        {
            for (int i = 0; i < customAnchorPoints.Length; i++)
            {
                customAnchorPoints[i].layer = 0;
            }
        }
    }

    void DecloakAnchors()
    {
        if (anchorPoints != null)
        {
            for (int j = 0; j < anchorPoints.Length; j++)
            {
                anchorPoints[j].layer = anchorLayer;
            }
        }

        if (customAnchorPoints != null)
        {
            for (int i = 0; i < customAnchorPoints.Length; i++)
            {
                customAnchorPoints[i].layer = anchorLayer;
            }
        }
    }

    void SnapToAnchor(GameObject anchor, GameObject newPoint)
    {
        float oldZ = gameObject.transform.position.z;
        anchor.transform.SetParent(null);
        gameObject.transform.SetParent(anchor.transform);

        anchor.transform.position = newPoint.transform.position;

        gameObject.transform.SetParent(null);
        anchor.transform.SetParent(gameObject.transform);
        transform.position = new Vector3(transform.position.x, transform.position.y, oldZ);
    }

    bool CheckAnchorCollisions(GameObject[] anchors)
    {
        if (anchors == null)
            return false;

        for (int i = 0; i < anchors.Length; i++)
        {
            CloakAnchors();
            Collider2D col = Physics2D.OverlapCircle(anchors[i].transform.position, snapRadius, LayerMask.GetMask("Anchor"));
            DecloakAnchors();

            if (col)
            {
                SnapToAnchor(anchors[i], col.gameObject);
                return true;
            }

        }
        return false;
    }

    public void Dropped(GameObject dropper)
    {
        grabbed = false;
        if (mouseHandler.globalSnapTo == true && mySnapTo == true)
        {
             if (!CheckAnchorCollisions(anchorPoints))
                CheckAnchorCollisions(customAnchorPoints);
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
        if (anchorPoints != null)
        {
            for (int i = 0; i < anchorPoints.Length; i++)
            {
                anchorPoints[i].SetActive(false);
            }
        }

        if (customAnchorPoints != null)
        {
            for (int j = 0; j < customAnchorPoints.Length; j++)
            {
                customAnchorPoints[j].SetActive(false);
            }
        }
    }

    public void EnableAnchors()
    {

        if (anchorPoints != null)
        {
            for (int i = 0; i < anchorPoints.Length; i++)
            {
                anchorPoints[i].SetActive(true);
            }
        }

        if (customAnchorPoints != null)
        {
            for (int j = 0; j < customAnchorPoints.Length; j++)
            {
                customAnchorPoints[j].SetActive(true);
            }
        }
    }

    public Grabbable CopyClean()
    {
        GameObject copy;


        if (anchorPoints != null)
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

        if (anchorPoints != null)
        {
            for (int i = 0; i < anchorPoints.Length; i++)
            {
                anchorPoints[i].transform.SetParent(gameObject.transform);
            }
        }

        return copy.GetComponent<Grabbable>();
    }
}
