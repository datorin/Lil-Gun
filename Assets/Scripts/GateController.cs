using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if(!other.transform.CompareTag(Values.PlayerTag)) return;
		transform.parent.GetComponent<Animator>().SetBool("isOpen", true);
		transform.parent.GetComponent<BoxCollider2D>().enabled = false;
	}
	
	private void OnTriggerExit2D(Collider2D other)
	{
		if(!other.transform.CompareTag(Values.PlayerTag)) return;
		
		transform.parent.GetComponent<Animator>().SetBool("isOpen", false);
		transform.parent.GetComponent<BoxCollider2D>().enabled = true;

		var camera = Camera.main.gameObject;
		if (other.transform.position.x > transform.position.x && transform.position.x > camera.transform.position.x)
		{
			camera.transform.position += new Vector3(32,0,0);
		}else if (other.transform.position.x < transform.position.x &&
		          transform.position.x < camera.transform.position.x)
		{
			camera.transform.position -= new Vector3(32,0,0);
		}
	}
}
