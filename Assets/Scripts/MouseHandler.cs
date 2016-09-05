using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MouseHandler : MonoBehaviour {

    public GameObject duplicatorPos;
    public GameObject currentDup;

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

                Destroy(currentDup);
                currentDup = Instantiate(myThing.gameObject, duplicatorPos.transform.position,Quaternion.identity,null) as GameObject;
                currentDup.GetComponent<Grabbable>().SetDupMode(true);
                
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
	}
}
