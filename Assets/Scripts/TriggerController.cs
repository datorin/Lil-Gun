using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerController : MonoBehaviour
{

	public bool IsGround;
	public bool IsEnemy;

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.CompareTag("Ground"))
		{
			IsGround = true;
		}
		
		if (other.gameObject.CompareTag("Enemy"))
		{
			IsEnemy = true;
		}
	}
	
	private void OnTriggerExit2D(Collider2D other)
	{
		if (other.gameObject.CompareTag("Ground"))
		{
			IsGround = false;
		}
		
		if (other.gameObject.CompareTag("Enemy"))
		{
			IsEnemy = false;
		}
	}
}
