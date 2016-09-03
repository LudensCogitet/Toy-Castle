using UnityEngine;
using System.Collections;

public class MouseHandler : MonoBehaviour {

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
                myThing = col.gameObject.GetComponent<Grabbable>();
                myThing.Grabbed(this.gameObject);
            }
        }
        else if (Input.GetMouseButtonDown(1))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Collider2D col = Physics2D.OverlapCircle(mousePos, grabRadius, LayerMask.GetMask("Grabbable"));

            if (col)
            {
                myThing = col.gameObject.GetComponent<Grabbable>().DupSelf();
                myThing.Grabbed(this.gameObject);
            }
        }
        else if (Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1))
        {
            myThing.Dropped(this.gameObject);
            myThing = null;
        }
	}
}
