#define VR

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ClickToApp : MonoBehaviour
{
	private bool Hovering = false;
	private RawImage ScreenStream;

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

	public void ButtonClick()
	{
		// Start AHK with command line parameters as the click pos
		SendAHKMouse( true );
	}

	public void HoverEnter()
	{
		Hovering = true;
	}

	public void HoverExit()
	{
		Hovering = false;
	}

	protected Vector2 GetLocalPos()
	{
		Vector2 point;
		{
			Vector2 screenpoint = Input.mousePosition;
#if VR
				screenpoint = RectTransformUtility.WorldToScreenPoint( Camera.main, Pointer.Instance.Dot.transform.position );
#endif
			RectTransformUtility.ScreenPointToLocalPointInRectangle( transform as RectTransform, screenpoint, Camera.main, out point );
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
		Vector2 point = GetLocalPos();
		System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
		{
			startInfo.FileName = Application.streamingAssetsPath + "/mouse.ahk";
			startInfo.Arguments = point.x + " " + point.y + " " + ( click ? "11" : "" );
		}
		System.Diagnostics.Process.Start( startInfo );
	}
}
