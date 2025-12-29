using Godot;

public partial class PauseButton : TextureRect
{
	private bool isPaused = false;
	private Control GamePausedUI;

	public override void _Ready()
	{
		MouseFilter = MouseFilterEnum.Stop;
		ProcessMode = ProcessModeEnum.Always;
		GamePausedUI = GetParent().GetNode<Control>("GamePausedUI");
		GamePausedUI.Visible = false;
	}

	public override void _GuiInput(InputEvent @event)
	{
		if (@event is InputEventMouseButton mb
			&& mb.ButtonIndex == MouseButton.Left
			&& mb.Pressed)
		{
			TogglePause();
			GetViewport().SetInputAsHandled();
		}
	}

	private void TogglePause()
	{
		isPaused = !isPaused;
		GetTree().Paused = isPaused;
		GamePausedUI.Visible = isPaused;
	}
}
