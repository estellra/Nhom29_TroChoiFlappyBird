using Godot;
using System;
using System.Collections.Generic;
using System.Text.Json;

public static class Leaderboard
{
	private const string FILE_PATH = "user://leaderboard.json";
	private const int MAX_ENTRIES = 10;

	public class ScoreEntry
	{
		public string PlayerName { get; set; }
		public int Score { get; set; }
		public long UnixTime { get; set; }
	}


	public static void AddScore(string name, int score)
	{
		if (score <= 0) return;

		var list = Load();

		list.Add(new ScoreEntry
		{
			PlayerName = SanitizeName(name),
			Score = score,
			UnixTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds()
		});

		list.Sort((a, b) => b.Score.CompareTo(a.Score));

		if (list.Count > MAX_ENTRIES)
			list = list.GetRange(0, MAX_ENTRIES);

		Save(list);
	}

	public static List<ScoreEntry> Load()
	{
		if (!FileAccess.FileExists(FILE_PATH))
			return new List<ScoreEntry>();

		try
		{
			using var file = FileAccess.Open(FILE_PATH, FileAccess.ModeFlags.Read);
			return JsonSerializer.Deserialize<List<ScoreEntry>>(file.GetAsText())
				   ?? new List<ScoreEntry>();
		}
		catch
		{
			GD.PrintErr("Failed to load leaderboard");
			return new List<ScoreEntry>();
		}
	}

	private static void Save(List<ScoreEntry> list)
	{
		using var file = FileAccess.Open(FILE_PATH, FileAccess.ModeFlags.Write);
		file.StoreString(JsonSerializer.Serialize(
			list,
			new JsonSerializerOptions { WriteIndented = true }
		));
	}

	private static string SanitizeName(string name)
	{
		var t = (name ?? "").Trim();
		if (t.Length < 1) t = "Player";
		if (t.Length > 16) t = t.Substring(0, 16);
		return t;
	}
}
