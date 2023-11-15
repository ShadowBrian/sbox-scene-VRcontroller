using Sandbox;

public sealed class VRAvatarDriver : BaseComponent
{
	[Property] public GameObject SourcePlayer { get; set; }

	CitizenAnimation anim { get; set; }
	AnimatedModelComponent animModel { get; set; }

	GameObject Head;
	GameObject LeftHand;
	GameObject RightHand;

	GameObject LeftBone;
	GameObject RightBone;

	bool Initialized;

	public override void OnStart()
	{
		Head = SourcePlayer.Children.Where( C => C.Name == "Head" ).First();

		LeftHand = SourcePlayer.GetComponents<AnimateVRHand>( false, true ).Where( C => C.GameObject.Name == "Left Hand Model" ).First().GameObject;
		RightHand = SourcePlayer.GetComponents<AnimateVRHand>( false, true ).Where( C => C.GameObject.Name == "Right Hand Model" ).First().GameObject;

		anim = GetComponent<CitizenAnimation>( false, true );

		animModel = anim.GetComponent<AnimatedModelComponent>();

		if ( Head.IsValid() && LeftHand.IsValid() && RightHand.IsValid() )
		{
			LeftBone = new GameObject();
			RightBone = new GameObject();
			Initialized = true;
		}

		Log.Info( "Head:" + Head.IsValid() + " LeftHand:" + LeftHand.IsValid() + " RightHand:" + RightHand.IsValid() );
	}

	Vector3 PositionDelta;

	Vector3 LastPosition;

	PhysicsTraceResult heighttr;

	public override void Update()
	{
		if ( !Initialized ) return;

		anim.WithLook( Head.Transform.Rotation.Forward );

		anim.IkLeftHand = LeftBone;

		anim.IkRightHand = RightBone;

		LeftBone.Transform.World = LeftHand.GetComponent<AnimatedModelComponent>().GetBoneTransform( LeftHand.GetComponent<AnimatedModelComponent>().Model.Bones.GetBone( "hand_R" ), true );

		LeftBone.Transform.LocalRotation *= Rotation.FromRoll( 180 );

		RightBone.Transform.World = RightHand.GetComponent<AnimatedModelComponent>().GetBoneTransform( RightHand.GetComponent<AnimatedModelComponent>().Model.Bones.GetBone( "hand_R" ), true );

		float ScaleOffset = 1f - (1f - (animModel.GetFloat( "scale_height" ) - 0.1f));


		heighttr = Physics.Trace.Ray( Head.Transform.Position, Head.Transform.Position.WithZ( SourcePlayer.Transform.Position.z ) ).WithoutTags( "player" ).Run();


		float LocalEyeHeight = heighttr.Distance / 64f;

		animModel.Set( "duck", (ScaleOffset - LocalEyeHeight) * 3f );

		animModel.SceneObject.SetBodyGroup( "Hands", 1 );

		Transform.Position = Head.Transform.Position.WithZ( heighttr.EndPosition.z ) - Head.Transform.Rotation.Forward.WithZ( 0 ) * animModel.GetFloat( "duck" ) * 20f;
		Transform.Rotation = Rotation.LookAt( Head.Transform.Rotation.Forward.WithZ( 0 ) );
	}

	public override void FixedUpdate()
	{
		if ( !Initialized ) return;

		Transform.Position = Head.Transform.Position.WithZ( heighttr.EndPosition.z ) - Head.Transform.Rotation.Forward.WithZ( 0 ) * animModel.GetFloat( "duck" ) * 20f;
		Transform.Rotation = Rotation.LookAt( Head.Transform.Rotation.Forward.WithZ( 0 ) );

		PositionDelta = Head.Transform.Position - LastPosition;

		anim.WithVelocity( PositionDelta * Transform.Rotation * Rotation.FromYaw( 180 ) * 40f );
		anim.WithWishVelocity( PositionDelta * Transform.Rotation * Rotation.FromYaw( 180 ) * 40f );

		LastPosition = Head.Transform.Position;

		anim.IsGrounded = heighttr.Hit;
	}
}
