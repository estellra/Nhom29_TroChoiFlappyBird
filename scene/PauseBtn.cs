using Godot;

public partial class PauseBtn : TextureRect
{

	public override void _Ready()
	{
		ProcessMode = ProcessModeEnum.Always;
		MouseFilter = MouseFilterEnum.Stop;
		GetNode<TextureRect>("../GamePausedPanel").Hide();
		Show();
	}

	public override void _GuiInput(InputEvent @event)
	{
		if (@event is InputEventMouseButton mb &&
			mb.ButtonIndex == MouseButton.Left &&
			mb.Pressed)
		{
			GD.Print("clicked");
			GetTree().Paused = true;
			Hide();
			GetNode<TextureRect>("../GamePausedPanel").Show();
		}
	}
}
