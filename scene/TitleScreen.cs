using Godot;
using System;
using System.Collections.Generic;

public partial class TitleScreen : CanvasLayer
{
	private Node2D player;
	private Control title;
	private float playerY;
	private float titleY;
	private float dy = 40;
	private float time = 1.5F;
	private List<RichTextLabel> menus;
	private int index;
	private int hoverindex = -1;
	private string HIGHLIGHT_START_TAG = "[color=yellow]";
	private string HIGHLIGHT_END_TAG = "[/color]";
	private string DEFAULT_COLOR_TAG = "[color=white]";
	private int SkinIdx = 0;
	private int TotalSkin = 3;
	public override void _Ready()
	{
		SkinIdx = GlobalData.SkinDangChon;
		player = GetNode<Node2D>("Player");
		title = GetNode<Control>("Title");
		playerY = player.Position.Y;
		titleY = title.Position.Y;
		movement(player, playerY, dy, time);
		movement(title, titleY, dy, time);
		
		menus = new List<RichTextLabel>();
		index = -1;
		VBoxContainer item = GetNode<VBoxContainer>("TItleScreenMenu");
		int j=0;
		foreach (var i in item.GetChildren())
		{
			if (i is RichTextLabel label)
			{
				int temp=j;
				label.SetMeta("original_text", label.Text);
				label.MouseEntered += () =>
			{
				hoverindex=temp;
				if (index != -1 && index < menus.Count)
					removehighlight(menus[index]);
				  	index = temp;
					updatehighlight();
			};
			label.MouseExited += () =>
			{
				hoverindex=-1;
				removehighlight(label);
				index = -1;
			};
			label.GuiInput += (InputEvent e) =>
			{
				if (e is InputEventMouseButton mb
					&& mb.Pressed
					&& mb.ButtonIndex == MouseButton.Left
					&& hoverindex == temp) 
				{
					enter(temp);
				}
			};
				menus.Add(label);
				j++;
			}
		}
			index = -1;
			updatehighlight();
		var btnLeft = GetNode<TextureButton>("btnLeft");
		var btnRight = GetNode<TextureButton>("btnRight");
		btnLeft.Pressed += OnLeftPressed;
		btnRight.Pressed += OnRightPressed;
	}

	public override void _Process(double delta)
	{
		if (Input.IsActionJustPressed("ui_down"))
		{
			changeindex(1);
		}
		if (Input.IsActionJustPressed("ui_up"))
		{
			changeindex(-1);
		}
		if (Input.IsActionJustPressed("ui_enter"))
		{
			if(hoverindex==-1)return;
			enter(index);
			GetViewport().SetInputAsHandled();
		}
	}

	private void OnLeftPressed()
	{
		SkinIdx--;
		if (SkinIdx < 0)
		{
			SkinIdx = TotalSkin - 1;
		}
		GlobalData.SkinDangChon = SkinIdx; 
	}

	private void OnRightPressed()
	{
		SkinIdx++;
		if (SkinIdx >= TotalSkin)
		{
			SkinIdx = 0;
		}
		GlobalData.SkinDangChon = SkinIdx;
	}

	private void movement(Node node, float Y, float d, float t)
	{
		var tween = CreateTween();
		tween.SetLoops();
		tween.TweenProperty(node, "position:y", Y + d, t)
		.SetTrans(Tween.TransitionType.Sine)
		.SetEase(Tween.EaseType.InOut);

		tween.TweenProperty(node, "position:y", Y, t)
		.SetTrans(Tween.TransitionType.Sine)
		.SetEase(Tween.EaseType.InOut);
	}
	private void changeindex(int delta)
	{
		removehighlight(menus[index]);
		index += delta;
		if (index >= menus.Count)
		{
			index -= menus.Count;
		}
		if (index < 0)
		{
			index += menus.Count;
		}
		updatehighlight();
	}
	private void hover(int newindex)
	{
		if (newindex == index) return;
		removehighlight(menus[index]);
		index = newindex;
		updatehighlight();
	}

	private void updatehighlight()
	{
		if (index < 0 || index > 3) return;
		RichTextLabel current = menus[index];
		string text = current.GetMeta("original_text").AsString();
		string newtext = HIGHLIGHT_START_TAG + text + HIGHLIGHT_END_TAG;
		current.Text = newtext;
	}

	private void removehighlight(RichTextLabel item)
	{
		string text = item.GetMeta("original_text").AsString();
		item.Text = text;
	}

	private void enter(int i)
	{
		switch (i)
		{
			case 0:
				GlobalData.SkinDangChon = SkinIdx;
				SceneTree tree = GetTree();
				tree.ChangeSceneToFile("res://scene/level_select.tscn");
				break;
			case 1:
				GetTree().ChangeSceneToFile("res://scene/ranking.tscn");
				break;
			case 2:
				break;
			case 3:
				GetTree().Quit();
				break;
			case -1:
				index = 0;
				updatehighlight();
				break;
		}
	}
}
