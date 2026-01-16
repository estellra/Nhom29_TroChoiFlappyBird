using Godot;
using System;

public partial class PausedRestartButton : TextureRect
{
	private Main main;
	
	public override void _Ready()
	{
		MouseFilter = MouseFilterEnum.Stop; 
	}

	public override void _GuiInput(InputEvent @event)
	{
		if (@event is InputEventMouseButton mouseEvent &&
			mouseEvent.Pressed &&
			mouseEvent.ButtonIndex == MouseButton.Left)
		{
			GetTree().Paused = false;
			GetTree().ReloadCurrentScene();
		}
	}
}
