using System.Drawing;

namespace NSMBe5.Plugin
{
	public abstract class PluginStageObj
	{
		public StageObjSettings Settings;
		public readonly string SettingsXML;

		public PluginStageObj(string settings)
		{
			SettingsXML = settings;
		}

		public abstract Rectangle GetGrabBounds(NSMBStageObj obj, int x, int y);
		public abstract Rectangle GetRenderBounds(NSMBStageObj obj, int x, int y);
		public abstract void Render(NSMBStageObj obj, Graphics gfx, LevelEditorControl ed, int renderX, int renderY, ref bool customRendered);
	}
}
