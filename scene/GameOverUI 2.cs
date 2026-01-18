using Godot;
using System;

public partial class GameOverUI : CanvasLayer
{
	//Khởi tạo đối tượng của cái label 
	private Player player;
	private RichTextLabel score;
	private RichTextLabel best;
	private TextureRect status;
	private TextureRect copper;
	private TextureRect silver;
	private TextureRect gold;
	private UserData data;
	
	//Link với ảnh rồi player
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
		
	}
	
	//Chạy các hàm
	public override void _Process(double delta)
	{
		ShowText();
		ShowStatus();
		ShowMedal();
	}
	
	//Hiện điểm với điểm cao nhất
	public void ShowText()
	{
		score.Text = player.point.ToString(); 
		best.Text = data.BestScore.ToString();
	}
	
	//Hiển thị chữ new nếu có best score mới
	public void ShowStatus()
	{
		if (data.BestScore != player.point)
			status.Hide();
		else
			status.Show();;
	}
	
	//Hiển thị huy chương
	public void ShowMedal()
	{
		if (player.point >=0)
			copper.Show();
		if (player.point >=5){
			copper.Hide();
			silver.Show();
		}
		if (player.point >= 10){
			silver.Hide();
			gold.Show();
		}
	}
	
}
