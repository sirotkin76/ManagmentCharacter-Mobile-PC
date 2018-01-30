using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagmentMenu : MonoBehaviour {

	// Update is called once per frame
	void FixedUpdate () {
		if (Input.GetKey(KeyCode.Escape)) Application.Quit();
	}
	
}
