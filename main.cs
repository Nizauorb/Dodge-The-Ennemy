using Godot;
using System;


public partial class main : Node2D
{
    
	[Export]
    public PackedScene EnnemyScene { get; set; }

    private int _score;	
	
	public void _on_joueur_hit()
	{
		GetNode<Timer>("EnnemyTimer").Stop();
		GetNode<Timer>("ScoreTimer").Stop();
		GetNode<hud>("HUD").ShowGameOver();
	}

	public void NewGame()
	{
		_score = 0;

		var player = GetNode<joueur>("Joueur");
		var startPosition = GetNode<Marker2D>("StartPosition");
		player.Start(startPosition.Position);
		var hud = GetNode<hud>("HUD");
		hud.UpdateScore(_score);
		hud.ShowMessage("Get Ready!");
		// Note that for calling Godot-provided methods with strings,
		// we have to use the original Godot snake_case name.
		GetTree().CallGroup("ennemy", Node.MethodName.QueueFree);

		GetNode<Timer>("StartTimer").Start();

		
	}

	private void _on_score_timer_timeout()
	{
		_score++;
		GetNode<hud>("HUD").UpdateScore(_score);
	}

	private void _on_start_timer_timeout()
	{
		GetNode<Timer>("EnnemyTimer").Start();
		GetNode<Timer>("ScoreTimer").Start();
	}

	private void _on_ennemy_timer_timeout()
	{
		// Note: Normally it is best to use explicit types rather than the `var`
		// keyword. However, var is acceptable to use here because the types are
		// obviously Mob and PathFollow2D, since they appear later on the line.

		// Create a new instance of the Mob scene.
		ennemy ennemy = EnnemyScene.Instantiate<ennemy>();

		// Choose a random location on Path2D.
		var ennemySpawnLocation = GetNode<PathFollow2D>("EnnemyPath/EnnemySpawnPosition");
		ennemySpawnLocation.ProgressRatio = GD.Randf();

		// Set the mob's direction perpendicular to the path direction.
		float direction = ennemySpawnLocation.Rotation + Mathf.Pi / 2;

		// Set the mob's position to a random location.
		ennemy.Position = ennemySpawnLocation.Position;

		// Add some randomness to the direction.
		direction += (float)GD.RandRange(-Mathf.Pi / 4, Mathf.Pi / 4);
		ennemy.Rotation = direction;

		// Choose the velocity.
		var velocity = new Vector2((float)GD.RandRange(150.0, 250.0), 0);
		ennemy.LinearVelocity = velocity.Rotated(direction);

		// Spawn the mob by adding it to the Main scene.
		AddChild(ennemy);
	}

	private void _on_hud_start_game()
	{
		NewGame();
	}
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
