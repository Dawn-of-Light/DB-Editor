using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Xml;

namespace xmlDbEditor
{
	/// <summary>
	/// I am an ordered listbox form and object
	/// </summary>
	public class editorReorder : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Button btnUp;
		private System.Windows.Forms.Button btnDown;
		private System.Windows.Forms.Button btnOK;
		public OrdListBox lbOrder;
		private System.Windows.Forms.Button btnCancel;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public editorReorder()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();	
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(editorReorder));
			this.btnUp = new System.Windows.Forms.Button();
			this.btnDown = new System.Windows.Forms.Button();
			this.btnOK = new System.Windows.Forms.Button();
			this.lbOrder = new xmlDbEditor.OrdListBox();
			this.btnCancel = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// btnUp
			// 
			this.btnUp.Image = ((System.Drawing.Image)(resources.GetObject("btnUp.Image")));
			this.btnUp.ImageAlign = System.Drawing.ContentAlignment.BottomRight;
			this.btnUp.Location = new System.Drawing.Point(8, 332);
			this.btnUp.Name = "btnUp";
			this.btnUp.Size = new System.Drawing.Size(24, 24);
			this.btnUp.TabIndex = 6;
			this.btnUp.Click += new System.EventHandler(this.btnUp_Click);
			// 
			// btnDown
			// 
			this.btnDown.Image = ((System.Drawing.Image)(resources.GetObject("btnDown.Image")));
			this.btnDown.ImageAlign = System.Drawing.ContentAlignment.TopRight;
			this.btnDown.Location = new System.Drawing.Point(40, 332);
			this.btnDown.Name = "btnDown";
			this.btnDown.Size = new System.Drawing.Size(24, 24);
			this.btnDown.TabIndex = 7;
			this.btnDown.Click += new System.EventHandler(this.btnDown_Click);
			// 
			// btnOK
			// 
			this.btnOK.Location = new System.Drawing.Point(80, 332);
			this.btnOK.Name = "btnOK";
			this.btnOK.Size = new System.Drawing.Size(64, 23);
			this.btnOK.TabIndex = 8;
			this.btnOK.Text = "OK";
			this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
			// 
			// lbOrder
			// 
			this.lbOrder.Location = new System.Drawing.Point(8, 8);
			this.lbOrder.Name = "lbOrder";
			this.lbOrder.Size = new System.Drawing.Size(216, 316);
			this.lbOrder.TabIndex = 9;
			// 
			// btnCancel
			// 
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(160, 332);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(64, 23);
			this.btnCancel.TabIndex = 10;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// editorReorder
			// 
			this.AcceptButton = this.btnOK;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(234, 359);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.lbOrder);
			this.Controls.Add(this.btnOK);
			this.Controls.Add(this.btnDown);
			this.Controls.Add(this.btnUp);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "editorReorder";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Re-Order Items";
			this.Load += new System.EventHandler(this.editorReorder_Load);
			this.ResumeLayout(false);

		}
		#endregion
		public bool isOK = false;
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new editorReorder());
		}

		private void btnUp_Click(object sender, System.EventArgs e)
		{
			int i = lbOrder.SelectedIndex;
			lbOrder.MoveUp(i);
			lbOrder.SelectedIndex = i-1;
		}

		private void btnDown_Click(object sender, System.EventArgs e)
		{
			int i = lbOrder.SelectedIndex;
			lbOrder.MoveDown(i);
			lbOrder.SelectedIndex = i+1;
		}

		private void btnOK_Click(object sender, System.EventArgs e)
		{
			isOK = true;
			this.Close();
		}

		private void editorReorder_Load(object sender, System.EventArgs e)
		{
			XmlDocument UIXml = myLocal.LoadUIXml();
			try
			{
				XmlNode aNode;				
				aNode = UIXml.SelectSingleNode("/UI/Reorder/Title");
				if (aNode != null) this.Text = aNode.InnerText;
				aNode = UIXml.SelectSingleNode("/UI/Reorder/OK");
				if (aNode != null) btnOK.Text = aNode.InnerText;
				aNode = UIXml.SelectSingleNode("/UI/Reorder/Cancel");
				if (aNode != null) btnCancel.Text = aNode.InnerText;
				UIXml = null;
			}
			catch
			{
				// just suppress it and use english
			}
		}

		private void btnCancel_Click(object sender, System.EventArgs e)
		{
			isOK = false;
			this.Close();
		}
	}

	public class OrdListBox : System.Windows.Forms.ListBox
	{		
		public OrdListBox()
		{
			Sorted = false;
		}

		public void AddOrderedItem(string ItemText, int ItemOrder, XmlElement SomeData)
		{
			ItemData data = new ItemData(ItemText,ItemOrder,SomeData);
			int c = Items.Count;			
			for (int i = 0; i < Items.Count; i++)
			{
				if (data.IsLess((ItemData)Items[i]))
				{
					Items.Insert(i,data);
					break;
				}
			}			
			if( c == Items.Count)
			{				
				Items.Insert(c,data);
			}
		}

		public int GetItemOrder(int index)
		{
			if(index < 0 || index >= Items.Count)
				return -1;
			else
				return ((ItemData)Items[index]).ItemOrder;
		}

		public XmlElement GetItemContents(int index)
		{
			if(index < 0 || index >= Items.Count)
				return null;
			else
				return ((ItemData)Items[index]).ItemContents;
		}

		public void MoveUp(int index)
		{
			if( index < 1 || index >= (Items.Count) )
				return;
			ItemData data = (ItemData)Items[index];
			ItemData dataprev = (ItemData)Items[index-1];
			Items.RemoveAt(index);
			Items.RemoveAt(index-1);
			AddOrderedItem(data.ToString(),dataprev.ItemOrder,data.ItemContents);
			AddOrderedItem(dataprev.ToString(),data.ItemOrder,dataprev.ItemContents);			
		}

		public void MoveDown(int index)
		{
			if( index < 0 || index >= (Items.Count-1) )
				return;
			ItemData data = (ItemData)Items[index];
			ItemData datanext = (ItemData)Items[index+1];
			Items.RemoveAt(index+1);
			Items.RemoveAt(index);			
			AddOrderedItem(data.ToString(),datanext.ItemOrder,data.ItemContents);
			AddOrderedItem(datanext.ToString(),data.ItemOrder,datanext.ItemContents);
		}

		class ItemData
		{
			public ItemData(string ItemText, int ItemOrder, XmlElement ItemContents)
			{
				_ItemText=ItemText;
				_ItemOrder=ItemOrder;
				_ItemContents=ItemContents;
			}
			private string _ItemText;
			private int _ItemOrder;
			private XmlElement _ItemContents;

			public override string ToString()
			{
				return _ItemText;
			}

			public bool IsLess(ItemData d)
			{
				int i1 = _ItemOrder;
				int i2 = d._ItemOrder;
				return ( i1 < i2 );
			}

			public int ItemOrder
			{
				get
				{
					return _ItemOrder;
				}
			}

			public XmlElement ItemContents
			{
				get
				{
					return _ItemContents;
				}
			}

		}
	}
}
