using Sandbox;
using Sandbox.Engine;

public sealed class AnimateVRHand : BaseComponent
{
	[Property] TrackedPoseComponent.PoseSources HandSide { get; set; }

	AnimatedModelComponent anim { get; set; }

	public override void OnAwake()
	{
		anim = GetComponent<AnimatedModelComponent>( false );
	}

	public Input.VrHand TranslateHandSide()
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
		anim.Set( "Index", TranslateHandSide().GetFingerCurl( (int)FingerValue.IndexCurl ) );
		anim.Set( "Middle", TranslateHandSide().GetFingerCurl( (int)FingerValue.MiddleCurl ) );
		anim.Set( "Ring", TranslateHandSide().GetFingerCurl( (int)FingerValue.RingCurl ) );
		anim.Set( "Pinky", TranslateHandSide().GetFingerCurl( (int)FingerValue.PinkyCurl ) );
		anim.Set( "Thumb", TranslateHandSide().GetFingerCurl( (int)FingerValue.ThumbCurl ) );
	}
}
