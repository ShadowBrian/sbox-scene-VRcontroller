﻿using Editor.EntityPrefabEditor;
using Sandbox;
using System.Collections.Generic;

namespace Editor.Inspectors;

[CanEdit( "asset:object" )]
public class PrefabFileInspector : Widget
{
	public PrefabFileInspector( Widget parent ) : base( parent )
	{
		// edit scene info ?
		// show number of game objects ?
	}
}
