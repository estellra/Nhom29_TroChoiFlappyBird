using Godot;
using System;
using Godot.Collections;

public partial class ProfileUI : CanvasLayer
{
	private LineEdit name;
	private UserData data;
	private Node silentWolf;
	private GodotObject scores;
	
	public override void _Ready()
	{
		name = GetNode<LineEdit>("ProfilePanel/ProfileText");
		data = GetNode<UserData>("../UserData");
		silentWolf = GetNode<Node>("/root/SilentWolf");
		scores = (GodotObject)silentWolf.Get("Scores"); 
		SetUpName();
		SetUpGlobal();
	}
	
	private void SetUpName(){
		name.FocusMode = Control.FocusModeEnum.All; 
		name.Editable = true;
		name.TextSubmitted += OnNameSubmitted;
		name.CallDeferred(nameof(FocusMe));
		
	}
	
	private void SetUpGlobal(){
		var config = new Dictionary
		{
			{ "api_key", "8sexJCDfMD6TH64p53OPP4L8jVqfqRLH7Go6PIeJ" },
			{ "game_id", "flappybird5" },
			{"log_level", 3},
		};

		silentWolf.Call("configure", config);

		var scoreConfig = new Dictionary
		{
			{ "open_scene_on_close", "res://scene/Leaderboard.tscn" }
		};

		silentWolf.Call("configure_scores", scoreConfig);
	}
	
	private void FocusMe()
	{
		name.GrabFocus();
	}

	private void OnNameSubmitted(string text)
	{
		data.UserName = text.Trim();
		GD.Print(data.UserName);
		scores.Call("save_score", data.BestScore, data.UserName);
  		scores.Call("persist_score", data.BestScore, data.UserName, "Test");

		scores.Call("persist_score", 10, "John", "Test");
	}
}
