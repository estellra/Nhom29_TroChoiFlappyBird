using Godot;
using System;

public partial class GameOverUI : CanvasLayer
{
	private Player player;
	private RichTextLabel score;
	private RichTextLabel best;
	private TextureRect status;
	private TextureRect copper;
	private TextureRect silver;
	private TextureRect gold;
	private UserData data;
	
	public override void _Ready()
	{
		player = GetNode<Player>("../Player"); 
		score = GetNode<RichTextLabel>("ScorePanel/ScoreNumber"); 
		best= GetNode<RichTextLabel>("ScorePanel/BestNumber");
		status= GetNode<TextureRect>("ScorePanel/BestStatus");
		data = GetNode<UserData>("../UserData");
		copper= GetNode<TextureRect>("Medal/Copper");
		silver= GetNode<TextureRect>("Medal/Silver");
		gold= GetNode<TextureRect>("Medal/Gold");
		
		var btnRanking = GetNode<TextureButton>("ScorePanel/RankingButton");
		btnRanking.Pressed += () =>
		{
			GetTree().ChangeSceneToFile("res://scene/LeaderboardUI.tscn"); 
		};
		var btnRestart = GetNode<TextureButton>("ScorePanel/GameOverRestart");
		btnRestart.Pressed += () =>
		{
			GetTree().ReloadCurrentScene();
		};
		var btnhome = GetNode<TextureButton>("ScorePanel/btnHome");
		btnhome.Pressed += () =>
		{
			GetTree().ChangeSceneToFile("res://scene/title_screen.tscn"); 
		};
	}

	public override void _Process(double delta)
	{
		ShowText();
		ShowStatus();
		ShowMedal();
	}
	
	public void ShowText()
{
	if (player != null) score.Text = player.point.ToString(); 
	// Best score lấy từ Global luôn cho chuẩn
	if (data != null) best.Text = GlobalData.CurrentBestScore.ToString();
}

public void ShowStatus()
{
	if (GlobalData.IsNewRecord == true)
	{
		status.Show(); // Hiện chữ NEW
	}
	else
	{
		status.Hide();
	}
}
	
	public void ShowMedal()
	{
		if (player == null) return;
		
		copper.Hide();
		silver.Hide();
		gold.Hide();

		if (player.point >= 10) gold.Show();
		else if (player.point >= 5) silver.Show();
		else if (player.point >= 0) copper.Show();
	}
}
