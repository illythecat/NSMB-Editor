namespace NSMBe5.Plugin
{
	public class PluginInfo
	{
		public readonly int id;
		public readonly string path;
		public readonly string name;
		public readonly bool enabled;
		public readonly int priority;

		public PluginInfo(int id, string path, string name, bool enabled, int priority)
		{
			this.id = id;
			this.path = path;
			this.name = name;
			this.enabled = enabled;
			this.priority = priority;
		}
	}
}
