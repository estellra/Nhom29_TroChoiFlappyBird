using Godot;
using System;

public partial class ScoreNumber : RichTextLabel
{
	private Player player;

	public override void _Ready()
	{
		ProcessMode = ProcessModeEnum.Always;
		player = GetNode<Player>("../../../../../Player");		
	}

	public override void _Process(double delta)
	{
		Text = player.tinhdiem();
	}

}
