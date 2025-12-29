using Godot;

public partial class Restart : RichTextLabel
{
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
			GoToTitle();
			GetViewport().SetInputAsHandled();
		}
	}

	private void GoToTitle()
	{
		GetTree().Paused = false;
		GetTree().ChangeSceneToFile("res://scene/title_screen.tscn");
	}
}
