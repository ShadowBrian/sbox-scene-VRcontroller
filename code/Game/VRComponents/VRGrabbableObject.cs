using Sandbox;
using Sandbox.Engine;

public sealed class VRGrabbableObject : BaseComponent
{
	[Property] public GameObject HandposeObject { get; set; }

	public float[] curlClamps = new float[5] { 0.5f, 0.5f, 0.5f, 0.5f, 0.5f };

	public override void OnAwake()
	{
		if ( HandposeObject.IsValid() )
		{
			HandposeObject.GetComponent<ModelComponent>().Enabled = false;
		}
	}

	public Input.VrHand TranslateHandSide( TrackedPoseComponent.PoseSources HandSide )
	{
		switch ( HandSide )
		{
			case TrackedPoseComponent.PoseSources.None:
				return Input.VR.RightHand;
			case TrackedPoseComponent.PoseSources.Head:
				return Input.VR.RightHand;
			case TrackedPoseComponent.PoseSources.LeftHand:
				return Input.VR.LeftHand;
			case TrackedPoseComponent.PoseSources.RightHand:
				return Input.VR.RightHand;
			default:
				return Input.VR.RightHand;
		}
	}

	public override void Update()
	{

	}
}
