using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MouseHandler : MonoBehaviour {

    public GameObject currentDup;
    public GameObject duplicator;
    public GameObject trashCan;

    public RectTransform duplicatorTarget;
    public RectTransform trashCanTarget;

    public float scrollSpeed;
    public float grabRadius = 0.1f;

    Grabbable myThing;

	// Use this for initialization
	void Start () {
    }
	
	// Update is called once per frame
	void Update () {
        float worldToPixels = ((Screen.height / 2.0f) / Camera.main.orthographicSize);

        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Collider2D col = Physics2D.OverlapCircle(mousePos, grabRadius, LayerMask.GetMask("Grabbable"));

            if (col)
            {
                myThing = col.gameObject.GetComponent<Grabbable>().Grabbed(this.gameObject);

                if (col.gameObject != currentDup)
                {
                    if (currentDup)
                        Destroy(currentDup);

                    Grabbable newDup = myThing.CopyClean();
                    newDup.SetDupMode(true);

                    Vector3 dupScreenPos = new Vector3(newDup.GetComponent<BoxCollider2D>().bounds.extents.x * worldToPixels, newDup.GetComponent<BoxCollider2D>().bounds.extents.y * worldToPixels, 10f);
                    duplicator.transform.position = Camera.main.ScreenToWorldPoint(dupScreenPos);
                    newDup.transform.position = duplicator.transform.position;

                    newDup.transform.SetParent(duplicator.transform);
                    currentDup = newDup.gameObject;
                }     
            }
        }
        else if (Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1))
        {
            if (myThing)
            {
                myThing.Dropped(this.gameObject);
                myThing = null;
            }
        }
        else if(Input.GetAxis("Mouse ScrollWheel") != 0f)
        {
            Debug.Log("hello");
            float dif = Input.GetAxis("Mouse ScrollWheel") * scrollSpeed;
            Camera.main.orthographicSize += dif;

            Vector3 trashScreenPos = new Vector3(Camera.main.pixelWidth - trashCan.GetComponent<BoxCollider2D>().bounds.extents.x * worldToPixels, trashCan.GetComponent<BoxCollider2D>().bounds.extents.y * worldToPixels, 10f);
            trashCan.transform.position = Camera.main.ScreenToWorldPoint(trashScreenPos);
            if (currentDup)
            {
                Vector3 dupScreenPos = new Vector3(currentDup.GetComponent<BoxCollider2D>().bounds.extents.x * worldToPixels, currentDup.GetComponent<BoxCollider2D>().bounds.extents.y * worldToPixels, 10f);
                duplicator.transform.position = Camera.main.ScreenToWorldPoint(dupScreenPos);
                currentDup.transform.position = duplicator.transform.position;
            }         

        }
	}
}
