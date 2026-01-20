using Godot;
using System;

public partial class UserData : Node
{
	public int BestScore { get; set; } = 0;

	public override void _Ready()
	{
		// Lấy điểm cao nhất từ lúc đăng nhập để so sánh
		BestScore = GlobalData.CurrentBestScore;
	}

		public void CheckAndSave(int currentScore)
	{
		// Mặc định là chưa có kỷ lục mới
		GlobalData.IsNewRecord = false; 

		// Nếu điểm hiện tại cao hơn kỷ lục cũ
		if (currentScore > BestScore)
		{
			// 1. BẬT CỜ LÊN
			GlobalData.IsNewRecord = true; 

			// 2. Cập nhật điểm như cũ
			BestScore = currentScore;
			GlobalData.CurrentBestScore = BestScore; 

			// 3. Lưu SQL (Code cũ của ông)
			if (currentScore > 0 && !string.IsNullOrEmpty(GlobalData.CurrentUser))
			{
				DatabaseManager.SaveScore(GlobalData.CurrentUser, BestScore);
			}
		}
	}

}
