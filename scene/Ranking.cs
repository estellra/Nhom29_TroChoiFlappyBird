using Godot;
using System;

public partial class Ranking : Control
{
	public override void _Ready()
	{
		var btnBack = GetNode<TextureButton>("btnBack");
		btnBack.Pressed += () =>
		{
			GetTree().ChangeSceneToFile("res://scene/title_screen.tscn");
		};
	}
}
