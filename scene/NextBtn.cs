using Godot;
using System;

public partial class NextBtn : TextureRect
{
	/*
	private UserData userData;
	
	public override void _Ready()
	{
		MouseFilter = MouseFilterEnum.Stop;
		userData = GetNode<UserData>("../../../UserData");
	}

	public override void _GuiInput(InputEvent @event)
	{
		if (@event is InputEventMouseButton mb &&
			mb.ButtonIndex == MouseButton.Left &&
			mb.Pressed)
		{
			GetNode<CanvasLayer>("../..").Hide();
			GetNode<CanvasLayer>("../../../LeaderboardUI").Show();
			GetNode<LeaderboardUI>("../../../LeaderboardUI").Refresh();
			Leaderboard.AddScore(userData.UserName, userData.BestScore);
		}
	}
	*/
}
