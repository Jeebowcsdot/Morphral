using Godot;

namespace Morph.Scripts;

/// <summary>
/// The script with all the attacks and features of the base player
/// </summary>
public partial class TestPlayer : Player
{
	private AnimatedSprite2D _sprite;
	private CollisionShape2D _collisionBody;
	private CollisionShape2D _collisionPelvis;
	private CollisionShape2D _collisionLegL;
	private CollisionShape2D _collisionLegR;
	private CollisionShape2D _dmgCollision;

	private RandomNumberGenerator _rng;
	
	public override void _Ready()
	{
		// Loads everything inside _Ready() from Player.cs
		base._Ready();
		
		// Setting all Node Variables
		_sprite = GetNode<AnimatedSprite2D>("PlayerSpriteAnimated");
		_collisionBody = GetNode<CollisionShape2D>("CollisionBody");
		_collisionPelvis = GetNode<CollisionShape2D>("CollisionPelvis");
		_collisionLegL = GetNode<CollisionShape2D>("CollisionLegL");
		_collisionLegR = GetNode<CollisionShape2D>("CollisionLegR");
		_dmgCollision = GetNode<CollisionShape2D>("DmgArea/DmgCollision");
		
		// Signals
		_sprite.FrameChanged += UpdateCollision;
		_sprite.AnimationFinished += OnAnimationFinished;
		
		// Basic tasks to do on Program start
		_sprite.Play("Idle");
		DamageArea.Hide();
		UpdateCollision();
	}

	public override void _Input(InputEvent @event)
	{
		if (Input.IsActionJustPressed("Attack"))
		{
			_sprite.Play("Kick");
		}
	}
	
	private void OnAnimationFinished()
	{
		if (_sprite.Animation == "Kick")
		{
			_sprite.Play("Idle");
		}
	}
	
	private void UpdateCollision()
	{
		switch (_sprite.Animation)
		{
			case "Idle":
				_collisionBody.Position = new Vector2(-3.0f, -106.5f);
				_collisionBody.RotationDegrees = 7.4f;
				((CapsuleShape2D)GetNode<CollisionShape2D>("CollisionBody").Shape).Radius = 75.0f;
				((CapsuleShape2D)GetNode<CollisionShape2D>("CollisionBody").Shape).Height = 690.0f;
				_collisionPelvis.Position = new Vector2(-3.0f, 97.5f);
				_collisionPelvis.RotationDegrees = 93.4f;
				((CapsuleShape2D)GetNode<CollisionShape2D>("CollisionPelvis").Shape).Radius = 121.0f;
				((CapsuleShape2D)GetNode<CollisionShape2D>("CollisionPelvis").Shape).Height = 323.0f;
				_collisionLegL.Position = new Vector2(-103.0f, 237.5f);
				_collisionLegL.RotationDegrees = 20.6f;
				((CapsuleShape2D)GetNode<CollisionShape2D>("CollisionLegL").Shape).Radius = 37.0f;
				((CapsuleShape2D)GetNode<CollisionShape2D>("CollisionLegL").Shape).Height = 795.0f;
				_collisionLegR.Position = new Vector2(118.0f, 226.5f);
				_collisionLegR.RotationDegrees = -21.2f;
				((CapsuleShape2D)GetNode<CollisionShape2D>("CollisionLegR").Shape).Radius = 37.0f;
				((CapsuleShape2D)GetNode<CollisionShape2D>("CollisionLegR").Shape).Height = 795.0f;
				break;
			case "Kick":
				switch (_sprite.Frame)
				{
					case 0:
						_collisionBody.Position = new Vector2(-3.0f, -106.5f);
						_collisionBody.RotationDegrees = 7.4f;
						((CapsuleShape2D)GetNode<CollisionShape2D>("CollisionBody").Shape).Radius = 75.0f;
						((CapsuleShape2D)GetNode<CollisionShape2D>("CollisionBody").Shape).Height = 690.0f;
						_collisionPelvis.Position = new Vector2(-3.0f, 97.5f);
						_collisionPelvis.RotationDegrees = 93.4f;
						((CapsuleShape2D)GetNode<CollisionShape2D>("CollisionPelvis").Shape).Radius = 121.0f;
						((CapsuleShape2D)GetNode<CollisionShape2D>("CollisionPelvis").Shape).Height = 323.0f;
						_collisionLegL.Position = new Vector2(-103.0f, 237.5f);
						_collisionLegL.RotationDegrees = 20.6f;
						((CapsuleShape2D)GetNode<CollisionShape2D>("CollisionLegL").Shape).Radius = 37.0f;
						((CapsuleShape2D)GetNode<CollisionShape2D>("CollisionLegL").Shape).Height = 795.0f;
						_collisionLegR.Position = new Vector2(118.0f, 226.5f);
						_collisionLegR.RotationDegrees = -21.2f;
						((CapsuleShape2D)GetNode<CollisionShape2D>("CollisionLegR").Shape).Radius = 37.0f;
						((CapsuleShape2D)GetNode<CollisionShape2D>("CollisionLegR").Shape).Height = 795.0f;
						break;
					case 1:
						_collisionBody.Position = new Vector2(12.5f, -92.5f);
						_collisionBody.RotationDegrees = 14.4f;
						_collisionPelvis.Position = new Vector2(-2.5f, 80.0f);
						_collisionPelvis.RotationDegrees = 81.9f;
						_collisionLegL.Position = new Vector2(-103.0f, 152.5f);
						_collisionLegL.RotationDegrees = 20.6f;
						_collisionLegR.Position = new Vector2(175.0f, 210.0f);
						_collisionLegR.RotationDegrees = -21.9f;
						break;
					case 2:
						_collisionBody.Position = new Vector2(122.5f, -92.5f);
						_collisionBody.RotationDegrees = -8.4f;
						_collisionPelvis.Position = new Vector2(115.0f, 62.5f);
						_collisionPelvis.RotationDegrees = -66.4f;
						_collisionLegL.Position = new Vector2(17.5f, 270.0f);
						_collisionLegL.RotationDegrees = 22.7f;
						_collisionLegR.Position = new Vector2(235.0f, 245.0f);
						_collisionLegR.RotationDegrees = -11.3f;
						break;
					case 3:
						_collisionBody.Position = new Vector2(167.5f, -117.5f);
						_collisionBody.RotationDegrees = -21.6f;
						_collisionPelvis.Position = new Vector2(272.5f, 62.5f);
						_collisionPelvis.RotationDegrees = 81.9f;
						_collisionLegL.Position = new Vector2(327.5f, 270.0f);
						_collisionLegL.RotationDegrees = 5.8f;
						((CapsuleShape2D)GetNode<CollisionShape2D>("CollisionLegL").Shape).Height = 675.0f;
						_collisionLegR.Position = new Vector2(235.0f, 245.0f);
						_collisionLegR.RotationDegrees = -11.3f;
						break;
					case 4:
						_dmgCollision.Position = new Vector2(837.5f, -457.5f);
						_dmgCollision.RotationDegrees = 50.3f;
						((CapsuleShape2D)GetNode<CollisionShape2D>("DmgArea/DmgCollision").Shape).Radius = 57.0f;
						((CapsuleShape2D)GetNode<CollisionShape2D>("DmgArea/DmgCollision").Shape).Height = 218.0f;
						DamageArea.Show();
						DamageArea.Monitoring = true;
						_collisionBody.Position = new Vector2(172.5f, -167.5f);
						_collisionBody.RotationDegrees = -33.1f;
						_collisionPelvis.Position = new Vector2(332.5f, -92.5f);
						_collisionPelvis.RotationDegrees = 22.7f;
						_collisionLegL.Position = new Vector2(287.5f, 305.0f);
						_collisionLegL.RotationDegrees = -0.6f;
						_collisionLegR.Position = new Vector2(522.5f, -197.5f);
						_collisionLegR.RotationDegrees = -128.8f;
						break;
					case 5:
						DamageArea.Hide();
						DamageArea.Monitoring = false;
						_collisionBody.Position = new Vector2(202.5f, -190.0f);
						_collisionBody.RotationDegrees = -51.2f;
						((CapsuleShape2D)GetNode<CollisionShape2D>("CollisionBody").Shape).Height = 576.0f;
						_collisionPelvis.Position = new Vector2(332.5f, -102.5f);
						_collisionPelvis.RotationDegrees = 22.7f;
						((CapsuleShape2D)GetNode<CollisionShape2D>("CollisionPelvis").Shape).Radius = 109.0f;
						((CapsuleShape2D)GetNode<CollisionShape2D>("CollisionPelvis").Shape).Height = 218.0f;
						_collisionLegL.Position = new Vector2(395.0f, 255.0f);
						_collisionLegL.RotationDegrees = 0.2f;
						((CapsuleShape2D)GetNode<CollisionShape2D>("CollisionLegL").Shape).Height = 745.0f;
						_collisionLegR.Position = new Vector2(400.0f, -194.5f);
						_collisionLegR.RotationDegrees = 52.0f;
						((CapsuleShape2D)GetNode<CollisionShape2D>("CollisionLegR").Shape).Height = 501.0f;
						break;
					case 6:
						_collisionBody.Position = new Vector2(195.0f, -140.0f);
						_collisionBody.RotationDegrees = -40.5f;
						_collisionPelvis.Position = new Vector2(332.5f, -92.5f);
						_collisionLegL.Position = new Vector2(387.5f, 210.0f);
						_collisionLegL.RotationDegrees = 0.2f;
						_collisionLegR.Position = new Vector2(307.5f, -62.5f);
						_collisionLegR.RotationDegrees = 22.7f;
						((CapsuleShape2D)GetNode<CollisionShape2D>("CollisionLegR").Shape).Height = 329.0f;
						break;
					case 7:
						_collisionBody.Position = new Vector2(207.5f, -167.5f);
						_collisionBody.RotationDegrees = -35.0f;
						((CapsuleShape2D)GetNode<CollisionShape2D>("CollisionBody").Shape).Height = 576.0f;
						_collisionPelvis.Position = new Vector2(297.5f, -60.0f);
						((CapsuleShape2D)GetNode<CollisionShape2D>("CollisionPelvis").Shape).Radius = 109.0f;
						((CapsuleShape2D)GetNode<CollisionShape2D>("CollisionPelvis").Shape).Height = 218.0f;
						_collisionLegL.Position = new Vector2(352.5f, 270.0f);
						_collisionLegL.RotationDegrees = 0.2f;
						((CapsuleShape2D)GetNode<CollisionShape2D>("CollisionLegL").Shape).Height = 745.0f;
						_collisionLegR.Position = new Vector2(205.0f, 132.5f);
						_collisionLegR.RotationDegrees = 41.2f;
						((CapsuleShape2D)GetNode<CollisionShape2D>("CollisionLegR").Shape).Height = 634.5f;
						break;
				}
				break;
		}
	}
}
