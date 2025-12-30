using Godot;
using System;

public partial class UserData : Node
{
	public string UserName;
	public int Score;
	private const string SAVE_PATH = "user://best_score.save";
	public int BestScore = 0;
	
	public override void _Ready()
	{
		LoadBestScore();
	}

	public void CheckAndSave(int score)
	{
		if (score > BestScore)
		{
			BestScore = score;
			SaveBestScore();
		}
	}

	private void SaveBestScore()
	{
		using var file = FileAccess.Open(SAVE_PATH, FileAccess.ModeFlags.Write);
		file.Store32((uint)BestScore);
	}

	private void LoadBestScore()
	{
		if (!FileAccess.FileExists(SAVE_PATH))
			return;
		using var file = FileAccess.Open(SAVE_PATH, FileAccess.ModeFlags.Read);
		BestScore = (int)file.Get32();
	}
}
