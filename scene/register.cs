using Godot;
using System;

public partial class register : Control
{
	public override void _Ready()
	{
		GetNode<TextureButton>("btnBack").Pressed += () =>
		{
			GetTree().ChangeSceneToFile("res://scene/ranking.tscn");
		};
		var btnReg = GetNode<TextureButton>("btnRegister");
		var inputUser = GetNode<LineEdit>("UserName"); 
		var inputPass = GetNode<LineEdit>("Pass");
		var inputConfirm = GetNode<LineEdit>("ConfirmPass"); 
		var inputEmail = GetNode<LineEdit>("Email");
		var btnShowPass = GetNode<TextureButton>("btnShowPass");
		var btnShowPassConfirm = GetNode<TextureButton>("btnShowPassConfirm");
		
		inputPass.Secret = true;
		inputConfirm.Secret = true;
		
		btnShowPass.Pressed += () =>
		{
			inputPass.Secret = !inputPass.Secret;
		};

		btnShowPassConfirm.Pressed += () =>
		{
			inputConfirm.Secret = !inputConfirm.Secret;
		};

		btnReg.Pressed += () =>
		{
			string u = inputUser.Text.Trim();
			string p = inputPass.Text.Trim();
			string c = inputConfirm.Text.Trim(); 
			string e = inputEmail.Text.Trim();

			if (u == "" || p == "" || c == "" || e == "") 
			{
				OS.Alert("Vui lòng điền đầy đủ thông tin vào các ô trống!", "⚠️ Thiếu thông tin");
				return;
			}
			
			if (p != c)
			{
				OS.Alert("Mật khẩu xác nhận không trùng khớp.\nVui lòng kiểm tra lại!", "❌ Lỗi Mật Khẩu");
				return; 
			}
			if (!e.ToLower().EndsWith("@gmail.com"))
			{
				OS.Alert("Email không hợp lệ!\nVui lòng sử dụng tài khoản Google (@gmail.com)", "❌ Lỗi Email");
				return;
			}
			
			int result = DatabaseManager.Register(u, p, e);
			
			switch (result)
			{
				case 0: 
					OS.Alert("Đăng ký tài khoản thành công!\nBạn có thể đăng nhập ngay bây giờ.", "✅ Chúc mừng");
					GetTree().ChangeSceneToFile("res://scene/login.tscn");
					break;

				case 1: 
					OS.Alert($"Tên tài khoản '{u}' đã có người sử dụng.\nVui lòng chọn tên khác!", "⛔ Tên đã tồn tại");
					break;

				case 2: 
					OS.Alert($"Email '{e}' đã được liên kết với một tài khoản khác!", "⛔ Email đã tồn tại");
					break;

				default: 
					OS.Alert("Có lỗi hệ thống xảy ra.\nVui lòng thử lại sau!", "🚫 Lỗi Hệ Thống");
					break;
			}
		};
	}
}
