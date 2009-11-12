using System;
using System.Drawing;

namespace xmlDbEditor
{
	/// <summary>
	/// I am a the basic editor implementation.  You can't design me in the editor, as I inherit
	/// an abstract form class.  The form class itself is in EditorCommonLib.
	/// </summary>

	public class GenericEditor : AbstractEditor
	{		
		override protected void event_load(object sender, System.EventArgs e)
		{
			this.Hide();
			Point pos = SetupEditorForm();			
			GenerateUI(pos.X, pos.Y, DynamicForm);
			this.CenterToParent();
			this.Text = sEditor;
			this.Show();
			OpenRecord();
		}	
	}
}
