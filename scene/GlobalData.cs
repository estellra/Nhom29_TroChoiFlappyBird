using Godot;
using System;

public partial class GlobalData : Node
{
	public static int SkinDangChon=0;
	public static string CurrentUser = ""; 
	public static int CurrentBestScore = 0;
	public static int buffspeed=0;
	public static int speedcount=0;
	public static int time=0;
	public static string ResetEmail = "";
	public enum difficulty
	{
		easy,medium,hard
	}
	public static difficulty currentdifficulty;
	public static bool IsNewRecord = false;
}
