using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Pointer : MonoBehaviour
{
	public static Pointer Instance;

	public float DefaultLength = 5;
	public GameObject Dot;

	private LineRenderer LineRenderer;

	void Awake()
	{
		Instance = this;
		LineRenderer = GetComponent<LineRenderer>();
	}

	void Update()
	{
		UpdateLine();
	}

	private void UpdateLine()
	{
		// Use default or distance
		PointerEventData data = VRInputModule.Instance.GetData();
		float targetlength = data.pointerCurrentRaycast.distance == 0 ? DefaultLength : data.pointerCurrentRaycast.distance;

		// Raycast
		RaycastHit hit = CreateRaycast( targetlength );

		// Default end
		Vector3 endpos = transform.position + transform.forward * targetlength;

		// Or based on hit
		if ( hit.collider != null )
		{
			endpos = hit.point;
		}

		// Position of dot
		Dot.transform.position = endpos;

		// Position of line renderer
		LineRenderer.SetPosition( 0, transform.position );
		LineRenderer.SetPosition( 1, endpos );
	}

	private RaycastHit CreateRaycast( float length )
	{
		RaycastHit hit = new RaycastHit();
		{
			Ray ray = new Ray( transform.position, transform.forward );
			Physics.Raycast( ray, out hit, DefaultLength );
		}
		return hit;
	}
}
