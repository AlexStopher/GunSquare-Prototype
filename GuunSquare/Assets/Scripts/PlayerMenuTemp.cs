using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerMenuTemp : MonoBehaviour {

    public GameObject Panel;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetButtonDown("B") == true || Input.GetMouseButtonDown(0) == true)
            Panel.SetActive(false);
	}
}
