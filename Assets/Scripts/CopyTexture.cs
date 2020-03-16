using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CopyTexture : MonoBehaviour
{
	public RawImage Source;

	protected RawImage Target;
	protected Texture Texture;

	void Awake()
	{
		Target = GetComponent<RawImage>();
	}

	void Update()
	{
		if ( Texture == null )
		{
			Texture = new Texture2D( Source.texture.width, Source.texture.height, TextureFormat.ARGB32, false );
		}
		Graphics.CopyTexture( Source.texture, Texture );
		Target.texture = Texture;
	}
}
