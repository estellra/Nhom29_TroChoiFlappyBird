using Godot;
using System;

public partial class Main : Node2D
{
	private bool play=false;
	private bool gameover=false;
	private bool pause=false;
	private bool menu=true;
	private Player player;
	private PipeSpawner pipes;
	private CanvasLayer gameOverUI;
	private TextureRect pausebtn;
	private UserData userData;
	
		/*public override void _Input(InputEvent input)
	{
		if(!gameover) return;
		if (input is InputEventKey keyEvent && keyEvent.Pressed && !keyEvent.Echo)
		{
			pipes.ClearPipe();
			player.Start();
			gameover = false;
			play = true;
			menu=false;
		}
	}*/
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		 player = GetNode<Player>("Player");
		 pipes = GetNode<PipeSpawner>("PipeSpawner");
		gameOverUI = GetNode<CanvasLayer>("GameOverUI");
		pausebtn = GetNode<TextureRect>("GamePausedUI/PauseBtn");
		userData = GetNode<UserData>("UserData");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if(menu)
		{
			StartGame();
			return;
		}
		if(play)
		{
			if(player.dead)
			{
				GameOver();
			}
			return;
		}
		if(pause)
		{
			
			return;
		}
		
	}
	
	public void StartGame()
	{
		
		play=true;
		gameover=false;
		pause=false;
		menu=false;
		player.Start();
	}
	
	public void GameOver()
	{
		play=false;
		gameover=true;
		pause=false;
		menu=false;
		
		if (userData != null)
		{
			GD.Print("Đang gọi UserData để lưu...");
			userData.CheckAndSave(player.point);
		}
		else
		{
			GD.Print("LỖI: Không tìm thấy Node UserData!");
		}
		gameOverUI.Show();
		pausebtn.Hide();
		GetNode<TextureRect>("GameOverUI/ScorePanel").Show();
	}
}
