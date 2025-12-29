using Godot;
using System;

public partial class GameOverUI : CanvasLayer
{
	
	public override void _Ready()
	{
		GetNode<CanvasLayer>("Ranking").Hide();
		GetNode<CanvasLayer>("Profile").Hide();
	}
}
