using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Webcam : MonoBehaviour
{
	void Start()
	{
		Application.runInBackground = true;

		RawImage rawimage = GetComponent<RawImage>();
		WebCamTexture webcamTexture = new WebCamTexture( "OBS-Camera" );
		rawimage.texture = webcamTexture;
		rawimage.material.mainTexture = webcamTexture;
		webcamTexture.Play();
	}
}
