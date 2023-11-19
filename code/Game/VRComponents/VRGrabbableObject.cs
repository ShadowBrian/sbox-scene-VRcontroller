using Sandbox;
using Sandbox.Engine;

public sealed class VRGrabbableObject : BaseComponent
{
	[Property] public GameObject HandposeObject { get; set; }

	//0=Thumb, 1=Index, 2=Middle, 3=Ring, 4=Pinky

	public float[] curlClamps = new float[5] { 0.5f, 0.5f, 0.5f, 0.5f, 0.5f };

	public override void OnAwake()
	{
		if ( HandposeObject.IsValid() )
		{
			HandposeObject.GetComponent<AnimatedModelComponent>().Enabled = false;
			var handpose = HandposeObject.GetComponent<VRHandPoseController>( false );
			if ( handpose != null )
			{
				curlClamps[0] = handpose.ThumbClamp;
				curlClamps[1] = handpose.IndexClamp;
				curlClamps[2] = handpose.MiddleClamp;
				curlClamps[3] = handpose.RingClamp;
				curlClamps[4] = handpose.PinkyClamp;
			}
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
