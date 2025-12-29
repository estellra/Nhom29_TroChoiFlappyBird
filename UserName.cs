using Godot;

public partial class UserName : LineEdit
{
	public override void _Ready()
	{
		FocusMode = FocusModeEnum.All;
		Editable = true;
		TextSubmitted += OnNameSubmitted;
		CallDeferred(nameof(FocusMe));
	}

	private void FocusMe()
	{
		GrabFocus();
	}

	private void OnNameSubmitted(string text)
	{
		UserData.UserName = text.Trim();
		GD.Print(UserData.UserName);
	}
}
