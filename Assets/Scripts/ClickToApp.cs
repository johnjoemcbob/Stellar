#define VR

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using VRTK;

public class ClickToApp : MonoBehaviour
{
	public bool AllowHands = true;

	private bool Hovering = false;
	private RawImage ScreenStream;
	private GameObject HoveredObject;

	void Start()
	{
		ScreenStream = transform.parent.GetComponentInChildren<RawImage>();

#if !VR
		VRInputModule.Instance.enabled = false;
		VRInputModule.Instance.gameObject.GetComponent<StandaloneInputModule>().enabled = true;
		GetComponentInParent<Canvas>().worldCamera = null;
#endif
	}

	void Update()
	{
		if ( Hovering && VRInputModule.Instance.GetData().pointerPress == null )
		{
			SendAHKMouse();
		}
	}

	float last = 0;
	private void OnTriggerStay( Collider other )
	{
		if ( !AllowHands && !other.name.Contains( "Ship" ) ) return;
		if ( last > Time.time ) return;
		last = Time.time + 0.1f;

		// Get the correct derive transform
		Transform trans = other.transform;
		if ( !AllowHands )
		{
			trans = other.GetComponentInParent<ShipPiece>().transform;
		}

		// TODO could convert to store lastpos of all trigger stay objects for the broom pusher thing
		// But for now just only update mouse pos if object is hand or grabbed
		if ( AllowHands || other.GetComponentInParent<VRTK_InteractableObject>().IsGrabbed() )
		{
			Rect rect = ( transform as RectTransform ).rect;

			// Move mouse
			Vector3 pos = transform.InverseTransformPoint( trans.position );
			pos = new Vector3( pos.x + rect.width, -pos.y, 0 );
			Vector2 point = new Vector2( pos.x, pos.y );// GetLocalPos( pos );
			{
				// Normalize
				point.x /= rect.width;
				point.y /= rect.height;
				//GetComponentInChildren<Text>().text = "Normalized; " + point;

				// Transform relative to image UVs
				float wid = ScreenStream.uvRect.width;
				float hei = ScreenStream.uvRect.height;
				point.x *= wid;
				point.y *= hei;
				point.x += ScreenStream.uvRect.x;
				point.y += 1 - ScreenStream.uvRect.y - hei;

				// Back to coordinates
				point.x *= Screen.currentResolution.width;
				point.y *= Screen.currentResolution.height;
			}
			SendAHKMouse( point, false );
			HoveredObject = other.gameObject;
		}
	}

	public void ButtonClick()
	{
		// Start AHK with command line parameters as the click pos
		SendAHKMouse( true );
		Debug.Log( HoveredObject );
		if ( HoveredObject != null && HoveredObject.GetComponentInParent<ShipPiece>() != null )
		{
			HoveredObject.GetComponentInParent<ShipPiece>().OnClick();
		}
	}

	public void HoverEnter()
	{
		Hovering = true;
	}

	public void HoverExit()
	{
		Hovering = false;
	}

	protected Vector2 GetLocalPos( Vector2 point )
	{
		{
			Rect rect = ( transform as RectTransform ).rect;
			point.x += rect.width;
			point.y *= -1;

			// Normalize
			point.x /= rect.width;
			point.y /= rect.height;

			// Transform relative to image UVs
			float wid = ScreenStream.uvRect.width;
			float hei = ScreenStream.uvRect.height;
			point.x *= wid;
			point.y *= hei;
			point.x += ScreenStream.uvRect.x;
			point.y += 1 - ScreenStream.uvRect.y - hei;

			// Back to coordinates
			point.x *= Screen.currentResolution.width;
			point.y *= Screen.currentResolution.height;
		}
		return point;
	}

	protected void SendAHKMouse( bool click = false )
	{
		Vector2 point = new Vector2();
		Vector2 screenpoint = Input.mousePosition;
#if VR
		screenpoint = RectTransformUtility.WorldToScreenPoint( Camera.main, Pointer.Instance.Dot.transform.position );
#endif
		RectTransformUtility.ScreenPointToLocalPointInRectangle( transform as RectTransform, screenpoint, Camera.main, out point );
		point = GetLocalPos( point );
		SendAHKMouse( point, click );
	}

	protected void SendAHKMouse( Vector3 point, bool click = false )
	{
		System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
		{
			startInfo.FileName = Application.streamingAssetsPath + "/mouse.ahk";
			startInfo.Arguments = point.x + " " + point.y + " " + ( click ? "11" : "" );
		}
		System.Diagnostics.Process.Start( startInfo );
	}
}
