using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour {

    public float panSpeed = 0.15f;
    Vector3 moveDelta = Vector3.zero;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        float currentPanSpeed = panSpeed;


        if (Input.GetKey(KeyCode.LeftShift))
            currentPanSpeed = panSpeed / 4;
        float worldToPixels = ((Screen.height / 2.0f) / Camera.main.orthographicSize);

        if (Input.mousePosition.x <= Camera.main.pixelWidth && Input.mousePosition.y <= Camera.main.pixelHeight && Input.mousePosition.x >= 0f && Input.mousePosition.y >= 0f)
        {
            moveDelta = Vector3.zero;
        }
        else
        {
            if (Input.mousePosition.x > Camera.main.pixelWidth && Input.GetAxis("Mouse X") > 0f)
            {
                moveDelta = Vector3.right * currentPanSpeed;
            }
            else if (Input.mousePosition.x < 0f && Input.GetAxis("Mouse X") < 0f)
            {
                moveDelta = Vector3.left * currentPanSpeed;
            }

            if (Input.mousePosition.y > Camera.main.pixelHeight && Input.GetAxis("Mouse Y") > 0f)
            {
                moveDelta = Vector3.up * currentPanSpeed;
            }
            else if (Input.mousePosition.y < 0f && Input.GetAxis("Mouse Y") < 0f)
            {
                moveDelta = Vector3.down * currentPanSpeed;
            }
        }

        if (Input.GetKey(KeyCode.LeftArrow)) {
            moveDelta = Vector3.left * currentPanSpeed;
        }
        if(Input.GetKey(KeyCode.RightArrow))
        {
            moveDelta = Vector3.right * currentPanSpeed;
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            moveDelta = Vector3.up * currentPanSpeed;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            moveDelta = Vector3.down * currentPanSpeed;
        }
        transform.position += moveDelta;
    }
}
