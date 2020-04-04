using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CopyTexture : MonoBehaviour
{
	protected RawImage Source;
	protected RawImage Target;
	protected Texture LockOverwrite;

	void Awake()
	{
		Source = FindObjectOfType<Webcam>().GetComponent<RawImage>();

		Target = GetComponent<RawImage>();
	}

	private void Update()
	{
		Target.texture = ( LockOverwrite!= null ) ? LockOverwrite : Source.texture;
	}

	// TODO this isn't implemented properly
	public void LockFrame()
	{
		if ( LockOverwrite == null )
		{
			LockOverwrite = new Texture2D( Source.texture.width, Source.texture.height, TextureFormat.ARGB32, false );
		}
		Graphics.CopyTexture( Source.texture, LockOverwrite );
	}

	public void UnlockFrame()
	{
		LockOverwrite = null;
	}
}
