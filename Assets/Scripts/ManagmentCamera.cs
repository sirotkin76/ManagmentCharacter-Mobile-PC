using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagmentCamera : MonoBehaviour {

	static GameObject player;
	// [SerializeField]
	float x;
	float z;

	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag("Player");

		// Разница между камерой и игроком.
		x = transform.position.x - player.transform.position.x;
		z = transform.position.z - player.transform.position.z;
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = new Vector3 (player.transform.position.x + x, this.transform.position.y, player.transform.position.z + z);
	}
}
