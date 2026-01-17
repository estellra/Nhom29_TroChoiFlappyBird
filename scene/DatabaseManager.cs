using Godot;
using System;
using System.Data.SqlClient;

public partial class DatabaseManager : Node
{
	private string connectionString;
	public override void _Ready()
	{
		string osName = OS.GetName();
		if (osName == "macOS" || osName == "Linux")
		{
			connectionString = "Server=127.0.0.1,1433;Database=GameLeaderboard;User Id=sa;Password=Password.1;TrustServerCertificate=True;";
			GD.Print("Đang chạy trên MAC -> Dùng cấu hình Docker.");
		}
		else 
		{
			connectionString = "Server=.\\SQLEXPRESS;Database=GameLeaderboard;Integrated Security=True;TrustServerCertificate=True;";
			GD.Print("Đang chạy trên WINDOWS -> Dùng cấu hình SQL Express.");
		}
	}

	public void SaveScore(string playerName, int score)
	{
		if (string.IsNullOrEmpty(connectionString)) return;
		try
		{
			using (SqlConnection conn = new SqlConnection(connectionString))
			{
				conn.Open();
				string query = "INSERT INTO PlayerScores (PlayerName, Score) VALUES (@name, @score)";
				using (SqlCommand cmd = new SqlCommand(query, conn))
				{
					cmd.Parameters.AddWithValue("@name", playerName);
					cmd.Parameters.AddWithValue("@score", score);
					cmd.ExecuteNonQuery();
				}
			}
			GD.Print($"[SQL] Đã lưu điểm: {score} cho {playerName}");
		}
		catch (Exception ex)
		{
			GD.PrintErr("[SQL Lỗi] " + ex.Message);
		}
	}
}
