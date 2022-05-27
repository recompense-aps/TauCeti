using Godot;
using System.Linq;
using System.Collections.Generic;

public class Map : Node2D
{
	public enum Environment { Sand = 0, Rock = 1, Water = 2 }
	private MapCell[] cells;
	private TileMap baseTileSet;
	public override void _Ready()
	{
		baseTileSet = GetNode<TileMap>("BaseLayer");
		Generate(20);
	}

	public override void _Process(float delta)
	{
		var mouse = GetViewport().GetMousePosition();
		var cell = GetCellByWorldPosition(mouse.x, mouse.y);
		GetNode<Label>("CurrentCellLabel").Text = $"({cell.GridPosition.x},{cell.GridPosition.y})";
		GetNode<Sprite>("Selector").Position = new Vector2(cell.WorldPosition + new Vector2(48, 48));
	}

	public void SetEnvironmentAt(int x, int y, Environment e)
	{
		baseTileSet.SetCell(x, y, (int)e);
	}

	public Environment GetEnvironmentAt(int x, int y)
	{
		return (Environment)baseTileSet.GetCell(x, y);
	}

	public MapCell GetCellByGridPosition(int xPos, int yPos)
	{
		return cells.SingleOrDefault(x => (int)x.GridPosition.x == xPos && (int)x.GridPosition.y == yPos);
	}

	public MapCell GetCellByWorldPosition(float xPos, float yPos)
	{
		return cells.FirstOrDefault(x =>
			xPos >= x.WorldPosition.x &&
			xPos <= x.WorldPosition.x + MapCell.CellSize &&
			yPos >= x.WorldPosition.y &&
			yPos <= x.WorldPosition.y + MapCell.CellSize
		);
	}

	public void Generate(int size)
	{
		cells = new MapCell[size * size];
		int i = 0;
		for (int x = 0; x < size; x++)
			for (int y = 0; y < size; y++)
				GenerateCell(x, y, i++);
	}

	private void GenerateCell(int x, int y, int i)
	{
		cells[i] = new MapCell(x,y);
	}
}
