using Godot;
using System;

public partial class Main : Node2D
{
<<<<<<< Updated upstream
	// Called when the node enters the scene tree for the first time.
=======
	//Khởi tạo trạng thái trò chơi & hiển thị UI
	private bool play=false;
	private bool gameover=false;
	private bool pause=false;
	private bool menu=true;
	private Player player;
	private PipeSpawner pipes;
	private CanvasLayer gameOverUI;
	private TextureRect pausebtn;
	
>>>>>>> Stashed changes
	public override void _Ready()
	{
	}

	//Trạng thái trò chơi
	public override void _Process(double delta)
	{
	}
}
