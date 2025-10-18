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
	private float timer=(float)2.5;
	public List<Pipe> PipeList=new List<Pipe>();
	
	public override void _Ready()
	{
		Player=GetNode<Node2D>("../Player");
		PipeSpawn=(PackedScene)ResourceLoader.Load("res://scene/pipe.tscn");
	}
	public override void _Process(double delta)
	{
		timer-=(float)delta;
		if(timer<=(float)0)
		{
			SpawnPipe();
			timer=(float)2.5;
		}
	}

	private void SpawnPipe()
	{
		Pipe pipe = PipeSpawn.Instantiate<Pipe>();
		GetParent().AddChild(pipe); 
		float centerY = (float)rng.NextDouble() * (MaxY - MinY) + MinY;
		pipe.Setup(centerY, SpawnGap);
		pipe.Position = new Vector2(Player.GlobalPosition.X + 500, 0);
		PipeList.Add(pipe);
	}
	
	public void RemovePipe(Pipe pipe)
	{
		PipeList.Remove(pipe);
	}
	
}
