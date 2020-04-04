using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AHK
{
    public static void Run( string program, string args = "" )
	{
		System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
		{
			startInfo.FileName = Application.streamingAssetsPath + "/" + program + ".ahk";
			startInfo.Arguments = args;
		}
		System.Diagnostics.Process.Start( startInfo );
	}
}
