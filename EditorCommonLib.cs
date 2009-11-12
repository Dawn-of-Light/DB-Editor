using System;
using System.Drawing;
using System.Windows.Forms;
using System.Data;
using System.Collections;
using Microsoft.Win32;
using System.Xml;
using System.IO;

namespace xmlDbEditor
{

	#region Abstract Editor Class
	/// <summary>
	/// I am a form builder abstract class.  An instance of me can only edit a single 
	/// table at the moment.  I could be fixed to itterate through all the related child 
	/// tables and generate forms for each, but there are no related tables in DOL XML 
	/// schemas so this code is simplified.  (it would take at least 3x the code to handle 
	/// child tables/relations in that manner).  It does handle implied relationships 
	/// such as merchants to merchant items by restricting your view when working with
	/// a specific merchant.
	/// </summary>
	public abstract class AbstractEditor : System.Windows.Forms.Form
	{
		protected System.Windows.Forms.Panel DynamicForm;
		protected System.Windows.Forms.ToolTip toolTip1;
		private System.ComponentModel.IContainer components;
		protected static string sRecord = "record";
		protected static string sOf = "of";
		protected static string sEditor = "Editor";
		protected static string sOK = "OK";
		protected static string sCancel = "Cancel";
		protected static string sNew = "New";
		protected static string sEdit = "Edit";
		protected static string sRegion = "Select Region";
		protected static string sRealm = "Select Realm";
		protected static string sColor = "Select Color";
		protected static string sEmblem = "Select Emblem";
		protected static string sModel = "Select Model";
		protected static string sEffect = "Select Effect";
		protected static string sSlot = "Select Slot";
		protected static string sObjectType = "Select Object Type";
		protected static string sItemType = "Select Item Type";
		protected static string sRace = "Select Race";
		protected static string sClass = "Select Class";
		protected static string sDamageType = "Select Damage Type";
		protected static string sPrivLevel = "Select Privilege Level";
		protected static string sHand = "Select Hand";
		protected static string sEquipment = "Select Equipment Template";
		protected static string sMerchant = "Select Merchant";
		protected static string sItemTemplate = "Select Item Template";
		protected static string sSpellLine = "Select Spell Line";
		protected static string sSpell = "Select Spell";
		protected static string sSpecialization = "Select Specialization";
		protected static string sStyle = "Select Style";
		protected static string sArrangeItems = "Reorder Items";
		protected static string sEditItems = "Edit Items";
		protected static string sAbilityEditor = "Ability Editor";
		protected static string sAccountEditor = "Account Editor";
		protected static string sBindPointEditor = "Bind Point Editor";    
		protected static string sCharacterEditor = "Character Editor";
		protected static string sInventoryItemEditor = "Inventory Item Editor";
		protected static string sMerchantItemEditor = "Merchant Item Editor";
		protected static string sMobEditor = "Mob Editor";
		protected static string sItemTemplateEditor = "Item Template Editor";
		protected static string sSpellLineEditor = "Spell Line Editor";
		protected static string sMerchantEditor = "Merchant Editor";
		protected static string sNPCEquipmentEditor = "NPC Equipment Editor";
		protected static string sSpecializationEditor = "Specialization Editor";
		protected static string sSpecStyleEditor = "Spec X Style Editor";
		protected static string sSpellEditor = "Spell Editor";
		protected static string sStyleEditor = "Style Editor";
		protected static string sWorldObjectEditor = "World Object Editor";
		protected static string sZonePointEditor = "Zone Point Editor";
		protected static string sLineXSpellEditor = "Line X Spell Editor";
		protected static string sRecordIncompleteMsg = "This record is incomplete and will not be saved.\n\nDo you want to fix it?";
		protected static string sRecordIncompleteTitle = "This record has a problem";
		private System.Windows.Forms.Button btnPaint;
		protected static bool WasLocalized = false;

		protected AbstractEditor()
		{
			InitializeComponent();
			RegistryKey hklm = Registry.CurrentUser;
			RegistryKey regKey = hklm.CreateSubKey("Software\\XMLDBEditor");
			_dbPath = regKey.GetValue("XMLDBPath", "C:\\").ToString();
			_AutoGenerateGUIDs = (regKey.GetValue("GUIDGen","True").ToString() == "True");
			_PreCache = (regKey.GetValue("PreCache","True").ToString() == "True");
			dgLookupEnabled = (regKey.GetValue("DGLookup","True").ToString() == "True");
			dgEquipmentState = (regKey.GetValue("EquipmentDG","False").ToString() == "True");
			dgMerchantState = (regKey.GetValue("MerchantDG","False").ToString() == "True");
			dgItemsState = (regKey.GetValue("ItemsDG","False").ToString() == "True");
			dgSpellLineState = (regKey.GetValue("SpellLineDG","False").ToString() == "True");
			dgSpellState = (regKey.GetValue("SpellDG","False").ToString() == "True");
			dgSpecializationState = (regKey.GetValue("SpecializationDG","False").ToString() == "True");
			dgStyleState = (regKey.GetValue("StyleDG","False").ToString() == "True");
			hklm.Close();
			regKey.Close();
			if (!WasLocalized)
			{
				XmlDocument UIXml = myLocal.LoadUIXml();
				try
				{
					XmlNode aNode;				
					aNode = UIXml.SelectSingleNode("/UI/CustomEditor/Record");
					if (aNode != null) sRecord = aNode.InnerText;
					aNode = UIXml.SelectSingleNode("/UI/CustomEditor/Of");
					if (aNode != null) sOf = aNode.InnerText;
					aNode = UIXml.SelectSingleNode("/UI/CustomEditor/Editor");
					if (aNode != null) sEditor = aNode.InnerText;
					aNode = UIXml.SelectSingleNode("/UI/CustomEditor/OK");
					if (aNode != null) sOK = aNode.InnerText;
					aNode = UIXml.SelectSingleNode("/UI/CustomEditor/New");
					if (aNode != null) sNew = aNode.InnerText;
					aNode = UIXml.SelectSingleNode("/UI/CustomEditor/Edit");
					if (aNode != null) sEdit = aNode.InnerText;
					aNode = UIXml.SelectSingleNode("/UI/CustomEditor/Region");
					if (aNode != null) sRegion = aNode.InnerText;
					aNode = UIXml.SelectSingleNode("/UI/CustomEditor/Realm");
					if (aNode != null) sRealm = aNode.InnerText;
					aNode = UIXml.SelectSingleNode("/UI/CustomEditor/Color");
					if (aNode != null) sColor = aNode.InnerText;
					aNode = UIXml.SelectSingleNode("/UI/CustomEditor/Emblem");
					if (aNode != null) sEmblem = aNode.InnerText;
					aNode = UIXml.SelectSingleNode("/UI/CustomEditor/Model");
					if (aNode != null) sModel = aNode.InnerText;
					aNode = UIXml.SelectSingleNode("/UI/CustomEditor/Effect");
					if (aNode != null) sEffect = aNode.InnerText;
					aNode = UIXml.SelectSingleNode("/UI/CustomEditor/Slot");
					if (aNode != null) sSlot = aNode.InnerText;
					aNode = UIXml.SelectSingleNode("/UI/CustomEditor/ObjectType");
					if (aNode != null) sObjectType = aNode.InnerText;
					aNode = UIXml.SelectSingleNode("/UI/CustomEditor/ItemType");
					if (aNode != null) sItemType = aNode.InnerText;
					aNode = UIXml.SelectSingleNode("/UI/CustomEditor/Race");
					if (aNode != null) sRace = aNode.InnerText;
					aNode = UIXml.SelectSingleNode("/UI/CustomEditor/Class");
					if (aNode != null) sClass = aNode.InnerText;
					aNode = UIXml.SelectSingleNode("/UI/CustomEditor/DamageType");
					if (aNode != null) sDamageType = aNode.InnerText;
					aNode = UIXml.SelectSingleNode("/UI/CustomEditor/PrivLevel");
					if (aNode != null) sPrivLevel = aNode.InnerText;
					aNode = UIXml.SelectSingleNode("/UI/CustomEditor/Hand");
					if (aNode != null) sHand = aNode.InnerText;
					aNode = UIXml.SelectSingleNode("/UI/CustomEditor/EquipmentTemplate");
					if (aNode != null) sEquipment = aNode.InnerText;
					aNode = UIXml.SelectSingleNode("/UI/CustomEditor/Merchant");
					if (aNode != null) sMerchant = aNode.InnerText;
					aNode = UIXml.SelectSingleNode("/UI/CustomEditor/ItemTemplate");
					if (aNode != null) sItemTemplate = aNode.InnerText;
					aNode = UIXml.SelectSingleNode("/UI/CustomEditor/SpellLine");
					if (aNode != null) sSpellLine = aNode.InnerText;
					aNode = UIXml.SelectSingleNode("/UI/CustomEditor/Spell");
					if (aNode != null) sSpell = aNode.InnerText;
					aNode = UIXml.SelectSingleNode("/UI/CustomEditor/Specialization");
					if (aNode != null) sSpecialization = aNode.InnerText;
					aNode = UIXml.SelectSingleNode("/UI/CustomEditor/Style");
					if (aNode != null) sStyle = aNode.InnerText;
					aNode = UIXml.SelectSingleNode("/UI/CustomEditor/ArrangeItems");
					if (aNode != null) sArrangeItems = aNode.InnerText;
					aNode = UIXml.SelectSingleNode("/UI/CustomEditor/EditItems");
					if (aNode != null) sEditItems = aNode.InnerText;
					aNode = UIXml.SelectSingleNode("/UI/CustomEditor/AbilityEditor");
					if (aNode != null) sAbilityEditor = aNode.InnerText;
					aNode = UIXml.SelectSingleNode("/UI/CustomEditor/AccountEditor");
					if (aNode != null) sAccountEditor = aNode.InnerText;
					aNode = UIXml.SelectSingleNode("/UI/CustomEditor/BindPointEditor");
					if (aNode != null) sBindPointEditor = aNode.InnerText;
					aNode = UIXml.SelectSingleNode("/UI/CustomEditor/CharacterEditor");
					if (aNode != null) sCharacterEditor = aNode.InnerText;
					aNode = UIXml.SelectSingleNode("/UI/CustomEditor/InventoryItemEditor");
					if (aNode != null) sInventoryItemEditor = aNode.InnerText;
					aNode = UIXml.SelectSingleNode("/UI/CustomEditor/MerchantItemEditor");
					if (aNode != null) sMerchantItemEditor = aNode.InnerText;
					aNode = UIXml.SelectSingleNode("/UI/CustomEditor/MobEditor");
					if (aNode != null) sMobEditor = aNode.InnerText;
					aNode = UIXml.SelectSingleNode("/UI/CustomEditor/ItemTemplateEditor");
					if (aNode != null) sItemTemplateEditor = aNode.InnerText;
					aNode = UIXml.SelectSingleNode("/UI/CustomEditor/SpellLineEditor");
					if (aNode != null) sSpellLineEditor = aNode.InnerText;
					aNode = UIXml.SelectSingleNode("/UI/CustomEditor/LineXSpellEditor");
					if (aNode != null) sLineXSpellEditor = aNode.InnerText;
					aNode = UIXml.SelectSingleNode("/UI/CustomEditor/MerchantEditor");
					if (aNode != null) sMerchantEditor = aNode.InnerText;
					aNode = UIXml.SelectSingleNode("/UI/CustomEditor/NPCEquipmentEditor");
					if (aNode != null) sNPCEquipmentEditor = aNode.InnerText;
					aNode = UIXml.SelectSingleNode("/UI/CustomEditor/SpecializationEditor");
					if (aNode != null) sSpecializationEditor = aNode.InnerText;
					aNode = UIXml.SelectSingleNode("/UI/CustomEditor/SpecStyleEditor");
					if (aNode != null) sSpecStyleEditor = aNode.InnerText;
					aNode = UIXml.SelectSingleNode("/UI/CustomEditor/SpellEditor");
					if (aNode != null) sSpellEditor = aNode.InnerText;
					aNode = UIXml.SelectSingleNode("/UI/CustomEditor/StyleEditor");
					if (aNode != null) sStyleEditor = aNode.InnerText;
					aNode = UIXml.SelectSingleNode("/UI/CustomEditor/WorldObjectEditor");
					if (aNode != null) sWorldObjectEditor = aNode.InnerText;
					aNode = UIXml.SelectSingleNode("/UI/CustomEditor/ZonePointEditor");
					if (aNode != null) sZonePointEditor = aNode.InnerText;
					aNode = UIXml.SelectSingleNode("/UI/ErrorMessages/RecordIncompleteMsg");
					if (aNode != null) sRecordIncompleteMsg = aNode.InnerText;
					aNode = UIXml.SelectSingleNode("/UI/ErrorMessages/RecordIncompleteTitle");
					if (aNode != null) sRecordIncompleteTitle = System.Text.RegularExpressions.Regex.Replace(aNode.InnerText,"-n","\n");
				}
				catch 
				{
					// just suppress it and use english text.
				}
				UIXml = null;
				WasLocalized = true;
			}
		}

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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(AbstractEditor));
			this.DynamicForm = new System.Windows.Forms.Panel();
			this.btnPaint = new System.Windows.Forms.Button();
			this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
			this.DynamicForm.SuspendLayout();
			this.SuspendLayout();
			// 
			// DynamicForm
			// 
			this.DynamicForm.AutoScroll = true;
			this.DynamicForm.Controls.Add(this.btnPaint);
			this.DynamicForm.Dock = System.Windows.Forms.DockStyle.Fill;
			this.DynamicForm.Location = new System.Drawing.Point(0, 0);
			this.DynamicForm.Name = "DynamicForm";
			this.DynamicForm.Size = new System.Drawing.Size(119, 0);
			this.DynamicForm.TabIndex = 4;
			// 
			// btnPaint
			// 
			this.btnPaint.Image = ((System.Drawing.Image)(resources.GetObject("btnPaint.Image")));
			this.btnPaint.Location = new System.Drawing.Point(0, 0);
			this.btnPaint.Name = "btnPaint";
			this.btnPaint.Size = new System.Drawing.Size(24, 24);
			this.btnPaint.TabIndex = 0;
			this.btnPaint.Visible = false;
			// 
			// AbstractEditor
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.AutoScroll = true;
			this.ClientSize = new System.Drawing.Size(119, 0);
			this.Controls.Add(this.DynamicForm);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "AbstractEditor";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Resize += new System.EventHandler(this.event_Resize);
			this.Closing += new System.ComponentModel.CancelEventHandler(this.event_Closing);
			this.Load += new System.EventHandler(this.event_load);
			this.Closed += new System.EventHandler(this.event_Closed);
			this.DynamicForm.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		public static string BaseEditor;
		public static int TextWidthAdjustment=120;
		public static int NumericWidthAdjustment=180;
		public static int FormWidthAdjustment=176;
		public static int FormHeightAdjustment=60;
		public static int SelectorHeight=126;
		protected static Form m_ParentForm;
		protected GroupBox _gb;
		private string _dbPath;
		private static bool _AutoGenerateGUIDs = true;
		protected static bool _PreCache = true;
		private DBContainer _db = new DBContainer();
		private string _sRowID; // for an edit you pass in the row identifier (the keyfield data)
		private int _iColumn; // and the keyfield column number, so this can find the right record
		private int _Maxwidth; // pass in the width of the largest textfield in the grid, so I can size the form.
		private DataTable _dt;
		private TableInfo _ti = new TableInfo();
		private int _newRow = -1;
		private bool _AllIsGood = false;
		private bool _HasChanges = false;
		private int _RegionSelection=-1;
		private int _RealmSelection=-1;
		protected string _EquipmentSelection="";
		protected string _MerchantSelection="";
		protected string _MerchantItemSelection="";
		protected string _ItemSelection="";
		protected string _MobSelection="";
		protected int _PrivilegeSelection=-1;
		protected int _HandSelection=-1;
		protected int _SlotSelection=-1;
		protected int _DamageSelection=-1;
		protected string _SpellLineSelection = "";
		protected int _SpellSelection = -1;
		protected string _SpecSelection = "";
		protected int _StyleSelection = -1;
		protected int _ObjectTypeSelection = -1;
		protected int _ItemTypeSelection = -1;
		protected int _RaceSelection = -1;
		protected int _ClassTypeSelection = -1;
		protected int _ColorSelection = -1;
		protected int _EmblemSelection = -1;
		protected int _ItemModelSelection = -1;
		protected int _MonsterModelSelection = -1;
		protected int _WeaponEffectSelection = -1;
		public static SortedList Regions = null;
		public static SortedList Realms = null;
		public static SortedList Equipment = null;
		public static SortedList Merchant = null;
		public static SortedList Items = null;
		public static SortedList ItemsIndex = null;
		public static SortedList Privileges = null;
		public static SortedList Hand = null;
		public static SortedList Slot = null;
		public static SortedList Damage = null;
		public static SortedList SpellLine = null;
		public static SortedList Spell = null;
		public static SortedList Spec = null;
		public static SortedList Style = null;
		public static SortedList ObjectType = null;
		public static SortedList ItemType = null;
		public static SortedList Race = null;
		public static SortedList ClassType = null;
		public static SortedList Color = null;
		public static SortedList Emblem = null;
		public static SortedList ItemModel = null;
		public static SortedList MonsterModel = null;
		public static SortedList WeaponEffect = null;
		public static ScrollingDataGrid dgEquipment = null;
		public static ScrollingDataGrid dgMerchant = null;
		public static ScrollingDataGrid dgItems = null;
		public static ScrollingDataGrid dgSpellLine = null;
		public static ScrollingDataGrid dgSpell = null;
		public static ScrollingDataGrid dgSpecialization = null;
		public static ScrollingDataGrid dgStyle = null;
		public static DBContainer cEquipment = null;
		public static DBContainer cMerchant = null;
		public static DBContainer cItems = null;
		public static DBContainer cSpellLine = null;
		public static DBContainer cSpell = null;
		public static DBContainer cSpecialization = null;
		public static DBContainer cStyle = null;
		public static int dgEquipmentWidth = 0;
		public static int dgMerchantWidth = 0;
		public static int dgItemsWidth = 0;
		public static int dgSpellLineWidth = 0;
		public static int dgSpellWidth = 0;
		public static int dgSpecializationWidth = 0;
		public static int dgStyleWidth = 0;
		public static bool dgLookupEnabled = false;
		public static bool dgEquipmentState = false;
		public static bool dgMerchantState = false;
		public static bool dgItemsState = false;
		public static bool dgSpellLineState = false;
		public static bool dgSpellState = false;
		public static bool dgSpecializationState = false;
		public static bool dgStyleState = false;
		private Control RegionCtrl = null;
		private Control BindRegionCtrl = null;
		private Control RealmCtrl = null;
		private Control GravestoneRegionCtrl = null;
		private Control EquipmentTemplateCtrl = null;
		private Control MerchantCtrl = null;
		private Control ItemTemplateCtrl = null;
		private Control PrivilegeCtrl = null;
		private Control HandCtrl = null;
		private Control SlotCtrl = null;
		private Control DamageCtrl = null;
		private Control SpellLineCtrl = null;
		private Control SpellCtrl = null;
		private Control SpecCtrl = null;
		private Control StyleCtrl = null;
		private Control GuidCtrl = null;
		private Control ObjectTypeCtrl = null;
		private Control ItemTypeCtrl = null;
		private Control RaceCtrl = null;
		private Control ClassTypeCtrl = null;
		private Control ColorCtrl = null;
		private Control EmblemCtrl = null;
		private Control ItemModelCtrl = null;
		private Control MonsterModelCtrl = null;
		private Control WeaponEffectCtrl = null;
		private string[] ControlBackup = new string[100];
		private static XmlDocument Defaults = null;
		private NIFViewer.frmNIFViewer NifView;

		public class ScrollingDataGrid : DataGrid
		{
			public const int WM_LBUTTONDOWN = 513; // 0x0201  
			public const int WM_LBUTTONUP = 514; // 0x0202  
			private int lastrow = 0;

			[System.Runtime.InteropServices.DllImport("user32.dll")] 
 
			static extern bool SendMessage(IntPtr hWnd, Int32 msg, Int32 wParam, Int32 lParam);  

			public void ClickRowHeader(int Row)  
			{  
				// I send a message to myself telling me that a user clicked on 
				// a particular row header, to emulate this behavior in code.
				Rectangle rect = this.GetCellBounds(Row,0);
				Int32 lparam = MakeLong(rect.Left - 4, rect.Top + 4);
				SendMessage(this.Handle, WM_LBUTTONDOWN, 0, lparam);  
				SendMessage(this.Handle, WM_LBUTTONUP, 0, lparam);  
			} 
  
			static int MakeLong(int LoWord, int HiWord)  
			{  
				return (HiWord << 16) | (LoWord & 0xffff);  
			}  

			public void ScrollToRow(int row)
			{ 
				if (this.DataSource != null)
				{
					// the lastrow code works around a bug if you try to scroll to the current location on the grid.
					// it does this but refused to refresh the grid no matter what you do.  So you still "see" the top
					// of the grid, even though the scroll bar is moved down to where it should be... bizzare...
					if (row == lastrow) this.GridVScrolled(this, new ScrollEventArgs(ScrollEventType.LargeIncrement, 0));
					this.GridVScrolled(this, new ScrollEventArgs(ScrollEventType.LargeIncrement, row));
					lastrow = row;
				}						
			}

			protected override void OnMouseMove(MouseEventArgs e)
			{
				// prevent draging stuff
				if ((e.Button != MouseButtons.Left))
				{
					base.OnMouseMove(e);
				}
			}

			protected override void OnMouseDown(System.Windows.Forms.MouseEventArgs e)
			{
				DataGrid.HitTestInfo hti = this.HitTest(new Point(e.X, e.Y));
				if (hti.Type == DataGrid.HitTestType.RowResize) return;
				base.OnMouseDown(e);
			}
		}

		public static string Apply_Defaults(string applyto)
		{
			bool DefaultsOK = true;
			if (Defaults == null)
			{
				Defaults = new XmlDocument();
				try
				{
					Defaults.Load("defaults.xml");
				}
				catch
				{
					DefaultsOK = false;
				}										
			}
			if (DefaultsOK)
			{
				if (applyto.IndexOf(".") > -1)
				{	
					string tablename = applyto.Split(new Char[] {'.'})[0];
					string columname = applyto.Split(new Char[] {'.'})[1];
					XmlNode Default = Defaults.SelectSingleNode("/Defaults/"+tablename+"/"+columname);
					if (Default != null)
					{
						return Default.InnerText;
					}
					else
					{
						return "";
					}
				}
				else
				{
					return "";
				}
			}
			else return "";
		}
           
		#region Events

		abstract protected void event_load(object sender, System.EventArgs e);

		// I am overridden in classes that are selectors... I am needed by tooltip lookups in classes that are editors.
		public virtual void Initialize() {} 

		 // I am overridden in editor classes that need tooltip lookups.
		protected virtual void event_MouseEnter(object sender, System.EventArgs e) {}

		protected virtual void event_Resize(object sender, System.EventArgs e)
		{
			if (_gb != null) _gb.Size = new Size(_gb.Parent.Width-20,_gb.Height);
		}

		protected virtual void event_OK(object sender, EventArgs e)
		{
			if (EnforceConstraints(true))
			{
				_AllIsGood = true;
				_HasChanges = true;
				this.Close();
			}
		}

		protected virtual void event_Escape(object sender, EventArgs e)
		{
			this.Close();
		}

		protected virtual void event_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			if (EnforceConstraints(false)) // change to true if you want the message 
			{							   // and cancel event to fire... 
				_AllIsGood = true;
				this.Close();
			}
			else
			{
				e.Cancel = true;
			}
		}

		private void event_Closed(object sender, System.EventArgs e)
		{
			RegistryKey hklm = Registry.CurrentUser;
			RegistryKey regKey = hklm.CreateSubKey("Software\\XMLDBEditor");
			if (dgEquipmentState) 
			{
				regKey.SetValue("EquipmentDG","True");
			}
			else
			{
				regKey.SetValue("EquipmentDG","False");
			}
			if (dgMerchantState)
			{
				regKey.SetValue("MerchantDG","True");
			}
			else
			{
				regKey.SetValue("MerchantDG","False");
			}
			if (dgItemsState)
			{
				regKey.SetValue("ItemsDG","True");
			}
			else
			{
				regKey.SetValue("ItemsDG","False");
			}
			if (dgSpellLineState)
			{
				regKey.SetValue("SpellLineDG","True");
			}
			else
			{
				regKey.SetValue("SpellLineDG","False");
			}
			if (dgSpellState)
			{ 
				regKey.SetValue("SpellDG","True");
			}
			else
			{
				regKey.SetValue("SpellDG","False");
			}
			if (dgSpecializationState)
			{
				regKey.SetValue("SpecializationDG","True");
			}
			else
			{
				regKey.SetValue("SpecializationDG","False");
			}
			if (dgStyleState)
			{
				regKey.SetValue("StyleDG","True");
			}
			else
			{
				regKey.SetValue("StyleDG","False");
			}
			regKey.Close();
			hklm.Close();		
		}


		protected virtual void GenerateUI(int ax, int ay, Control gbParent)
		{
			_gb = SetupGroupBoxWithNavigator(ax,ay,gbParent);
			int rx=8; int ry=40; 			
			ry = BuildColumns(rx,ry);
			ry += 10;
			this.AcceptButton = MakeButton(sOK, rx, ry, 75, 23, new EventHandler(event_OK), _gb);
			Button btn = MakeButton(sCancel, rx+2500, ry+2500, 75, 23, new EventHandler(event_Escape), _gb);
			btn.TabStop = false;
			this.CancelButton = btn;
			ry += 25;
			AdjustFormSize(ay,ry,ax,rx);
		}

		protected void event_Gimmie_Guid(object sender, EventArgs e)
		{
			GuidCtrl.Text = System.Guid.NewGuid().ToString();
			GuidCtrl.Select();
		}

		protected void event_Lookup_Color(object sender, EventArgs e)
		{			
			AbstractEditor r = new ColorSelector();
			try
			{
				r.ColorSelection = int.Parse(ColorCtrl.Text);
			}
			catch
			{
				r.ColorSelection = 0;
			}
			r.ShowDialog();
			ColorCtrl.Text = r.ColorSelection.ToString();
			ColorCtrl.Select(); // you must do this to get data change to commit if the control wasn't selected.
			r.ColorSelection = -1;
		}

		protected void event_Lookup_Emblem(object sender, EventArgs e)
		{			
			AbstractEditor r = new EmblemSelector();
			try
			{
				r.EmblemSelection = int.Parse(EmblemCtrl.Text);
			}
			catch
			{
				r.EmblemSelection = 0;
			}
			r.ShowDialog();
			EmblemCtrl.Text = r.EmblemSelection.ToString();
			EmblemCtrl.Select(); // you must do this to get data change to commit if the control wasn't selected.
			r.EmblemSelection = -1;
		}

		protected void event_Lookup_ItemModel(object sender, EventArgs e)
		{			
			AbstractEditor r = new ItemModelSelector();
			try
			{
				r.ItemModelSelection = int.Parse(ItemModelCtrl.Text);
			}
			catch
			{
				r.ItemModelSelection = 0;
			}
			r.ShowDialog();
			ItemModelCtrl.Text = r.ItemModelSelection.ToString();
			ItemModelCtrl.Select(); // you must do this to get data change to commit if the control wasn't selected.
			r.ItemModelSelection = -1;
		}

		protected void event_Render_ItemModel(object sender, EventArgs e)
		{		
			NifView = new NIFViewer.frmNIFViewer();
			NifView.DoubleClick += new System.EventHandler(event_NIFViewer_Item_DoubleClick);
			NifView.Load += new System.EventHandler(event_NIFViewer_Item_Load);
			NifView.ShowDialog();
		}

		protected void event_NIFViewer_Item_Load(object sender, EventArgs e)
		{
			NifView.LoadItem(int.Parse(ItemModelCtrl.Text));
		}

		protected void event_NIFViewer_Item_DoubleClick(object sender, EventArgs e)
		{			
			int i = NifView.SelectedModel;
			if (i > -1)
			{
				ItemModelCtrl.Text = i.ToString();
				ItemModelCtrl.Select(); // you must do this to get data change to commit if the control wasn't selected.
			}
			NifView.Close();
		}

		protected void event_Lookup_MonsterModel(object sender, EventArgs e)
		{			
			AbstractEditor r = new MonsterModelSelector();
			try
			{
				r.MonsterModelSelection = int.Parse(MonsterModelCtrl.Text);
			}
			catch
			{
				r.MonsterModelSelection = 0;
			}
			r.ShowDialog();
			MonsterModelCtrl.Text = r.MonsterModelSelection.ToString();
			MonsterModelCtrl.Select(); // you must do this to get data change to commit if the control wasn't selected.
			r.MonsterModelSelection = -1;
		}

		protected void event_Render_MonsterModel(object sender, EventArgs e)
		{		
			NifView = new NIFViewer.frmNIFViewer();
			NifView.DoubleClick += new System.EventHandler(event_NIFViewer_Monster_DoubleClick);
			NifView.Load += new System.EventHandler(event_NIFViewer_Monster_Load);
			NifView.ShowDialog();
		}

		protected void event_NIFViewer_Monster_Load(object sender, EventArgs e)
		{
			NifView.LoadMob(int.Parse(MonsterModelCtrl.Text));
		}

		protected void event_NIFViewer_Monster_DoubleClick(object sender, EventArgs e)
		{			
			int i = NifView.SelectedModel;
			if (i > -1)
			{
				MonsterModelCtrl.Text = i.ToString();
				MonsterModelCtrl.Select(); // you must do this to get data change to commit if the control wasn't selected.
			}
			NifView.Close();			
		}

		protected void event_Lookup_WeaponEffect(object sender, EventArgs e)
		{			
			AbstractEditor r = new WeaponEffectSelector();
			try
			{
				r.WeaponEffectSelection = int.Parse(WeaponEffectCtrl.Text);
			}
			catch
			{
				r.WeaponEffectSelection = 0;
			}
			r.ShowDialog();
			WeaponEffectCtrl.Text = r.WeaponEffectSelection.ToString();
			WeaponEffectCtrl.Select(); // you must do this to get data change to commit if the control wasn't selected.
			r.WeaponEffectSelection = -1;
		}

		protected void event_Lookup_Realm(object sender, EventArgs e)
		{			
			AbstractEditor r = new RealmSelector();
			try
			{
				r.RealmSelection = int.Parse(RealmCtrl.Text);
			}
			catch
			{
				r.RealmSelection = 0;
			}
			r.ShowDialog();
			RealmCtrl.Text = r.RealmSelection.ToString();
			RealmCtrl.Select(); // you must do this to get data change to commit if the control wasn't selected.
			r.RealmSelection = -1;
		}

		protected void event_Lookup_ObjectType(object sender, EventArgs e)
		{			
			AbstractEditor r = new ObjectTypeSelector();
			try
			{
				r.ObjectTypeSelection = int.Parse(ObjectTypeCtrl.Text);
			}
			catch
			{
				r.ObjectTypeSelection = 0;
			}
			r.ShowDialog();
			ObjectTypeCtrl.Text = r.ObjectTypeSelection.ToString();
			ObjectTypeCtrl.Select(); // you must do this to get data change to commit if the control wasn't selected.
			r.ObjectTypeSelection = -1;
		}

		protected void event_Lookup_ClassType(object sender, EventArgs e)
		{			
			AbstractEditor r = new ClassTypeSelector();
			try
			{
				r.ClassTypeSelection = int.Parse(ClassTypeCtrl.Text);
			}
			catch
			{
				r.ClassTypeSelection = 0;
			}
			r.ShowDialog();
			ClassTypeCtrl.Text = r.ClassTypeSelection.ToString();
			ClassTypeCtrl.Select(); // you must do this to get data change to commit if the control wasn't selected.
			r.ClassTypeSelection = -1;
		}

		protected void event_Lookup_ItemType(object sender, EventArgs e)
		{			
			AbstractEditor r = new ItemTypeSelector();
			try
			{
				r.ItemTypeSelection = int.Parse(ItemTypeCtrl.Text);
			}
			catch
			{
				r.ItemTypeSelection = 0;
			}
			r.ShowDialog();
			ItemTypeCtrl.Text = r.ItemTypeSelection.ToString();
			ItemTypeCtrl.Select(); // you must do this to get data change to commit if the control wasn't selected.
			r.ItemTypeSelection = -1;
		}

		protected void event_Lookup_Race(object sender, EventArgs e)
		{			
			AbstractEditor r = new RaceSelector();
			try
			{
				r.RaceSelection = int.Parse(RaceCtrl.Text);
			}
			catch
			{
				r.RaceSelection = 0;
			}
			r.ShowDialog();
			RaceCtrl.Text = r.RaceSelection.ToString();
			RaceCtrl.Select(); // you must do this to get data change to commit if the control wasn't selected.
			r.RaceSelection = -1;
		}

		protected void event_Lookup_Privilege(object sender, EventArgs e)
		{			
			AbstractEditor r = new PrivilegeSelector();
			try
			{
				r.PrivilegeSelection = int.Parse(PrivilegeCtrl.Text);
			}
			catch
			{
				r.PrivilegeSelection = 1;
			}
			r.ShowDialog();
			PrivilegeCtrl.Text = r.PrivilegeSelection.ToString();
			PrivilegeCtrl.Select(); // you must do this to get data change to commit if the control wasn't selected.
			r.PrivilegeSelection = -1;
		}

		protected void event_Lookup_Hand(object sender, EventArgs e)
		{			
			AbstractEditor r = new HandSelector();
			try
			{
				r.HandSelection = int.Parse(HandCtrl.Text);
			}
			catch
			{
				r.HandSelection = 1;
			}
			r.ShowDialog();
			HandCtrl.Text = r.HandSelection.ToString();
			HandCtrl.Select(); // you must do this to get data change to commit if the control wasn't selected.
			r.HandSelection = -1;
		}

		protected void event_Lookup_Slot(object sender, EventArgs e)
		{			
			AbstractEditor r = new SlotSelector();
			try
			{
				r.SlotSelection = int.Parse(SlotCtrl.Text);
			}
			catch
			{
				r.SlotSelection = 10;
			}
			r.ShowDialog();
			SlotCtrl.Text = r.SlotSelection.ToString();
			SlotCtrl.Select(); // you must do this to get data change to commit if the control wasn't selected.
			r.SlotSelection = -1;
		}

		protected void event_Lookup_Region(object sender, EventArgs e)
		{			
			AbstractEditor r = new RegionSelector();
			try
			{
				r.RegionSelection = int.Parse(RegionCtrl.Text);
			}
			catch
			{
				r.RegionSelection = 1;
			}
			r.ShowDialog();
			RegionCtrl.Text = r.RegionSelection.ToString();
			RegionCtrl.Select(); // you must do this to get data change to commit if the control wasn't selected.
			r.RegionSelection = -1;
		}

		protected void event_Lookup_BindRegion(object sender, EventArgs e)
		{			
			AbstractEditor r = new RegionSelector();
			try
			{
				r.RegionSelection = int.Parse(BindRegionCtrl.Text);
			}
			catch
			{
				r.RegionSelection = 1;
			}
			r.ShowDialog();
			BindRegionCtrl.Text = r.RegionSelection.ToString();
			BindRegionCtrl.Select(); // you must do this to get data change to commit if the control wasn't selected.
			r.RegionSelection = -1;
		}

		protected void event_Lookup_GravestoneRegion(object sender, EventArgs e)
		{			
			AbstractEditor r = new RegionSelector();
			try
			{
				r.RegionSelection = int.Parse(GravestoneRegionCtrl.Text);
			}
			catch
			{
				r.RegionSelection = 1;
			}
			r.ShowDialog();
			GravestoneRegionCtrl.Text = r.RegionSelection.ToString();
			GravestoneRegionCtrl.Select(); // you must do this to get data change to commit if the control wasn't selected.
			r.RegionSelection = -1;
		}

		protected void event_Lookup_Damage(object sender, EventArgs e)
		{			
			AbstractEditor r = new DamageSelector();
			try
			{
				r.DamageSelection = int.Parse(DamageCtrl.Text);
			}
			catch
			{
				r.DamageSelection = 1;
			}
			r.ShowDialog();
			DamageCtrl.Text = r.DamageSelection.ToString();
			DamageCtrl.Select(); // you must do this to get data change to commit if the control wasn't selected.
			r.DamageSelection = -1;
		}

		protected void event_Lookup_Equipment(object sender, EventArgs e)
		{			
			AbstractEditor r = new EquipmentSelector();
			r.EquipmentSelection = EquipmentTemplateCtrl.Text;
			r.ShowDialog();
			EquipmentTemplateCtrl.Text = r.EquipmentSelection;
			EquipmentTemplateCtrl.Select(); // you must do this to get data change to commit if the control wasn't selected.
			r.EquipmentSelection = "";
		}

		protected void event_Lookup_Item(object sender, EventArgs e)
		{			
			AbstractEditor r = new ItemSelector();
			r.ItemSelection = ItemTemplateCtrl.Text;
			r.ShowDialog();
			ItemTemplateCtrl.Text = r.ItemSelection;
			ItemTemplateCtrl.Select(); // you must do this to get data change to commit if the control wasn't selected.
			r.ItemSelection = "";
		}

		protected void event_Lookup_Spell_Line(object sender, EventArgs e)
		{			
			AbstractEditor r = new SpellLineSelector();
			r.SpellLineSelection = SpellLineCtrl.Text;
			r.ShowDialog();
			SpellLineCtrl.Text = r.SpellLineSelection;
			SpellLineCtrl.Select(); // you must do this to get data change to commit if the control wasn't selected.
			r.SpellLineSelection = "";
		}

		protected void event_Lookup_Spell(object sender, EventArgs e)
		{			
			AbstractEditor r = new SpellSelector();
			try
			{
				r.SpellSelection = int.Parse(SpellCtrl.Text);
			}
			catch
			{
				r.SpellSelection = 1;
			}
			r.ShowDialog();
			SpellCtrl.Text = r.SpellSelection.ToString();
			SpellCtrl.Select(); // you must do this to get data change to commit if the control wasn't selected.
			r.SpellSelection = -1;
		}

		protected void event_Lookup_Spec(object sender, EventArgs e)
		{			
			AbstractEditor r = new SpecSelector();
			r.SpecSelection = SpecCtrl.Text;
			r.ShowDialog();
			SpecCtrl.Text = r.SpecSelection;
			SpecCtrl.Select(); // you must do this to get data change to commit if the control wasn't selected.
			r.SpecSelection = "";
		}

		protected void event_Lookup_Style(object sender, EventArgs e)
		{			
			AbstractEditor r = new StyleSelector();
			try
			{
				r.StyleSelection = int.Parse(StyleCtrl.Text);
			}
			catch
			{
				r.StyleSelection = 1;
			}
			r.ShowDialog();
			StyleCtrl.Text = r.StyleSelection.ToString();
			StyleCtrl.Select(); // you must do this to get data change to commit if the control wasn't selected.
			r.StyleSelection = -1;
		}


		protected void event_Lookup_Merchant(object sender, EventArgs e)
		{			
			AbstractEditor r = new MerchantSelector();
			r.MerchantSelection = MerchantCtrl.Text;
			r.ShowDialog();
			MerchantCtrl.Text = r.MerchantSelection;
			MerchantCtrl.Select(); // you must do this to get data change to commit if the control wasn't selected.
			r.MerchantSelection = "";
		}

		private void event_boolcvt(object sender, ConvertEventArgs e)
		{
			if (e.Value.GetType()==typeof(System.DBNull))
			{
				e.Value=false;
			}
		}

		private void event_New(object sender, EventArgs e)
		{
			NewRec(false);
		}

		private void event_First(object sender, EventArgs e)
		{
			if (EnforceConstraints(true))
			{
				_ti.pos=1;
				BindingContext[_dt].Position=0;
				UpdateRecordCountInfo();
			}
		}

		private void event_Prev(object sender, EventArgs e)
		{
			if (EnforceConstraints(true))
			{
				if (_ti.pos > 1)
				{
					_ti.pos--;
					BindingContext[_dt].Position--;
					UpdateRecordCountInfo();
				}
			}
		}

		private void event_Next(object sender, EventArgs e)
		{
			if (EnforceConstraints(true))
			{
				if (_ti.pos < _ti.rows)
				{
					_ti.pos++;
					BindingContext[_dt].Position++;
					UpdateRecordCountInfo();
				}
			}
		}

		private void event_Last(object sender, EventArgs e)
		{
			if (EnforceConstraints(true))
			{
				int n=_dt.Rows.Count;
				_ti.pos=_ti.rows;
				BindingContext[_dt].Position=_dt.Rows.Count-1;
				UpdateRecordCountInfo();
			}
		}

		private void event_Delete(object sender, EventArgs e)
		{
			if (EnforceConstraints(true))
			{
				if (BindingContext[_dt].Position > -1)
				{
					_dt.Rows.RemoveAt(BindingContext[_dt].Position);
					Delete();
				}
			}
		}

		#endregion

		#region Properties

		public DBContainer db
		{
			get
			{
				return _db;
			}
		}

		public bool AllIsGood
		{
			get
			{
				return _AllIsGood;
			}
		}

		public bool HasChanges
		{
			get
			{
				return _HasChanges;
			}
		}

		public string dbPath
		{
			get
			{
				return _dbPath;
			}
		}

		public int ColorSelection
		{
			set
			{
				_ColorSelection = value;
			}
			get
			{
				return _ColorSelection;
			}
		}

		public int EmblemSelection
		{
			set
			{
				_EmblemSelection = value;
			}
			get
			{
				return _EmblemSelection;
			}
		}

		public int ItemModelSelection
		{
			set
			{
				_ItemModelSelection = value;
			}
			get
			{
				return _ItemModelSelection;
			}
		}

		public int MonsterModelSelection
		{
			set
			{
				_MonsterModelSelection = value;
			}
			get
			{
				return _MonsterModelSelection;
			}
		}

		public int WeaponEffectSelection
		{
			set
			{
				_WeaponEffectSelection = value;
			}
			get
			{
				return _WeaponEffectSelection;
			}
		}

		public int RegionSelection
		{
			set
			{
				_RegionSelection = value;
			}
			get
			{
				return _RegionSelection;
			}
		}

		public int ObjectTypeSelection
		{
			set
			{
				_ObjectTypeSelection = value;
			}
			get
			{
				return _ObjectTypeSelection;
			}
		}

		public int ItemTypeSelection
		{
			set
			{
				_ItemTypeSelection = value;
			}
			get
			{
				return _ItemTypeSelection;
			}
		}

		public int RaceSelection
		{
			set
			{
				_RaceSelection = value;
			}
			get
			{
				return _RaceSelection;
			}
		}

		public int ClassTypeSelection
		{
			set
			{
				_ClassTypeSelection = value;
			}
			get
			{
				return _ClassTypeSelection;
			}
		}

		public int RealmSelection
		{
			set
			{
				_RealmSelection = value;
			}
			get
			{
				return _RealmSelection;
			}
		}

		public string SpellLineSelection
		{
			set
			{
				_SpellLineSelection = value;
			}
			get
			{
				return _SpellLineSelection;
			}
		}

		public int SpellSelection
		{
			set
			{
				_SpellSelection = value;
			}
			get
			{
				return _SpellSelection;
			}
		}

		public string SpecSelection
		{
			set
			{
				_SpecSelection = value;
			}
			get
			{
				return _SpecSelection;
			}
		}

		public int StyleSelection
		{
			set
			{
				_StyleSelection = value;
			}
			get
			{
				return _StyleSelection;
			}
		}

		public int PrivilegeSelection
		{
			set
			{
				_PrivilegeSelection = value;
			}
			get
			{
				return _PrivilegeSelection;
			}
		}

		public int HandSelection
		{
			set
			{
				_HandSelection = value;
			}
			get
			{
				return _HandSelection;
			}
		}

		public int SlotSelection
		{
			set
			{
				_SlotSelection = value;
			}
			get
			{
				return _SlotSelection;
			}
		}

		public int DamageSelection
		{
			set
			{
				_DamageSelection = value;
			}
			get
			{
				return _DamageSelection;
			}
		}

		public string EquipmentSelection
		{
			set
			{
				_EquipmentSelection = value;
			}
			get
			{
				return _EquipmentSelection;
			}
		}

		public string ItemSelection
		{
			set
			{
				_ItemSelection = value;
			}
			get
			{
				return _ItemSelection;
			}
		}

		public string MerchantSelection
		{
			set
			{
				_MerchantSelection = value;
			}
			get
			{
				return _MerchantSelection;
			}
		}

		public string MerchantItemSelection
		{
			set
			{
				_MerchantItemSelection = value;
			}
			get
			{
				return _MerchantItemSelection;
			}
		}

		public string MobSelection
		{
			set
			{
				_MobSelection = value;
			}
			get
			{
				return _MobSelection;
			}
		}

		public string sRowID
		{
			set
			{
				_sRowID = value;
			}
			get
			{
				return _sRowID;
			}
		}

		public int iColumn
		{
			set
			{
				_iColumn = value;
			}
			get
			{
				return _iColumn;
			}
		}

		public int Maxwidth
		{
			set
			{
				_Maxwidth = value;
			}
			get
			{
				return _Maxwidth;
			}
		}

		public Form myParentForm
		{
			set
			{
				m_ParentForm = value;
			}
		}

		protected TableInfo ti
		{
			get 
			{
				return _ti;
			}
		}

		protected DataTable dt
		{
			get 
			{
				return _dt;
			}
			set
			{
				_dt = value;
			}
		}

		protected int newRow
		{
			get
			{
				return _newRow;
			}
			set
			{
				_newRow = value;
			}
		}

		#endregion

		#region Methods

		protected int SafeIntParse(string sInt)
		{
			try
			{
				return int.Parse(sInt);
			}
			catch
			{
				return -1;
			}
		}

		protected void UpdateRecordCountInfo()
		{
			_ti.pos=BindingContext[dt].Position+1;
			if (_ti.tb != null)
			{
				_ti.tb.Text="  "+sRecord+" "+_ti.pos.ToString()+" "+sOf+" "+_ti.rows.ToString();
			}
		}

		protected int findRow(int colnum, string criteria)
		{
			for (int i = 0; i < _dt.Rows.Count; i++)
			{
				if (_dt.Rows[i][colnum].ToString() == criteria)
				{
					return i;
				}
			}
			return -1;
		}

		protected class TableInfo
		{
			public TableInfo()
			{
				tb=null;
				rows=0;
				pos=0;
				keycol="";
				keyval="";
			}

			public TextBox tb;
			public int rows;
			public int pos;
			public string keycol;
			public string keyval;
		}

		protected void NewRec(bool fix)
		{
			if (EnforceConstraints(true))
			{				
				_dt.DataSet.EnforceConstraints = false;
				_dt.Rows.Add(_dt.NewRow());
				_newRow=_dt.Rows.Count-1;
				BindingContext[dt].Position=_newRow;
				_ti.pos=_newRow+1;
				_ti.rows=_newRow+1;

				if (fix)
				{
					// put back old values...
					int c = 0;
					foreach(Control gbctrl in this.DynamicForm.Controls)
					{
						GroupBox gb = gbctrl as GroupBox;
						if (gb != null) 
						{
							foreach (Control ctrl in gb.Controls)
							{
								TextBox tb = ctrl as TextBox;
								if (tb != null) 
								{
									tb.Text = ControlBackup[c];
									tb.Select();
									c++;
								}
								CheckBox cb = ctrl as CheckBox;
								if (cb != null) 
								{
									cb.Checked = (ControlBackup[c] == "True");
									cb.Select();
									c++;
								}
							}							
							for (int n=c; n<ControlBackup.Length; n++) ControlBackup[n] = "";
						}
					}
				}
				else
				{
					// apply defaults
					foreach(Control gbctrl in this.DynamicForm.Controls)
					{
						GroupBox gb = gbctrl as GroupBox;
						if (gb != null) 
						{
							foreach (Control ctrl in gb.Controls)
							{
								TextBox tb = ctrl as TextBox;
								if (tb != null)
								{
									tb.Text = Apply_Defaults(tb.Name);
									tb.Select();
								}
								CheckBox cb = ctrl as CheckBox;
								if (cb != null) 
								{
									cb.Checked = true;
									cb.Select();
								}
							}
						}
					}
				}
				// if this is an editor sub-form (merchant items for a merchant for example)
				// populate the key.
				if ((ti.keycol != "") & (ti.keyval != ""))
				{
					_dt.Rows[_newRow][ti.keycol] = ti.keyval;
				}
				if ((this.GuidCtrl != null) & (_AutoGenerateGUIDs))
				{	
					this.GuidCtrl.Text = System.Guid.NewGuid().ToString();
					this.GuidCtrl.Select();
				}
				UpdateRecordCountInfo();
			}
		}

		protected void GotoRow(DataRow row)
		{
			for (int i=0; i<_dt.Rows.Count; i++)
			{
				if (_dt.Rows[i]==row)
				{
					BindingContext[dt].Position=i;
					break;
				}
			}
		}

		protected bool EnforceConstraints(bool Prompt)
		{
			_dt.AcceptChanges();
			if (_newRow != -1)
			{
				try
				{	
					m_ParentForm.Cursor = System.Windows.Forms.Cursors.WaitCursor;
					int c = 0;
					foreach(Control gbctrl in this.DynamicForm.Controls)
					{
						GroupBox gb = gbctrl as GroupBox;
						if (gb != null) 
						{
							foreach (Control ctrl in gb.Controls)
							{
								TextBox tb = ctrl as TextBox;
								if (tb != null) 
								{
									ControlBackup[c] = tb.Text;
									c++;
								}								
								CheckBox cb = ctrl as CheckBox;
								if (cb != null) 
								{
									ControlBackup[c] = cb.Checked.ToString();								
									c++;
								}
							}
						}
					}
					_dt.DataSet.EnforceConstraints = true;					
					_newRow = -1;
					m_ParentForm.Cursor = System.Windows.Forms.Cursors.Default;
					return true;
				}
				catch (Exception ex)
				{
					m_ParentForm.Cursor = System.Windows.Forms.Cursors.Default;
					if (Prompt)
					{
						DialogResult dlgRes = MessageBox.Show(sRecordIncompleteMsg,sRecordIncompleteTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
						if (dlgRes == DialogResult.Yes)
						{
							_dt.DataSet.EnforceConstraints = false;
							//GotoRow(_dt.Rows[_newRow]);

							_dt.Rows[_newRow].Delete();
							Delete();
							_newRow = -1;
							_dt.AcceptChanges();
							NewRec(true);
							return false;
						}
						else
						{
							_dt.Rows[_newRow].Delete();
							Delete();
							_newRow = -1;
							return true;
						}
					}
					else
					{
						_dt.Rows[_newRow].Delete();
						Delete();
						_newRow = -1;
						return true;
					}
				}
			}
			else
			{
				return true;
			}
		}

		private void Delete()
		{
			int n=_dt.Rows.Count;			
			_ti.rows=n;
			if (_ti.pos > _ti.rows)
			{
				_ti.pos=_ti.rows;
			}
			UpdateRecordCountInfo();
			if (n != 0)
			{
				BindingContext[dt].Position=_ti.pos-1;
			}

		}

		protected void Edit_Item(string tablename, string RowID, ComboBox cb, out int Selection, int keycolumn, int namecolumn, bool returnkey, ref SortedList ExistingList)
		{
			// edit/create records (int selection index)
			string strSelection;
			Edit_Item(tablename, RowID, cb, out strSelection, keycolumn, namecolumn, returnkey, ref ExistingList);			
			try
			{
				Selection = int.Parse(strSelection);
			}
			catch
			{
				Selection = -1;
			}
		}

		protected void Edit_Item(string tablename, string RowID, ComboBox cb, out string Selection, int keycolumn, int namecolumn, bool returnkey, ref SortedList ExistingList)
		{
			// edit/create records (string selection index)
			m_ParentForm.Cursor = System.Windows.Forms.Cursors.WaitCursor;
			Selection = "";
			DBContainer tempdb = new DBContainer();
			tempdb.LoadSchema(dbPath + "\\" + tablename + ".xsd");
			tempdb.LoadDocument(dbPath + "\\" + tablename + ".xml");
			tempdb.FixXMLDataSet(tempdb.doc.DataSet,false,tablename);
			AbstractEditor editor = (new EditorSelector()).SetEditor(tablename,false);
			editor.db.AssignDocument(tempdb.doc);
			editor.sRowID = RowID;
			// we need to figure out how big the editor should be
			Graphics g = editor.CreateGraphics();
			int newwidth;
			editor.Maxwidth = 0;
			foreach (DataColumn dc in tempdb.doc.DataSet.Tables[tablename].Columns)
			{
				newwidth = tempdb.LongestField(tempdb.doc.DataSet,tablename,dc.ColumnName,editor.Font,g);
				if (newwidth > editor.Maxwidth)
				{
					editor.Maxwidth = newwidth;
				}
			}
			m_ParentForm.Cursor = System.Windows.Forms.Cursors.Default;
			editor.ShowDialog();
			m_ParentForm.Cursor = System.Windows.Forms.Cursors.WaitCursor;
			if ((editor.AllIsGood) & (editor.HasChanges)) // OK was pressed, no constraint violations... good to go.
			{
				// now we have to save what they changed.. saving is 2 steps,
				// saving, then committing once the datadocument is freed.
				editor.Dispose();
				m_ParentForm.Refresh();
				tempdb.FixXMLDataSet(tempdb.doc.DataSet,true,tablename); // fix null rows if any
				tempdb.SaveXml(dbPath + "\\" + tablename + ".xml");
				tempdb.ClearDocument();
				tempdb.CommitXml(dbPath + "\\" + tablename + ".xml");
				tempdb = null;
				// add new items to the listbox and and select the new item we find.
				ExistingList = InitializeList(tablename,keycolumn,namecolumn,false,false);
				IDictionaryEnumerator en;
				en = ExistingList.GetEnumerator();
				string item;
				if (RowID == "")
				{
					// find first new item they added (best we can do)
					while (en.MoveNext())
					{	
						item = en.Key.ToString();
						if (!cb.Items.Contains(item))
						{		
							if (returnkey)
							{
								Selection = item;
							}
							else
							{
								Selection = en.Value.ToString();		
							}
							cb.Text = item;
							break;
						}		
					}
				}
				else				
				{
					Selection = RowID;
				}
				en.Reset();
				// we need to clear it and re-add all the items in case they deleted stuff
				cb.Items.Clear(); 
				while (en.MoveNext())
				{	
					item = en.Key.ToString();
					cb.Items.Add(item);					
				}
			}
			else
			{
				editor.Dispose();
				m_ParentForm.Refresh();
			}
			System.GC.Collect();
			m_ParentForm.Cursor = System.Windows.Forms.Cursors.Default;
		}

		protected void Edit_Item(string tablename, string pkcolname,  string fkcolname, AbstractEditor editor, string Selection)
		{
			// edit/create records enforcing pk/fk relationship (merchants to items, etc.)
			m_ParentForm.Cursor = System.Windows.Forms.Cursors.WaitCursor;	
			DBContainer tempdb = new DBContainer();
			tempdb.LoadSchema(dbPath + "\\" + tablename + ".xsd");
			tempdb.LoadDocument(dbPath + "\\" + tablename + ".xml");
			tempdb.FixXMLDataSet(tempdb.doc.DataSet,false,tablename);
			// Selection = db.doc.DataSet.Tables[0].Rows[ti.pos-1][pkcolname].ToString();
			// this was no good for editing a new (unsaved) merchants items... so I make 
			// them pass it in now.  I should really make them save the merchant before
			// I let them edit it's items, but it would be a pain... 
			XmlDataDocument docFragment = new XmlDataDocument();
			docFragment.DataSet.ReadXmlSchema(dbPath + "\\" + tablename + ".xsd");	
			XmlElement rElem=docFragment.CreateElement(tablename);
			docFragment.AppendChild(rElem);	
			string XPath = "/" + tablename + "/*[name() = '" + tablename + "'][" + fkcolname + "=\"" + Selection + "\"]";
			XmlNodeList nodeList = tempdb.doc.SelectNodes(XPath);
			foreach (XmlNode aNode in nodeList) 
			{
				XmlDocumentFragment docFrag = docFragment.CreateDocumentFragment();
				XmlElement elem = docFragment.CreateElement(aNode.LocalName);
				elem.InnerXml = aNode.InnerXml;
				docFrag.AppendChild(elem);
				docFragment.DocumentElement.AppendChild(docFrag);
				docFragment.DataSet.AcceptChanges();
			}
			tempdb.FixXMLDataSet(docFragment.DataSet,false,tablename);
			editor.db.AssignDocument(docFragment);
			editor.ti.keycol = fkcolname;
			editor.ti.keyval = Selection;
			if (editor.db.doc.DataSet.Tables[0].Rows.Count > 0)
			{
				// edit starting at first row
				editor.sRowID = editor.db.doc.DataSet.Tables[0].Rows[0][fkcolname].ToString();
			}
			else
			{
				editor.sRowID = ""; // insert 
			}
			// we need to figure out how big the editor should be
			Graphics g = editor.CreateGraphics();
			int newwidth;
			editor.Maxwidth = 0;
			foreach (DataColumn dc in tempdb.doc.DataSet.Tables[tablename].Columns)
			{
				newwidth = tempdb.LongestField(tempdb.doc.DataSet,tablename,dc.ColumnName,editor.Font,g);
				if (newwidth > editor.Maxwidth)
				{
					editor.Maxwidth = newwidth;
				}
			}
			m_ParentForm.Cursor = System.Windows.Forms.Cursors.Default;
			editor.ShowDialog();
			m_ParentForm.Cursor = System.Windows.Forms.Cursors.WaitCursor;	
			if (editor.AllIsGood) // OK was pressed, no constraint violations... good to go.
			{
				// now we have to save what they changed.. saving is 2 steps,
				// saving, then committing once the datadocument is freed.
				editor.Dispose();
				m_ParentForm.Refresh();
				tempdb.doc.DataSet.Merge(docFragment.DataSet,false,MissingSchemaAction.Add);
				tempdb.FixXMLDataSet(tempdb.doc.DataSet,true,tablename); // fix null rows if any
				tempdb.SaveXml(dbPath + "\\" + tablename + ".xml");
				tempdb.ClearDocument();
				tempdb.CommitXml(dbPath + "\\" + tablename + ".xml");
			}
			else
			{
				editor.Dispose();
				m_ParentForm.Refresh();
			}
			System.GC.Collect();
			m_ParentForm.Cursor = System.Windows.Forms.Cursors.Default;
		}

		protected void Arrange_Items(string tablename, string pkcolname, string fkcolname, string friendlynamecolumn, ref SortedList friendlynamelookuplist, string friendlynamelookupcolumn, string fktablePKcolumnname)
		{   // tablename is name of FK table... we already have PK table open...
			m_ParentForm.Cursor = System.Windows.Forms.Cursors.WaitCursor;
			editorReorder editor = new editorReorder();
			// load the items into a custom listbox with order buttons (editorReorder)
			DBContainer tempdb = new DBContainer();
			tempdb.LoadSchema(dbPath + "\\" + tablename + ".xsd");
			tempdb.LoadDocument(dbPath + "\\" + tablename + ".xml");
			tempdb.FixXMLDataSet(tempdb.doc.DataSet,false,tablename);
			string Selection = db.doc.DataSet.Tables[0].Rows[ti.pos-1][pkcolname].ToString();
			XmlDataDocument docFragment = new XmlDataDocument();
			docFragment.DataSet.ReadXmlSchema(dbPath + "\\" + tablename + ".xsd");	
			XmlElement rElem=docFragment.CreateElement(tablename);
			docFragment.AppendChild(rElem);	
			string XPath = "/" + tablename + "/*[name() = '" + tablename + "'][" + fkcolname + "=\"" + Selection + "\"]";
			XmlNodeList nodeList = tempdb.doc.SelectNodes(XPath);
			int index = 0;
			Hashtable h = new Hashtable();
			string nametoadd;
			foreach (XmlNode aNode in nodeList) 
			{
				XmlElement elem = docFragment.CreateElement(aNode.LocalName);
				elem.InnerXml = aNode.InnerXml;
				if (friendlynamelookuplist != null)
				{
					nametoadd = FindKey(ref friendlynamelookuplist,aNode.SelectSingleNode(friendlynamecolumn).InnerText);
				}
				else
				{
					nametoadd = friendlynamecolumn;
				}
				editor.lbOrder.AddOrderedItem(nametoadd,index,elem);
				h.Add(index,aNode.SelectSingleNode(fktablePKcolumnname).InnerText);
				index++;
			}
			m_ParentForm.Cursor = System.Windows.Forms.Cursors.Default;
			editor.ShowDialog();
			m_ParentForm.Cursor = System.Windows.Forms.Cursors.WaitCursor;
			if (editor.isOK)
			{
				// build a recordset of the reordered items 
				for (int i=editor.lbOrder.Items.Count-1; i>= 0; i--)
				{
					XmlDocumentFragment docFrag = docFragment.CreateDocumentFragment();
					docFrag.AppendChild(editor.lbOrder.GetItemContents(i));
					docFragment.DocumentElement.AppendChild(docFrag);
				}
				docFragment.DataSet.AcceptChanges();
				editor.Dispose();
				m_ParentForm.Refresh();
				// remove the old items from the main table
				IDictionaryEnumerator en;
				en = h.GetEnumerator();
				while (en.MoveNext())
				{
					tempdb.doc.DataSet.Tables[0].Rows.Find(en.Value.ToString()).Delete();
				}
				tempdb.doc.DataSet.AcceptChanges();
				// now we have to save what they changed.. saving is 2 steps,
				// saving, then committing once the datadocument is freed.
				tempdb.doc.DataSet.Merge(docFragment.DataSet,false,MissingSchemaAction.Add);
				tempdb.FixXMLDataSet(tempdb.doc.DataSet,true,tablename); // fix null rows if any
				tempdb.SaveXml(dbPath + "\\" + tablename + ".xml");
				tempdb.ClearDocument();
				tempdb.CommitXml(dbPath + "\\" + tablename + ".xml");			
			}
			else
			{
				editor.Dispose();
				m_ParentForm.Refresh();
			}
			System.GC.Collect();
			m_ParentForm.Cursor = System.Windows.Forms.Cursors.Default;
		}

		private string Sel_Item(ref SortedList h, string Selected)
		{
			string Selection = "";
			IDictionaryEnumerator en;
			en = h.GetEnumerator();
			while (en.MoveNext())
			{	
				if (en.Key.ToString() == Selected) 
				{
					Selection = en.Value.ToString();
					break;
				}
			}
			return Selection;
		}

		protected string Selected_Item(ref SortedList h, string Selected)
		{
			return Sel_Item(ref h,Selected);
		}
		protected int Selected_Item(ref SortedList h, string Selected, bool dummyparam)
			//dummyparam just because c# is stupid and doesn't recognize int and string
			//return values as a different implementation.
		{
			try
			{
				return int.Parse(Sel_Item(ref h,Selected));
			}
			catch
			{
				return 0;
			}
		}

		protected string FindKey(ref SortedList h, string Selection)
		{
			int i = h.IndexOfValue(Selection);
			if (i != -1)
			{
				return (string)h.GetKey(i);
			}
			else
			{
				return "";
			}
		}

		protected string FindKey(ref SortedList h, int Selection)
		{
			int i = h.IndexOfValue(Selection.ToString());
			if (i != -1)
			{
				return (string)h.GetKey(i);
			}
			else
			{
				return "";
			}
		}

		#endregion

		#region ListInitialization
		// the methods here are used by inheritors to instantiate lists they will use.  
		// we don't want to instantiate them all all the time, as some are quite large.

		protected void InitializeFromXml(ref SortedList List, string XPath)
		{
			List = new SortedList();
			XmlDocument doc = new XmlDocument();
			if (File.Exists(System.Globalization.CultureInfo.CurrentCulture.ThreeLetterISOLanguageName+"_"+"selectcfg.xml"))
			{
				doc.Load(System.Globalization.CultureInfo.CurrentCulture.ThreeLetterISOLanguageName+"_"+"selectcfg.xml");
			}
			else
			{
				doc.Load("selectcfg.xml");
			}
			XmlNodeList nodeList = doc.SelectNodes(XPath);
			foreach (XmlNode aNode in nodeList) 
			{
				List.Add(aNode.SelectSingleNode("name").InnerText,aNode.SelectSingleNode("id").InnerText);
			}
			nodeList = null;
			doc = null;
		}

		protected void InitializeRealms()
		{
			InitializeFromXml(ref Realms,"/selectorconfig/Realms/*[name() = 'Realm']");
		}

		protected void InitializePrivileges()
		{
			InitializeFromXml(ref Privileges,"/selectorconfig/Privileges/*[name() = 'Privilege']");
		}

		protected void InitializeHand()
		{
			InitializeFromXml(ref Hand,"/selectorconfig/Hands/*[name() = 'Hand']");
		}

		protected void InitializeDamage()
		{
			InitializeFromXml(ref Damage,"/selectorconfig/DamageType/*[name() = 'Damage']");
		}

		protected void InitializeSlot()
		{
			InitializeFromXml(ref Slot,"/selectorconfig/Slots/*[name() = 'Slot']");
		}

		protected void InitializeColor()
		{
			InitializeFromXml(ref Color,"/selectorconfig/colors/*[name() = 'color']");
		}

		protected void InitializeEmblem()
		{
			InitializeFromXml(ref Emblem,"/selectorconfig/emblemids/*[name() = 'emblem']");
		}

		protected void InitializeItemModel()
		{
			InitializeFromXml(ref ItemModel,"/selectorconfig/itemmodelids/*[name() = 'itemmodel']");
		}

		protected void InitializeMonsterModel()
		{	
			InitializeFromXml(ref MonsterModel,"/selectorconfig/monstermodelids/*[name() = 'monstermodel']");
		}

		protected void InitializeWeaponEffect()
		{
			InitializeFromXml(ref WeaponEffect,"/selectorconfig/weaponeffects/*[name() = 'effect']");
		}

		protected void InitializeObjectType()
		{
			InitializeFromXml(ref ObjectType,"/selectorconfig/ObjectTypes/*[name() = 'ObjectType']");
		}

		protected void InitializeItemType()
		{
			InitializeFromXml(ref ItemType,"/selectorconfig/ItemTypes/*[name() = 'ItemType']");
		}

		protected void InitializeRace()
		{
			InitializeFromXml(ref Race,"/selectorconfig/Races/*[name() = 'Race']");
		}

		protected void InitializeClassType()
		{
			InitializeFromXml(ref ClassType,"/selectorconfig/Classes/*[name() = 'Class']");
		}

		protected void InitializeRegions()
		{
			InitializeFromXml(ref Regions,"/selectorconfig/Regions/*[name() = 'Region']");
		}

		protected SortedList InitializeList(string tablename, int keycolumn, int namecolumn, bool uniquevalues, bool showkeycolumn)
		{
			m_ParentForm.Cursor = System.Windows.Forms.Cursors.WaitCursor;		    
			DBContainer tempdb = new DBContainer();
			tempdb.LoadSchema(_dbPath + "\\" + tablename + ".xsd");
			tempdb.LoadDocument(_dbPath + "\\" + tablename + ".xml");
			tempdb.FixXMLDataSet(tempdb.doc.DataSet,false, tablename);
			if (dgLookupEnabled)
			{
				switch (tablename)
				{
					case "NPCEquipment" :
						cEquipment = tempdb;
						dgEquipment = new ScrollingDataGrid();
						dgEquipment.ReadOnly = true;
						dgEquipment.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
						dgEquipment.SetDataBinding(cEquipment.doc.DataSet,tablename);
						cEquipment.SetTableStyles(tablename, dgEquipment, cEquipment.doc.DataSet,out dgEquipmentWidth);
						break;
					case "Merchant" :
						cMerchant = tempdb;
						dgMerchant = new ScrollingDataGrid();
						dgMerchant.ReadOnly = true;
						dgMerchant.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
						dgMerchant.SetDataBinding(cMerchant.doc.DataSet,tablename);
						cMerchant.SetTableStyles(tablename, dgMerchant, cMerchant.doc.DataSet,out dgMerchantWidth);
						break;
					case "ItemTemplate" :
						cItems = tempdb;
						dgItems = new ScrollingDataGrid();
						dgItems.ReadOnly = true;
						dgItems.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
						dgItems.SetDataBinding(cItems.doc.DataSet,tablename);
						cItems.SetTableStyles(tablename, dgItems, cItems.doc.DataSet,out dgItemsWidth);
						break;
					case "SpellLine" :
						cSpellLine = tempdb;
						dgSpellLine = new ScrollingDataGrid();
						dgSpellLine.ReadOnly = true;
						dgSpellLine.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
						dgSpellLine.SetDataBinding(cSpellLine.doc.DataSet,tablename);
						cSpellLine.SetTableStyles(tablename, dgSpellLine, cSpellLine.doc.DataSet,out dgSpellLineWidth);
						break;
					case "Spell" :
						cSpell = tempdb;
						dgSpell = new ScrollingDataGrid();
						dgSpell.ReadOnly = true;
						dgSpell.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
						dgSpell.SetDataBinding(cSpell.doc.DataSet,tablename);
						cSpell.SetTableStyles(tablename, dgSpell, cSpell.doc.DataSet,out dgSpellWidth);
						break;
					case "Specialization" :
						cSpecialization = tempdb;
						dgSpecialization = new ScrollingDataGrid();
						dgSpecialization.ReadOnly = true;
						dgSpecialization.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
						dgSpecialization.SetDataBinding(cSpecialization.doc.DataSet,tablename);
						cSpecialization.SetTableStyles(tablename, dgSpecialization, cSpecialization.doc.DataSet,out dgSpecializationWidth);
						break;
					case "Style" :
						cStyle = tempdb;
						dgStyle = new ScrollingDataGrid();
						dgStyle.ReadOnly = true;
						dgStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
						dgStyle.SetDataBinding(cStyle.doc.DataSet,tablename);
						cStyle.SetTableStyles(tablename, dgStyle, cStyle.doc.DataSet,out dgStyleWidth);
						break;
				}
			}
			// build sorted lists for selectors and tooltips
			SortedList h = new SortedList();			
			foreach (DataRow dr in tempdb.doc.DataSet.Tables[0].Rows)
			{	
				// name (key) and id (value)
				if (uniquevalues)
				{
					if (!h.ContainsKey(dr[namecolumn].ToString()))
					{
						if (showkeycolumn)
						{
							h.Add(dr[namecolumn].ToString()+" ["+dr[keycolumn].ToString()+"]",dr[keycolumn].ToString());
						}
						else
						{
							h.Add(dr[namecolumn].ToString(),dr[keycolumn].ToString());
						}
					}
				}
				else
				{
					if (h.ContainsKey(dr[namecolumn].ToString()) | (showkeycolumn))
					{
						h.Add(dr[namecolumn].ToString()+" ["+dr[keycolumn].ToString()+"]",dr[keycolumn].ToString());
					}
					else
					{
						h.Add(dr[namecolumn].ToString(),dr[keycolumn].ToString());

					}
				}
			}		
			m_ParentForm.Cursor = System.Windows.Forms.Cursors.Default;
			if (!dgLookupEnabled)
			{
				tempdb.ClearDocument();
				tempdb = null;
			}
			return h;
		}

		#endregion

		#region UI Generation

		protected ComboBox MakeComboBox(int x, int y, int w, Control parent, ref SortedList contents, EventHandler ev)
		{			
			ComboBox cb=new ComboBox();
			cb.Location=new Point(x, y+2);
			cb.Size=new Size(w, 15);
			cb.Font=new Font(cb.Font, FontStyle.Regular);
			// sort is already handled by the sortedlist, which sorts large data sets
			// tremendously faster then the listbox sort does.
			cb.Sorted = false; 
			for ( int i = 0; i < contents.Count; i++ )  
			{
				cb.Items.Add(contents.GetKey(i));
			}
			cb.Visible=true;
			cb.Parent=parent;
			cb.SelectedIndexChanged += ev;
			return cb;
		}

		protected Label MakeLabel(int x, int y, string name, Control parent)
		{
			Label lbl=new Label();
			lbl.Location=new Point(x, y+2);
			lbl.Size=new Size(TextWidthAdjustment, 15);
			lbl.Text=name;
			lbl.Font=new Font(lbl.Font, FontStyle.Regular);
			lbl.Visible=true;
			lbl.Parent=parent;
			return lbl;
		}

		protected Control BuildNavBar(int x, int y, Control parent)
		{
			Panel navPanel=new Panel();
			navPanel.Location=new Point(x, y);
			navPanel.Size=new Size(280, 19);
			navPanel.Visible=true;
			navPanel.Parent=parent;
			navPanel.BorderStyle=BorderStyle.FixedSingle;
			MakeButton("<<", 0, 0, 30, 17, new EventHandler(event_First), navPanel);
			MakeButton("<", 30, 0, 30, 17, new EventHandler(event_Prev), navPanel);
			MakeButton("*", 60, 0, 30, 17, new EventHandler(event_New), navPanel);
			MakeButton(">", 90, 0, 30, 17, new EventHandler(event_Next), navPanel);
			MakeButton(">>", 120, 0, 30, 17, new EventHandler(event_Last), navPanel);
			_ti.tb=new TextBox();
			_ti.tb.Location=new Point(150, 2);
			_ti.tb.Size=new Size(100, 18);
			_ti.tb.BorderStyle=BorderStyle.None;
			_ti.tb.Font=new Font("Tahoma", 8, FontStyle.Regular);
			_ti.tb.Parent=navPanel;
			_ti.tb.Visible=true;
			_ti.tb.Text="  "+sRecord+" 0 "+sOf+ " 0";
			MakeButton("X", 250, 0, 30, 17, new EventHandler(event_Delete), navPanel);
			return navPanel;
		}

		protected Button MakeButton(string text, int x, int y, int sx, int sy, EventHandler ev, Control parent)
		{
			Button aButton=new Button();
			aButton.Text=text;
			aButton.Location=new Point(x, y);
			aButton.Size=new Size(sx, sy);
			aButton.Parent=parent;
			aButton.Visible=true;
			aButton.Font=new Font("Tahoma", 8, FontStyle.Regular);
			aButton.Click+=ev;
			return aButton;
		}

		private void AddMouseEnterEvent(Control ctrl, string ColumnName)
		{
			ctrl.Name = ColumnName;
			ctrl.MouseEnter += new System.EventHandler(event_MouseEnter);
		}

		protected Control CreateEditControl(string tablename, int x, int y, string DataType, Control parent, DataColumn dc)
		{
			Control ctrl=null;
			TextBox tb=null;
			if (DataType == "Boolean")
			{
				CheckBox ck1=new CheckBox();
				ck1.Location=new Point(x, y);
				ck1.Size=new Size(60, 20);
				ck1.Font=new Font(ck1.Font, FontStyle.Regular);
				ck1.Visible=true;
				ck1.Text="";
				ck1.Name = tablename + "." + dc.ColumnName;
				ck1.Parent=parent;
				ck1.AutoCheck=true;
				ctrl=ck1;
				Binding b=new Binding("Checked", _dt, dc.ColumnName);
				b.Format+=new ConvertEventHandler(event_boolcvt);
				ctrl.DataBindings.Add(b);
			}
			else if (DataType == "Char" | DataType == "String" | DataType == "DateTime")
			{
				tb=new TextBox();
				tb.Location=new Point(x, y);
				tb.Size=new Size(_Maxwidth, 20);
				tb.Font=new Font(tb.Font, FontStyle.Regular);
				tb.Visible=true;
				//tb.Name = tablename + name;
				tb.Parent=parent;
				ctrl=tb;
				ctrl.DataBindings.Add("Text", _dt, dc.ColumnName);
				AddMouseEnterEvent(ctrl,tablename + "." + dc.ColumnName);
			}
			else 
			{
				tb=new TextBox();
				tb.Location=new Point(x, y);
				tb.Size=new Size(60, 20);
				tb.Font=new Font(tb.Font, FontStyle.Regular);
				tb.Visible=true;
				//tb.Name = tablename + name;
				tb.Parent=parent;
				tb.TextAlign=HorizontalAlignment.Right;
				ctrl=tb;
				ctrl.DataBindings.Add("Text", _dt, dc.ColumnName);
				AddMouseEnterEvent(ctrl,tablename + "." + dc.ColumnName);
			}
			if (tb != null)
			{				
				if (ti.keyval != "")
				{
					if (dc.ColumnName == ti.keycol)
					{
						tb.ReadOnly = true;
					}
				}
			}
			return ctrl;
		}

		protected int FormHeight(int ay, int ry)
		{
			int h = ay + ry + FormHeightAdjustment;
			if (h > 580) h = 580;
			return h;
		}

		protected int FormWidth(int ax, int rx, int mw)
		{
			int w = ax + rx + mw + FormWidthAdjustment;
			if (w < 330) 
			{
				w = 330;
			}
			else  if (w > 780) 
			{
				w = 780;
			}
			return w;
		}

		protected GroupBox SetupGroupBoxWithNavigator(int ax, int ay, Control gbParent)
		{
			GroupBox gb=new GroupBox();
			gb.Font=new Font(gb.Font, FontStyle.Bold);
			gb.Text=dt.TableName;
			gb.Location=new Point(ax, ay);
			gb.Parent=gbParent;
			gb.Visible=true;			
			BuildNavBar(10, 15, gb);
			if (this.Text == "Editor")
			{
				this.Text = sEditor;
			}
			return gb;
		}

		protected GroupBox SetupDockGroupBox(int ax, int ay, int height, DockStyle Dock, bool visible, Control gbParent)
		{
			GroupBox gb=new GroupBox();
			gb.Font=new Font(gb.Font, FontStyle.Bold);
			gb.Location=new Point(ax, ay);
			gb.Height = height;
			gb.Dock = Dock;
			gb.Parent=gbParent;
			gb.Visible=visible;
			return gb;
		}

		protected Point SetupEditorForm()
		{
			dt = db.doc.DataSet.Tables[0];
			return new Point(10, 10);
		}

		protected int BuildColumns(int rx, int ry)
		{
			Control ctrl = null;
			foreach (DataColumn dc in dt.Columns)
			{
				MakeLabel(rx, ry, dc.ColumnName, _gb);
				ctrl = CreateEditControl(dt.TableName,rx+TextWidthAdjustment, ry, db.DataType(dc.DataType), _gb, dc);
				AddLookupButtonIfNeeded(dc.ColumnName,ctrl,rx,ry,_gb);
				ry += 20;
			}			
			return ry;
		}

		protected void AdjustFormSize(int ay, int ry, int ax, int rx)
		{
			this.Height = FormHeight(ay,ry);
			this.Width = FormWidth(ax,rx,Maxwidth);
			_gb.Size = new Size(_gb.Parent.Width-20,ry+10);
		}

		protected void OpenRecord()
		{
			int iRow;
			_ti.pos=1;
			_ti.rows=_dt.Rows.Count;

			if (_sRowID == "") // new
			{
				BindingContext[_dt].Position=0;
				NewRec(false);
			}
			else // edit
			{
				if (_ti.rows > 0) 
				{
					iRow = findRow(_iColumn,_sRowID);
					if (iRow > -1) GotoRow(_dt.Rows[iRow]);
				}
			}
			UpdateRecordCountInfo();		
		}

		protected int CalcWidth(int rx)
		{
			return rx+NumericWidthAdjustment;
		}
		protected int CalcWidth(int rx, int Maxwidth)
		{
			return rx+TextWidthAdjustment+Maxwidth;
		}

		protected void AddLookupButtonIfNeeded(string Column, Control ctrl, int rx, int ry, GroupBox gb)
		{
			// if you ever need to make sure you cant edit a table from itself just do
			// if db.doc.DataSet.Tables[0].TableName != "tablename" around offending code
			switch (Column)
			{				
					// LOOKUP EVENTS
				case "Region" :			
					RegionCtrl = ctrl;
					MakeButton("...",CalcWidth(rx), ry, 23, 21, new EventHandler(event_Lookup_Region), gb);
					break;
				case "BindRegion" :			
					BindRegionCtrl = ctrl;
					MakeButton("...", CalcWidth(rx), ry, 23, 21, new EventHandler(event_Lookup_BindRegion), gb);
					break;
				case "GravestoneRegion" :			
					GravestoneRegionCtrl = ctrl;
					MakeButton("...", CalcWidth(rx), ry, 23, 21, new EventHandler(event_Lookup_GravestoneRegion), gb);
					break;
				case "Realm" :			
					RealmCtrl = ctrl;
					MakeButton("...", CalcWidth(rx), ry, 23, 21, new EventHandler(event_Lookup_Realm), gb);
					break;
				case "EquipmentTemplateID" :
					EquipmentTemplateCtrl = ctrl;
					MakeButton("...", CalcWidth(rx,Maxwidth), ry, 23, 21, new EventHandler(event_Lookup_Equipment), gb);
					break;
				case "MerchantID" : 
					if (ti.keycol != "MerchantID") // don't allow ... in specificic merchants form.
					{
						MerchantCtrl = ctrl;
						MakeButton("...", CalcWidth(rx,Maxwidth), ry, 23, 21, new EventHandler(event_Lookup_Merchant), gb);
					}
					break;
				case "ItemTemplateID" : 
					ItemTemplateCtrl = ctrl;
					MakeButton("...", CalcWidth(rx,Maxwidth), ry, 23, 21, new EventHandler(event_Lookup_Item), gb);
					break;
				case "PrivLevel" : 
					PrivilegeCtrl = ctrl;
					MakeButton("...", CalcWidth(rx), ry, 23, 21, new EventHandler(event_Lookup_Privilege), gb);
					break;
				case "Hand" : 
					HandCtrl = ctrl;
					MakeButton("...", CalcWidth(rx), ry, 23, 21, new EventHandler(event_Lookup_Hand), gb);
					break;	
				case "Slot" : 
					SlotCtrl = ctrl;
					MakeButton("...", CalcWidth(rx), ry, 23, 21, new EventHandler(event_Lookup_Slot), gb);
					break;
				case "SlotPosition" : 
					SlotCtrl = ctrl;
					MakeButton("...", CalcWidth(rx), ry, 23, 21, new EventHandler(event_Lookup_Slot), gb);
					break;	
				case "Type_Damage" : 
					DamageCtrl = ctrl;
					MakeButton("...", CalcWidth(rx), ry, 23, 21, new EventHandler(event_Lookup_Damage), gb);
					break;	
				case "DamageType" : 
					DamageCtrl = ctrl;
					MakeButton("...", CalcWidth(rx), ry, 23, 21, new EventHandler(event_Lookup_Damage), gb);
					break;	
				case "LineName" : 
					SpellLineCtrl = ctrl;
					MakeButton("...", CalcWidth(rx,Maxwidth), ry, 23, 21, new EventHandler(event_Lookup_Spell_Line), gb);
					break;	
				case "SpellID" : 
					if (ti.keycol != "Spell_ID") // don't allow ... in spell form :)
					{
						SpellCtrl = ctrl;
						MakeButton("...", CalcWidth(rx), ry, 23, 21, new EventHandler(event_Lookup_Spell), gb);
					}
					break;		
				case "SpecKeyName" : 
					SpecCtrl = ctrl;
					MakeButton("...", CalcWidth(rx,Maxwidth), ry, 23, 21, new EventHandler(event_Lookup_Spec), gb);
					break;	
				case "StyleID" : 
					if (ti.keycol != "Style_ID") // don't allow ... in style form :)
					{
						StyleCtrl = ctrl;
						MakeButton("...", CalcWidth(rx), ry, 23, 21, new EventHandler(event_Lookup_Style), gb);
					}
					break;
				case "Object_Type" :
					ObjectTypeCtrl = ctrl;
					MakeButton("...", CalcWidth(rx), ry, 23, 21, new EventHandler(event_Lookup_ObjectType), gb);
					break;
				case "Item_Type" :
					ItemTypeCtrl = ctrl;
					MakeButton("...", CalcWidth(rx), ry, 23, 21, new EventHandler(event_Lookup_ItemType), gb);
					break;
				case "Race" :
					RaceCtrl = ctrl;
					MakeButton("...", CalcWidth(rx), ry, 23, 21, new EventHandler(event_Lookup_Race), gb);
					break;
				case "Class" :
					ClassTypeCtrl = ctrl;
					MakeButton("...", CalcWidth(rx), ry, 23, 21, new EventHandler(event_Lookup_ClassType), gb);
					break;
				case "Color" :
					ColorCtrl = ctrl;
					MakeButton("...", CalcWidth(rx), ry, 23, 21, new EventHandler(event_Lookup_Color), gb);
					break;
				case "Emblem" :
					EmblemCtrl = ctrl;
					MakeButton("...", CalcWidth(rx), ry, 23, 21, new EventHandler(event_Lookup_Emblem), gb);
					break;
				case "Effect" :
					WeaponEffectCtrl = ctrl;
					MakeButton("...", CalcWidth(rx), ry, 23, 21, new EventHandler(event_Lookup_WeaponEffect), gb);
					break;
				case "Model" :
					if (db.doc.DataSet.Tables[0].TableName == "InventoryItem" | db.doc.DataSet.Tables[0].TableName == "ItemTemplate" 
						| db.doc.DataSet.Tables[0].TableName == "NPCEquipment" | db.doc.DataSet.Tables[0].TableName == "WorldObject")
					{
						ItemModelCtrl = ctrl;
						MakeButton("...", CalcWidth(rx), ry, 23, 21, new EventHandler(event_Lookup_ItemModel), gb);
						Button ItemModelBtn = MakeButton("",CalcWidth(rx)+25, ry, 23, 21, new EventHandler(event_Render_ItemModel), gb);
						System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(AbstractEditor));
						ItemModelBtn.Image = ((System.Drawing.Image)(resources.GetObject("btnPaint.Image")));
					}
					else
					{
						MonsterModelCtrl = ctrl;
						MakeButton("...", CalcWidth(rx), ry, 23, 21, new EventHandler(event_Lookup_MonsterModel), gb);					
						Button MobModelBtn = MakeButton("",CalcWidth(rx)+25, ry, 23, 21, new EventHandler(event_Render_MonsterModel), gb);
						System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(AbstractEditor));
						MobModelBtn.Image = ((System.Drawing.Image)(resources.GetObject("btnPaint.Image")));
					}
					break;
					// GUID Generators
				case "Ability_ID" :
					if (db.doc.DataSet.Tables[0].TableName == "Ability")
					{
						GuidCtrl = ctrl;
						MakeButton("g", CalcWidth(rx,Maxwidth), ry, 23, 21, new EventHandler(event_Gimmie_Guid), gb);
					}
					break;
				case "Account_ID" :
					if (db.doc.DataSet.Tables[0].TableName == "Account")
					{
						GuidCtrl = ctrl;
						MakeButton("g", CalcWidth(rx,Maxwidth), ry, 23, 21, new EventHandler(event_Gimmie_Guid), gb);
					}
					break;
				case "BindPoint_ID" :
					if (db.doc.DataSet.Tables[0].TableName == "BindPoint")
					{
						GuidCtrl = ctrl;
						MakeButton("g", CalcWidth(rx,Maxwidth), ry, 23, 21, new EventHandler(event_Gimmie_Guid), gb);
					}
					break;
				case "DOLCharacters_ID" :
					if (db.doc.DataSet.Tables[0].TableName == "DOLCharacters")
					{
						GuidCtrl = ctrl;
						MakeButton("g", CalcWidth(rx,Maxwidth), ry, 23, 21, new EventHandler(event_Gimmie_Guid), gb);
					}
					break;
				case "DOLOptions_ID" :
					if (db.doc.DataSet.Tables[0].TableName == "DOLOptions")
					{
						GuidCtrl = ctrl;
						MakeButton("g", CalcWidth(rx,Maxwidth), ry, 23, 21, new EventHandler(event_Gimmie_Guid), gb);
					}
					break;
				case "InventoryItem_ID" :
					if (db.doc.DataSet.Tables[0].TableName == "InventoryItem")
					{
						GuidCtrl = ctrl;
						MakeButton("g", CalcWidth(rx,Maxwidth), ry, 23, 21, new EventHandler(event_Gimmie_Guid), gb);
					}
					break;
				case "ItemTemplate_ID" :
					if (db.doc.DataSet.Tables[0].TableName == "ItemTemplate")
					{
						GuidCtrl = ctrl;
						MakeButton("g", CalcWidth(rx,Maxwidth), ry, 23, 21, new EventHandler(event_Gimmie_Guid), gb);
					}
					break;
				case "LineXSpell_ID" :
					if (db.doc.DataSet.Tables[0].TableName == "LineXSpell")
					{
						GuidCtrl = ctrl;
						MakeButton("g", CalcWidth(rx,Maxwidth), ry, 23, 21, new EventHandler(event_Gimmie_Guid), gb);
					}
					break;
				case "Merchant_ID" :
					if (db.doc.DataSet.Tables[0].TableName == "Merchant")
					{
						GuidCtrl = ctrl;
						MakeButton("g", CalcWidth(rx,Maxwidth), ry, 23, 21, new EventHandler(event_Gimmie_Guid), gb);
					}
					break;
				case "MerchantItem_ID" :
					if (db.doc.DataSet.Tables[0].TableName == "MerchantItem")
					{
						GuidCtrl = ctrl;
						MakeButton("g", CalcWidth(rx,Maxwidth), ry, 23, 21, new EventHandler(event_Gimmie_Guid), gb);
					}
					break;
				case "Mob_ID" :
					if (db.doc.DataSet.Tables[0].TableName == "Mob")
					{
						GuidCtrl = ctrl;
						MakeButton("g", CalcWidth(rx,Maxwidth), ry, 23, 21, new EventHandler(event_Gimmie_Guid), gb);
					}
					break;
				case "NPCEquipment_ID" :
					if (db.doc.DataSet.Tables[0].TableName == "NPCEquipment")
					{
						GuidCtrl = ctrl;
						MakeButton("g", CalcWidth(rx,Maxwidth), ry, 23, 21, new EventHandler(event_Gimmie_Guid), gb);
					}
					break;
				case "Specialization_ID" :
					if (db.doc.DataSet.Tables[0].TableName == "Specialization")
					{
						GuidCtrl = ctrl;
						MakeButton("g", CalcWidth(rx,Maxwidth), ry, 23, 21, new EventHandler(event_Gimmie_Guid), gb);
					}
					break;
				case "SpecXStyle_ID" :
					if (db.doc.DataSet.Tables[0].TableName == "SpecXStyle")
					{
						GuidCtrl = ctrl;
						MakeButton("g", CalcWidth(rx,Maxwidth), ry, 23, 21, new EventHandler(event_Gimmie_Guid), gb);
					}
					break;
				case "Spell_ID" :
					if (db.doc.DataSet.Tables[0].TableName == "Spell")
					{
						GuidCtrl = ctrl;
						MakeButton("g", CalcWidth(rx,Maxwidth), ry, 23, 21, new EventHandler(event_Gimmie_Guid), gb);
					}
					break;
				case "SpellLine_ID" :
					if (db.doc.DataSet.Tables[0].TableName == "SpellLine")
					{
						GuidCtrl = ctrl;
						MakeButton("g", CalcWidth(rx,Maxwidth), ry, 23, 21, new EventHandler(event_Gimmie_Guid), gb);
					}
					break;
				case "Style_ID" :
					if (db.doc.DataSet.Tables[0].TableName == "Style")
					{
						GuidCtrl = ctrl;
						MakeButton("g", CalcWidth(rx,Maxwidth), ry, 23, 21, new EventHandler(event_Gimmie_Guid), gb);
					}
					break;
				case "WorldObject_ID" :
					if (db.doc.DataSet.Tables[0].TableName == "WorldObject")
					{
						GuidCtrl = ctrl;
						MakeButton("g", CalcWidth(rx,Maxwidth), ry, 23, 21, new EventHandler(event_Gimmie_Guid), gb);
					}
					break;
				case "ZonePoint_ID" :
					if (db.doc.DataSet.Tables[0].TableName == "ZonePoint")
					{
						GuidCtrl = ctrl;
						MakeButton("g", CalcWidth(rx,Maxwidth), ry, 23, 21, new EventHandler(event_Gimmie_Guid), gb);
					}
					break;
			}
			if ((GuidCtrl != null) & (_AutoGenerateGUIDs))
			{	
				GuidCtrl.Name = "GUIDCtrl";
			}
		}

		#endregion

	}
	#endregion

	#region Editor Selector Class

	public class EditorSelector
	{
		public AbstractEditor SetEditor(string TableName, bool SetBase)
		{
			AbstractEditor editor;
			string sBaseEditor = "";
			if (SetBase) sBaseEditor = TableName;
			switch (TableName)
			{
				case "Ability" :					
					editor = new AbilityEditor(sBaseEditor);
					break;
				case "Account" :
					editor = new AccountEditor(sBaseEditor);
					break;
				case "BindPoint" :
					editor = new BindPointEditor(sBaseEditor);
					break;
				case "DOLCharacters" :
					editor = new CharacterEditor(sBaseEditor);
					break;
				case "InventoryItem" :
					editor = new InventoryItemEditor(sBaseEditor);
					break;
				case "ItemTemplate" :
					editor = new ItemTemplateEditor(sBaseEditor);
					break;
				case "LineXSpell" :
					editor = new LineXSpellEditor(sBaseEditor);
					break;
				case "Merchant" :
					editor = new MerchantEditor(sBaseEditor);
					break;
				case "MerchantItem" :
					editor = new MerchantItemEditor(sBaseEditor);
					break;
				case "Mob" :
					editor = new MobEditor(sBaseEditor);
					break;
				case "NPCEquipment" :
					editor = new NPCEquipmentEditor(sBaseEditor);
					break;
				case "Specialization" :
					editor = new SpecEditor(sBaseEditor);
					break;
				case "SpecXStyle" : 
					editor = new SpecXStyleEditor(sBaseEditor);
					break;
				case "Spell" : 
					editor = new SpellEditor(sBaseEditor);
					break;
				case "SpellLine" :
					editor = new SpellLineEditor(sBaseEditor);
					break;
				case "Style" :
					editor = new StyleEditor(sBaseEditor);
					break;
				case "WorldObject" :
					editor = new WorldObjectEditor(sBaseEditor);
					break;
				case "ZonePoint" :
					editor = new ZonePointEditor(sBaseEditor);
					break;
				default : 
					editor = new GenericEditor();
					break;
			}
			return editor;
		}
	}
	#endregion

	#region Region Selector Class
	/// <summary>
	/// I am a region selection form.
	/// </summary>
	public class RegionSelector : AbstractEditor
	{
		public override void Initialize()
		{
			InitializeRegions();
		}

		protected override void GenerateUI(int ax, int ay, Control cParent)
		{			
			Initialize();
			ComboBox cb = MakeComboBox(8, 8, 180, cParent, ref Regions, new EventHandler(event_cbChange));
			cb.MaxDropDownItems = 20;
			cb.Text = FindKey(ref Regions,RegionSelection);
			this.AcceptButton = MakeButton(sOK, 8, 40, 75, 23, new EventHandler(event_OK), cParent);
			Button btn = MakeButton(sCancel, 2500, 2500, 75, 23, new EventHandler(event_Escape), _gb);
			btn.TabStop = false;
			this.CancelButton = btn;
			this.Height = 106;
			this.Width = 200;
		}

		protected void event_cbChange(object sender, EventArgs e)
		{
			RegionSelection = Selected_Item(ref Regions, ((ComboBox)sender).SelectedItem.ToString(),true);
		}

		override protected void event_OK(object sender, EventArgs e)
		{			
			this.Close();
		}

		override protected void event_load(object sender, System.EventArgs e)
		{
			this.Hide();
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			Point pos=new Point(10, 10);			
			GenerateUI(pos.X, pos.Y, DynamicForm);
			this.CenterToParent();
			this.Text = sRegion;
			this.Show();
		}	

		override protected void event_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			this.Close();
		}
	}
	#endregion

	#region Realm Selector Class
	/// <summary>
	/// I am a realm selection form.
	/// </summary>
	public class RealmSelector : AbstractEditor
	{
		public override void Initialize()
		{
			InitializeRealms();
		}

		protected override void GenerateUI(int ax, int ay, Control cParent)
		{
			Initialize();
			ComboBox cb = MakeComboBox(8, 8, 180, cParent, ref Realms, new EventHandler(event_cbChange));
			cb.MaxDropDownItems = 4;
			cb.Text = FindKey(ref Realms,RealmSelection);
			this.AcceptButton = MakeButton(sOK, 8, 40, 75, 23, new EventHandler(event_OK), cParent);
			Button btn = MakeButton(sCancel, 2500, 2500, 75, 23, new EventHandler(event_Escape), _gb);
			btn.TabStop = false;
			this.CancelButton = btn;
			this.Height = 106;
			this.Width = 200;
		}

		protected void event_cbChange(object sender, EventArgs e)
		{
			RealmSelection = Selected_Item(ref Realms, ((ComboBox)sender).SelectedItem.ToString(),true);
		}

		override protected void event_OK(object sender, EventArgs e)
		{		
			this.Close();
		}

		override protected void event_load(object sender, System.EventArgs e)
		{
			this.Hide();
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			Point pos=new Point(10, 10);
			GenerateUI(pos.X, pos.Y, DynamicForm);
			this.CenterToParent();
			this.Text = sRealm;
			this.Show();
		}	

		override protected void event_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			this.Close();
		}
	}
	#endregion

	#region Color Selector Class
	/// <summary>
	/// I am a Color selection form.
	/// </summary>
	public class ColorSelector : AbstractEditor
	{
		public override void Initialize()
		{
			InitializeColor();
		}

		protected override void GenerateUI(int ax, int ay, Control cParent)
		{
			Initialize();
			ComboBox cb = MakeComboBox(8, 8, 180, cParent, ref Color, new EventHandler(event_cbChange));
			cb.MaxDropDownItems = 20;
			cb.Text = FindKey(ref Color,ColorSelection);
			this.AcceptButton = MakeButton(sOK, 8, 40, 75, 23, new EventHandler(event_OK), cParent);
			Button btn = MakeButton(sCancel, 2500, 2500, 75, 23, new EventHandler(event_Escape), _gb);
			btn.TabStop = false;
			this.CancelButton = btn;
			this.Height = 106;
			this.Width = 200;
		}

		protected void event_cbChange(object sender, EventArgs e)
		{
			ColorSelection = Selected_Item(ref Color, ((ComboBox)sender).SelectedItem.ToString(),true);
		}

		override protected void event_OK(object sender, EventArgs e)
		{		
			this.Close();
		}

		override protected void event_load(object sender, System.EventArgs e)
		{
			this.Hide();
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			Point pos=new Point(10, 10);
			GenerateUI(pos.X, pos.Y, DynamicForm);
			this.CenterToParent();
			this.Text = sColor;
			this.Show();
		}	

		override protected void event_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			this.Close();
		}
	}
	#endregion

	#region Emblem Selector Class
	/// <summary>
	/// I am a Emblem selection form.
	/// </summary>
	public class EmblemSelector : AbstractEditor
	{
		public override void Initialize()
		{
			InitializeEmblem();
		}

		protected override void GenerateUI(int ax, int ay, Control cParent)
		{
			Initialize();
			ComboBox cb = MakeComboBox(8, 8, 180, cParent, ref Emblem, new EventHandler(event_cbChange));
			cb.MaxDropDownItems = 20;
			cb.Text = FindKey(ref Emblem,EmblemSelection);
			this.AcceptButton = MakeButton(sOK, 8, 40, 75, 23, new EventHandler(event_OK), cParent);
			Button btn = MakeButton(sCancel, 2500, 2500, 75, 23, new EventHandler(event_Escape), _gb);
			btn.TabStop = false;
			this.CancelButton = btn;
			this.Height = 106;
			this.Width = 200;
		}

		protected void event_cbChange(object sender, EventArgs e)
		{
			EmblemSelection = Selected_Item(ref Emblem, ((ComboBox)sender).SelectedItem.ToString(),true);
		}

		override protected void event_OK(object sender, EventArgs e)
		{		
			this.Close();
		}

		override protected void event_load(object sender, System.EventArgs e)
		{
			this.Hide();
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			Point pos=new Point(10, 10);
			GenerateUI(pos.X, pos.Y, DynamicForm);
			this.CenterToParent();
			this.Text = sEmblem;
			this.Show();
		}	

		override protected void event_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			this.Close();
		}
	}
	#endregion

	#region Item Model Selector Class
	/// <summary>
	/// I am a ItemModel selection form.
	/// </summary>
	public class ItemModelSelector : AbstractEditor
	{
		public override void Initialize()
		{
			InitializeItemModel();
		}

		protected override void GenerateUI(int ax, int ay, Control cParent)
		{
			Initialize();
			ComboBox cb = MakeComboBox(8, 8, 180, cParent, ref ItemModel, new EventHandler(event_cbChange));
			cb.MaxDropDownItems = 20;
			cb.Text = FindKey(ref ItemModel,ItemModelSelection);
			this.AcceptButton = MakeButton(sOK, 8, 40, 75, 23, new EventHandler(event_OK), cParent);
			Button btn = MakeButton(sCancel, 2500, 2500, 75, 23, new EventHandler(event_Escape), _gb);
			btn.TabStop = false;
			this.CancelButton = btn;
			this.Height = 106;
			this.Width = 200;
		}

		protected void event_cbChange(object sender, EventArgs e)
		{
			ItemModelSelection = Selected_Item(ref ItemModel, ((ComboBox)sender).SelectedItem.ToString(),true);
		}

		override protected void event_OK(object sender, EventArgs e)
		{		
			this.Close();
		}

		override protected void event_load(object sender, System.EventArgs e)
		{
			this.Hide();
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			Point pos=new Point(10, 10);
			GenerateUI(pos.X, pos.Y, DynamicForm);
			this.CenterToParent();
			this.Text = sModel;
			this.Show();
		}	

		override protected void event_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			this.Close();
		}
	}
	#endregion

	#region Monster Model Selector Class
	/// <summary>
	/// I am a MonsterModel selection form.
	/// </summary>
	public class MonsterModelSelector : AbstractEditor
	{
		public override void Initialize()
		{
			InitializeMonsterModel();
		}

		protected override void GenerateUI(int ax, int ay, Control cParent)
		{
			Initialize();
			ComboBox cb = MakeComboBox(8, 8, 180, cParent, ref MonsterModel, new EventHandler(event_cbChange));
			cb.MaxDropDownItems = 20;
			cb.Text = FindKey(ref MonsterModel,MonsterModelSelection);
			this.AcceptButton = MakeButton(sOK, 8, 40, 75, 23, new EventHandler(event_OK), cParent);
			Button btn = MakeButton(sCancel, 2500, 2500, 75, 23, new EventHandler(event_Escape), _gb);
			btn.TabStop = false;
			this.CancelButton = btn;
			this.Height = 106;
			this.Width = 200;
		}

		protected void event_cbChange(object sender, EventArgs e)
		{
			MonsterModelSelection = Selected_Item(ref MonsterModel, ((ComboBox)sender).SelectedItem.ToString(),true);
		}

		override protected void event_OK(object sender, EventArgs e)
		{		
			this.Close();
		}

		override protected void event_load(object sender, System.EventArgs e)
		{
			this.Hide();
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			Point pos=new Point(10, 10);
			GenerateUI(pos.X, pos.Y, DynamicForm);
			this.CenterToParent();
			this.Text = sModel;
			this.Show();
		}	

		override protected void event_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			this.Close();
		}
	}
	#endregion

	#region Weapon Effect Selector Class
	/// <summary>
	/// I am a WeaponEffect selection form.
	/// </summary>
	public class WeaponEffectSelector : AbstractEditor
	{
		public override void Initialize()
		{
			InitializeWeaponEffect();
		}

		protected override void GenerateUI(int ax, int ay, Control cParent)
		{
			Initialize();
			ComboBox cb = MakeComboBox(8, 8, 180, cParent, ref WeaponEffect, new EventHandler(event_cbChange));
			cb.MaxDropDownItems = 20;
			cb.Text = FindKey(ref WeaponEffect,WeaponEffectSelection);
			this.AcceptButton = MakeButton(sOK, 8, 40, 75, 23, new EventHandler(event_OK), cParent);
			Button btn = MakeButton(sCancel, 2500, 2500, 75, 23, new EventHandler(event_Escape), _gb);
			btn.TabStop = false;
			this.CancelButton = btn;
			this.Height = 106;
			this.Width = 200;
		}

		protected void event_cbChange(object sender, EventArgs e)
		{
			WeaponEffectSelection = Selected_Item(ref WeaponEffect, ((ComboBox)sender).SelectedItem.ToString(),true);
		}

		override protected void event_OK(object sender, EventArgs e)
		{		
			this.Close();
		}

		override protected void event_load(object sender, System.EventArgs e)
		{
			this.Hide();
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			Point pos=new Point(10, 10);
			GenerateUI(pos.X, pos.Y, DynamicForm);
			this.CenterToParent();
			this.Text = sEffect;
			this.Show();
		}	

		override protected void event_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			this.Close();
		}
	}
	#endregion

	#region Slot Selector Class
	/// <summary>
	/// I am a slot selection form.
	/// </summary>
	public class SlotSelector : AbstractEditor
	{
		public override void Initialize()
		{
			InitializeSlot();
		}

		protected override void GenerateUI(int ax, int ay, Control cParent)
		{
			Initialize();
			ComboBox cb = MakeComboBox(8, 8, 180, cParent, ref Slot, new EventHandler(event_cbChange));
			cb.MaxDropDownItems = 20;
			cb.Text = FindKey(ref Slot,SlotSelection);
			this.AcceptButton = MakeButton(sOK, 8, 40, 75, 23, new EventHandler(event_OK), cParent);
			Button btn = MakeButton(sCancel, 2500, 2500, 75, 23, new EventHandler(event_Escape), _gb);
			btn.TabStop = false;
			this.CancelButton = btn;
			this.Height = 106;
			this.Width = 200;
		}

		protected void event_cbChange(object sender, EventArgs e)
		{
			SlotSelection = Selected_Item(ref Slot, ((ComboBox)sender).SelectedItem.ToString(),true);
		}

		override protected void event_OK(object sender, EventArgs e)
		{			
			this.Close();
		}

		override protected void event_load(object sender, System.EventArgs e)
		{
			this.Hide();
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			Point pos=new Point(10, 10);
			GenerateUI(pos.X, pos.Y, DynamicForm);
			this.CenterToParent();
			this.Text = sSlot;
			this.Show();
		}	

		override protected void event_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			this.Close();
		}
	}
	#endregion

	#region Object Type Selector Class
	/// <summary>
	/// I am a object type selection form.
	/// </summary>
	public class ObjectTypeSelector : AbstractEditor
	{
		public override void Initialize()
		{
			InitializeObjectType();
		}

		protected override void GenerateUI(int ax, int ay, Control cParent)
		{
			Initialize();
			ComboBox cb = MakeComboBox(8, 8, 180, cParent, ref ObjectType, new EventHandler(event_cbChange));
			cb.MaxDropDownItems = 20;
			cb.Text = FindKey(ref ObjectType,ObjectTypeSelection);
			this.AcceptButton = MakeButton(sOK, 8, 40, 75, 23, new EventHandler(event_OK), cParent);
			Button btn = MakeButton(sCancel, 2500, 2500, 75, 23, new EventHandler(event_Escape), _gb);
			btn.TabStop = false;
			this.CancelButton = btn;
			this.Height = 106;
			this.Width = 200;
		}

		protected void event_cbChange(object sender, EventArgs e)
		{
			ObjectTypeSelection = Selected_Item(ref ObjectType, ((ComboBox)sender).SelectedItem.ToString(),true);
		}

		override protected void event_OK(object sender, EventArgs e)
		{			
			this.Close();
		}

		override protected void event_load(object sender, System.EventArgs e)
		{
			this.Hide();
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			Point pos=new Point(10, 10);
			GenerateUI(pos.X, pos.Y, DynamicForm);
			this.CenterToParent();
			this.Text = sObjectType;
			this.Show();
		}	

		override protected void event_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			this.Close();
		}
	}
	#endregion

	#region Item Type Selector Class
	/// <summary>
	/// I am a Item type selection form.
	/// </summary>
	public class ItemTypeSelector : AbstractEditor
	{
		public override void Initialize()
		{
			InitializeItemType();
		}

		protected override void GenerateUI(int ax, int ay, Control cParent)
		{
			Initialize();
			ComboBox cb = MakeComboBox(8, 8, 180, cParent, ref ItemType, new EventHandler(event_cbChange));
			cb.MaxDropDownItems = 20;
			cb.Text = FindKey(ref ItemType,ItemTypeSelection);
			this.AcceptButton = MakeButton(sOK, 8, 40, 75, 23, new EventHandler(event_OK), cParent);
			Button btn = MakeButton(sCancel, 2500, 2500, 75, 23, new EventHandler(event_Escape), _gb);
			btn.TabStop = false;
			this.CancelButton = btn;
			this.Height = 106;
			this.Width = 200;
		}

		protected void event_cbChange(object sender, EventArgs e)
		{
			ItemTypeSelection = Selected_Item(ref ItemType, ((ComboBox)sender).SelectedItem.ToString(),true);
		}

		override protected void event_OK(object sender, EventArgs e)
		{			
			this.Close();
		}

		override protected void event_load(object sender, System.EventArgs e)
		{
			this.Hide();
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			Point pos=new Point(10, 10);
			GenerateUI(pos.X, pos.Y, DynamicForm);
			this.CenterToParent();
			this.Text = sItemType;
			this.Show();
		}	

		override protected void event_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			this.Close();
		}
	}
	#endregion

	#region Race Selector Class
	/// <summary>
	/// I am a race type selection form.
	/// </summary>
	public class RaceSelector : AbstractEditor
	{
		public override void Initialize()
		{
			InitializeRace();
		}

		protected override void GenerateUI(int ax, int ay, Control cParent)
		{
			Initialize();
			ComboBox cb = MakeComboBox(8, 8, 180, cParent, ref Race, new EventHandler(event_cbChange));
			cb.MaxDropDownItems = 20;
			cb.Text = FindKey(ref Race,RaceSelection);
			this.AcceptButton = MakeButton(sOK, 8, 40, 75, 23, new EventHandler(event_OK), cParent);
			Button btn = MakeButton(sCancel, 2500, 2500, 75, 23, new EventHandler(event_Escape), _gb);
			btn.TabStop = false;
			this.CancelButton = btn;
			this.Height = 106;
			this.Width = 200;
		}

		protected void event_cbChange(object sender, EventArgs e)
		{
			RaceSelection = Selected_Item(ref Race, ((ComboBox)sender).SelectedItem.ToString(),true);
		}

		override protected void event_OK(object sender, EventArgs e)
		{			
			this.Close();
		}

		override protected void event_load(object sender, System.EventArgs e)
		{
			this.Hide();
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			Point pos=new Point(10, 10);
			GenerateUI(pos.X, pos.Y, DynamicForm);
			this.CenterToParent();
			this.Text = sRace;
			this.Show();
		}	

		override protected void event_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			this.Close();
		}
	}
	#endregion

	#region Class Selector Class
	/// <summary>
	/// I am a class selection form.
	/// </summary>
	public class ClassTypeSelector : AbstractEditor
	{
		public override void Initialize()
		{
			InitializeClassType();
		}

		protected override void GenerateUI(int ax, int ay, Control cParent)
		{
			Initialize();
			ComboBox cb = MakeComboBox(8, 8, 180, cParent, ref ClassType, new EventHandler(event_cbChange));
			cb.MaxDropDownItems = 20;
			cb.Text = FindKey(ref ClassType,ClassTypeSelection);
			this.AcceptButton = MakeButton(sOK, 8, 40, 75, 23, new EventHandler(event_OK), cParent);
			Button btn = MakeButton(sCancel, 2500, 2500, 75, 23, new EventHandler(event_Escape), _gb);
			btn.TabStop = false;
			this.CancelButton = btn;
			this.Height = 106;
			this.Width = 200;
		}

		protected void event_cbChange(object sender, EventArgs e)
		{
			ClassTypeSelection = Selected_Item(ref ClassType, ((ComboBox)sender).SelectedItem.ToString(),true);
		}

		override protected void event_OK(object sender, EventArgs e)
		{			
			this.Close();
		}

		override protected void event_load(object sender, System.EventArgs e)
		{
			this.Hide();
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			Point pos=new Point(10, 10);
			GenerateUI(pos.X, pos.Y, DynamicForm);
			this.CenterToParent();
			this.Text = sClass;
			this.Show();
		}	

		override protected void event_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			this.Close();
		}
	}
	#endregion

	#region Damage Selector Class
	/// <summary>
	/// I am a weapon damage selection form.
	/// </summary>
	public class DamageSelector : AbstractEditor
	{

		public override void Initialize()
		{
			InitializeDamage();
		}

		protected override void GenerateUI(int ax, int ay, Control cParent)
		{
			Initialize();
			ComboBox cb = MakeComboBox(8, 8, 180, cParent, ref Damage, new EventHandler(event_cbChange));
			cb.MaxDropDownItems = 10;
			cb.Text = FindKey(ref Damage,DamageSelection);
			this.AcceptButton = MakeButton(sOK, 8, 40, 75, 23, new EventHandler(event_OK), cParent);
			Button btn = MakeButton(sCancel, 2500, 2500, 75, 23, new EventHandler(event_Escape), _gb);
			btn.TabStop = false;
			this.CancelButton = btn;
			this.Height = 106;
			this.Width = 200;
		}

		protected void event_cbChange(object sender, EventArgs e)
		{
			DamageSelection = Selected_Item(ref Damage, ((ComboBox)sender).SelectedItem.ToString(),true);
		}

		override protected void event_OK(object sender, EventArgs e)
		{			
			this.Close();
		}

		override protected void event_load(object sender, System.EventArgs e)
		{
			this.Hide();
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			Point pos=new Point(10, 10);
			GenerateUI(pos.X, pos.Y, DynamicForm);
			this.CenterToParent();
			this.Text = sDamageType;
			this.Show();
		}	

		override protected void event_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			this.Close();
		}
	}
	#endregion

	#region Privilege Level Selector Class
	/// <summary>
	/// I am a privlege level selection form.
	/// </summary>
	public class PrivilegeSelector : AbstractEditor
	{
		public override void Initialize()
		{
			InitializePrivileges();
		}

		protected override void GenerateUI(int ax, int ay, Control cParent)
		{
			Initialize();
			ComboBox cb = MakeComboBox(8, 8, 180, cParent, ref Privileges, new EventHandler(event_cbChange));
			cb.MaxDropDownItems = 4;
			cb.Text = FindKey(ref Privileges,PrivilegeSelection);
			this.AcceptButton = MakeButton(sOK, 8, 40, 75, 23, new EventHandler(event_OK), cParent);
			Button btn = MakeButton(sCancel, 2500, 2500, 75, 23, new EventHandler(event_Escape), _gb);
			btn.TabStop = false;
			this.CancelButton = btn;
			this.Height = 106;
			this.Width = 200;
		}

		protected void event_cbChange(object sender, EventArgs e)
		{
			PrivilegeSelection = Selected_Item(ref Privileges, ((ComboBox)sender).SelectedItem.ToString(),true);
		}

		override protected void event_OK(object sender, EventArgs e)
		{			
			this.Close();
		}

		override protected void event_load(object sender, System.EventArgs e)
		{
			this.Hide();
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			Point pos=new Point(10, 10);
			GenerateUI(pos.X, pos.Y, DynamicForm);
			this.CenterToParent();
			this.Text = sPrivLevel;
			this.Show();
		}
	
		override protected void event_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			this.Close();
		}
	}
	#endregion

	#region Hand Selector Class
	/// <summary>
	/// I am a hand selection form.
	/// </summary>
	public class HandSelector : AbstractEditor
	{

		public override void Initialize()
		{
			InitializeHand();
		}

		protected override void GenerateUI(int ax, int ay, Control cParent)
		{
			Initialize();
			ComboBox cb = MakeComboBox(8, 8, 180, cParent, ref Hand, new EventHandler(event_cbChange));
			cb.MaxDropDownItems = 4;
			cb.Text = FindKey(ref Hand,HandSelection);
			this.AcceptButton = MakeButton(sOK, 8, 40, 75, 23, new EventHandler(event_OK), cParent);
			Button btn = MakeButton(sCancel, 2500, 2500, 75, 23, new EventHandler(event_Escape), _gb);
			btn.TabStop = false;
			this.CancelButton = btn;
			this.Height = 106;
			this.Width = 200;
		}

		protected void event_cbChange(object sender, EventArgs e)
		{
			HandSelection = Selected_Item(ref Hand, ((ComboBox)sender).SelectedItem.ToString(),true);
		}

		override protected void event_OK(object sender, EventArgs e)
		{			
			this.Close();
		}

		override protected void event_load(object sender, System.EventArgs e)
		{
			this.Hide();
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			Point pos=new Point(10, 10);
			GenerateUI(pos.X, pos.Y, DynamicForm);
			this.CenterToParent();
			this.Text = sHand;
			this.Show();
		}	

		override protected void event_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			this.Close();
		}
	}
	#endregion

	#region Equipment Selector Class
	/// <summary>
	/// I am a equipment selection form.
	/// </summary>
	public class EquipmentSelector : AbstractEditor
	{
		private ComboBox cb;
		private GroupBox gbTop;
		private GroupBox gbBottom;
		private const string tablename = "NPCEquipment";
		private const int keycolumn = 1;
		private const int namecolumn = 1;
		private const int height = 80;
		private const int width = 496;
		private const int expandedheight = 428;
		private const int expandedwidth = 640;
		private bool noscroll = false;
		private Button btnExpand;
		private ContextMenu contextMenu1;
		private MenuItem menuItemNew;
		private MenuItem menuItemEdit;

		public override void Initialize()
		{
			if (Equipment == null) Equipment = InitializeList(tablename,keycolumn,namecolumn,true,false);
		}

		protected override void GenerateUI(int ax, int ay, Control cParent)
		{
			Initialize();
			gbTop = SetupDockGroupBox(2,2,44,DockStyle.Top,true,cParent);
			gbBottom = SetupDockGroupBox(2,45,350,DockStyle.Bottom,false,cParent);
			cb = MakeComboBox(8, 12, 230, gbTop, ref Equipment, new EventHandler(event_cbChange));
			cb.MaxDropDownItems = 20;
			this.AcceptButton = MakeButton(sOK, 248, 12, 58, 23, new EventHandler(event_OK), gbTop);
			Button btn = MakeButton(sCancel, 2500, 2500, 75, 23, new EventHandler(event_Escape), _gb);
			btn.TabStop = false;
			this.CancelButton = btn;
			MakeButton(sNew, 312, 12, 58, 23, new EventHandler(event_New_Item), gbTop);
			MakeButton(sEdit, 376, 12, 58, 23, new EventHandler(event_Edit_Item), gbTop);
			if (dgLookupEnabled) 
			{
				dgEquipment.Dock = DockStyle.Fill;
				dgEquipment.Parent = gbBottom;
				dgEquipment.CurrentCellChanged += new System.EventHandler(event_CurCellChange);	
				dgEquipment.DoubleClick += new System.EventHandler(event_OK);
				gbBottom.Visible = false;
			    contextMenu1 = new ContextMenu();
				menuItemNew = new MenuItem();
				menuItemEdit = new MenuItem();
				menuItemNew.Index = 0;
				menuItemNew.Text = sNew;
				menuItemNew.Click += new System.EventHandler(event_New_Item);
				menuItemEdit.Index = 1;
				menuItemEdit.Text = sEdit;
				menuItemEdit.Click += new System.EventHandler(event_Edit_Item);
				contextMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {menuItemNew,menuItemEdit});
				dgEquipment.ContextMenu = contextMenu1;
				btnExpand = MakeButton("+", 442, 12, 23, 23, new EventHandler(event_dg), gbTop);
				this.Height = height;
				this.Width = width;
			}
			else
			{
				this.Height = height;
				this.Width = width-33;
				gbBottom.Visible = false;
			}
			// this is a special case where the key field is not what we want... see below.
			if (Equipment.ContainsValue(EquipmentSelection)) cb.Text = EquipmentSelection;
			ScrollToRow(EquipmentSelection);
		}

		protected void ScrollToRow(string identifier)
		{
			if ((dgLookupEnabled) & (!noscroll))
			{			
				for (int row = 0; row < cEquipment.doc.DataSet.Tables[0].Rows.Count; row++)
				{
					if (dgEquipment[row, namecolumn].ToString() == identifier)
					{
						dgEquipment.ScrollToRow(row);
						dgEquipment.ClickRowHeader(row);
						noscroll = false;
						break;
					}
				}
			}
			else
			{
				noscroll = false;
			}			
		}

		protected void event_cbChange(object sender, EventArgs e)
		{
			// this is a special case where the key field is not what we want 
			// to populate in the table... we actually want the name of the template.
			// there can be many items in a template each item attached to a mob/merchant/npc.
			EquipmentSelection = ((ComboBox)sender).SelectedItem.ToString();
		    ScrollToRow(EquipmentSelection);
		}

		override protected void event_OK(object sender, EventArgs e)
		{			
			this.Close();
		}

		protected void event_New_Item(object sender, EventArgs e)
		{			
			Edit_Item(tablename, "", cb, out _EquipmentSelection,keycolumn,namecolumn,false,ref Equipment);
			this.Close(); // close selector and return last entered value as the result
		}

		protected void event_Edit_Item(object sender, EventArgs e)
		{
			// we don't know what they are going to do, just assume current selection is what they want back.
			string HoldCurrSelection = dgEquipment[dgEquipment.CurrentCell.RowNumber,keycolumn].ToString(); 
			string RowID = dgEquipment[dgEquipment.CurrentCell.RowNumber,0].ToString();  // we want 0, as it's "really" the key column...
			Edit_Item(tablename, RowID, cb, out _EquipmentSelection,keycolumn,namecolumn,false,ref Equipment);
			EquipmentSelection = HoldCurrSelection; 
			this.Close();
		}

		override protected void event_load(object sender, System.EventArgs e)
		{
			this.Hide();
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			Point pos=new Point(10, 10);
			GenerateUI(pos.X, pos.Y, DynamicForm);	
			this.CenterToParent();
			this.Text = sEquipment;
			this.Show();
			if (dgEquipmentState) 
			{
				event_dg(btnExpand,null);
			}
		}	

		protected void event_dg(object sender, EventArgs e)
		{
			if (((Button)sender).Text == "+")
			{
				((Button)sender).Text = "-";
				dgEquipmentState = true;
				gbBottom.Visible = true;
				if (dgEquipmentWidth > expandedwidth)
				{
					this.Width = expandedwidth;
				}
				else if (dgEquipmentWidth < width)
				{
					this.Width = width;
				}
				else
				{
					this.Width = dgEquipmentWidth;
				}				
				this.Height = expandedheight;
			}
			else
			{
				((Button)sender).Text = "+";
				dgEquipmentState = false;
				gbBottom.Visible = false;
				this.Width = width;
				this.Height = height;
			}
		}

		protected void event_CurCellChange(object sender, EventArgs e)
		{
			try
			{
				noscroll = true;
				cb.Text = dgEquipment[dgEquipment.CurrentCell.RowNumber, namecolumn].ToString();
				if (cEquipment.doc.DataSet.Tables[0].Rows.Count > 0) dgEquipment.Select(dgEquipment.CurrentCell.RowNumber);
			}
			catch
			{
				// suppress it... this only happens if they try to drag and drop stuff
				// on the grid... which they shouldn't be doing...
			}
		}

		override protected void event_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			this.Close();
		}
	}
	#endregion

	#region Merchant Selector Class
	/// <summary>
	/// I am a merchant selection form.
	/// </summary>
	public class MerchantSelector : AbstractEditor
	{
		private ComboBox cb;
		private GroupBox gbTop;
		private GroupBox gbBottom;
		private const string tablename = "Merchant";
		private const int keycolumn = 0;
		private const int namecolumn = 2;
		private const int height = 80;
		private const int width = 496;
		private const int expandedheight = 428;
		private const int expandedwidth = 640;
		private bool noscroll = false;
		private Button btnExpand;
		private ContextMenu contextMenu1;
		private MenuItem menuItemNew;
		private MenuItem menuItemEdit;

		public override void Initialize()
		{
			if (Merchant == null) Merchant = InitializeList(tablename,keycolumn,namecolumn,false,false);
		}

		protected override void GenerateUI(int ax, int ay, Control cParent)
		{
			Initialize();
			gbTop = SetupDockGroupBox(2,2,44,DockStyle.Top,true,cParent);
			gbBottom = SetupDockGroupBox(2,45,350,DockStyle.Bottom,false,cParent);
			cb = MakeComboBox(8, 12, 230, gbTop, ref Merchant, new EventHandler(event_cbChange));
			cb.MaxDropDownItems = 20;
			this.AcceptButton = MakeButton(sOK, 248, 12, 58, 23, new EventHandler(event_OK), gbTop);
			Button btn = MakeButton(sCancel, 2500, 2500, 75, 23, new EventHandler(event_Escape), _gb);
			btn.TabStop = false;
			this.CancelButton = btn;
			MakeButton(sNew, 312, 12, 58, 23, new EventHandler(event_New_Item), gbTop);
			MakeButton(sEdit, 376, 12, 58, 23, new EventHandler(event_Edit_Item), gbTop);
			if (dgLookupEnabled) 
			{
				dgMerchant.Dock = DockStyle.Fill;
				dgMerchant.Parent = gbBottom;
				dgMerchant.CurrentCellChanged += new System.EventHandler(event_CurCellChange);	
				dgMerchant.DoubleClick += new System.EventHandler(event_OK);
				gbBottom.Visible = false;
				contextMenu1 = new ContextMenu();
				menuItemNew = new MenuItem();
				menuItemEdit = new MenuItem();
				menuItemNew.Index = 0;
				menuItemNew.Text = sNew;
				menuItemNew.Click += new System.EventHandler(event_New_Item);
				menuItemEdit.Index = 1;
				menuItemEdit.Text = sEdit;
				menuItemEdit.Click += new System.EventHandler(event_Edit_Item);
				contextMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {menuItemNew,menuItemEdit});
				dgMerchant.ContextMenu = contextMenu1;
				btnExpand = MakeButton("+", 442, 12, 23, 23, new EventHandler(event_dg), gbTop);
				this.Height = height;
				this.Width = width;
			}
			else
			{
				this.Height = height;
				this.Width = width-33;
				gbBottom.Visible = false;
			}
			cb.Text = FindKey(ref Merchant,MerchantSelection);
			ScrollToRow(MerchantSelection);
		}

		protected void ScrollToRow(string identifier)
		{
			if ((dgLookupEnabled) & (!noscroll))
			{
				for (int row = 0; row < cMerchant.doc.DataSet.Tables[0].Rows.Count; row++)
				{
					if (dgMerchant[row, keycolumn].ToString() == identifier)
					{
						dgMerchant.ScrollToRow(row);
						dgMerchant.ClickRowHeader(row);
						noscroll = false;
						break;
					}
				}
			}
			else
			{
				noscroll = false;
			}
		}

		protected void event_cbChange(object sender, EventArgs e)
		{
			MerchantSelection = Selected_Item(ref Merchant, ((ComboBox)sender).SelectedItem.ToString());
			ScrollToRow(MerchantSelection);
		}

		override protected void event_OK(object sender, EventArgs e)
		{			
			this.Close();
		}

		protected void event_New_Item(object sender, EventArgs e)
		{			
			Edit_Item(tablename, "", cb, out _MerchantSelection,keycolumn,namecolumn,false,ref Merchant);
			this.Close(); // close selector and return last entered value as the result
		}

		protected void event_Edit_Item(object sender, EventArgs e)
		{
			string RowID = dgMerchant[dgMerchant.CurrentCell.RowNumber,0].ToString();  // we want 0, as it's "really" the key column...
			Edit_Item(tablename, RowID, cb, out _MerchantSelection,keycolumn,namecolumn,false,ref Merchant);
			this.Close();
		}

		override protected void event_load(object sender, System.EventArgs e)
		{
			this.Hide();
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			Point pos=new Point(10, 10);
			GenerateUI(pos.X, pos.Y, DynamicForm);	
			this.CenterToParent();
			this.Text = sMerchant;
			this.Show();
			if (dgMerchantState) 
			{
				event_dg(btnExpand,null);
			}
		}
	
		protected void event_dg(object sender, EventArgs e)
		{
			if (((Button)sender).Text == "+")
			{
				((Button)sender).Text = "-";
				dgMerchantState = true;
				gbBottom.Visible = true;
				if (dgMerchantWidth > expandedwidth)
				{
					this.Width = expandedwidth;
				}
				else if (dgMerchantWidth < width)
				{
					this.Width = width;
				}
				else
				{
					this.Width = dgMerchantWidth;
				}
				this.Height = expandedheight;
			}
			else
			{
				((Button)sender).Text = "+";
				dgMerchantState = false;
				gbBottom.Visible = false;
				this.Width = width;
				this.Height = height;
			}
		}

		protected void event_CurCellChange(object sender, EventArgs e)
		{
			try
			{
				noscroll = true;
				cb.Text = dgMerchant[dgMerchant.CurrentCell.RowNumber, namecolumn].ToString();
				if (cMerchant.doc.DataSet.Tables[0].Rows.Count > 0) dgMerchant.Select(dgMerchant.CurrentCell.RowNumber);
			}
			catch
			{
				// suppress it... this only happens if they try to drag and drop stuff
				// on the grid... which they shouldn't be doing...
			}

		}

		override protected void event_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			this.Close();
		}
	}
	#endregion

	#region Item Selector Class
	/// <summary>
	/// I am a equipment selection form.
	/// </summary>
	public class ItemSelector : AbstractEditor
	{
		private ComboBox cb;
		private GroupBox gbTop;
		private GroupBox gbBottom;
		private const string tablename = "ItemTemplate";
		private const int keycolumn = 1;
		private const int namecolumn = 2;
		private const int height = 80;
		private const int width = 496;
		private const int expandedheight = 428;
		private const int expandedwidth = 640;
		private bool noscroll = false;
		private Button btnExpand;
		private ContextMenu contextMenu1;
		private MenuItem menuItemNew;
		private MenuItem menuItemEdit;

		public override void Initialize()
		{
			if (Items == null) Items = InitializeList(tablename,keycolumn,namecolumn,false,false);
		}

		protected override void GenerateUI(int ax, int ay, Control cParent)
		{
			Initialize();
			gbTop = SetupDockGroupBox(2,2,44,DockStyle.Top,true,cParent);
			gbBottom = SetupDockGroupBox(2,45,350,DockStyle.Bottom,false,cParent);
			cb = MakeComboBox(8, 12, 230, gbTop, ref Items, new EventHandler(event_cbChange));
			cb.MaxDropDownItems = 20;
			this.AcceptButton = MakeButton(sOK, 248, 12, 58, 23, new EventHandler(event_OK), gbTop);
			Button btn = MakeButton(sCancel, 2500, 2500, 75, 23, new EventHandler(event_Escape), _gb);
			btn.TabStop = false;
			this.CancelButton = btn;
			MakeButton(sNew, 312, 12, 58, 23, new EventHandler(event_New_Item), gbTop);
			MakeButton(sEdit, 376, 12, 58, 23, new EventHandler(event_Edit_Item), gbTop);
			if (dgLookupEnabled) 
			{
				dgItems.Dock = DockStyle.Fill;
				dgItems.Parent = gbBottom;
				dgItems.CurrentCellChanged += new System.EventHandler(event_CurCellChange);	
				dgItems.DoubleClick += new System.EventHandler(event_OK);
				gbBottom.Visible = false;
				contextMenu1 = new ContextMenu();
				menuItemNew = new MenuItem();
				menuItemEdit = new MenuItem();
				menuItemNew.Index = 0;
				menuItemNew.Text = sNew;
				menuItemNew.Click += new System.EventHandler(event_New_Item);
				menuItemEdit.Index = 1;
				menuItemEdit.Text = sEdit;
				menuItemEdit.Click += new System.EventHandler(event_Edit_Item);
				contextMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {menuItemNew,menuItemEdit});
				dgItems.ContextMenu = contextMenu1;
				btnExpand = MakeButton("+", 442, 12, 23, 23, new EventHandler(event_dg), gbTop);
				this.Height = height;
				this.Width = width;
		}
		else
		{
			this.Height = height;
			this.Width = width-33;
			gbBottom.Visible = false;
		}
		cb.Text = FindKey(ref Items,ItemSelection);
		ScrollToRow(ItemSelection);
	}

		protected void ScrollToRow(string identifier)
		{
			if ((dgLookupEnabled) & (!noscroll)) 
			{
				for (int row = 0; row < cItems.doc.DataSet.Tables[0].Rows.Count; row++)
				{
					if (dgItems[row, keycolumn].ToString() == identifier)
					{
						dgItems.ScrollToRow(row);
						dgItems.ClickRowHeader(row);
						noscroll = false;
						break;
					}
				}
			}
			else
			{
				noscroll = false;
			}
		}

		protected void event_cbChange(object sender, EventArgs e)
		{
			ItemSelection = Selected_Item(ref Items, ((ComboBox)sender).SelectedItem.ToString());
			ScrollToRow(ItemSelection);
		}

		override protected void event_OK(object sender, EventArgs e)
		{			
			this.Close();
		}

		protected void event_dg(object sender, EventArgs e)
		{
			if (((Button)sender).Text == "+")
			{
				((Button)sender).Text = "-";
				dgItemsState = true;
				gbBottom.Visible = true;
				if (dgItemsWidth > expandedwidth)
				{
					this.Width = expandedwidth;
				}
				else if (dgItemsWidth < width)
				{
					this.Width = width;
				}
				else
				{
					this.Width = dgItemsWidth;
				}
				this.Height = expandedheight;
			}
			else
			{
				((Button)sender).Text = "+";
				dgItemsState = false;
				gbBottom.Visible = false;
				this.Width = width;
				this.Height = height;
			}
		}

		protected void event_CurCellChange(object sender, EventArgs e)
		{
			try
			{
				noscroll = true;
				cb.Text = dgItems[dgItems.CurrentCell.RowNumber, namecolumn].ToString();
				if (cItems.doc.DataSet.Tables[0].Rows.Count > 0) dgItems.Select(dgItems.CurrentCell.RowNumber);
			}
			catch
			{
				// suppress it... this only happens if they try to drag and drop stuff
				// on the grid... which they shouldn't be doing...
			}
		}

		protected void event_New_Item(object sender, EventArgs e)
		{			
			Edit_Item(tablename, "", cb, out _ItemSelection,keycolumn,namecolumn,false,ref Items);
			this.Close(); // close selector and return last entered value as the result
		}

		protected void event_Edit_Item(object sender, EventArgs e)
		{
			string RowID = dgItems[dgItems.CurrentCell.RowNumber,0].ToString();  // we want 0, as it's "really" the key column...
			Edit_Item(tablename, RowID, cb, out _ItemSelection,keycolumn,namecolumn,false,ref Items);
			this.Close();
		}

		override protected void event_load(object sender, System.EventArgs e)
		{
			this.Hide();
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			Point pos=new Point(10, 10);
			GenerateUI(pos.X, pos.Y, DynamicForm);	
			this.CenterToParent();
			this.Text = sItemTemplate;
			this.Show();
			if (dgItemsState) 
			{
				event_dg(btnExpand,null);
			}
		}	

		override protected void event_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			this.Close();
		}
	}
	#endregion

	#region Spell Line Selector Class
	/// <summary>
	/// I am a Spell Line selection form.
	/// </summary>
	public class SpellLineSelector : AbstractEditor
	{
		private ComboBox cb;
		private GroupBox gbTop;
		private GroupBox gbBottom;
		private const string tablename = "SpellLine";
		private const int keycolumn = 0;
		private const int namecolumn = 1;
		private const int height = 80;
		private const int width = 496;
		private const int expandedheight = 428;
		private const int expandedwidth = 640;
		private bool noscroll = false;
		private Button btnExpand;
		private ContextMenu contextMenu1;
		private MenuItem menuItemNew;
		private MenuItem menuItemEdit;

		public override void Initialize()
		{
			if (SpellLine == null) SpellLine = InitializeList(tablename,keycolumn,namecolumn,true,false);
		}

		protected override void GenerateUI(int ax, int ay, Control cParent)
		{
			Initialize();
			gbTop = SetupDockGroupBox(2,2,44,DockStyle.Top,true,cParent);
			gbBottom = SetupDockGroupBox(2,45,350,DockStyle.Bottom,false,cParent);
			cb = MakeComboBox(8, 12, 230, gbTop, ref SpellLine, new EventHandler(event_cbChange));
			cb.MaxDropDownItems = 20;
			this.AcceptButton = MakeButton(sOK, 248, 12, 58, 23, new EventHandler(event_OK), gbTop);
			Button btn = MakeButton(sCancel, 2500, 2500, 75, 23, new EventHandler(event_Escape), _gb);
			btn.TabStop = false;
			this.CancelButton = btn;
			MakeButton(sNew, 312, 12, 58, 23, new EventHandler(event_New_Item), gbTop);
			MakeButton(sEdit, 376, 12, 58, 23, new EventHandler(event_Edit_Item), gbTop);
			if (dgLookupEnabled) 
			{
				dgSpellLine.Dock = DockStyle.Fill;
				dgSpellLine.Parent = gbBottom;
				dgSpellLine.CurrentCellChanged += new System.EventHandler(event_CurCellChange);	
				dgSpellLine.DoubleClick += new System.EventHandler(event_OK);
				gbBottom.Visible = false;
				contextMenu1 = new ContextMenu();
				menuItemNew = new MenuItem();
				menuItemEdit = new MenuItem();
				menuItemNew.Index = 0;
				menuItemNew.Text = sNew;
				menuItemNew.Click += new System.EventHandler(event_New_Item);
				menuItemEdit.Index = 1;
				menuItemEdit.Text = sEdit;
				menuItemEdit.Click += new System.EventHandler(event_Edit_Item);
				contextMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {menuItemNew,menuItemEdit});
				dgSpellLine.ContextMenu = contextMenu1;
				btnExpand = MakeButton("+", 442, 12, 23, 23, new EventHandler(event_dg), gbTop);
				this.Height = height;
				this.Width = width;
			}
			else
			{
				this.Height = height;
				this.Width = width-33;
				gbBottom.Visible = false;
			}
			// this is a special case where the key field is not what we want... see below.
			if (SpellLine.ContainsKey(SpellLineSelection)) cb.Text = SpellLineSelection;
			ScrollToRow(SpellLineSelection);
		}

		protected void ScrollToRow(string identifier)
		{
			if ((dgLookupEnabled) & (!noscroll))
			{
				for (int row = 0; row < cSpellLine.doc.DataSet.Tables[0].Rows.Count; row++)
				{
					if (dgSpellLine[row, namecolumn].ToString() == identifier)
					{
						dgSpellLine.ScrollToRow(row);
						dgSpellLine.ClickRowHeader(row);
						noscroll = false;
						break;
					}
				}
			}
			else
			{
				noscroll = false;
			}
		}

		protected void event_cbChange(object sender, EventArgs e)
		{
			// this is a special case where the key field is not what we want 
			// to populate in the table... we actually want the name of the template.
			// there can be many items in a template each item attached to a mob/merchant/npc.
			SpellLineSelection = ((ComboBox)sender).SelectedItem.ToString();
			ScrollToRow(SpellLineSelection);
		}

		override protected void event_OK(object sender, EventArgs e)
		{			
			this.Close();
		}

		protected void event_New_Item(object sender, EventArgs e)
		{			
			Edit_Item(tablename, "", cb, out _SpellLineSelection,keycolumn,namecolumn,true,ref SpellLine);
			this.Close(); // close selector and return last entered value as the result
		}

		protected void event_Edit_Item(object sender, EventArgs e)
		{
			string RowID = dgSpellLine[dgSpellLine.CurrentCell.RowNumber,keycolumn].ToString();
			Edit_Item(tablename, RowID, cb, out _SpellLineSelection,keycolumn,namecolumn,false,ref SpellLine);
			_SpellLineSelection = FindKey(ref SpellLine, _SpellLineSelection);
			this.Close();
		}

		override protected void event_load(object sender, System.EventArgs e)
		{
			this.Hide();
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			Point pos=new Point(10, 10);
			GenerateUI(pos.X, pos.Y, DynamicForm);	
			this.CenterToParent();
			this.Text = sSpellLine;
			this.Show();
			if (dgSpellLineState) 
			{
				event_dg(btnExpand,null);
			}
		}
	
		protected void event_dg(object sender, EventArgs e)
		{
			if (((Button)sender).Text == "+")
			{
				((Button)sender).Text = "-";
				dgSpellLineState = true;
				gbBottom.Visible = true;
				if (dgSpellLineWidth > expandedwidth)
				{
					this.Width = expandedwidth;
				}
				else if (dgSpellLineWidth < width)
				{
					this.Width = width;
				}
				else
				{
					this.Width = dgSpellLineWidth;
				}
				this.Height = expandedheight;
			}
			else
			{
				((Button)sender).Text = "+";
				dgSpellLineState = false;
				gbBottom.Visible = false;
				this.Width = width;
				this.Height = height;
			}
		}

		protected void event_CurCellChange(object sender, EventArgs e)
		{
			try
			{
				noscroll = true;
				cb.Text = dgSpellLine[dgSpellLine.CurrentCell.RowNumber, namecolumn].ToString();
				if (cSpellLine.doc.DataSet.Tables[0].Rows.Count > 0) dgSpellLine.Select(dgSpellLine.CurrentCell.RowNumber);
			}
			catch
			{
				// suppress it... this only happens if they try to drag and drop stuff
				// on the grid... which they shouldn't be doing...
			}
		}

		override protected void event_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			this.Close();
		}
	}
	#endregion

	#region Spell Selector Class
	/// <summary>
	/// I am a Spell selection form.
	/// </summary>
	public class SpellSelector : AbstractEditor
	{
		private ComboBox cb;
		private GroupBox gbTop;
		private GroupBox gbBottom;
		private const string tablename = "Spell";
		private const int keycolumn = 1;
		private const int namecolumn = 2;
		private const int height = 80;
		private const int width = 496;
		private const int expandedheight = 428;
		private const int expandedwidth = 640;
		private bool noscroll = false;
		private Button btnExpand;
		private ContextMenu contextMenu1;
		private MenuItem menuItemNew;
		private MenuItem menuItemEdit;

		public override void Initialize()
		{
			if (Spell == null) Spell = InitializeList(tablename,keycolumn,namecolumn,true,false);
		}

		protected override void GenerateUI(int ax, int ay, Control cParent)
		{
			Initialize();
			gbTop = SetupDockGroupBox(2,2,44,DockStyle.Top,true,cParent);
			gbBottom = SetupDockGroupBox(2,45,350,DockStyle.Bottom,false,cParent);
			cb = MakeComboBox(8, 12, 230, gbTop, ref Spell, new EventHandler(event_cbChange));
			cb.MaxDropDownItems = 20;
			this.AcceptButton = MakeButton(sOK, 248, 12, 58, 23, new EventHandler(event_OK), gbTop);
			Button btn = MakeButton(sCancel, 2500, 2500, 75, 23, new EventHandler(event_Escape), _gb);
			btn.TabStop = false;
			this.CancelButton = btn;
			MakeButton(sNew, 312, 12, 58, 23, new EventHandler(event_New_Item), gbTop);
			MakeButton(sEdit, 376, 12, 58, 23, new EventHandler(event_Edit_Item), gbTop);
			if (dgLookupEnabled) 
			{
				dgSpell.Dock = DockStyle.Fill;
				dgSpell.Parent = gbBottom;
				dgSpell.CurrentCellChanged += new System.EventHandler(event_CurCellChange);	
				dgSpell.DoubleClick += new System.EventHandler(event_OK);
				gbBottom.Visible = false;
				contextMenu1 = new ContextMenu();
				menuItemNew = new MenuItem();
				menuItemEdit = new MenuItem();
				menuItemNew.Index = 0;
				menuItemNew.Text = sNew;
				menuItemNew.Click += new System.EventHandler(event_New_Item);
				menuItemEdit.Index = 1;
				menuItemEdit.Text = sEdit;
				menuItemEdit.Click += new System.EventHandler(event_Edit_Item);
				contextMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {menuItemNew,menuItemEdit});
				dgSpell.ContextMenu = contextMenu1;
				btnExpand = MakeButton("+", 442, 12, 23, 23, new EventHandler(event_dg), gbTop);
				this.Height = height;
				this.Width = width;
			}
			else
			{
				this.Height = height;
				this.Width = width-33;
				gbBottom.Visible = false;
			}
			cb.Text = FindKey(ref Spell,SpellSelection);
			ScrollToRow(SpellSelection);
		}

		protected void ScrollToRow(int identifier)
		{
			if ((dgLookupEnabled) & (!noscroll))
			{
				for (int row = 0; row < cSpell.doc.DataSet.Tables[0].Rows.Count; row++)
				{
					if ((int)dgSpell[row, keycolumn] == identifier)
					{
						dgSpell.ScrollToRow(row);
						dgSpell.ClickRowHeader(row);
						noscroll = false;
						break;
					}
				}
			}
			else
			{
				noscroll = false;
			}
		}

		protected void event_cbChange(object sender, EventArgs e)
		{
			SpellSelection = Selected_Item(ref Spell, ((ComboBox)sender).SelectedItem.ToString(),true);
			ScrollToRow(SpellSelection);
		}

		override protected void event_OK(object sender, EventArgs e)
		{			
			this.Close();
		}

		protected void event_New_Item(object sender, EventArgs e)
		{			
			Edit_Item(tablename, "", cb, out _SpellSelection, keycolumn, namecolumn,false,ref Spell);
			this.Close(); // close selector and return last entered value as the result
		}

		protected void event_Edit_Item(object sender, EventArgs e)
		{
			// we don't know what they are going to do, just assume current selection is what they want back.
			int HoldCurrSelection = (int)dgSpell[dgSpell.CurrentCell.RowNumber,keycolumn]; 
			string RowID = dgSpell[dgSpell.CurrentCell.RowNumber,0].ToString();  // we want 0, as it's "really" the key column...
			Edit_Item(tablename, RowID, cb, out _SpellSelection,keycolumn,namecolumn,false,ref Spell);
			SpellSelection = HoldCurrSelection; 
			this.Close();
		}

		override protected void event_load(object sender, System.EventArgs e)
		{
			this.Hide();
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			Point pos=new Point(10, 10);
			GenerateUI(pos.X, pos.Y, DynamicForm);	
			this.CenterToParent();
			this.Text = sSpell;
			this.Show();
			if (dgSpellState) 
			{
				event_dg(btnExpand,null);
			}
		}	

		protected void event_dg(object sender, EventArgs e)
		{
			if (((Button)sender).Text == "+")
			{
				((Button)sender).Text = "-";
				dgSpellState = true;
				gbBottom.Visible = true;
				if (dgSpellWidth > expandedwidth)
				{
					this.Width = expandedwidth;
				}
				else if (dgSpellWidth < width)
				{
					this.Width = width;
				}
				else
				{
					this.Width = dgSpellWidth;
				}
				this.Height = expandedheight;
			}
			else
			{
				((Button)sender).Text = "+";
				dgSpellState = false;
				gbBottom.Visible = false;
				this.Width = width;
				this.Height = height;
			}
		}

		protected void event_CurCellChange(object sender, EventArgs e)
		{
			try
			{
				noscroll = true;
				cb.Text = dgSpell[dgSpell.CurrentCell.RowNumber, namecolumn].ToString();
				if (cSpell.doc.DataSet.Tables[0].Rows.Count > 0) dgSpell.Select(dgSpell.CurrentCell.RowNumber);
			}
			catch
			{
				// suppress it... this only happens if they try to drag and drop stuff
				// on the grid... which they shouldn't be doing...
			}
		}

		override protected void event_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			this.Close();
		}
	}
	#endregion

	#region Spec Selector Class
	/// <summary>
	/// I am a Spec selection form.
	/// </summary>
	public class SpecSelector : AbstractEditor
	{
		private ComboBox cb;
		private GroupBox gbTop;
		private GroupBox gbBottom;
		private const string tablename = "Specialization";
		private const int keycolumn = 1;
		private const int namecolumn = 2;
		private const int height = 80;
		private const int width = 496;
		private const int expandedheight = 428;
		private const int expandedwidth = 640;
		private bool noscroll = false;
		private Button btnExpand;
		private ContextMenu contextMenu1;
		private MenuItem menuItemNew;
		private MenuItem menuItemEdit;

		public override void Initialize()
		{
			if (Spec == null) Spec = InitializeList(tablename,keycolumn,namecolumn,true,false);
		}

		protected override void GenerateUI(int ax, int ay, Control cParent)
		{
			Initialize();
			gbTop = SetupDockGroupBox(2,2,44,DockStyle.Top,true,cParent);
			gbBottom = SetupDockGroupBox(2,45,350,DockStyle.Bottom,false,cParent);
			cb = MakeComboBox(8, 12, 230, gbTop, ref Spec, new EventHandler(event_cbChange));
			cb.MaxDropDownItems = 20;
			this.AcceptButton = MakeButton(sOK, 248, 12, 58, 23, new EventHandler(event_OK), gbTop);
			Button btn = MakeButton(sCancel, 2500, 2500, 75, 23, new EventHandler(event_Escape), _gb);
			btn.TabStop = false;
			this.CancelButton = btn;
			MakeButton(sNew, 312, 12, 58, 23, new EventHandler(event_New_Item), gbTop);
			MakeButton(sEdit, 376, 12, 58, 23, new EventHandler(event_Edit_Item), gbTop);
			if (dgLookupEnabled) 
			{
				dgSpecialization.Dock = DockStyle.Fill;
				dgSpecialization.Parent = gbBottom;
				dgSpecialization.CurrentCellChanged += new System.EventHandler(event_CurCellChange);	
				dgSpecialization.DoubleClick += new System.EventHandler(event_OK);
				gbBottom.Visible = false;
				contextMenu1 = new ContextMenu();
				menuItemNew = new MenuItem();
				menuItemEdit = new MenuItem();
				menuItemNew.Index = 0;
				menuItemNew.Text = sNew;
				menuItemNew.Click += new System.EventHandler(event_New_Item);
				menuItemEdit.Index = 1;
				menuItemEdit.Text = sEdit;
				menuItemEdit.Click += new System.EventHandler(event_Edit_Item);
				contextMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {menuItemNew,menuItemEdit});
				dgSpecialization.ContextMenu = contextMenu1;
				btnExpand = MakeButton("+", 442, 12, 23, 23, new EventHandler(event_dg), gbTop);
				this.Height = height;
				this.Width = width;
			}
			else
			{
				this.Height = height;
				this.Width = width-33;
				gbBottom.Visible = false;
			}
			cb.Text = FindKey(ref Spec,SpecSelection);
			ScrollToRow(SpecSelection);
		}

		protected void ScrollToRow(string identifier)
		{
			if ((dgLookupEnabled) & (!noscroll))
			{
				for (int row = 0; row < cSpecialization.doc.DataSet.Tables[0].Rows.Count; row++)
				{
					if (dgSpecialization[row, keycolumn].ToString() == identifier)
					{
						dgSpecialization.ScrollToRow(row);
						dgSpecialization.ClickRowHeader(row);
						noscroll = false;
						break;
					}
				}
			}
			else
			{
				noscroll = false;
			}
		}

		protected void event_cbChange(object sender, EventArgs e)
		{
			SpecSelection = Selected_Item(ref Spec, ((ComboBox)sender).SelectedItem.ToString());
			ScrollToRow(SpecSelection);
		}

		override protected void event_OK(object sender, EventArgs e)
		{			
			this.Close();
		}

		protected void event_New_Item(object sender, EventArgs e)
		{			
			Edit_Item(tablename, "", cb, out _SpecSelection,keycolumn,namecolumn,false,ref Spec);
			this.Close(); // close selector and return last entered value as the result
		}

		protected void event_Edit_Item(object sender, EventArgs e)
		{
			// we don't know what they are going to do, just assume current selection is what they want back.
			string HoldCurrSelection = dgSpecialization[dgSpecialization.CurrentCell.RowNumber,keycolumn].ToString(); 
			string RowID = dgSpecialization[dgSpecialization.CurrentCell.RowNumber,0].ToString();  // we want 0, as it's "really" the key column...
			Edit_Item(tablename, RowID, cb, out _SpecSelection,keycolumn,namecolumn,false,ref Spec);
			SpecSelection = HoldCurrSelection; 
			this.Close();
		}

		override protected void event_load(object sender, System.EventArgs e)
		{
			this.Hide();
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			Point pos=new Point(10, 10);
			GenerateUI(pos.X, pos.Y, DynamicForm);	
			this.CenterToParent();
			this.Text = sSpecialization;
			this.Show();
			if (dgSpecializationState) 
			{
				event_dg(btnExpand,null);
			}
		}	

		protected void event_dg(object sender, EventArgs e)
		{
			if (((Button)sender).Text == "+")
			{
				((Button)sender).Text = "-";
				dgSpecializationState = true;
				gbBottom.Visible = true;
				if (dgSpecializationWidth > expandedwidth)
				{
					this.Width = expandedwidth;
				}
				else if (dgSpecializationWidth < width)
				{
					this.Width = width;
				}
				else
				{
					this.Width = dgSpecializationWidth;
				}
				this.Height = expandedheight;
			}
			else
			{
				((Button)sender).Text = "+";
				dgSpecializationState = false;
				gbBottom.Visible = false;
				this.Width = width;
				this.Height = height;
			}
		}

		protected void event_CurCellChange(object sender, EventArgs e)
		{
			try
			{
				noscroll = true;
				cb.Text = dgSpecialization[dgSpecialization.CurrentCell.RowNumber, namecolumn].ToString();
				if (cSpecialization.doc.DataSet.Tables[0].Rows.Count > 0) dgSpecialization.Select(dgSpecialization.CurrentCell.RowNumber);
			}
			catch
			{
				// suppress it... this only happens if they try to drag and drop stuff
				// on the grid... which they shouldn't be doing...
			}			
		}

		override protected void event_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			this.Close();
		}
	}
	#endregion

	#region Style Selector Class
	/// <summary>
	/// I am a Style selection form.
	/// </summary>
	public class StyleSelector : AbstractEditor
	{
		private ComboBox cb;
		private GroupBox gbTop;
		private GroupBox gbBottom;
		private const string tablename = "Style";
		private const int keycolumn = 1;
		private const int namecolumn = 2;
		private const int height = 80;
		private const int width = 496;
		private const int expandedheight = 428;
		private const int expandedwidth = 640;
		private bool noscroll = false;
		private Button btnExpand;
		private ContextMenu contextMenu1;
		private MenuItem menuItemNew;
		private MenuItem menuItemEdit;

		public override void Initialize()
		{
			if (Style == null) Style = InitializeList(tablename,keycolumn,namecolumn,true,false);
		}

		protected override void GenerateUI(int ax, int ay, Control cParent)
		{
			Initialize();
			gbTop = SetupDockGroupBox(2,2,44,DockStyle.Top,true,cParent);
			gbBottom = SetupDockGroupBox(2,45,350,DockStyle.Bottom,false,cParent);
			cb = MakeComboBox(8, 12, 230, gbTop, ref Style, new EventHandler(event_cbChange));
			cb.MaxDropDownItems = 20;
			this.AcceptButton = MakeButton(sOK, 248, 12, 58, 23, new EventHandler(event_OK), gbTop);
			Button btn = MakeButton(sCancel, 2500, 2500, 75, 23, new EventHandler(event_Escape), _gb);
			btn.TabStop = false;
			this.CancelButton = btn;
			MakeButton(sNew, 312, 12, 58, 23, new EventHandler(event_New_Item), gbTop);
			MakeButton(sEdit, 376, 12, 58, 23, new EventHandler(event_Edit_Item), gbTop);
			if (dgLookupEnabled) 
			{
				dgStyle.Dock = DockStyle.Fill;
				dgStyle.Parent = gbBottom;
				dgStyle.CurrentCellChanged += new System.EventHandler(event_CurCellChange);	
				dgStyle.DoubleClick += new System.EventHandler(event_OK);
				gbBottom.Visible = false;
				contextMenu1 = new ContextMenu();
				menuItemNew = new MenuItem();
				menuItemEdit = new MenuItem();
				menuItemNew.Index = 0;
				menuItemNew.Text = sNew;
				menuItemNew.Click += new System.EventHandler(event_New_Item);
				menuItemEdit.Index = 1;
				menuItemEdit.Text = sEdit;
				menuItemEdit.Click += new System.EventHandler(event_Edit_Item);
				contextMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {menuItemNew,menuItemEdit});
				dgStyle.ContextMenu = contextMenu1;
				btnExpand = MakeButton("+", 442, 12, 23, 23, new EventHandler(event_dg), gbTop);
				this.Height = height;
				this.Width = width;
			}
			else
			{
				this.Height = height;
				this.Width = width-33;
				gbBottom.Visible = false;
			}
			cb.Text = FindKey(ref Style,StyleSelection);
			ScrollToRow(StyleSelection);
		}

		protected void ScrollToRow(int identifier)
		{
			if ((dgLookupEnabled) & (!noscroll))
			{
				for (int row = 0; row < cStyle.doc.DataSet.Tables[0].Rows.Count; row++)
				{
					if ((int)dgStyle[row, keycolumn] == identifier)
					{
						dgStyle.ScrollToRow(row);
						dgStyle.ClickRowHeader(row);
						noscroll = false;
						break;
					}
				}
			}
			else
			{
				noscroll = false;
			}
		}

		protected void event_cbChange(object sender, EventArgs e)
		{
			StyleSelection = Selected_Item(ref Style, ((ComboBox)sender).SelectedItem.ToString(),true);
			ScrollToRow(StyleSelection);
		}

		override protected void event_OK(object sender, EventArgs e)
		{			
			this.Close();
		}

		protected void event_New_Item(object sender, EventArgs e)
		{			
			Edit_Item(tablename, "", cb, out _StyleSelection, keycolumn, namecolumn,false,ref Style);
			this.Close(); // close selector and return last entered value as the result
		}

		protected void event_Edit_Item(object sender, EventArgs e)
		{
			// we don't know what they are going to do, just assume current selection is what they want back.
			int HoldCurrSelection = (int)dgStyle[dgStyle.CurrentCell.RowNumber,keycolumn]; 
			string RowID = dgStyle[dgStyle.CurrentCell.RowNumber,0].ToString();  // we want 0, as it's "really" the key column...
			Edit_Item(tablename, RowID, cb, out _StyleSelection,keycolumn,namecolumn,false,ref Style);
			StyleSelection = HoldCurrSelection; 
			this.Close();
		}

		override protected void event_load(object sender, System.EventArgs e)
		{
			this.Hide();
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			Point pos=new Point(10, 10);
			GenerateUI(pos.X, pos.Y, DynamicForm);
			this.CenterToParent();	
			this.Text = sStyle;
			this.Show();
			if (dgStyleState) 
			{
				event_dg(btnExpand,null);
			}
		}	

		protected void event_dg(object sender, EventArgs e)
		{
			if (((Button)sender).Text == "+")
			{
				((Button)sender).Text = "-";
				dgStyleState = true;
				gbBottom.Visible = true;
				if (dgStyleWidth > expandedwidth)
				{
					this.Width = expandedwidth;
				}
				else if (dgStyleWidth < width)
				{
					this.Width = width;
				}
				else
				{
					this.Width = dgStyleWidth;
				}
				this.Height = expandedheight;
			}
			else
			{
				((Button)sender).Text = "+";
				dgStyleState = false;
				gbBottom.Visible = false;
				this.Width = width;
				this.Height = height;
			}
		}

		protected void event_CurCellChange(object sender, EventArgs e)
		{
			try
			{
				noscroll = true;
				cb.Text = dgStyle[dgStyle.CurrentCell.RowNumber, namecolumn].ToString();
				if (cStyle.doc.DataSet.Tables[0].Rows.Count > 0) dgStyle.Select(dgStyle.CurrentCell.RowNumber);
			}
			catch
			{
				// suppress it... this only happens if they try to drag and drop stuff
				// on the grid... which they shouldn't be doing...
			}
		}

		override protected void event_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			this.Close();
		}
	}
	#endregion

}
