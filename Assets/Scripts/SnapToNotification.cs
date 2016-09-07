using UnityEngine;
using System.Collections;

public class SnapToNotification : MonoBehaviour {

    public Sprite snapped;
    public Sprite notSnapped;
    public float onTime;

    SpriteRenderer myRenderer;

    // Use this for initialization
	void Start () {
        myRenderer = GetComponent<SpriteRenderer>();
        transform.position = new Vector3(10000f, 10000f, -12.1f);
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Snapped(Vector3 position)
    {
        myRenderer.sprite = snapped;
        transform.position = new Vector3(position.x,position.y,-9.1f);

        Invoke("Done", onTime);
    }

    public void NotSnapped(Vector3 position)
    {
        myRenderer.sprite = notSnapped;
        transform.position = new Vector3(position.x, position.y, -9.1f);

        Invoke("Done", onTime);
    }

    void Done()
    {
        transform.position = new Vector3(10000f, 10000f, -12.1f);
    }


}
