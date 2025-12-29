using Godot;
using System;

public partial class BestNumber : RichTextLabel
{
	private UserData userData;

	public override void _Ready()
	{
		userData = GetNode<UserData>("../../../../UserData");
		UpdateText();
	}

	public override void _Process(double delta)
	{
		UpdateText(); 
	}

	private void UpdateText()
	{
		Text = userData.BestScore.ToString();
	}
}
	
	
