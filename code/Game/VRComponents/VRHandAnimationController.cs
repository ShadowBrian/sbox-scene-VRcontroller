using Sandbox;
using Sandbox.Engine;

public sealed class VRHandAnimationController : BaseComponent
{
	[Property] TrackedPoseComponent.PoseSources HandSide { get; set; }

	AnimatedModelComponent anim { get; set; }

	//0=Thumb, 1=Index, 2=Middle, 3=Ring, 4=Pinky

	public float[] curlClamps = new float[5] { 1f, 1f, 1f, 1f, 1f };

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

	public float ClampedFingerCurl( FingerValue finger )
	{
		float rawCurl = TranslateHandSide().GetFingerCurl( (int)finger );
		return MathX.Clamp( rawCurl, 0f, curlClamps[(int)finger] );
	}

	public void SetCurlClamp( FingerValue finger, float clamp )
	{
		curlClamps[(int)finger] = clamp;
	}

	public void ResetCurlClamps()
	{
		curlClamps = new float[5] { 1f, 1f, 1f, 1f, 1f };
	}

	public void SetCurlClamps( float[] clamps )
	{
		if ( curlClamps.Length == clamps.Length )
		{
			curlClamps = clamps;
		}
	}

	public override void Update()
	{
		anim.Set( "Thumb", ClampedFingerCurl( FingerValue.ThumbCurl ) );
		anim.Set( "Index", ClampedFingerCurl( FingerValue.IndexCurl ) );
		anim.Set( "Middle", ClampedFingerCurl( FingerValue.MiddleCurl ) );
		anim.Set( "Ring", ClampedFingerCurl( FingerValue.RingCurl ) );
		anim.Set( "Pinky", ClampedFingerCurl( FingerValue.PinkyCurl ) );
	}
}
