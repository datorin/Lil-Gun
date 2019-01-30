using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunTriggerController : MonoBehaviour {

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.CompareTag(Values.PlayerTag))
		{
			if (transform.parent.parent == null &&
			    other.GetComponent<PlayerController>().Gun == null)
			{
				Destroy(transform.parent.GetComponent<Rigidbody2D>());
				transform.parent.SetParent(other.transform);
				transform.parent.localPosition = Vector3.zero;
				transform.parent.localRotation = Quaternion.identity;
				
				other.GetComponent<PlayerController>().Gun = transform.parent.gameObject;

				gameObject.SetActive(false);
			}
		}
	}
}
