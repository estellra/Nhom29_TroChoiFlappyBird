using Godot;
using System;
using Godot.Collections;

public partial class Global : Node
{
	public override void _Ready()
	{
		var silentWolf = GetNode<Node>("/root/SilentWolf");

		var config = new Dictionary
		{
			{ "api_key", "8sexJCDfMD6TH64p53OPP4L8jVqfqRLH7Go6PIeJ" },
			{ "game_id", "flappybird5" },
			{"log_level", 1},
		};

		silentWolf.Call("configure", config);

		var scoreConfig = new Dictionary
		{
			{ "open_scene_on_close", "res://scene/Leaderboard.tscn" }
		};

		silentWolf.Call("configure_scores", scoreConfig);
	}
	
	//silentWolf.Call("persist_score",data.BestScore,data.UserName,"Test");
}
