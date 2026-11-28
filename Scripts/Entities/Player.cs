using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
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

	public enum Actions
	{
		RoundhouseKick,
		Dash
	}

	public enum Dir
	{
		Left,
		Right,
		Up,
		Down
	}

	public enum Characters
	{
		TestPlayer,
		TestEnemy
	}
	
	// Morphing
	public Characters PlayerChar { get; protected set; } = Characters.TestPlayer;
	protected Godot.Collections.Dictionary InitialDict { get; set; }= 
		new Godot.Collections.Dictionary();

	protected Godot.Collections.Dictionary<Player.Characters, Attacks> AttackDict { get; set; } =
		new Godot.Collections.Dictionary<Characters, Attacks>();
	private const string JsonPath = "res://Json/Attacks.json";
	
	// Movement and Velocity
	public Vector2 Direction { get; protected set; } = Vector2.Zero;
	public float Speed { get; protected set; } = 800.0f;
	public float SprintSpeed { get; protected set; } = 1200.0f;
	public float Acceleration { get; protected set; } = 100.0f;
	public StaminaWheel Wheel { get; protected set; }
	public Dir CurrentDir { get; protected set; } = Dir.Right; 

	// Statistics and Combat
	public float BaseAttack { get; protected init; } = 20.0f;
	public  float KickDamage { get; protected set; } = 20.0f;
	public float Defense { get; protected set; } = 25.0f;
	public float BaseDefense { get; protected set; } = 25.0f;
	public float DefenseSubtraction { get; protected set; } = 10.0f;
	public float Stamina { get; protected set; } = 80.0f;
	public float StaminaMax { get; protected set; }
	public UI.CooldownBar Bar { get; protected set; }
	public List<Attacks> PlayerAttacks { get; protected set; }
	
	protected float nBar = 0.0f;
	public List<Actions> CooldownActions { get; protected set; } = new List<Actions>();

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
		// Setup
		LoadAttacks();
		
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
				UpdateDir(Dir.Right);

			}
			else if (GetGlobalMousePosition().X < this.Position.X)
			{
				Scale = new Vector2(initialScale.X, -Math.Abs(initialScale.Y));
				RotationDegrees = 180.0f;
				UpdateDir(Dir.Left);
			}
		}
		
	}

	protected void UpdateDir(Dir dir)
	{
		CurrentDir = dir;
	}

	protected void SpawnCooldownBar(float n, Player player, float t, Actions action, string s)
	{
		// Basic setup
		CooldownActions.Add(action);
		UI.CooldownBar bar = new UI.CooldownBar(this);
		player.AddChild(bar);
		nBar++;
		
		// Child Node setup
		bar.Position = new Vector2(0.0f, -690.0f) + n * new Vector2(0.0f, -75.0f);
		bar.Label.Text = s;
		
		// Timer setup
		Timer timer = new Timer();
		timer.WaitTime = t / 16.0f;
		timer.Timeout += () => OnSpawnCooldownBarTimeout(bar, timer, action);
		this.AddChild(timer);
		timer.Start();
	}
	
	private void OnSpawnCooldownBarTimeout(UI.CooldownBar bar, Timer timer, Actions action)
	{
		if (bar.i == 16)
		{
			CooldownActions.Remove(action);
			nBar -= 1;
			bar.QueueFree();
			timer.QueueFree();
			return;
		}
		bar.Sprite.Texture = bar.TextureList[bar.i];
		bar.i += 1;
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

	private void LoadAttacks()
	{
		if (!FileAccess.FileExists(JsonPath))
		{
			GD.PushError("File not found");
			return;
		}

		FileAccess file = FileAccess.Open(JsonPath, FileAccess.ModeFlags.Read);
		Variant data = Json.ParseString(file.GetAsText());
		InitialDict = data.AsGodotDictionary();
		
		foreach (var (key, value) in InitialDict)
		{
			int index = (int)key;
			Characters Char = (Characters)index;
			
			Godot.Collections.Array<Godot.Collections.Dictionary<string, string>> valueArray 
				= value.AsGodotArray<Godot.Collections.Dictionary<string, string>>(); // converts the Variant into an Array

			List<Attacks> list = new List<Attacks>();
			for (int i = 0; i < valueArray.Count; i++)
			{
				Godot.Collections.Dictionary<string, string> dict = valueArray[i];
				string name = dict["Name"];
				Texture2D texture = GD.Load<Texture2D>(dict["Texture"]);
				Attacks a = new Attacks((Characters)index, name, texture);
				list.Add(a);
			}
			// AttackDict
			GD.Print(AttackDict);
			
		}
	}
}
