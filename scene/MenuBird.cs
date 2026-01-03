using Godot;
using System;

public partial class MenuBird : AnimatedSprite2D
{
	// Xóa hàm _Ready đi, dùng hàm này:
	public override void _Process(double delta)
	{
		// 1. Lấy tên skin đang được chọn trong GlobalData
		string tenSkinCanDung = "skin_" + GlobalData.SkinDangChon;

		// 2. Kiểm tra: Nếu con chim đang KHÔNG diễn cái skin đó -> Thì bắt nó đổi ngay
		if (Animation != tenSkinCanDung)
		{
			Play(tenSkinCanDung);
			// GD.Print("Đã đổi sang: " + tenSkinCanDung); // Thích thì bật dòng này lên để debug
		}
	}
}
