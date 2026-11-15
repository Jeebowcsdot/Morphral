using System.Collections.Generic;
using System.Runtime.InteropServices.JavaScript;
using Godot;

namespace Morph.Scripts;

public partial class StaminaWheel : Node2D
{
	public List<Texture2D> NormalTextureList { get; private set; }
	public List<Texture2D> TiredTextureList { get; private set; }
	
	private const string JsonPath = "res://Json/StaminaWheel.json";
	private Sprite2D _sprite;
	
	public StaminaWheel()
	{
		LoadItem();
	}

	public override void _Ready()
	{
		_sprite = GetNode<Sprite2D>("Sprite2D");
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
			TiredTextureList.Add(
				GD.Load<Texture2D>(path));
		}
	}
}
