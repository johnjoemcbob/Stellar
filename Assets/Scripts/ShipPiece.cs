using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class ShipPiece : MonoBehaviour
{
	[Header( "Variables" )]
	public string Key = "";

	[Header( "Assets" )]
	public GameObject ParticlePrefab;

	private bool Grabbed = false;

	private void OnEnable()
	{
		GetComponent<Rigidbody>().centerOfMass = new Vector3( 0, -1, 0 );

		// VRTK input here
		var interact = GetComponent<VRTK_InteractableObject>();
		interact.InteractableObjectGrabbed += OnGrab;
		interact.InteractableObjectUngrabbed += OnUnGrab;
	}

	//private void OnCollisionEnter( Collision collision )
	public void OnClick()
	{
		// TODO Keep track of last trigger entered, to send the current position to click at


		// Spawn particle
		GameObject part = Instantiate( ParticlePrefab, transform );
			part.transform.localPosition = new Vector3( 0, 0.01f, 0 );
			part.transform.localEulerAngles = Vector3.zero;
		Destroy( part, 1.1f );
		StaticHelpers.SpawnResourceAudioSource( "confirm", transform.position, 1, 0.7f );
	}

	public void OnGrab( object sender, InteractableObjectEventArgs e )
	{
		Grabbed = true;

		RunProgram( "key" );

		StartCoroutine( DelayedSelect() );
	}

	public void OnUnGrab( object sender, InteractableObjectEventArgs e )
	{
		Grabbed = false;

		GetComponentInChildren<CopyTexture>().LockFrame();
		RunProgram( "click" );

		StartCoroutine( DelayedDeselect() );
	}

	public void RunProgram( string program, string overwrite = "" )
	{
		System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
		{
			startInfo.FileName = Application.streamingAssetsPath + "/" + program + ".ahk";
			startInfo.Arguments = string.IsNullOrEmpty( overwrite ) ? Key : overwrite;
		}
		System.Diagnostics.Process.Start( startInfo );
	}

	private IEnumerator DelayedSelect()
	{
		yield return new WaitForSeconds( 0.3f );

		if ( Grabbed )
		{
			GetComponentInChildren<CopyTexture>().UnlockFrame();
		}
	}

	private IEnumerator DelayedDeselect()
	{
		yield return new WaitForSeconds( 0.1f );

		if ( !Grabbed )
		{
			RunProgram( "key", "Esc" );
		}
	}
}
