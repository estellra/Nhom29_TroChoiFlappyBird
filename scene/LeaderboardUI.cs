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
		// Xử lý nút Đóng (X)
		var btnClose = GetNode<TextureButton>("BoardPanel/CloseBtn");
		if (btnClose != null) {
			btnClose.Pressed += () => GetTree().ChangeSceneToFile("res://scene/title_screen.tscn");
		}

		RefreshLeaderboard();
	}

	public void RefreshLeaderboard()
	{
		foreach (var child in _list.GetChildren())
		{
			if (child != template) ((Node)child).QueueFree();
		}
		List<LeaderboardItem> topPlayers = DatabaseManager.GetLeaderboard();
		if (topPlayers == null || topPlayers.Count == 0)
		{
			return;
		}
		for (int i = 0; i < topPlayers.Count; i++)
		{
			var player = topPlayers[i];
			string ten = player.Name.Length > 12 ? player.Name.Substring(0, 10)+".." : player.Name;
			var row = (Control)template.Duplicate();
			row.Visible = true; 
			row.GetNode<Label>("Tem/No").Text = "  "+(i + 1).ToString();
			row.GetNode<Label>("Tem/Name").Text = ten;
			row.GetNode<Label>("Tem/BestScore").Text = player.Score.ToString();
			_list.AddChild(row);
		}
	}
}
