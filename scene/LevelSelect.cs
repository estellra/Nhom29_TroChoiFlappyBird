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
		GlobalData.currentdifficulty = GlobalData.difficulty.easy;
		GetTree().ChangeSceneToFile("res://scene/main.tscn");
	}

	private void OnMediumPressed()
	{
		GlobalData.currentdifficulty = GlobalData.difficulty.medium;
		GetTree().ChangeSceneToFile("res://scene/main.tscn");
	}

	private void OnHardPressed()
	{
		GlobalData.currentdifficulty = GlobalData.difficulty.hard;
		GetTree().ChangeSceneToFile("res://scene/main.tscn");
	}
}
