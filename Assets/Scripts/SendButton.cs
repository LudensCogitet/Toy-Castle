using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SendButton : MonoBehaviour {

    public InputField emailAddress;
    public GameObject parent;
    public EventSystem eventSystem;

	// Use this for initialization
	void Start () {
	   
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0) && !eventSystem.IsPointerOverGameObject())
        {
            parent.SetActive(false);
        }

	}

    public void sendScreenshot()
    {
        mono_gmail.Send(emailAddress.text);
        parent.SetActive(false);
    }
}
