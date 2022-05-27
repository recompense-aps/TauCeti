using Godot;
public static class Extensions
{
	public static (int x, int y) ToGrid(this Vector2 v)
	{
		return 
		(
			(int)v.x,
			(int)v.y
		);
	}
}