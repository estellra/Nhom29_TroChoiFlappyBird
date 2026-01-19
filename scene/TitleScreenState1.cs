using Godot;
using System;

public partial class TitleScreenState1 : RichTextLabel
{
	private Control PressAnyKeyUI;
	private Control TitleScreenMenu;
	public override void _Ready()
	{
		PressAnyKeyUI=GetNode<RichTextLabel>("../TitleScreenState1");
		TitleScreenMenu=GetNode<VBoxContainer>("../TItleScreenMenu");
		if (PressAnyKeyUI != null)
		{
			PressAnyKeyUI.Visible = true;
		}

		if (TitleScreenMenu != null)
		{
			TitleScreenMenu.Visible = false;
		}
	}

	public override void _Input(InputEvent @event)
	{
		if (TitleScreenMenu != null && TitleScreenMenu.Visible) return;
		if (@event is InputEventKey keyEvent && keyEvent.IsPressed())
		{			
			ShowMenu();
		}
		else if (@event is InputEventMouseButton mouseEvent && mouseEvent.IsPressed())
		{
			ShowMenu();
		}
	   
		
	}

	private void ShowMenu()
	{
		if (TitleScreenMenu != null && !TitleScreenMenu.Visible)
		{
			GetViewport().SetInputAsHandled();
			PressAnyKeyUI.Visible = false;
			TitleScreenMenu.Visible = true;
		}
	}
}
