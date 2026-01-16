using Godot;
using System;

public partial class UserData : Node
{
	public string UserName;
	
	private const string SAVE_PATH = "user://best_score.save";
	private const string SAVE_PATH_2 = "user://user_name.save";
	public int BestScore { get; set; } = 0;
	
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
	
	public void SaveUserName(){
		using var file = FileAccess.Open(SAVE_PATH_2, FileAccess.ModeFlags.Write);
		   file.StoreString(UserName + "\n");
	} 

	private void LoadBestScore()
	{
		if (!FileAccess.FileExists(SAVE_PATH))
			return;
		using var file = FileAccess.Open(SAVE_PATH, FileAccess.ModeFlags.Read);
		BestScore = (int)file.Get32();
	}
}
