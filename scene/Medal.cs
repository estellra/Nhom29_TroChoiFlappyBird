using Godot;

public partial class Medal : TextureRect
{
	private TextureRect copper;
	private TextureRect silver;
	private TextureRect gold;
	private Player player;
		
	public override void _Ready()
	{
		player = GetNode<Player>("../../Player"); 
		copper = GetNode<TextureRect>("Copper");
		silver = GetNode<TextureRect>("Silver");
		gold   = GetNode<TextureRect>("Gold");
		
		copper.Hide();
		silver.Hide();
		gold.Hide();
	}

	public override void _Process(double delta)
	{
		int score =  player.point; 
		if (score >=0)
			copper.Show();
		if (score >=1){
			copper.Hide();
			silver.Show();
			}
		if (score >= 2){
			silver.Hide();
			gold.Show();
		}
	}
	}
