using Godot;
using System;

public partial class login : Control
{
	public override void _Ready()
	{
		GetNode<TextureButton>("btnBack").Pressed += () =>
		{
			GetTree().ChangeSceneToFile("res://scene/ranking.tscn");
		};
		GetNode<TextureButton>("btnLogin").Pressed += () =>
		{
			GetTree().ChangeSceneToFile("res://scene/title_screen.tscn");
		};
	}
}
