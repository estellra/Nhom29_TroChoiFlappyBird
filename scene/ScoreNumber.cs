using Godot;
using System;

public partial class ScoreNumber : RichTextLabel
{
	private Player player;
	public override void _Ready()
	{
		player = GetNode<Player>("../../../../Player"); 
	}

	public override void _Process(double delta)
	{
		if (player == null) return;
		Text = player.point.ToString(); 
	}
}
