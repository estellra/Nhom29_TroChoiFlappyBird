using Godot;
using System;

public partial class ranking : Control
{
	public override void _Ready()
	{
		GetNode<TextureButton>("btnGuest").Pressed += () =>
		{
			GetTree().ChangeSceneToFile("res://scene/title_screen.tscn");
		};
		GetNode<TextureButton>("btnLogin").Pressed += () =>
		{
			GetTree().ChangeSceneToFile("res://scene/login.tscn");
		};
		GetNode<TextureButton>("btnRegister").Pressed += () =>
		{
			GetTree().ChangeSceneToFile("res://scene/register.tscn");
		};
	}
}
