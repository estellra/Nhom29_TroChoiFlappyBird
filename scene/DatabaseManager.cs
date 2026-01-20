using Godot;
using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;

public static class DatabaseManager
{
	private static string connectionString = @"Server=.\CSDL;Database=FlappyBirdDB;Integrated Security=True;TrustServerCertificate=True;";

	public static int Register(string username, string password, string email)
	{
		try
		{
			using (SqlConnection conn = new SqlConnection(connectionString))
			{
				conn.Open();

				// Check trùng tên
				string checkUser = "SELECT COUNT(*) FROM Accounts WHERE Username = @u";
				using (SqlCommand cmd = new SqlCommand(checkUser, conn)) {
					cmd.Parameters.AddWithValue("@u", username);
					if ((int)cmd.ExecuteScalar() > 0) return 1;
				}

				// Check trùng email
				string checkEmail = "SELECT COUNT(*) FROM Accounts WHERE Email = @e";
				using (SqlCommand cmd = new SqlCommand(checkEmail, conn)) {
					cmd.Parameters.AddWithValue("@e", email);
					if ((int)cmd.ExecuteScalar() > 0) return 2;
				}

				// Lưu Account
				string query = "INSERT INTO Accounts (Username, Password, Email) VALUES (@u, @p, @e)";
				using (SqlCommand cmd = new SqlCommand(query, conn))
				{
					cmd.Parameters.AddWithValue("@u", username);
					cmd.Parameters.AddWithValue("@p", password);
					cmd.Parameters.AddWithValue("@e", email);
					cmd.ExecuteNonQuery();
				}
			}
			return 0; 
		}
		catch (Exception e) { GD.PrintErr(e.Message); return -1; }
	}

	public static bool Login(string username, string password, out int bestScore)
	{
		bestScore = 0;
		try
		{
			using (SqlConnection conn = new SqlConnection(connectionString))
			{
				conn.Open();
				string query = @"
                    SELECT a.AccountID, 
                           (SELECT MAX(Score) FROM Scores WHERE AccountID = a.AccountID) as MaxScore
                    FROM Accounts a
					WHERE a.Username = @u AND a.Password = @p";

				using (SqlCommand cmd = new SqlCommand(query, conn))
				{
					cmd.Parameters.AddWithValue("@u", username);
					cmd.Parameters.AddWithValue("@p", password);
					
					using (SqlDataReader reader = cmd.ExecuteReader())
					{
						if (reader.Read()) // Nếu tìm thấy user
						{
							if (reader["MaxScore"] != DBNull.Value)
							{
								bestScore = Convert.ToInt32(reader["MaxScore"]);
							}
							return true;
						}
					}
				}
			}
		}
		catch (Exception e) { GD.PrintErr("Lỗi Login: " + e.Message); }
		return false;
	}

	public static void SaveScore(string username, int score)
	{
		try
		{
			using (SqlConnection conn = new SqlConnection(connectionString))
			{
				conn.Open();
				string query = @"
                    INSERT INTO Scores (AccountID, Score)
					SELECT AccountID, @s FROM Accounts WHERE Username = @u";

				using (SqlCommand cmd = new SqlCommand(query, conn))
				{
					cmd.Parameters.AddWithValue("@s", score);
					cmd.Parameters.AddWithValue("@u", username);
					cmd.ExecuteNonQuery();
				}
			}
		}
		catch (Exception e) { GD.PrintErr("Lỗi SaveScore: " + e.Message); }
	}

	public static List<LeaderboardItem> GetLeaderboard()
	{
		var list = new List<LeaderboardItem>();
		try
		{
			using (SqlConnection conn = new SqlConnection(connectionString))
			{
				conn.Open();
				string query = @"
                    SELECT TOP 10 a.Username, MAX(s.Score) as BestScore
                    FROM Scores s
                    JOIN Accounts a ON s.AccountID = a.AccountID
                    GROUP BY a.Username
					ORDER BY BestScore DESC";
				using (SqlCommand cmd = new SqlCommand(query, conn))
				{
					using (SqlDataReader reader = cmd.ExecuteReader())
					{
						while (reader.Read())
						{
							list.Add(new LeaderboardItem
							{
								Name = reader["Username"].ToString(),
								Score = Convert.ToInt32(reader["BestScore"])
							});
						}
					}
				}
			}
		}
		catch (Exception e) { GD.PrintErr("Lỗi BXH: " + e.Message); }
		return list;
	}
	
	public static void SaveOTP(string email, string otp)
	{
		 try {
			using (SqlConnection conn = new SqlConnection(connectionString)) {
				conn.Open();
				string query = "UPDATE Accounts SET OTP_Code = @otp WHERE Email = @email";
				using (SqlCommand cmd = new SqlCommand(query, conn)) {
					cmd.Parameters.AddWithValue("@otp", otp);
					cmd.Parameters.AddWithValue("@email", email);
					cmd.ExecuteNonQuery();
				}
			}
		} catch (Exception e) { GD.PrintErr(e.Message); }
	}
	public static bool VerifyOTP(string email, string otpInput)
	{
		 try {
			using (SqlConnection conn = new SqlConnection(connectionString)) {
				conn.Open();
				string query = "SELECT COUNT(*) FROM Accounts WHERE Email = @email AND OTP_Code = @otp";
				using (SqlCommand cmd = new SqlCommand(query, conn)) {
					cmd.Parameters.AddWithValue("@email", email);
					cmd.Parameters.AddWithValue("@otp", otpInput);
					int count = (int)cmd.ExecuteScalar();
					return count > 0; 
				}
			}
		} catch { return false; }
	}
	public static void ResetPassword(string email, string newPass)
	{
		  try {
			using (SqlConnection conn = new SqlConnection(connectionString)) {
				conn.Open();
				string query = "UPDATE Accounts SET Password = @p, OTP_Code = NULL WHERE Email = @e";
				using (SqlCommand cmd = new SqlCommand(query, conn)) {
					cmd.Parameters.AddWithValue("@p", newPass);
					cmd.Parameters.AddWithValue("@e", email);
					cmd.ExecuteNonQuery();
				}
			}
		} catch (Exception e) { GD.PrintErr(e.Message); }
	}
}

public class LeaderboardItem
{
	public string Name { get; set; }
	public int Score { get; set; }
}
