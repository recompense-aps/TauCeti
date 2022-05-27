using System.Collections.Generic;
public abstract class Factory
{
	private List<string> ParsingErrors = new List<string>();
	public abstract void AddValue(string id, object value);
	public abstract void LoadAssets();

	protected void LogParsingError(string error)
	{
		ParsingErrors.Add(error);
	}
}