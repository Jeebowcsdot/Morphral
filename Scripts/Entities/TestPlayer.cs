using System;
using Godot;

namespace Morph.Scripts;

/// <summary>
/// The script with all the attacks and features of the base player
/// </summary>
public partial class TestPlayer : Player
{
	// Animation
	private AnimatedSprite2D _sprite;
	private CollisionShape2D _collisionBody;
	private CollisionShape2D _collisionPelvis;
	private CollisionShape2D _collisionLegL;
	private CollisionShape2D _collisionLegR;
	private CollisionShape2D _dmgCollision;

	// Combat
	private int _nAttacks = 2;
	
	// Others
	private RandomNumberGenerator _rng;
	
	public override void _Ready()
	{
		// Set to morphed character
		PlayerChar = Characters.TestPlayer;
		
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
		base._Input(@event);
		
		if (Input.IsActionJustPressed("Attack") && CurrentStatus != Status.Tired && !CooldownActions.Contains(Actions.RoundhouseKick))
		{
			_sprite.Play("RoundhouseKick");
			ChangeStamina(-8.0f);
			SpawnCooldownBar(nBar, this, 0.8f, Actions.RoundhouseKick, "Roundhouse Kick");
		}

		if (Input.IsActionJustPressed("Dash") && CurrentStatus != Status.Tired && Direction != Vector2.Zero && !CooldownActions.Contains(Actions.Dash))
		{
			ChangeStamina(-15.0f);
			Timer timer = new Timer();
			timer.WaitTime = 0.1f;
			timer.Timeout += () => OnDashTimeout(timer);
			GetParent<Node2D>().AddChild(timer);
			timer.Start();
			SpawnCooldownBar(nBar, this, 1.0f, Actions.Dash, "Dash");
			Velocity = Direction * Speed * 5.0f;
		}
	}

	private void OnDashTimeout(Timer timer)
	{
		timer.QueueFree();
		Velocity = Vector2.Zero;
	}
	
	private void OnAnimationFinished()
	{
		if (_sprite.Animation == "RoundhouseKick")
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
			case "RoundhouseKick":
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
			case "SpinHookKick":
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
						_collisionBody.Position = new Vector2(402.5f, -50.0f);
						_collisionBody.RotationDegrees = 14.5f;
						((CapsuleShape2D)GetNode<CollisionShape2D>("CollisionBody").Shape).Height = 576.0f;
						_collisionPelvis.Position = new Vector2(365.0f, 87.5f);
						_collisionPelvis.RotationDegrees = -80.7f;
						((CapsuleShape2D)GetNode<CollisionShape2D>("CollisionPelvis").Shape).Height = 241.5f;
						_collisionLegL.Position = new Vector2(302.5f, 357.5f);
						_collisionLegL.RotationDegrees = 14.6f;
						_collisionLegR.Position = new Vector2(572.5f, 247.5f);
						_collisionLegR.RotationDegrees = -35.1f;
						break;
					case 2:
						_collisionBody.Position = new Vector2(322.5f, -5.0f);
						_collisionBody.RotationDegrees = 25.0f;
						_collisionPelvis.Position = new Vector2(317.5f, 145.0f);
						_collisionPelvis.RotationDegrees = -108.5f;
						_collisionLegL.Position = new Vector2(315.0f, 377.5f);
						_collisionLegL.RotationDegrees = 20.0f;
						_collisionLegR.Position = new Vector2(567.5f, 277.5f);
						_collisionLegR.RotationDegrees = -41.8f;
						break;
					case 3:
						_collisionBody.Position = new Vector2(362.5f, -147.5f);
						_collisionBody.RotationDegrees = 28.0f;
						((CapsuleShape2D)GetNode<CollisionShape2D>("CollisionBody").Shape).Height = 576.0f;
						_collisionPelvis.Position = new Vector2(272.5f, -37.5f);
						_collisionPelvis.RotationDegrees = -44.0f;
						((CapsuleShape2D)GetNode<CollisionShape2D>("CollisionPelvis").Shape).Radius = 106.0f;
						((CapsuleShape2D)GetNode<CollisionShape2D>("CollisionPelvis").Shape).Height = 220.0f;
						_collisionLegL.Position = new Vector2(287.5f, 365.0f);
						_collisionLegL.RotationDegrees = -0.7f;
						((CapsuleShape2D)GetNode<CollisionShape2D>("CollisionLegL").Shape).Height = 675.0f;
						_collisionLegR.Position = new Vector2(85.0f, -165.0f);
						_collisionLegR.RotationDegrees = -51.5f;
						((CapsuleShape2D)GetNode<CollisionShape2D>("CollisionLegR").Shape).Radius = 55.0f;
						((CapsuleShape2D)GetNode<CollisionShape2D>("CollisionLegR").Shape).Height = 522.0f;
						_dmgCollision.Position = new Vector2(-197.5f, -387.5f);
						_dmgCollision.RotationDegrees = -48.8f;
						((CapsuleShape2D)GetNode<CollisionShape2D>("DmgArea/DmgCollision").Shape).Radius = 71.0f;
						((CapsuleShape2D)GetNode<CollisionShape2D>("DmgArea/DmgCollision").Shape).Height = 190.0f;
						DamageArea.Monitoring = true;
						DamageArea.Show();
						break;
					case 4:
						DamageArea.Monitoring = false;
						DamageArea.Hide();
						_collisionBody.Position = new Vector2(357.5f, -165.0f);
						_collisionBody.RotationDegrees = 47.1f;
						((CapsuleShape2D)GetNode<CollisionShape2D>("CollisionBody").Shape).Radius = 95.0f;
						_collisionPelvis.Position = new Vector2(170.0f, -30.0f);
						_collisionPelvis.RotationDegrees = -44.0f;
						((CapsuleShape2D)GetNode<CollisionShape2D>("CollisionPelvis").Shape).Radius = 119.0f;
						((CapsuleShape2D)GetNode<CollisionShape2D>("CollisionShape").Shape).Height = 237.0f;
						_collisionLegL.Position = new Vector2(110.0f, 360.0f);
						_collisionLegL.RotationDegrees = -0.7f;
						((CapsuleShape2D)GetNode<CollisionShape2D>("CollisionLegL").Shape).Radius = 40.0f;
						((CapsuleShape2D)GetNode<CollisionShape2D>("CollisionLegL").Shape).Height = 740.0f;
						_collisionLegR.Position = new Vector2(250.0f, 87.5f);
						_collisionLegR.RotationDegrees = -32.3f;
						((CapsuleShape2D)GetNode<CollisionShape2D>("CollisionLegR").Shape).Radius = 40.0f;
						((CapsuleShape2D)GetNode<CollisionShape2D>("CollisionLegR").Shape).Height = 522.0f;
						break;
				}
				break;
		}
	}
}
