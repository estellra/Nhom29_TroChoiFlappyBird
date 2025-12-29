using Godot;
using System;

public partial class RankingButton : TextureRect
{
	public override void _Ready()
	{
		ProcessMode = ProcessModeEnum.Always;
		MouseFilter = MouseFilterEnum.Stop;
	}

	public override void _GuiInput(InputEvent @event)
	{
		if (@event is InputEventMouseButton mb &&
			mb.ButtonIndex == MouseButton.Left &&
			mb.Pressed)
		{
			GetNode<TextureRect>("../..").Hide();
			GetNode<TextureRect>("../../../Medal").Hide();
			GetNode<CanvasLayer>("../../../Profile").Show();
		}
	}
}
