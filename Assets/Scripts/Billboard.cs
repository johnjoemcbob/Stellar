using UnityEngine;
using System.Collections;

public class Billboard : MonoBehaviour
{
	void Update()
	{
		if ( Camera.current == null ) return;

		transform.LookAt( Camera.current.transform );
		transform.localEulerAngles = new Vector3( 0, transform.localEulerAngles.y, 0 );
	}
}