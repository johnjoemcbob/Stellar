using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Valve.VR;

public class VRInputModule : BaseInputModule
{
	public static VRInputModule Instance;

	public Camera Camera;
	//public SteamVR_Input_Sources TargetSource;
	//public SteamVR_Action_Boolean ClickAction;

	private GameObject CurrentObject = null;
	private PointerEventData Data = null;

	protected override void Awake()
	{
		base.Awake();

		Instance = this;

		Data = new PointerEventData( eventSystem );
	}

	public override void Process()
	{
		// Reset data
		Data.Reset();

		// Set camera
		Data.position = new Vector2( Camera.pixelWidth / 2, Camera.pixelHeight / 2 );

		// Raycast
		eventSystem.RaycastAll( Data, m_RaycastResultCache );
		Data.pointerCurrentRaycast = FindFirstRaycast( m_RaycastResultCache );
		CurrentObject = Data.pointerCurrentRaycast.gameObject;

		// Clear raycast
		m_RaycastResultCache.Clear();

		// Hover states
		HandlePointerExitAndEnter( Data, CurrentObject );

		// Press
		if ( input.GetMouseButtonDown( 0 ) )
		{
			ProcessPress( Data );
		}

		// Release
		//if ( ClickAction.GetStateUp( TargetSource ) )
		//{
		//	ProcessRelease( Data );
		//}
	}

	public PointerEventData GetData()
	{
		return Data;
	}

	private void ProcessPress( PointerEventData data )
	{
		// Set raycast
		data.pointerPressRaycast = data.pointerCurrentRaycast;

		// Get the down handler, call it
		GameObject pointerpress = ExecuteEvents.ExecuteHierarchy( CurrentObject, data, ExecuteEvents.pointerDownHandler );

		// If no down handler, go for click
		if ( pointerpress == null )
		{
			pointerpress = ExecuteEvents.GetEventHandler<IPointerClickHandler>( CurrentObject );
		}

		// Set data
		data.pressPosition = data.position;
		data.pointerPress = pointerpress;
		data.rawPointerPress = CurrentObject;
	}

	private void ProcessRelease( PointerEventData data )
	{
		// Set raycast
		//data.pointerPressRaycast = data.pointerCurrentRaycast;

		// Get the up handler, call it
		ExecuteEvents.ExecuteHierarchy( CurrentObject, data, ExecuteEvents.pointerUpHandler );

		// Check for click handler
		GameObject pointerup = ExecuteEvents.GetEventHandler<IPointerClickHandler>( CurrentObject );
		
		// Check if same as down
		if ( data.pointerPress == pointerup )
		{
			ExecuteEvents.Execute( data.pointerPress, data, ExecuteEvents.pointerClickHandler );
		}

		// Clear selected gameobject
		eventSystem.SetSelectedGameObject( null );

		// Reset data
		data.pressPosition = Vector2.zero;
		data.pointerPress = null;
		data.rawPointerPress = null;
	}
}
