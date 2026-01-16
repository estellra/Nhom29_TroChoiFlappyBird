using Godot;
using System;

public partial class MenuBtn : TextureRect
{	
	public override void _Ready()
	{
		MouseFilter = MouseFilterEnum.Stop; 
	}

	public override void _GuiInput(InputEvent @event)
	{
		if (@event is InputEventMouseButton mb &&
			mb.ButtonIndex == MouseButton.Left &&
			mb.Pressed)
		{
			GetTree().Paused = false;
			GetTree().ChangeSceneToFile("res://scene/title_screen.tscn");
		}
	}
}
