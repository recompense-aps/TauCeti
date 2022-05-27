using System.Collections.Generic;
using Godot;

public class FactionFactory : Factory
{
	public string FactionId { get; private set; }
	public string Color { get; private set; }
	private Dictionary<string, Texture> textures = new Dictionary<string, Texture>();
	public override void AddValue(string id, object value)
	{
		switch (id)
		{
			case "name":
				FactionId = value.ToString();
				break;
			case "color":
				Color = value.ToString();
				break;
			default:
				LogParsingError($"{nameof(FactionFactory)} '{id}' is not a valid {nameof(FactionFactory)} modifier");
				break;
		}
	}

	public override void LoadAssets()
	{
	}

	public Faction Instance()
	{
		return new Faction()
		{
			FactionId = FactionId,
			Color = Color
		};
	}
}
