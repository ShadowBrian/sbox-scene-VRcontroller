using Sandbox;

public sealed class VRHandPoseController : BaseComponent, BaseComponent.ExecuteInEditor
{
	//0=Thumb, 1=Index, 2=Middle, 3=Ring, 4=Pinky
	[Property] public float ThumbClamp { get; set; } = 1f;
	[Property] public float IndexClamp { get; set; } = 1f;
	[Property] public float MiddleClamp { get; set; } = 1f;
	[Property] public float RingClamp { get; set; } = 1f;
	[Property] public float PinkyClamp { get; set; } = 1f;

	AnimatedModelComponent model;

	public override void Update()
	{
		if ( model == null )
		{
			model = GetComponent<AnimatedModelComponent>( false );
			if ( model == null )
			{
				return;
			}
		}

		if ( !model.Enabled )
		{
			return;
		}

		model.SceneObject.Update( 0.1f );

		model.Set( "thumb", ThumbClamp );
		model.Set( "index", IndexClamp );
		model.Set( "middle", MiddleClamp );
		model.Set( "ring", RingClamp );
		model.Set( "pinky", PinkyClamp );



	}
}
