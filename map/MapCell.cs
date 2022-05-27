using Godot;
using System.Collections.Generic;

public class MapCell
{
	public enum CellPosition { Center, TopLeft, TopRight, BottomLeft, BottomRight }
	public Map.Environment Environment => TauCeti.Instance.Map.GetEnvironmentAt(GridPosition.ToGrid().x, GridPosition.ToGrid().y);
	public Vector2 GridPosition { get; private set;}
	public Vector2 WorldPosition => GridPosition * CellSize;
	public Vector2 CenterPosition => new Vector2(WorldPosition.x + HalfCellSize, WorldPosition.y + HalfCellSize);
	public Vector2 TopLeftPosition => new Vector2(WorldPosition.x + QuarterCellSize, WorldPosition.y + QuarterCellSize);
	public Vector2 BottomLeftPosition => new Vector2(WorldPosition.x + QuarterCellSize, WorldPosition.y + CellSize - QuarterCellSize);
	public Vector2 TopRightPosition => new Vector2(WorldPosition.x + CellSize - QuarterCellSize, WorldPosition.y + QuarterCellSize);
	public Vector2 BottomRightPosition => new Vector2(WorldPosition.x + CellSize - QuarterCellSize, WorldPosition.y + CellSize - QuarterCellSize);
	private static Dictionary<string, PackedScene> scenes = new Dictionary<string, PackedScene>()
	{
		{ "minerals", GD.Load<PackedScene>("res://map/terrain/MineralTerrain.tscn") }
	};

	public const int CellSize = 64 + 32;
	private int HalfCellSize => CellSize / 2;
	private int QuarterCellSize => CellSize / 4;

    public MapCell(int x, int y)
	{
		GridPosition = new Vector2(x,y);
	}

	public override string ToString()
	{
		return $"({GridPosition.x},{GridPosition.y})";
	}

	public T GenerateTerrainAt<T>(string id, CellPosition position, int width, int height) where T:Node2D
	{
		T terrain = scenes[id].Instance<T>();

		switch (position)
		{
			case CellPosition.Center:
				terrain.Position = GetPositionToCenter(width, height);
				break;
			case CellPosition.TopLeft:
				terrain.Position = GetPositionToTopLeft(width, height);
				break;
			case CellPosition.TopRight:
				terrain.Position = GetPositionToTopRight(width, height);
				break;
			case CellPosition.BottomLeft:
				terrain.Position = GetPositionToBottomLeft(width, height);
				break;
			case CellPosition.BottomRight:
				terrain.Position = GetPositionToBottomRight(width, height);
			break;
		}

		return terrain;
	}

	public Vector2 GetPositionToCenter(int width, int height)
	{
		return new Vector2(CenterPosition.x, CenterPosition.y);
	}

	public Vector2 GetPositionToTopLeft(int width, int height)
	{
		return new Vector2(TopLeftPosition.x - width / 2, TopLeftPosition.y - height / 2);
	}

	public Vector2 GetPositionToBottomLeft(int width, int height)
	{
		return new Vector2(BottomLeftPosition.x - width / 2, BottomLeftPosition.y - height / 2);
	}

	public Vector2 GetPositionToTopRight(int width, int height)
	{
		return new Vector2(TopRightPosition.x - width / 2, TopRightPosition.y - height / 2);
	}

	public Vector2 GetPositionToBottomRight(int width, int height)
	{
		return new Vector2(BottomRightPosition.x - width / 2, BottomRightPosition.y - height / 2);
	}
}
