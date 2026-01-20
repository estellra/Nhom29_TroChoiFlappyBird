using Godot;
using System;

public partial class login : Control
{
	public override void _Ready()
	{
		GetNode<TextureButton>("btnBack").Pressed += () =>
		{
			GetTree().ChangeSceneToFile("res://scene/ranking.tscn");
		};
		GetNode<TextureButton>("btnForget").Pressed += () => 
		{
			GetTree().ChangeSceneToFile("res://scene/otpmail.tscn");
		};
		var btnLogin = GetNode<TextureButton>("btnLogin");
		var inputUser = GetNode<LineEdit>("UserName");
		var inputPass = GetNode<LineEdit>("Pass");
		var btnShowPass = GetNode<TextureButton>("btnShowPass");
		
		inputPass.Secret = true;
		if (btnShowPass != null) 
		{
			btnShowPass.Pressed += () =>
			{
				inputPass.Secret = !inputPass.Secret;
			};
		}

		btnLogin.Pressed += () =>
		{
			string u = inputUser.Text.Trim();
			string p = inputPass.Text.Trim();
			
			if (u == "" || p == "")
			{
				OS.Alert("Vui lòng nhập đầy đủ Tài khoản và Mật khẩu!", "⚠️ Thiếu thông tin");
				return;
			}
			int scoreLoadDuoc = 0;
			bool success = DatabaseManager.Login(u, p, out scoreLoadDuoc);

			if (success)
			{
				GlobalData.CurrentUser = u;
				GlobalData.CurrentBestScore = scoreLoadDuoc;
				OS.Alert($"Chào mừng trở lại, {u}!\nĐiểm cao nhất của bạn: {scoreLoadDuoc}", "✅ Đăng nhập thành công");
				GetTree().ChangeSceneToFile("res://scene/title_screen.tscn");
			}
			else
			{
				OS.Alert("Tài khoản hoặc Mật khẩu không chính xác.\nVui lòng kiểm tra lại!", "❌ Đăng nhập thất bại");
			}
		};
	}
}
