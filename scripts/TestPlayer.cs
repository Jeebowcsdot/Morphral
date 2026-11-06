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
		_collisionBody = GetNode<CollisionShape2D>("CollisionBody");
		_collisionPelvis = GetNode<CollisionShape2D>("CollisionPelvis");
		_collisionLegL = GetNode<CollisionShape2D>("CollisionLegL");
		_collisionLegR = GetNode<CollisionShape2D>("CollisionLegR");
		
		// Setting the Shape for all CollisionShape2D Nodes
		// _collisionBody.Shape = new CapsuleShape2D();
		// _collisionPelvis.Shape = new CapsuleShape2D();
		// _collisionLegL.Shape = new CapsuleShape2D();
		// _collisionLegR.Shape = new CapsuleShape2D();
		
		_sprite.Play("Idle");
		updateCollision();

		// Signals
		_sprite.FrameChanged += updateCollision;
		_sprite.AnimationFinished += OnAnimationFinished;
	}

	public override void _PhysicsProcess(double delta)
	{
		_direction = Input.GetVector("MoveLeft", "MoveRight", "MoveUp", "MoveDown");
		Velocity = _direction * _speed;
		MoveAndSlide();
	}

	public override void _Input(InputEvent @event)
	{
		if (Input.IsActionJustPressed("Attack"))
		{
			_sprite.Play("Kick");
		}
	}

	private void updateCollision()
	{
		switch (_sprite.Animation)
		{
			case "Idle":
				_collisionBody.Position = new Vector2(-313.0f, -149.0f);
				_collisionBody.RotationDegrees = 7.4f;
				((CapsuleShape2D)GetNode<CollisionShape2D>("CollisionBody").Shape).Radius = 75.0f;
				((CapsuleShape2D)GetNode<CollisionShape2D>("CollisionBody").Shape).Height = 690.0f;
				_collisionPelvis.Position = new Vector2(-313.0f, 55.0f);
				_collisionPelvis.RotationDegrees = 93.4f;
				((CapsuleShape2D)GetNode<CollisionShape2D>("CollisionPelvis").Shape).Radius = 121.0f;
				((CapsuleShape2D)GetNode<CollisionShape2D>("CollisionPelvis").Shape).Height = 323.0f;
				_collisionLegL.Position = new Vector2(-413.0f, 195.0f);
				_collisionLegL.RotationDegrees = 20.6f;
				((CapsuleShape2D)GetNode<CollisionShape2D>("CollisionLegL").Shape).Radius = 37.0f;
				((CapsuleShape2D)GetNode<CollisionShape2D>("CollisionLegL").Shape).Height = 795.0f;
				_collisionLegR.Position = new Vector2(-192.0f, 184.0f);
				_collisionLegR.RotationDegrees = -21.2f;
				((CapsuleShape2D)GetNode<CollisionShape2D>("CollisionLegR").Shape).Radius = 37.0f;
				((CapsuleShape2D)GetNode<CollisionShape2D>("CollisionLegR").Shape).Height = 795.0f;
				break;
			case "Kick":
				switch (_sprite.Frame)
				{
					case 0:
						_collisionBody.Position = new Vector2(-313.0f, -149.0f);
						_collisionBody.RotationDegrees = 7.4f;
						((CapsuleShape2D)GetNode<CollisionShape2D>("CollisionBody").Shape).Radius = 75.0f;
						((CapsuleShape2D)GetNode<CollisionShape2D>("CollisionBody").Shape).Height = 690.0f;
						_collisionPelvis.Position = new Vector2(-313.0f, 55.0f);
						_collisionPelvis.RotationDegrees = 93.4f;
						((CapsuleShape2D)GetNode<CollisionShape2D>("CollisionPelvis").Shape).Radius = 121.0f;
						((CapsuleShape2D)GetNode<CollisionShape2D>("CollisionPelvis").Shape).Height = 323.0f;
						_collisionLegL.Position = new Vector2(-413.0f, 195.0f);
						_collisionLegL.RotationDegrees = 20.6f;
						((CapsuleShape2D)GetNode<CollisionShape2D>("CollisionLegL").Shape).Radius = 37.0f;
						((CapsuleShape2D)GetNode<CollisionShape2D>("CollisionLegL").Shape).Height = 795.0f;
						_collisionLegR.Position = new Vector2(-192.0f, 184.0f);
						_collisionLegR.RotationDegrees = -21.2f;
						((CapsuleShape2D)GetNode<CollisionShape2D>("CollisionLegR").Shape).Radius = 37.0f;
						((CapsuleShape2D)GetNode<CollisionShape2D>("CollisionLegR").Shape).Height = 795.0f;
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
						_collisionBody.Position = new Vector2(-112.5f, -182.5f);
						_collisionBody.RotationDegrees = -51.2f;
						((CapsuleShape2D)GetNode<CollisionShape2D>("CollisionBody").Shape).Height = 576.0f;
						_collisionPelvis.Position = new Vector2(22.5f, -145.0f);
						_collisionPelvis.RotationDegrees = 22.7f;
						((CapsuleShape2D)GetNode<CollisionShape2D>("CollisionPelvis").Shape).Radius = 109.0f;
						((CapsuleShape2D)GetNode<CollisionShape2D>("CollisionPelvis").Shape).Height = 218.0f;
						_collisionLegL.Position = new Vector2(85.0f, 212.5f);
						_collisionLegL.RotationDegrees = 0.2f;
						((CapsuleShape2D)GetNode<CollisionShape2D>("CollisionLegL").Shape).Height = 745.0f;
						_collisionLegR.Position = new Vector2(90.0f, -237.5f);
						_collisionLegR.RotationDegrees = 52.0f;
						((CapsuleShape2D)GetNode<CollisionShape2D>("CollisionLegR").Shape).Height = 501.0f;
						break;
					case 6:
						_collisionBody.Position = new Vector2(-115.0f, -182.5f);
						_collisionBody.Rotation = -40.5f;
						_collisionPelvis.Position = new Vector2(-2.5f, -105.0f);
						// _collisionPelvis.RotationDegrees = 22.7f;
						_collisionLegL.Position = new Vector2(77.5f, 167.5f);
						_collisionLegL.RotationDegrees = 0.2f;
						_collisionLegR.Position = new Vector2(-2.5f, -105.0f);
						_collisionLegR.RotationDegrees = 22.7f;
						((CapsuleShape2D)GetNode<CollisionShape2D>("CollisionLegR").Shape).Height = 329.0f;
						break;
					case 7:
						_collisionBody.Position = new Vector2(-172.5f, -190.0f);
						_collisionBody.RotationDegrees = -35.0f;
						((CapsuleShape2D)GetNode<CollisionShape2D>("CollisionBody").Shape).Height = 576.0f;
						_collisionPelvis.Position = new Vector2(-75.0f, 218.0f);
						// _collisionPelvis.RotationDegrees = 22.7f;
						((CapsuleShape2D)GetNode<CollisionShape2D>("CollisionPelvis").Shape).Radius = 109.0f;
						((CapsuleShape2D)GetNode<CollisionShape2D>("CollisionPelvis").Shape).Height = 218.0f;
						_collisionLegL.Position = new Vector2(-65.0f, 185.0f);
						_collisionLegL.RotationDegrees = 0.2f;
						((CapsuleShape2D)GetNode<CollisionShape2D>("CollisionLegL").Shape).Height = 745.0f;
						_collisionLegR.Position = new Vector2(-220.0f, 92.5f);
						_collisionLegR.RotationDegrees = 41.2f;
						((CapsuleShape2D)GetNode<CollisionShape2D>("CollisionLegR").Shape).Height = 634.5f;
						break;
				}

				break;
		}
	}

	private void OnAnimationFinished()
	{
		if (_sprite.Animation == "Kick")
		{
			_sprite.Play("Idle");
		}
	}
}
