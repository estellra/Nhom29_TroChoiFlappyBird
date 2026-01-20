using Godot;
using System;
using System.Collections.Generic;

public partial class PipeSpawner : Node2D
{
	public PackedScene PipeSpawn;
	public Node2D Player;
	public float SpawnGap = 180;
	public float MinY = -50;
	public float MaxY = 200;
	private Random rng = new Random();
	private float timer=(float)1.5;
	public List<Pipe> PipeList=new List<Pipe>();
	public bool stop;
	public bool reset;
	public bool medium=false;
	public bool hard=false;
	
	public override void _Ready()
	{
		Player=GetNode<Node2D>("../Player");
		PipeSpawn=(PackedScene)ResourceLoader.Load("res://scene/pipe.tscn");
		stop=false;
		reset=false;
		if(GlobalData.currentdifficulty == GlobalData.difficulty.medium) 
		{
			medium=true;
			hard=false;
		}
		else if(GlobalData.currentdifficulty == GlobalData.difficulty.hard) 
		{
			hard=true;
			medium=false;
		}
	}
	public override void _Process(double delta)
	{
		if(stop) return;
		timer-=(float)delta;
		if(timer<=(float)0)
		{
			SpawnPipe();
			timer=(float)1.5 ;
		}
	}

	private void SpawnPipe()
	{
		Pipe pipe = PipeSpawn.Instantiate<Pipe>();
		GetParent().AddChild(pipe); 
		float centerY = (float)rng.NextDouble() * (MaxY - MinY) + MinY;
		pipe.Setup(centerY, SpawnGap);
		pipe.Position = new Vector2(Player.GlobalPosition.X + 1000, 0);
		PipeList.Add(pipe);
	}
	
	public void RemovePipe(Pipe pipe)
	{
		PipeList.Remove(pipe);
	}
	public void ClearPipe()
	{
		foreach (var pipe in PipeList)
		{
			if (IsInstanceValid(pipe))
			pipe.QueueFree();
		}
		PipeList.Clear();
		reset = false;
	}
	
}
