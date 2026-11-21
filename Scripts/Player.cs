using System;
using System.Text.RegularExpressions;
using Godot;

namespace Morph.Scripts;

/// <summary>
/// the general player class with everything included that is needed for every player.
/// </summary>
public partial class Player : CharacterBody2D
{
	public enum Status
	{
		Default,
		Tired
	}

	// Movement and Velocity
	public Vector2 Direction { get; protected set; } = Vector2.Zero;
	public float Speed { get; protected set; } = 800.0f;
	public float SprintSpeed { get; protected set; } = 1200.0f;
	public float Acceleration { get; protected set; } = 100.0f;
	public StaminaWheel Wheel { get; protected set; }

	// Statistics and Combat
	public float BaseAttack { get; protected init; } = 20.0f;
	public  float KickDamage { get; protected set; } = 20.0f;
	public float Defense { get; protected set; } = 25.0f;
	public float BaseDefense { get; protected set; } = 25.0f;
	public float DefenseSubtraction { get; protected set; } = 10.0f;
	public float Stamina { get; protected set; } = 80.0f;
	public float StaminaMax { get; protected set; }

	public Status CurrentStatus = Status.Default;
	
	// (Child) Nodes
	public Area2D DamageArea { get; protected set; }
	public CollisionShape2D DamageCollision { get; protected set; }
	public Node2D AttackPoint { get; protected set; }
	
	// Others
	public Vector2 initialScale { get; protected set; }
	private RandomNumberGenerator _rng;
	protected Timer exhaustionTimer;
	protected float timerTime = 5.0f;

	public override void _Ready()
	{
		// Setting Variables
		DamageArea = GetNode<Area2D>("DmgArea");
		DamageCollision = GetNode<CollisionShape2D>("DmgArea/DmgCollision");
		AttackPoint = GetNode<Node2D>("DmgArea/AttackPoint");
		initialScale = GetScale();
		StaminaMax = Stamina;
		
		// Signals
		DamageArea.BodyEntered += OnDmgBodyEntered;
		
		// Basic Setup on Ready
		DamageArea.Monitoring = false;
		
		// Spawn Stamina Wheel
		Wheel = new StaminaWheel(StaminaMax);
		Wheel.Position = new Vector2(-137.5f, -512.5f);
		this.AddChild(Wheel);
	}

	public override void _PhysicsProcess(double delta)
	{
		Direction = Input.GetVector("MoveLeft", "MoveRight", "MoveUp", "MoveDown");
		if (CurrentStatus == Status.Default)
		{
			if (Input.IsActionPressed("Sprint") && Direction != Vector2.Zero)
			{
				Velocity = Velocity.MoveToward(Direction * SprintSpeed, Acceleration);
				ChangeStamina(-8.0f * (float)delta);
			}
			else
			{
				Velocity = Velocity.MoveToward(Direction * Speed, Acceleration);
				ChangeStamina(8.0f * (float)delta);
			}
		} 
		else if (CurrentStatus == Status.Tired)
		{
			Velocity = Velocity.MoveToward(Direction * Speed / 3.0f, Acceleration);

			Stamina += StaminaMax / timerTime * (float)delta;
			float ratio = Mathf.Clamp(Stamina / StaminaMax, 0.0f, 1.0f);
			int i = Mathf.Clamp((int)(ratio * 16.0f), 1, 16);
			Wheel.Sprite.Texture = Wheel.TiredTextureList[i];
			
			if (exhaustionTimer == null)
			{
				exhaustionTimer = new Timer();
				exhaustionTimer.Timeout += OnStaminaExhaustionTimeout;
				this.AddChild(exhaustionTimer);
				exhaustionTimer.WaitTime = timerTime;
				exhaustionTimer.Start();
			}
		}
		MoveAndSlide();
	}

	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventMouseButton eventMouseButton && eventMouseButton.ButtonIndex == MouseButton.Right)
		{
			if (GetGlobalMousePosition().X > this.Position.X)
			{
				Scale = initialScale;
				RotationDegrees = 0.0f;
			}
			else if (GetGlobalMousePosition().X < this.Position.X)
			{
				Scale = new Vector2(initialScale.X, -Math.Abs(initialScale.Y));
				RotationDegrees = 180.0f;
			}
		}
		
	}
	
	protected void OnStaminaExhaustionTimeout()
	{
		CurrentStatus = Status.Default;
		Defense = BaseDefense;
		Wheel.StaminaReset();
		Stamina = StaminaMax;
		exhaustionTimer.QueueFree();
		exhaustionTimer = null;
	}

	protected void ChangeStamina(float amount)
	{
		if (Stamina + amount > StaminaMax) { Stamina = StaminaMax; }
		else if (Stamina + amount < 0) { Stamina = 0.0f; }
		else { Stamina += amount; }
		Wheel.LowerStamina();
	}
	
	public void OnTired()
	{
		Wheel.Sprite.Texture = Wheel.TiredTextureList[0];
		CurrentStatus = Status.Tired;
		Defense = BaseDefense - DefenseSubtraction;
	}
	
	protected void OnDmgBodyEntered(Node2D body)
	{
		if (body.IsInGroup("Enemy"))
		{
			Enemy enemy = GetNode<Enemy>(body.GetPath());
			var (damage, isCrit) = DamageCalc(enemy.Defense, false);
			enemy.takeDamage(damage, AttackPoint.GlobalPosition, isCrit);
		}
	}

	protected (int Damage, bool isCrit) DamageCalc(float defense, bool invincible = false)
	{
		if (invincible)
		{
			return (0, false);
		}
		bool isCrit = false;
		_rng = new RandomNumberGenerator();
		float totalDamage = KickDamage * BaseAttack / (KickDamage + Defense);
		float offset = _rng.RandfRange((-1) * totalDamage * 0.1f, totalDamage * 0.1f);
		totalDamage += offset;

		int criticalHit = _rng.RandiRange(0, 19); // 5% Chance
		if (criticalHit == 1)
		{
			totalDamage *= 1.5f;
			isCrit = true;
		}

		return (Mathf.RoundToInt(totalDamage), isCrit);
	}
}
