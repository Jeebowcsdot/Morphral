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
						_collisionBody.RotationDegrees = 7.4f;
						_collisionPelvis.Position = new Vector2(-313.0f, 55.0f);
						_collisionPelvis.RotationDegrees = 93.4f;
						_collisionLegL.Position = new Vector2(-413.0f, 195.0f);
						_collisionLegL.RotationDegrees = 20.6f;
						_collisionLegR.Position = new Vector2(-192.0f, 184.0f);
						_collisionLegR.RotationDegrees = -21.2f;
						break;
					case 1:
						_collisionBody.Position = new Vector2(-297.5f, -135.0f);
						_collisionBody.RotationDegrees = 14.4f;
						_collisionPelvis.Position = new Vector2(-312.5f, 37.5f);
						_collisionPelvis.RotationDegrees = 81.9f;
						_collisionLegL.Position = new Vector2(-413.0f, 195.0f);
						_collisionLegL.RotationDegrees = 20.6f;
						_collisionLegR.Position = new Vector2(-135.0f, 167.5f);
						_collisionLegR.RotationDegrees = -21.9f;
						break;
					case 2:
						_collisionBody.Position = new Vector2(-187.5f, -135.0f);
						_collisionBody.RotationDegrees = -8.4f;
						_collisionPelvis.Position = new Vector2(-172.5f, 75.0f);
						_collisionPelvis.RotationDegrees = 81.9f;
						_collisionLegL.Position = new Vector2(-267.5f, 185.0f);
						_collisionLegL.RotationDegrees = 22.7f;
						_collisionLegR.Position = new Vector2(-92.5f, 155.0f);
						_collisionLegR.RotationDegrees = -11.3f;
						break;
					case 3:
						_collisionBody.Position = new Vector2(-137.5f, -152.5f);
						_collisionBody.RotationDegrees = -22.3f;
						_collisionPelvis.Position = new Vector2(-37.5f, 20.0f);
						_collisionPelvis.RotationDegrees = 81.9f;
						_collisionLegL.Position = new Vector2(-55.0f, 185.0f);
						_collisionLegL.RotationDegrees = -6.5f;
						_collisionLegR.Position = new Vector2(-92.5f, 155.0f);
						_collisionLegR.RotationDegrees = -11.3f;
						break;
					case 4:
						_collisionBody.Position = new Vector2(-137.5f, -210.0f);
						_collisionBody.RotationDegrees = -33.1f;
						_collisionPelvis.Position = new Vector2(0.0f, -132.5f);
						_collisionPelvis.RotationDegrees = 22.7f;
						_collisionLegL.Position = new Vector2(-20.0f, 185.0f);
						_collisionLegL.RotationDegrees = -0.6f;
						_collisionLegR.Position = new Vector2(-20.0f, 185.0f);
						_collisionLegR.RotationDegrees = -0.6f;
						break;
					case 5:
						
				}
		}
	}
}
