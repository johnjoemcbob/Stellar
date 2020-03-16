using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[System.Serializable]
public class SimpleTest
{
	public object galaxy;
	public object stars;
	public object planets;
	public object empires;
	public object alliances;
	public object independencies;
}

public class JSONLoad : MonoBehaviour
{
	void Start()
	{
		string path = "Assets/Resources/JSON/simple.sav";

		//Read the text from directly from the test.txt file
		StreamReader reader = new StreamReader(path);
		string jsonObj = reader.ReadToEnd();
		Debug.Log( jsonObj );
		reader.Close();

		SimpleTest glyphMap = JsonUtility.FromJson<SimpleTest>(jsonObj);
		print( glyphMap.galaxy );
	}
}
