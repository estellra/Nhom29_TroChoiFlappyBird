using Godot;
using System.Collections.Generic;

public partial class LeaderboardUI : CanvasLayer
{
	private VBoxContainer _list;
	private Control template;

	public override void _Ready()
	{
		// Lấy đúng đường dẫn theo hình
		_list = GetNode<VBoxContainer>("BoardPanel/Scroll/List");
		template = GetNode<Control>("BoardPanel/Scroll/List/Template");
		
		// Ẩn mẫu đi
		template.Visible = false;

		// Xử lý nút Back (Nếu nút nằm TRONG BoardPanel thì dùng dòng dưới)
		// var btnBack = GetNode<TextureButton>("BoardPanel/btnBack");
		// Nếu nút nằm NGOÀI BoardPanel (như hình 14.39.23) thì dùng dòng này:

		// Xử lý nút Đóng (X)
		var btnClose = GetNode<TextureButton>("BoardPanel/CloseBtn");
		if (btnClose != null) {
			btnClose.Pressed += () => GetTree().ChangeSceneToFile("res://scene/title_screen.tscn");
		}

		RefreshLeaderboard();
	}

	public void RefreshLeaderboard()
	{
		// Xóa hết dòng cũ trừ Template
		foreach (var child in _list.GetChildren())
		{
			if (child != template) ((Node)child).QueueFree();
		}

		// Gọi DB lấy list
		List<LeaderboardItem> topPlayers = DatabaseManager.GetLeaderboard();

		if (topPlayers == null || topPlayers.Count == 0)
		{
			GD.Print("Danh sách trống hoặc lỗi DB!");
			return;
		}

		// Tạo dòng mới
		for (int i = 0; i < topPlayers.Count; i++)
		{
			var player = topPlayers[i];
			
			var row = (Control)template.Duplicate();
			row.Visible = true; 
			
			// --- ĐOẠN QUAN TRỌNG: ĐIỀN DỮ LIỆU ---
			// Dựa theo hình 14.33.33 của ông:
			row.GetNode<Label>("Tem/No").Text = (i + 1).ToString();
			row.GetNode<Label>("Tem/Name").Text = player.Name;
			row.GetNode<Label>("Tem/BestScore").Text = player.Score.ToString();

			_list.AddChild(row);
		}
	}
}
