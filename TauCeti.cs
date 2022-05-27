using Godot;
using System.Linq;

public class TauCeti : Node2D
{
	public static TauCeti Instance { get; private set; }
	public Rules Rules { get; set; } = new Rules();
	public Map Map { get; set; }
	private bool consoleOpen = false;
	public override void _Ready()
	{
		Instance = this;
		Map = GetNode<Map>("Map");
		GetNode<Node2D>("ConsoleOverlay").Hide();
		Load();

		Console.ProcessCommand("/scn scenarios/TESTING");
	}

	public TauCeti Load()
	{
		return 
			 LoadGameRules()
			.LoadAssets()
		;
	}
	public TauCeti LoadAssets()
	{
		Rules.UnitFactories.ForEach(f => f.LoadAssets());
		return this; 
	}
	public TauCeti LoadGameRules()
	{ 
		Rules.Load();
		return this;
	}
	public TauCeti ActivateInitialScene(){ return this; }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(float delta)
	{
		if (Input.IsActionJustPressed("console_toggle"))
		{
			var console = GetNode<Node2D>("ConsoleOverlay");
			var lineEdit = console.GetNode<LineEdit>("LineEdit");
			if (!consoleOpen)
			{
				consoleOpen = true;
				console.Show();
				lineEdit.GrabFocus();
				lineEdit.CaretPosition = 1;
			}
			else 
			{
				consoleOpen = false;
				lineEdit.Text = "/";
				lineEdit.ReleaseFocus();
				console.Hide();
			}
		}

		if (consoleOpen && Input.IsActionJustPressed("console_enter"))
		{
			var lineEdit = GetNode<LineEdit>("ConsoleOverlay/LineEdit");
			Console.ProcessCommand(lineEdit.Text);
		}
	}
}
