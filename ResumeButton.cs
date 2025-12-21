using Godot;

public partial class ResumeButton : RichTextLabel
{
	private Control GamePausedUI;
	
	public override void _Ready()
	{
		ProcessMode = ProcessModeEnum.Always;
		MouseFilter = MouseFilterEnum.Stop; 
		GamePausedUI = GetNode<Control>("../..");
		GamePausedUI.Visible = false;
	}

	public override void _GuiInput(InputEvent @event)
	{
		if (@event is InputEventMouseButton mb &&
			mb.ButtonIndex == MouseButton.Left &&
			mb.Pressed)
		{
			GetTree().Paused = false;
			GamePausedUI.Visible = false;
		}
	}
}
