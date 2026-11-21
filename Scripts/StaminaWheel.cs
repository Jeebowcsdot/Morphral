using System.Collections.Generic;
using System.Runtime.InteropServices.JavaScript;
using Godot;

namespace Morph.Scripts;

public partial class StaminaWheel : Node2D
{
	public float StaminaMax;
	public float Stamina;
	public float StaminaPercent = 100.0f;
	public List<Texture2D> NormalTextureList { get; private set; } = new List<Texture2D>();
	public List<Texture2D> TiredTextureList { get; private set; } = new List<Texture2D>();
	
	private const string JsonPath = "res://Json/StaminaWheel.json";
	public Sprite2D Sprite; 
	private Player _player;
	
	public StaminaWheel(float playerStamina)
	{
		StaminaMax = playerStamina;
		LoadItem();
	}
	
	public override void _Ready()
	{
		_player = this.GetParent<Player>();
		Stamina = _player.Stamina;
		StaminaMax = _player.StaminaMax;
		// Sprite = GetNode<Sprite2D>("Sprite2D");
		Sprite = new Sprite2D();
		Sprite.Texture = GD.Load<Texture2D>("res://Sprites/Stamina/Normal/wheelNormal16.png");
		Sprite.Scale = new Vector2(0.25f, 0.25f);
		AddChild(Sprite);

		Stamina = StaminaMax;
	}

	public void LowerStamina()
	{
		Stamina = _player.Stamina;
		if (Stamina <= 0)
		{
			_player.OnTired();
		}
		else
		{
			Sprite.Texture = NormalTextureList[GetTextureInt()];
		}
		// else if (Stamina == StaminaMax || Stamina >= 15 * StaminaMax / 16)
		// {
		// 	_sprite.Texture = NormalTextureList[16];
		// }
		// else if (Stamina < 15 * StaminaMax / 16 && Stamina >= 14 * StaminaMax / 16)
		// {
		// 	_sprite.Texture = NormalTextureList[15];
		// }
		// else if (Stamina < 14 * StaminaMax / 16 && Stamina >= 13 * StaminaMax / 16)
		// {
		// 	_sprite.Texture = NormalTextureList[14];
		// }
		// else if (Stamina < 13 * StaminaMax / 16 && Stamina >= 12 * StaminaMax / 16)
		// {
		// 	_sprite.Texture = NormalTextureList[13];
		// }
		// else if (Stamina < 12 * StaminaMax / 16 && Stamina >= 11 * StaminaMax / 16)
		// {
		// 	_sprite.Texture = NormalTextureList[12];
		// }
		// else if (Stamina < 11 * StaminaMax / 16 && Stamina >= 10 * StaminaMax / 16)
		// {
		// 	_sprite.Texture = NormalTextureList[11];
		// }
		// else if (Stamina < 10 * StaminaMax / 16 && Stamina >= 9 * StaminaMax / 16)
		// {
		// 	_sprite.Texture = NormalTextureList[10];
		// }
		// else if (Stamina < 9 * StaminaMax / 16 && Stamina >= 8 * StaminaMax / 16)
		// {
		// 	_sprite.Texture = NormalTextureList[9];
		// }
		// else if (Stamina < 8 * StaminaMax / 16 && Stamina >= 7 * StaminaMax / 16)
		// {
		// 	_sprite.Texture = NormalTextureList[8];
		// }
		// else if (Stamina < 7 * StaminaMax / 16 && Stamina >= 6 * StaminaMax / 16)
		// {
		// 	_sprite.Texture = NormalTextureList[7];
		// }
		// else if (Stamina < 6 * StaminaMax / 16 && Stamina >= 5 * StaminaMax / 16)
		// {
		// 	_sprite.Texture = NormalTextureList[6];
		// }
		// else if (Stamina < 5 * StaminaMax / 16 && Stamina >= 4 * StaminaMax / 16)
		// {
		// 	_sprite.Texture = NormalTextureList[5];
		// }
		// else if (Stamina < 4 * StaminaMax / 16 && Stamina >= 3 * StaminaMax / 16)
		// {
		// 	_sprite.Texture = NormalTextureList[4];
		// }
		// else if (Stamina < 3 * StaminaMax / 16 && Stamina >= 2 * StaminaMax / 16)
		// {
		// 	_sprite.Texture = NormalTextureList[3];
		// }
		// else if (Stamina < 2 * StaminaMax / 16 && Stamina >= StaminaMax / 16)
		// {
		// 	_sprite.Texture = NormalTextureList[2];
		// }
		// else if (Stamina < StaminaMax / 16 && Stamina >= 0.0f)
		// {
		// 	_sprite.Texture = NormalTextureList[1];
		// }
	}

	public int GetTextureInt()
	{
		float ratio = Mathf.Clamp(Stamina / StaminaMax, 0f, 1f);
		int i = Mathf.Clamp((int)(ratio * 16.0f), 0, 16);
		return i;
	}

	public void StaminaReset()
	{
		Stamina = StaminaMax;
		Sprite.Texture = NormalTextureList[16];
	}
	
	// public void GetTired()
	// {
	// 	_sprite.Texture = TiredTextureList[0];
	// 	_player.CurrentStatus = Player.Status.Tired;
	// 	_player.Defense = _player.DefenseSubtraction;
	// }
	
	protected void LoadItem()
	{
		if (!FileAccess.FileExists(JsonPath))
		{
			GD.PushError("Json not found.");
			return;
		}

		FileAccess file = FileAccess.Open(JsonPath, FileAccess.ModeFlags.Read);
		Variant data = Json.ParseString(file.GetAsText());
		Godot.Collections.Dictionary dataDict = data.AsGodotDictionary();
		dataDict.TryGetValue("normal", out Variant variantNormal);
		dataDict.TryGetValue("tired", out Variant variantTired);
		Godot.Collections.Array arrayNormal = variantNormal.AsGodotArray();
		Godot.Collections.Array arrayTired = variantTired.AsGodotArray();
		
		foreach (Variant item in arrayNormal)
		{
			Godot.Collections.Dictionary dict = item.AsGodotDictionary();
			string path = dict["path"].AsString();
			NormalTextureList.Add(
				GD.Load<Texture2D>(path));
		}
		
		foreach (Variant item in arrayTired)
		{
			Godot.Collections.Dictionary dict = item.AsGodotDictionary();
			string path = dict["path"].AsString();
			TiredTextureList.Add(
				GD.Load<Texture2D>(path));
		}
	}
}
