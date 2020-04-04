using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

[Serializable]
public class SegmentSize
{
	public Vector2 Min;
	public Vector2 Max;
	public float Scale;
}

public class TabletManager : MonoBehaviour
{
	[Header( "Variables" )]
	public string Key = "";
	public bool Lerps = false;
	public float LerpSpeed = 5;
	public SegmentSize MinSize;
	public SegmentSize MaxSize;
	public SegmentSize ExtraSize;

	[Header( "References" )]
	public Transform HandleTop;
	public Transform HandleBottom;
	public RectTransform Canvas;

	// Gotta assume all are closed by default
	private bool Open = false;
	private bool Extended = false;
	private bool ExtraPanel = false;
	private bool Grabbed = false;
	private ScreenSegment Segment;
	private GameObject LastSound;
	private float CoroutineRunning = 0;
	private float CoroutineDelay = 0.3f;
	

	#region MonoBehaviour
	private void Start()
	{
		// VRTK input here
		var interact = GetComponent<VRTK_InteractableObject>();
		interact.InteractableObjectTouched += OnTouch;
		interact.InteractableObjectUntouched += OnUnTouch;
		interact.InteractableObjectUsed += OnUse;

		Segment = GetComponentInChildren<ScreenSegment>();
	}

	private void Update()
	{
		if ( Lerps )
		{
			// Lerping size
			SegmentSize target = MinSize;
			if ( Extended ) target = MaxSize;
			if ( ExtraPanel ) target = ExtraSize;

			// Display/screen segment
			Segment.Min = Vector2.Lerp( Segment.Min, target.Min, Time.deltaTime * LerpSpeed );
			Segment.Max = Vector2.Lerp( Segment.Max, target.Max, Time.deltaTime * LerpSpeed );
			Segment.Scale = Mathf.Lerp( Segment.Scale, target.Scale, Time.deltaTime * LerpSpeed );
			Segment.UpdateTriggerSize();

			// Handles
			HandleTop.localPosition = Vector3.zero;
			HandleBottom.localPosition = new Vector3( 0, -Canvas.sizeDelta.y * Canvas.localScale.y, -Canvas.sizeDelta.x * Canvas.localScale.x );
			//Vector3.Lerp( HandleBottom.localPosition, targetpos, Time.deltaTime * LerpSpeed );
		}
		
		// Input
		if ( Grabbed )
		{
		}
	}
	#endregion

	#region Trigger - Update View
	private void OnTriggerEnter( Collider other )
	{
		if ( Open ) return;
		Debug.Log( "ENTER " + other.name );

		Open = true;

		StartCoroutine( DelayedEnter() );
	}

	private void OnTriggerExit( Collider other )
	{
		if ( !Open ) return;
		Debug.Log( "EXIT " + other.name );

		Open = false;

		StartCoroutine( DelayedExit() );
	}

	private IEnumerator DelayedEnter()
	{
		// Quick fix to avoid running multiple coroutines overlapping?
		float wait = CoroutineRunning - Time.time;
		if ( wait > 0 )
		{
			yield return new WaitForSeconds( wait );
		}
		CoroutineRunning = Time.time + CoroutineDelay + 0.1f;

		if ( Open )
		{
			AHK.Run( "key", Key );
		}

		yield return new WaitForSeconds( CoroutineDelay );

		if ( Open )
		{
			GetComponentInChildren<CopyTexture>().UnlockFrame();
		}
	}

	private IEnumerator DelayedExit()
	{
		// Quick fix to avoid running multiple coroutines overlapping?
		float wait = CoroutineRunning - Time.time;
		if ( wait > 0 )
		{
			yield return new WaitForSeconds( wait );
		}
		CoroutineRunning = Time.time + CoroutineDelay + 0.1f;

		if ( !Open )
		{
			GetComponentInChildren<CopyTexture>().LockFrame();
		}

		yield return new WaitForSeconds( CoroutineDelay );

		if ( !Open )
		{
			AHK.Run( "key", Key );
		}
	}
	#endregion

	#region VRTK - Grabs/Use
	public void OnUse( object sender, InteractableObjectEventArgs e )
	{
		AHK.Run( "mouse_click" );
	}

	public void OnSecondaryUse( object sender, ControllerInteractionEventArgs e )
	{
		Extended = !Extended;

		if ( LastSound != null )
		{
			Destroy( LastSound );
		}
		if ( Extended )
		{
			LastSound = StaticHelpers.SpawnResourceAudioSource( "open", transform.position, 1, 0.2f );
		}
		else
		{
			LastSound = StaticHelpers.SpawnResourceAudioSource( "close", transform.position, 1, 0.2f );
		}
	}

	// TODO refactor and try to make automatic?
	public void OnOpenExtraPanel( object sender, ControllerInteractionEventArgs e )
	{
		ExtraPanel = !ExtraPanel;
	}

	public void OnTouch( object sender, InteractableObjectEventArgs e )
	{
		Grabbed = true;
		e.interactingObject.GetComponent<VRTK_ControllerEvents>().ButtonOnePressed += OnSecondaryUse;
		e.interactingObject.GetComponent<VRTK_ControllerEvents>().ButtonTwoPressed += OnOpenExtraPanel;
	}

	public void OnUnTouch( object sender, InteractableObjectEventArgs e )
	{
		Grabbed = false;
		e.interactingObject.GetComponent<VRTK_ControllerEvents>().ButtonOnePressed -= OnSecondaryUse;
		e.interactingObject.GetComponent<VRTK_ControllerEvents>().ButtonTwoPressed -= OnOpenExtraPanel;
	}
	#endregion
}
