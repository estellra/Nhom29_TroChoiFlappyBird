using Godot;

public partial class ResumeBtn : TextureRect
{
	
	public override void _Ready()
	{
		MouseFilter = Control.MouseFilterEnum.Stop;
	}

	public override void _GuiInput(InputEvent @event)
	{
		if (@event is InputEventMouseButton mb &&
			mb.ButtonIndex == MouseButton.Left &&
			mb.Pressed)
		{
			GetTree().Paused = false;
			GetNode<TextureRect>("../../PauseBtn").Show();
			GetNode<TextureRect>("../../GamePausedPanel").Hide();
		}
	}
}
