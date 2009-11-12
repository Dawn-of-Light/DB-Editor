using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using Microsoft.Win32;

namespace NIFAPI
{
	/// <summary>
	/// Summary description for frmNifView.
	/// </summary>
	public class frmNifView : System.Windows.Forms.Form
	{
		private View view1;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public frmNifView()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.Size = new System.Drawing.Size(300,300);
			this.Text = "frmNifView";
			this.view1 = new View();
			RegistryKey hklm = Registry.CurrentUser;
			RegistryKey regKey = hklm.CreateSubKey("Software\\XMLDBEditor");
			NIFLIB.SetRootDirectory(regKey.GetValue("DAOCPath", "C:\\").ToString());
			regKey.Close();
			hklm.Close();
			// 
			// view1
			// 
			this.view1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.view1.Location = new System.Drawing.Point(0, 0);
			this.view1.Name = "view1";
			this.view1.Size = new System.Drawing.Size(648, 486);
			this.view1.TabIndex = 0;
		}
		#endregion

		public void LoadMob(int MobId)
		{
			view1.loadDAOC(0,MobId);
		}

		public void LoadItem(int ItemId)
		{
			view1.loadDAOC(1,ItemId);
		}

		public void LoadZone(int ZoneId)
		{
			view1.loadDAOC(2,ZoneId);			
		}
	}
}
