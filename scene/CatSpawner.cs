using Godot;
using System;
using System.Collections.Generic;

public partial class CatSpawner : Node2D
{
	public PackedScene CatSpawn;
	public Node2D Player;
	private Random rng = new Random();
	private float timer=(float)5;
	public List<Cat> CatList=new List<Cat>();
	public bool stop;
	public bool reset;
	
	public override void _Ready()
	{
		Player=GetNode<Node2D>("../Player");
		CatSpawn=(PackedScene)ResourceLoader.Load("res://scene/cat.tscn");
		stop=false;
		reset=false;
	}
	public override void _Process(double delta)
	{
		if(stop) return;
		timer-=(float)delta;
		if(timer<=(float)0)
		{
			SpawnCat();
			timer=(float)2.5 ;
		}
	}

	private void SpawnCat()
	{
		Cat cat = CatSpawn.Instantiate<Cat>();
		GetParent().AddChild(cat); 
		float posy = (float)rng.NextDouble() * (GetViewportRect().Size.Y-500);
		cat.Setup(posy);
		cat.Position = new Vector2(Player.GlobalPosition.X + 1000, 0);
		CatList.Add(cat);
	}
	
	public void RemoveCat(Cat cat)
	{
		CatList.Remove(cat);
	}
	public void ClearCat()
	{
		foreach (var cat in CatList)
		{
			if (IsInstanceValid(cat))
			cat.QueueFree();
		}
		CatList.Clear();
		reset = false;
	}
	
}
