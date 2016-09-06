using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour {

    public float panSpeed = 3f;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 delta = Vector3.zero;
        float currentPanSpeed = panSpeed;
        if (Input.GetKey(KeyCode.LeftShift))
            currentPanSpeed = panSpeed / 4;

        if (Input.GetKey(KeyCode.LeftArrow)) {
            delta += Vector3.left * currentPanSpeed;
        }
        if(Input.GetKey(KeyCode.RightArrow))
        {
            delta += Vector3.right * currentPanSpeed;
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            delta += Vector3.up * currentPanSpeed;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            delta += Vector3.down * currentPanSpeed;
        }
        transform.position += delta;
    }
}
