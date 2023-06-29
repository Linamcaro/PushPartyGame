using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bounce : MonoBehaviour
{
	private float force = 500f; //Force 10000f
	public float stunTime = 0.5f;
	private Vector3 hitDir;

	void OnCollisionEnter(Collision collision)
	{
		foreach (ContactPoint contact in collision.contacts)
		{
			
			if (collision.gameObject.tag == "Player")
			{	
				hitDir = contact.normal;
				collision.gameObject.GetComponent<PlayerMovement>().HitPlayer(-hitDir * force, stunTime);
				return;
			}
		}

	}
}
