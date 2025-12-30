using Godot;

public partial class PauseBtn : TextureRect
{

	public override void _Ready()
	{
		MouseFilter = Control.MouseFilterEnum.Stop;
		GetNode<TextureRect>("../GamePausedPanel").Hide();
	}

	public override void _GuiInput(InputEvent @event)
	{
		if (@event is InputEventMouseButton mb &&
			mb.ButtonIndex == MouseButton.Left &&
			mb.Pressed)
		{
			GetTree().Paused = true;
			Hide();
			GetNode<TextureRect>("../GamePausedPanel").Show();
		}
	}
}
