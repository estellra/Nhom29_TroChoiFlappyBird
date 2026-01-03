using Godot;
using System;

public partial class LevelSelect : Control
{
	public override void _Ready()
	{
		GetNode<Button>("VBoxContainer/BtnEasy").Pressed += OnEasyPressed;
		GetNode<Button>("VBoxContainer/BtnMedium").Pressed += OnMediumPressed;
		GetNode<Button>("VBoxContainer/BtnHard").Pressed += OnHardPressed;
	}

	private void OnEasyPressed()
	{
		GetTree().ChangeSceneToFile("res://scene/level_easy.tscn");
	}

	private void OnMediumPressed()
	{
		GetTree().ChangeSceneToFile("res://scene/level_medium.tscn");
	}

	private void OnHardPressed()
	{
		GetTree().ChangeSceneToFile("res://scene/level_hard.tscn");
	}
}
