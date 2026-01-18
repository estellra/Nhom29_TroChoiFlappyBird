using Godot;
using System;

public partial class GameOverRestartButton : TextureRect
{
	//Nhận click chuột
	public override void _Ready()
	{
		MouseFilter = MouseFilterEnum.Stop; 
	}
	
	//Tải lại scene đó
	public override void _GuiInput(InputEvent @event)
	{
		if (@event is InputEventMouseButton mouseEvent &&
			mouseEvent.Pressed &&
			mouseEvent.ButtonIndex == MouseButton.Left)
		{
			GetTree().ReloadCurrentScene();
		}
	}
}
