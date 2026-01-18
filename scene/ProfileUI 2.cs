using Godot;
using System;
using Godot.Collections;

public partial class ProfileUI : CanvasLayer
{
	//Khởi tạo tên
	private LineEdit name;
	private UserData data;
	
	//Link với tên với bên db
	public override void _Ready()
	{
		name = GetNode<LineEdit>("ProfilePanel/ProfileText");
		data = GetNode<UserData>("../UserData");
		SetUpName();
	}
	
	//Nhập tên
	private void SetUpName(){
		name.FocusMode = Control.FocusModeEnum.All; 
		name.Editable = true;
		name.TextSubmitted += _ => Commit();
		name.CallDeferred(nameof(FocusMe));
		
	}
	
	//Chỉ con trỏ vào nhập tên
	private void FocusMe()
	{
		name.GrabFocus();
	}

	//Lưu tên vô trong user Data
	public void Commit()
	{
		var t = (name.Text ?? "").Trim();
		data.UserName = t;
		data.SaveUserName();
	}
}
