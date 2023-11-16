using Godot;
using System;
using System.Runtime.CompilerServices;

public partial class joueur : Area2D
{
	[Export]
	 public int Speed { get; set; } = 400;

	[Signal]
	public delegate void HitEventHandler();

	private AnimatedSprite2D animatedSprite2D;

	private CollisionShape2D collisionShape2D;

	private Vector2 velocity;

	private Vector2 screenSize;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{	
		Hide();
		screenSize = GetViewportRect().Size;
		animatedSprite2D = (AnimatedSprite2D)GetNode("AnimatedSprite2D");
		collisionShape2D = (CollisionShape2D)GetNode("CollisionShape2D");
	}

	private void _on_body_entered(Node2D body)
	{
		Hide(); // Player disappears after being hit.
		EmitSignal(SignalName.Hit);
		// Must be deferred as we can't change physics properties on a physics callback.
		GetNode<CollisionShape2D>("CollisionShape2D").SetDeferred(CollisionShape2D.PropertyName.Disabled, true);
	}

	public void Start(Vector2 position)
	{
		Position = position;
		Show();
		GetNode<CollisionShape2D>("CollisionShape2D").Disabled = false;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double   delta)
	{
		velocity = new Vector2();
		if(Input.IsActionPressed("move_right"))
		{
			velocity.X += 1;
		}
		if(Input.IsActionPressed("move_left"))
		{
			velocity.X -= 1;
		}
		if(Input.IsActionPressed("move_down"))
		{
			velocity.Y += 1;
		}
		if(Input.IsActionPressed("move_up"))
		{
			velocity.Y -= 1;
		}
		if(velocity.Length() > 0)
		{
			velocity = velocity.Normalized() * Speed;
			animatedSprite2D.Play();
		}
		else
		{
			animatedSprite2D.Stop();
		}

		Position += velocity * (float)delta;

		Position = new Vector2(
			Mathf.Clamp(Position.X, 0, screenSize.X),
			Mathf.Clamp(Position.Y, 0, screenSize.Y)
		);

		if(velocity.X != 0){
			animatedSprite2D.Animation = "walk";
			animatedSprite2D.FlipH = velocity.X < 0;
			animatedSprite2D.FlipV = false;
		}
		else if(velocity.Y != 0)
		{
			animatedSprite2D.Animation = "up";
			animatedSprite2D.FlipV = velocity.Y > 0;
		}


	}	
}
