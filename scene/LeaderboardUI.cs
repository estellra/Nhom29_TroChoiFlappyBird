using Godot;

public partial class LeaderboardUI : CanvasLayer
{
	private VBoxContainer _list;
	private Label template;

	public override void _Ready()
	{
		_list = GetNode<VBoxContainer>("BoardPanel/Scroll/List");
		template = GetNode<Label>("BoardPanel/Scroll/List/Template");
	}

	public void Refresh()
	{
		foreach (var child in _list.GetChildren())
			(child as Node)?.QueueFree();

		var entries = Leaderboard.Load();

		if (entries.Count == 0)
		{
			_list.AddChild(new Label { Text = "No scores yet." });
			return;
		}

		for (int i = 0; i < entries.Count; i++)
		{
			var e = entries[i];
			var row = template.Duplicate() ;
			row.GetNode<Label>("Tem/No").Text = "  "+ (i+1).ToString();
			row.GetNode<Label>("Tem/Name").Text = e.PlayerName;
			row.GetNode<Label>("Tem/BestScore").Text = e.Score.ToString();
			_list.AddChild(row);
		}
	}
}
