using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;

[System.Serializable]
public class Coordinate
{
	public float x;
	public float y;

	public int origin;
	public string randomized;
}

[System.Serializable]
public class HyperLane
{
	public int to;
	public int length;
}

[System.Serializable]
public class GalacticObject
{
	public Coordinate coordinate;
	public string type;
	public string name;
	public HyperLane[] hyperlane;
}

[System.Serializable]
public class GameState
{
	public GalacticObject[] galactic_object;
}

[System.Serializable]
public class SimpleTest
{
	public object meta;
	public GameState gamestate;
}

[System.Serializable]
public class JSONParent
{
	public SimpleTest test;
}

public class JSONLoad : MonoBehaviour
{
	[Header( "Variables" )]
	public float SystemScale = 1;
	public float BaseHeight = 0.5f;

	[Header( "Assets" )]
	public GameObject SystemPrefab;

	private List<GameObject> Systems = new List<GameObject>();

	void Start()
	{
		string path = "Assets/Resources/JSON/tiny.json";

		//Read the text from directly from the test.txt file
		StreamReader reader = new StreamReader(path);
		string jsonObj = reader.ReadToEnd();
		Debug.Log( jsonObj );
		reader.Close();

		//SimpleTest glyphMap = JsonUtility.FromJson<SimpleTest>(jsonObj);
		dynamic test = JsonConvert.DeserializeObject( jsonObj );

		// First pass to get all systems with positions
		int system = 0;
		while ( test.gamestate.galactic_object[system.ToString()] != null )
		{
			//print( system + " " + test.gamestate.galactic_object[system.ToString()].coordinate.x + " " + test.gamestate.galactic_object[system.ToString()].coordinate.y );
			float x = test.gamestate.galactic_object[system.ToString()].coordinate.x;
			float y = test.gamestate.galactic_object[system.ToString()].coordinate.y;
			GameObject obj = Instantiate( SystemPrefab, transform );
			obj.transform.localPosition = new Vector3( x, BaseHeight + Random.Range( 0.1f, 0.4f ), y ) * SystemScale;
			Systems.Add( obj );

			system++;
		}

		// Second pass for all hyperlanes
		system = 0;
		while ( test.gamestate.galactic_object[system.ToString()] != null )
		{
			if ( test.gamestate.galactic_object[system.ToString()].hyperlane != null )
			{
				foreach ( var hyperlane in test.gamestate.galactic_object[system.ToString()].hyperlane )
				{
					CreateHyperlane( system, hyperlane.to.ToObject<int>() );
				}
			}

			system++;
		}
	}

	private void CreateHyperlane( int from, int to )
	{
		var go = new GameObject( "Hyperlane" );
		var lr = go.AddComponent<LineRenderer>();
		lr.widthMultiplier = 0.01f;
		lr.startColor = Color.cyan;
		lr.endColor = Color.cyan;
		lr.SetPosition( 0, Systems[from].transform.position );
		lr.SetPosition( 1, Systems[to].transform.position );
		go.transform.SetParent( Systems[from].transform );
	}
}
