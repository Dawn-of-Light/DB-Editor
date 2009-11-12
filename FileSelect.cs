using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Text;

namespace NIFViewer
{
	/// <summary>
	/// Object, terrain and figure browser
	/// </summary>
	public class FileSelect : System.Windows.Forms.Form
	{
		public System.Windows.Forms.ListView result_view;
		private System.Windows.Forms.ColumnHeader field1;
		private System.Windows.Forms.ColumnHeader field2;
		private System.ComponentModel.Container components = null;
		public Form m_ParentForm;

		public	int		m_type; // 0-> monster, 1-> item, 2->zone
		public	int		m_ID;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.Panel panel1;
		public  string  m_Name;

		public FileSelect()
		{

			InitializeComponent();
		}

		/// <summary>
		/// Clear resources
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

		#region Windows Form Designer Generated Code
		private void InitializeComponent()
		{
			this.result_view = new System.Windows.Forms.ListView();
			this.field1 = new System.Windows.Forms.ColumnHeader();
			this.field2 = new System.Windows.Forms.ColumnHeader();
			this.button1 = new System.Windows.Forms.Button();
			this.button2 = new System.Windows.Forms.Button();
			this.panel1 = new System.Windows.Forms.Panel();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// result_view
			// 
			this.result_view.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
																						  this.field1,
																						  this.field2});
			this.result_view.Dock = System.Windows.Forms.DockStyle.Fill;
			this.result_view.FullRowSelect = true;
			this.result_view.Location = new System.Drawing.Point(0, 0);
			this.result_view.MultiSelect = false;
			this.result_view.Name = "result_view";
			this.result_view.Size = new System.Drawing.Size(368, 414);
			this.result_view.TabIndex = 1;
			this.result_view.View = System.Windows.Forms.View.Details;
			this.result_view.DoubleClick += new System.EventHandler(this.result_view_DoubleClick);
			// 
			// field1
			// 
			this.field1.Text = "ID";
			// 
			// field2
			// 
			this.field2.Text = "Name";
			this.field2.Width = 288;
			// 
			// button1
			// 
			this.button1.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.button1.Location = new System.Drawing.Point(8, 8);
			this.button1.Name = "button1";
			this.button1.TabIndex = 2;
			this.button1.Text = "OK";
			this.button1.Click += new System.EventHandler(this.result_view_DoubleClick);
			// 
			// button2
			// 
			this.button2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.button2.Location = new System.Drawing.Point(280, 8);
			this.button2.Name = "button2";
			this.button2.TabIndex = 3;
			this.button2.Text = "Cancel";
			this.button2.Click += new System.EventHandler(this.button2_Click);
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.button1);
			this.panel1.Controls.Add(this.button2);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panel1.Location = new System.Drawing.Point(0, 374);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(368, 40);
			this.panel1.TabIndex = 4;
			// 
			// FileSelect
			// 
			this.AcceptButton = this.button1;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.button2;
			this.ClientSize = new System.Drawing.Size(368, 414);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.result_view);
			this.Name = "FileSelect";
			this.Text = "Select Model";
			this.Load += new System.EventHandler(this.FileSelect_Load);
			this.panel1.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void result_view_DoubleClick(object sender, System.EventArgs e)
		{				
			m_ID = (int)result_view.SelectedItems[0].Tag;
			m_Name = result_view.SelectedItems[0].SubItems[1].Text;
			DialogResult = DialogResult.OK;
			Close();
		}

		private void FileSelect_Load(object sender, System.EventArgs e)
		{
			m_ParentForm.Cursor = System.Windows.Forms.Cursors.WaitCursor;
			int[] ids = new int[0];
			int nof = 0;

			// Get ID's depending on selection
			if (m_type == 0)
			{
				// get all IDs of Monsters
				nof = NIF.DAOCAPI.GetMonsterIDs(ids,0);
				ids = new int[nof];
				nof = NIF.DAOCAPI.GetMonsterIDs(ids,nof);
			}
			if (m_type == 1)
			{
				// get all IDs of Items
				nof = NIF.DAOCAPI.GetItemIDs(ids,0);
				ids = new int[nof];
				nof = NIF.DAOCAPI.GetItemIDs(ids,nof);
			}
			if (m_type == 2)
			{
				// get all IDs of Items
				nof = NIF.DAOCAPI.GetZoneIDs(ids,0);
				ids = new int[nof];
				nof = NIF.DAOCAPI.GetZoneIDs(ids,nof);
			}

			// begin listview fill
			result_view.BeginUpdate();
			result_view.Items.Clear();

			for (int t=0;t<nof;t++)
			{
				StringBuilder s_name = new StringBuilder(256);

				if (m_type == 0)
				{
					NIF.DAOCAPI.GetMonsterName(ids[t],s_name,256);
				}
				if (m_type == 1)
				{
					NIF.DAOCAPI.GetItemName(ids[t],s_name,256);
				}
				if (m_type == 2)
				{
					NIF.DAOCAPI.GetZoneName(ids[t],s_name,256);
				}

				ListViewItem item1 = new ListViewItem(String.Format("{0}", ids[t]));
				item1.SubItems.Add(String.Format("{0}", s_name));
				item1.Tag = ids[t];

				result_view.Items.Add(item1);
				Application.DoEvents();
			}

			result_view.Columns[0].Width = -1;
			result_view.Columns[1].Width = -1;
			result_view.EndUpdate();
			// end
			m_ParentForm.Cursor = System.Windows.Forms.Cursors.Default;		
		}

		private void button2_Click(object sender, System.EventArgs e)
		{
			DialogResult = DialogResult.Abort;
			Close();
		}
	}
}
