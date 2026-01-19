using Godot;
using System;

public partial class LevelSelect : Control
{
	public override void _Ready()
	{
		var btnBack = GetNode<TextureButton>("btnBack");
		btnBack.Pressed += () =>
		{
			GetTree().ChangeSceneToFile("res://scene/title_screen.tscn"); 
		};
		GetNode<TextureButton>("BtnEasy").Pressed += OnEasyPressed;
		GetNode<TextureButton>("BtnMedium").Pressed += OnMediumPressed;
		GetNode<TextureButton>("BtnHard").Pressed += OnHardPressed;
	}

	private void OnEasyPressed()
	{
		GetTree().ChangeSceneToFile("res://scene/main.tscn");
	}

	private void OnMediumPressed()
	{
		GetTree().ChangeSceneToFile("res://scene/main.tscn");
	}

	private void OnHardPressed()
	{
		GetTree().ChangeSceneToFile("res://scene/main.tscn");
	}
}
