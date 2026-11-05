using Godot;
using System;

/// <summary>
/// ! Initial Positions for the collisionShapes:
/// CollisionBody: (-313.0, -149.0)
/// CollisionPelvis: (-313.0, 55.0)
/// CollisionLegL: (-413.0, 195.0)
/// CollisionLegR: (-192.0, 184.0)
/// </summary>


public partial class TestPlayer : CharacterBody2D
{
	private Vector2 _direction = Vector2.Zero;
	private float _speed = 650.0f;

	private AnimatedSprite2D _sprite;
	private CollisionShape2D _collisionBody;
	private CollisionShape2D _collisionPelvis;
	private CollisionShape2D _collisionLegL;
	private CollisionShape2D _collisionLegR;
	
	public override void _Ready()
	{
		// Setting all Node Variables
		_sprite = GetNode<AnimatedSprite2D>("PlayerSpriteAnimated");
		_collisionBody = GetNode<CollisionShape2D>("COllisionBody");
		_collisionPelvis = GetNode<CollisionShape2D>("CollisionPelvis");
		_collisionLegL = GetNode<CollisionShape2D>("CollisionLegL");
		_collisionLegR = GetNode<CollisionShape2D>("CollisionLegR");

		_sprite.FrameChanged += updateCollision;
	}

	public override void _PhysicsProcess(double delta)
	{
		_direction = Input.GetVector("MoveLeft", "MoveRight", "MoveUp", "MoveDown");
		Velocity = _direction * _speed;
		MoveAndSlide();
	}

	private void updateCollision()
	{
		switch (_sprite.Animation)
		{
			case "Kick":
				switch (_sprite.Frame)
				{
					case 0:
						_collisionBody.Position = new Vector2(-313.0f, -149.0f);
						_collisionPelvis.Position = new Vector2(-313.0f, 55.0f);
						_collisionLegL.Position = new Vector2(-413.0f, 195.0f);
						_collisionLegR.Position = new Vector2(-192.0f, 184.0f);
						break;
					case 1:
						
				}
		}
	}
}
