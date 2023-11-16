using Sandbox;

public sealed class WebSwinging : BaseComponent
{
	[Property] TrackedPoseComponent.PoseSources HandSide { get; set; }

	public Transform handTransform; // The hand (controller) from which the web is attached
	[Property] public float webForce { get; set; } = 10f;    // Force applied to the rigidbody when swinging
	float maxSwingDistance = 10000f; // Maximum distance the player can swing

	private bool isSwinging = false;
	private PhysicsComponent rb;

	public override void OnStart()
	{
		rb = GameObject.Parent.GetComponent<PhysicsComponent>();
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
		handTransform = Transform.World;

		if ( WebAttachPosition == Vector3.Zero )
		{
			Gizmo.Draw.Line( handTransform.Position, handTransform.Position + (handTransform.Rotation.Forward + handTransform.Rotation.Down) * maxSwingDistance );
		}
		else
		{
			Gizmo.Draw.Line( handTransform.Position, WebAttachPosition );
		}


		if ( TranslateHandSide().Trigger.Value < 0.5f )
		{
			StopSwinging();
		}

		if ( TranslateHandSide().Trigger.Value > 0.5f )
		{
			StartSwinging();
		}

	}

	Vector3 WebAttachPosition;

	void StartSwinging()
	{
		var hit = Physics.Trace.Ray( handTransform.Position, handTransform.Position + (handTransform.Rotation.Forward + handTransform.Rotation.Down) * maxSwingDistance ).Run();

		if ( hit.Hit && !isSwinging )
		{
			WebAttachPosition = hit.HitPosition;

			isSwinging = true;
		}

		if ( isSwinging )
		{
			Vector3 swingDirection = WebAttachPosition - handTransform.Position;

			rb.Velocity += (swingDirection.Normal * webForce);
		}
	}

	void StopSwinging()
	{
		WebAttachPosition = Vector3.Zero;
		isSwinging = false;
	}
}
