using UnityEngine;
using UnityEngine.UI;
using VRTK;
using VRTK.Controllables;

public class RecenterButton : MonoBehaviour
{
	protected VRTK_BaseControllable controllable;

	protected virtual void OnEnable()
	{
		controllable = ( controllable == null ? GetComponent<VRTK_BaseControllable>() : controllable );
		controllable.ValueChanged += ValueChanged;
		controllable.MaxLimitReached += MaxLimitReached;
		controllable.MinLimitReached += MinLimitReached;
	}

	protected virtual void ValueChanged( object sender, ControllableEventArgs e )
	{
		
	}

	protected virtual void MaxLimitReached( object sender, ControllableEventArgs e )
	{
		AHK.Run( "resetview" );
		StaticHelpers.SpawnResourceAudioSource( "confirm", transform.position, 1, 0.7f );
	}

	protected virtual void MinLimitReached( object sender, ControllableEventArgs e )
	{
		
	}
}