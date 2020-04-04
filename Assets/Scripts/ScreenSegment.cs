using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class ScreenSegment : MonoBehaviour
{
	public Vector2 Min;
	public Vector2 Max;
	public float Scale = 1;
	public float UpdateFrequency = 0;
	public float WaitTimeBefore = 0.8f;
	public float WaitTimeAfter = 0.01f;
	public string Key = "";
	public string OverrideScript = "";

	[HideInInspector]
	public float CurrentPriority = 0;

	protected RectTransform RectTrans;
	protected RawImage Image;
	protected CopyTexture Copy;

	void Awake()
	{
		RectTrans = GetComponent<RectTransform>();
		Image = GetComponentInChildren<RawImage>();
		Copy = GetComponentInChildren<CopyTexture>();
		Copy.enabled = true;

		//StartCoroutine( UpdateTexture() );

		UpdateTriggerSize();
	}

	void Update()
	{
		float width = Max.x - Min.x;
		float height = Max.y - Min.y;

		// Canvas
		RectTrans.sizeDelta = new Vector2( Screen.currentResolution.width * width, Screen.currentResolution.height * height );
		RectTrans.localScale = Vector3.one / Screen.currentResolution.width * Scale;

		// Image
		Rect rect = Image.uvRect;
		{
			rect.x = Min.x;
			rect.y = Min.y;
			rect.width = Max.x - Min.x;
			rect.height = Max.y - Min.y;
		}
		Image.uvRect = rect;

		CurrentPriority += Time.deltaTime;// / UpdateFrequency;
	}

	IEnumerator UpdateTexture()
	{
		while ( true )
		{
			yield return new WaitForSeconds( UpdateFrequency );

			// Update
			yield return new WaitForSeconds( WaitTimeBefore ); // Wait for correct input
			Copy.enabled = true;
			PressAHKKey();
			yield return new WaitForEndOfFrame();
			yield return new WaitForEndOfFrame();
			yield return new WaitForEndOfFrame();
			yield return new WaitForEndOfFrame();
			Copy.enabled = false;
		}
	}

	//public IEnumerator UpdateTexture()
	//{
	//	{
	//		float wait = PressAHKKey();
	//		yield return new WaitForSeconds( wait ); // Wait for correct input
	//		Copy.enabled = true;
	//		yield return new WaitForSeconds( 0.01f ); // Wait while captures
	//		Copy.enabled = false;
	//	}

	//	CurrentPriority = 0;
	//}

	public void UpdateTriggerSize()
	{
		// Create Trigger Collider
		if ( gameObject.GetComponent<BoxCollider>() == null )
		{
			gameObject.AddComponent<BoxCollider>();
		}
		var recttrans = GetComponent<RectTransform>();
		int dir = 1;
		foreach ( var collider in GetComponentsInChildren<BoxCollider>() )
		{
			collider.isTrigger = true;
			collider.size = new Vector3( recttrans.sizeDelta.x, recttrans.sizeDelta.y, collider.size.z );
			collider.center = new Vector3( dir * collider.size.x / 2, -collider.size.y / 2, 0 );
			dir = -1;
		}
	}

	public float PressAHKKey()
	{
		if ( Key == "" && OverrideScript == "" ) return 0;

		System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
		{
			string program = OverrideScript != "" ? OverrideScript : "key";
			Debug.Log( program );
			startInfo.FileName = Application.streamingAssetsPath + "/" + program + ".ahk";
			startInfo.Arguments = Key;
		}
		System.Diagnostics.Process.Start( startInfo );

		return WaitTimeBefore;
	}
}
