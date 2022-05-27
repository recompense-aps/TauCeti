	using System.Collections.Generic;
	using System.Linq;
	public class ModableStat
	{
		public int Base { get; private set;} = 0;
		public int Computed => Aggregate(Base, Modifiers);
		public Dictionary<string, int> Modifiers { get; private set; } = new Dictionary<string, int>();
		private int Aggregate(int seed, Dictionary<string,int> mods)
		{
			return mods.Values.Count > 0 ? mods.Values.Aggregate(seed, (total, next) => total + next) : seed;
		}

		public ModableStat Modify(string id, int mod)
		{
			Modifiers.Add(id, mod);
			return this;
		}

		public ModableStat UnModify(string id, int mod)
		{
			Modifiers.Remove(id);
			return this;
		}
	}