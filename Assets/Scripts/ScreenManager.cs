using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenManager : MonoBehaviour
{
	protected List<ScreenSegment> Segments = new List<ScreenSegment>();

	void Start()
	{
		// Find all ScreenSegments
		Segments.AddRange( GetComponentsInChildren<ScreenSegment>() );

		//StartCoroutine( IEnumUpdate() );
	}

	//IEnumerator IEnumUpdate()
	//{
	//	while ( true )
	//	{
	//		// Cycle through constantly to next highest priority screen
	//		Segments.Sort( ( x, y ) => x.CurrentPriority.CompareTo( y.CurrentPriority ) );

	//		Debug.Log( "Update; " + Segments[Segments.Count-1].name );
	//		//yield return StartCoroutine( Segments[Segments.Count-1].UpdateTexture() );

	//		// Safety while testing
	//		yield return new WaitForSeconds( 2 );
	//	}
	//}
}
