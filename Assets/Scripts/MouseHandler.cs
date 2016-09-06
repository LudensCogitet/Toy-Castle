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
                    newDup.gameObject.transform.position = duplicator.transform.position;
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
            float dif = Input.GetAxis("Mouse ScrollWheel") * scrollSpeed;
            Camera.main.orthographicSize += dif;

            duplicator.transform.position = Camera.main.ScreenToWorldPoint(duplicatorTarget.position);
            trashCan.transform.position = Camera.main.ScreenToWorldPoint(trashCanTarget.position);

            if (currentDup)
                currentDup.transform.position = duplicator.transform.position;
            

        }
	}
}
