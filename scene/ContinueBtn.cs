using Godot;

public partial class ContinueBtn : TextureRect
{
	private CanvasLayer GamePausedUI;
	
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
			GetTree().Paused = false;
			GetNode<TextureRect>("../../../PauseBtn").Show();
			GetNode<TextureRect>("../../../GamePausedPanel").Hide();
		}
	}
}
