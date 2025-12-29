using Godot;
using System;

public partial class NextBtn : TextureRect
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
			GetNode<CanvasLayer>("../..").Hide();
			GetNode<CanvasLayer>("../../../Ranking").Show();
		}
	}
}
