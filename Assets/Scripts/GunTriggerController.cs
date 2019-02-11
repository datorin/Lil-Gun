using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunTriggerController : MonoBehaviour {

	private void OnTriggerEnter2D(Collider2D other)
	{
		var gun = transform.parent;
		
		if (other.gameObject.CompareTag(Values.PlayerTag))
		{
			if (gun.parent == null &&
			    other.GetComponent<PlayerController>().Gun == null)
			{
				Destroy(gun.GetComponent<Rigidbody2D>());
				gun.GetComponent<SpriteRenderer>().enabled = false;
				gun.GetComponent<BoxCollider2D>().enabled = false;
				gun.SetParent(other.transform);
				gun.localPosition = new Vector3(-0.03125f, -0.125f, 0);
				gun.localRotation = Quaternion.identity;
				
				other.GetComponent<PlayerController>().Gun = gun.gameObject;
				other.GetComponent<Animator>().SetBool("hasGun",true);

				gameObject.SetActive(false);
			}
		}
	}
}
