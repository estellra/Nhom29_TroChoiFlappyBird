using Godot;
using System;

public partial class register : Control
{
	public override void _Ready()
	{
		GetNode<TextureButton>("btnBack").Pressed += () =>
		{
			GetTree().ChangeSceneToFile("res://scene/ranking.tscn");
		};
		GetNode<TextureButton>("btnRegister").Pressed += () =>
		{
			GetTree().ChangeSceneToFile("res://scene/login.tscn");
		};
	}
}
