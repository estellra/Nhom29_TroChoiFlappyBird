using Godot;
using System;

public partial class BestStatus : RichTextLabel
{
	private UserData userData;

	public override void _Ready()
	{
		userData = GetNode<UserData>("../../../UserData");
		UpdateText();
	}

	public override void _Process(double delta)
	{
		UpdateText(); 
	}

	private void UpdateText()
	{
		if (userData.BestScore != userData.Score)
			Hide();
		else
			Show();
	}
}
