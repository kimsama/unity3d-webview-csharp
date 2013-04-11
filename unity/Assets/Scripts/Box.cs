using UnityEngine;
using System.Collections;

public class Box : MonoBehaviour 
{
	// Update is called once per frame
	void Update () 
	{
		if (transform.position.y < -2.0) Destroy(gameObject);
	}
}
