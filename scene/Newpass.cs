using Godot;
using System;

public partial class Newpass : Control
{
	public override void _Ready()
	{
		var inputPass = GetNode<LineEdit>("Pass");
		var inputConfirm = GetNode<LineEdit>("ConfirmPass");
		var btnOK = GetNode<TextureButton>("btnOK");
		var btnBack = GetNode<TextureButton>("btnBack");
		btnBack.Pressed += () => GetTree().ChangeSceneToFile("res://scene/login.tscn");
		btnOK.Pressed += () =>
		{
			string p1 = inputPass.Text.Trim();
			string p2 = inputConfirm.Text.Trim();
			string email = GlobalData.ResetEmail; 
			if (email == "") {
				OS.Alert("Lỗi mất kết nối! Vui lòng làm lại.", "Lỗi");
				GetTree().ChangeSceneToFile("res://scene/login.tscn");
				return;
			}

			if (p1 == "" || p2 == "") { OS.Alert("Không được để trống mật khẩu!", "Thiếu"); return; }
			if (p1 != p2) { OS.Alert("Mật khẩu nhập lại không khớp!", "Lỗi"); return; }
			if (p1.Length < 3) { OS.Alert("Mật khẩu quá ngắn!", "Yếu"); return; }

			DatabaseManager.ResetPassword(email, p1);

			OS.Alert("Đổi mật khẩu thành công! Hãy đăng nhập lại.", "Hoàn tất");
			GetTree().ChangeSceneToFile("res://scene/login.tscn");
		};
	}
}
