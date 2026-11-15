using System.Collections.Generic;
using System.Runtime.InteropServices.JavaScript;
using Godot;

namespace Morph.Scripts;

public partial class StaminaWheel : Node2D
{
	public enum StaminaPercentage
	{
		ZeroSixteen,
		OneSixteen,
		TwoSixteen,
		ThreeSixteen,
		FourSixteen,
		FiveSixteen,
		SixSixteen,
		SevenSixteen,
		EightSixteen,
		NineSixteen,
		TenSixteen,
		ElevenSixteen,
		TwelveSixteen,
		ThirteenSixteen,
		FourteenSixteen,
		FifteenSixteen,
		SixteenSixteen
	}

	public StaminaPercentage Stamina = StaminaPercentage.SixteenSixteen;
	public List<Texture2D> NormalTextureList { get; private set; } = new List<Texture2D>();
	public List<Texture2D> TiredTextureList { get; private set; } = new List<Texture2D>();
	
	private const string JsonPath = "res://Json/StaminaWheel.json";
	private Sprite2D _sprite;
	
	public StaminaWheel()
	{
		LoadItem();
	}
	
	public override void _Ready()
	{
		_sprite = GetNode<Sprite2D>("Sprite2D");

		Sprite2D sprite = new Sprite2D();
		sprite.Texture = GD.Load<Texture2D>("res://Sprites/Stamina/Normal/wheelNormal16.png");
		sprite.Scale = new Vector2(0.25f, 0.25f);
		AddChild(sprite);
		// sprite.Hide();

	}

	public void ChangeStamina()
	{
		
	}
	
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
			GD.Print(path);
			TiredTextureList.Add(
				GD.Load<Texture2D>(path));
		}
	}
}
