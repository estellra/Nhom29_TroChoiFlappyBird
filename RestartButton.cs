using Godot;
using System;

public partial class RestartButton : TextureRect
{
	private Main main;
	
	public override void _Ready()
	{
		main = GetNode<Main>("../../../..");
		MouseFilter = MouseFilterEnum.Stop; 
	}

	public override void _GuiInput(InputEvent @event)
	{
		if (@event is InputEventMouseButton mouseEvent &&
			mouseEvent.Pressed &&
			mouseEvent.ButtonIndex == MouseButton.Left)
		{
			main.Restart();
		}
	}
}
