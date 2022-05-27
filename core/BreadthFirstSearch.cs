using System;
using System.Collections.Generic;

public class BFS<T>
{
	public static BFS<T> Create()
	{
		return new BFS<T>();
	}

	private T origin;
	private Dictionary<string, T> cameFrom = new Dictionary<string, T>();
	private Dictionary<string, int> costTo = new Dictionary<string, int>();
	private Func<T, List<T>> getNeighbors = x => new List<T>();
	private Func<T, int> getCost = x => 1;
	private Func<T, bool> passable = x => true;
	private const int MAX_SEARCHES = 1000;

	public BFS<T> WithNeighbors(Func<T, List<T>> func)
	{
		getNeighbors = func;
		return this;
	}

	public BFS<T> WithCost(Func<T, int> func)
	{
		getCost = func;
		return this;
	}

	public BFS<T> WithPassable(Func<T, bool> func)
	{
		passable = func;
		return this;
	}

	public BFS<T> Search(T start)
	{
		origin = start;
		Queue<T> frontier = new Queue<T>();
		frontier.Enqueue(start);
		costTo.Add(start.ToString(), 0);
		int searches = 0;

		while(frontier.Count > 0 && searches++ < MAX_SEARCHES)
		{
			T current = frontier.Dequeue();

			foreach(var n in getNeighbors(current))
			{
				if (!passable(n)) continue;

				int cost = getCost(n) + costTo[current.ToString()];

				// refine this to incorporate minimum cost
				if (!cameFrom.ContainsKey(n.ToString()) || cost < costTo[n.ToString()])
				{
					costTo[n.ToString()] = cost;
					cameFrom[n.ToString()] = current;
					frontier.Enqueue(n);
				}
			}
		}

		return this;
	}

	public List<T> PathTo(T destination)
	{
		List<T> path = new List<T>(){ destination };

		T next = destination;

		while(next != null)
		{
			path.Add(next);

			next = cameFrom.ContainsKey(next.ToString()) && next.ToString() != origin.ToString() ? cameFrom[next.ToString()] : default(T);
		}

		path.Reverse();
		return path;
	}

	public int CostTo(T destination)
	{
		return costTo.ContainsKey(destination.ToString()) ? costTo[destination.ToString()] : 0;
	}

}