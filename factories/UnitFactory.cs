using System.Collections.Generic;
using Godot;

public class UnitFactory : Factory
{
	public string UnitId { get; private set; }
	public int Moves { get; private set; } = 0;
	public int Attack { get; private set; } = 0;
	public int Defense { get; private set; } = 0;
	public int Health { get; private set; } = 0;
	public float Scale { get; private set; } = 1;
	private Dictionary<string, Texture> textures = new Dictionary<string, Texture>();
	public override void AddValue(string id, object value)
	{
		switch (id)
		{
			case "name":
				UnitId = value.ToString();
				break;
			case "moves":
				Moves = int.Parse(value.ToString());
				break;
			case "attack":
				Attack = int.Parse(value.ToString());
				break;
			case "defense":
				Defense = int.Parse(value.ToString());
				break;
			case "health":
				Health = int.Parse(value.ToString());
				break;
			case "scale":
				Scale = float.Parse(value.ToString());
				break;
			default:
				LogParsingError($"{nameof(UnitFactory)} '{id}' is not a valid {nameof(UnitFactory)} modifier");
				break;
		}
	}

	public override void LoadAssets()
	{
		var colors = new List<string>(){ "blue", "green", "gray", "red" };

		foreach(var color in colors)
			textures.Add($"{UnitId}_{color}", GD.Load<Texture>($"res://assets/units/{UnitId}_{color}.png"));
	}

	public Unit Instance(string color = "blue")
	{
		var unit = new Unit();

		unit.Moves.Modify("game", Moves);
		unit.Attack.Modify("game", Attack);
		unit.Defense.Modify("game", Defense);
		unit.Health.Modify("game", Health);

		var sprite = new Sprite();
		sprite.Name = "Sprite";
		sprite.Texture = textures[$"{UnitId}_{color}"];
		sprite.Scale = Vector2.One * Scale;
		unit.AddChild(sprite);

		return unit;
	}
}
