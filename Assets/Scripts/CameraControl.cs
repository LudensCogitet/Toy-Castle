using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CameraControl : MonoBehaviour {

    public GameObject sendEmailUI;
    public GameObject otherUI;

    public float panSpeed = 0.15f;
    Vector3 moveDelta = Vector3.zero;

    Vector3 savedTrashCanPos;
    Vector3 savedDupPos;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        float currentPanSpeed = panSpeed;


        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            currentPanSpeed = panSpeed / 4;
        float worldToPixels = ((Screen.height / 2.0f) / Camera.main.orthographicSize);

        if (Input.GetMouseButton(2))
        {
            moveDelta = new Vector3(-Input.GetAxis("Mouse X") * currentPanSpeed, -Input.GetAxis("Mouse Y") * currentPanSpeed, 0f);
        }
        else
        {
            moveDelta = Vector3.zero;
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            DisableUI();
            Application.CaptureScreenshot(Application.persistentDataPath + "Screenshot.png");
            Invoke("DelayScreenCapRecovery", 1f);
        }

        /*if (Input.mousePosition.x < Camera.main.pixelWidth && Input.mousePosition.y < Camera.main.pixelHeight && Input.mousePosition.x > 0f && Input.mousePosition.y > 0f)
        {
            moveDelta = Vector3.zero;
        }
        else
        {
            if (Input.mousePosition.x >= Camera.main.pixelWidth && Input.GetAxis("Mouse X") > 0f)
            {
                moveDelta = Vector3.right * currentPanSpeed;
            }
            else if (Input.mousePosition.x <= 0f && Input.GetAxis("Mouse X") <= 0f)
            {
                moveDelta = Vector3.left * currentPanSpeed;
            }

            if (Input.mousePosition.y >= Camera.main.pixelHeight && Input.GetAxis("Mouse Y") > 0f)
            {
                moveDelta = Vector3.up * currentPanSpeed;
            }
            else if (Input.mousePosition.y <= 0f && Input.GetAxis("Mouse Y") < 0f)
            {
                moveDelta = Vector3.down * currentPanSpeed;
            }
        }*/

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

    void DelayScreenCapRecovery()
    {

        EnableUI();
        sendEmailUI.SetActive(true);
        sendEmailUI.GetComponentInChildren<InputField>().Select();
        sendEmailUI.GetComponentInChildren<InputField>().ActivateInputField();

    }

    void DisableUI()
    {
        MouseHandler mouseHandler = FindObjectOfType<MouseHandler>();

        mouseHandler.trashCan.SetActive(false);  //GetComponent<SpriteRenderer>().enabled = false;
        if (mouseHandler.currentDup)
            mouseHandler.currentDup.SetActive(false);//GetComponent<SpriteRenderer>().enabled = false;
        otherUI.SetActive(false);
    }
    
    void EnableUI()
    {
        MouseHandler mouseHandler = FindObjectOfType<MouseHandler>();

        mouseHandler.trashCan.SetActive(true); //GetComponent<SpriteRenderer>().enabled = true;
        if (mouseHandler.currentDup)
            mouseHandler.currentDup.SetActive(true);
        otherUI.SetActive(true);
    }

}
