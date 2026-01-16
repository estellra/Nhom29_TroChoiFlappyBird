using Godot;
using System;
using Godot.Collections;

public partial class ProfileUI : CanvasLayer
{
	private LineEdit name;
	private UserData data;
	
	public override void _Ready()
	{
		name = GetNode<LineEdit>("ProfilePanel/ProfileText");
		data = GetNode<UserData>("../UserData");
		SetUpName();
	}
	
	private void SetUpName(){
		name.FocusMode = Control.FocusModeEnum.All; 
		name.Editable = true;
		name.TextSubmitted += _ => Commit();
		name.CallDeferred(nameof(FocusMe));
		
	}
	
	private void FocusMe()
	{
		name.GrabFocus();
	}

	public void Commit()
	{
		var t = (name.Text ?? "").Trim();
		data.UserName = t;
		data.SaveUserName();
	}
}
