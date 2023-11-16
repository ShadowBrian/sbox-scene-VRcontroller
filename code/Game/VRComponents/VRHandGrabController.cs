using Sandbox;
using Sandbox.Engine;

public sealed class VRHandGrabController : BaseComponent
{
	[Property] TrackedPoseComponent.PoseSources HandSide { get; set; }

	[Property] GameObject HandOffset { get; set; }

	VRHandAnimationController anim { get; set; }

	GameObject HoverObject { get; set; }

	public GameObject HoldingObject { get; set; }

	VRHandGrabController OtherHand { get; set; }


	public override void OnAwake()
	{
		anim = GetComponent<VRHandAnimationController>( false, true );

		OtherHand = GameObject.Parent.GetComponents<VRHandGrabController>( false, true ).Where( X => X != this ).First();
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
		var grabtr = Physics.Trace.Ray( Transform.Position, Transform.Position ).Radius( 8f ).WithoutTags( "player" ).Run();

		if ( grabtr.Hit && grabtr.Body != null && (grabtr.Body.GameObject as GameObject).Tags.Has( "grabbable" ) )
		{
			HoverObject = grabtr.Body.GameObject as GameObject;
		}
		else
		{
			HoverObject = null;
		}

		//Gizmo.Draw.Line( Transform.Position, Transform.Position + (TranslateHandSide().Velocity * Rotation.FromRoll( 90 ) * Rotation.FromYaw( 270 ) * VRLegs.instance.GameObject.Transform.Rotation) * 5f );

		if ( HoverObject != null )
		{
			if ( HoldingObject == null && TranslateHandSide().Grip.Value > 0.5f && OtherHand.HoldingObject != HoverObject )
			{
				HoldingObject = HoverObject;

				var grabobj = HoldingObject.GetComponent<VRGrabbableObject>( false, true );

				if ( grabobj != null )
				{
					anim.SetCurlClamps( grabobj.curlClamps );
				}

				var physobj = HoldingObject.GetComponent<PhysicsComponent>( false, true );

				if ( physobj != null )
				{
					physobj.Gravity = false;
					physobj.Velocity = Vector3.Zero;
					physobj.AngularVelocity = Vector3.Zero;
				}
			}
		}

		if ( HoldingObject != null )
		{
			if ( TranslateHandSide().Grip.Value < 0.5f )
			{
				HoldingObject.Tags.Remove( "player" );
				var physobj = HoldingObject.GetComponent<PhysicsComponent>( false, true );

				if ( physobj != null )
				{
					physobj.Gravity = true;
					physobj.Velocity = TranslateHandSide().Velocity * Rotation.FromRoll( 90 ) * Rotation.FromYaw( 270 ) * VRLegs.instance.GameObject.Transform.Rotation;
					//physobj.AngularVelocity = TranslateHandSide().AngularVelocity.AsVector3();// * VRLegs.instance.GameObject.Transform.Rotation * Rotation.FromYaw( -90f ) * Rotation.FromPitch( 90f );
				}

				HoldingObject = null;

				anim.ResetCurlClamps();
			}
			else
			{
				HoldingObject.Tags.Add( "player" );

				var grabobj = HoldingObject.GetComponent<VRGrabbableObject>( false, true );

				if ( grabobj != null )
				{
					anim.SetCurlClamps( grabobj.curlClamps );
				}

				HoldingObject.Transform.Position = anim.Transform.World.Position + (HandOffset.Transform.LocalPosition * Transform.World.Rotation);
				HoldingObject.Transform.Rotation = anim.Transform.World.Rotation * Rotation.FromPitch( 35f );// * (HandOffset.Transform.LocalRotation.Inverse * Transform.World.Rotation);
			}
		}
	}
}
