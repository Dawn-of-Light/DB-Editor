using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections;
using System.Threading;


/// <summary>
/// These are little editor templates that use EditorCommonLib to drive their forms.
/// Most of the real work is in EditorCommonLib.  Note you CAN NOT open these in the 
/// designer... they are forms based on an abstract class and the designer hates it.
/// </summary>

namespace xmlDbEditor
{

	#region Ability Editor
	/// <summary>
	/// I am a Ability editor. 
	/// </summary>
	public class AbilityEditor : AbstractEditor
	{	

		public AbilityEditor(string SetBaseEditor)
		{
			if (SetBaseEditor != "") BaseEditor = SetBaseEditor;			
		}

		override protected void event_load(object sender, System.EventArgs e)
		{
			this.Hide();
			Point pos = SetupEditorForm();
			GenerateUI(pos.X, pos.Y, DynamicForm);
			this.CenterToParent();
			this.Text = sAbilityEditor;
			this.Show();
			OpenRecord();
		}
	}
	#endregion

	#region Account Editor
	/// <summary>
	/// I am a Account editor.  
	/// </summary>
	
	public class AccountEditor : AbstractEditor
	{	
		public AccountEditor(string SetBaseEditor)
		{
			if (SetBaseEditor != "") BaseEditor = SetBaseEditor;			
		}

		override protected void event_MouseEnter(object sender, System.EventArgs e)
		{   // show tooltip translations of fields that are keys
			int i;
			string s = ((TextBox)sender).Text;
			string n = ((TextBox)sender).Name.ToString();
			if (n.IndexOf(".") > -1) n = n.Split(new Char[] {'.'})[1];
			switch (n)
			{
				case "Realm" :
					if (Realms == null) (new RealmSelector()).Initialize();
					i = SafeIntParse(s);
					if (i != -1) toolTip1.SetToolTip((Control)sender, FindKey(ref Realms,i));
					break;
				case "PrivLevel" :
					if (Privileges == null) (new PrivilegeSelector()).Initialize();
					i = SafeIntParse(s);
					if (i != -1) toolTip1.SetToolTip((Control)sender, FindKey(ref Privileges,i));
					break;
			}
		}

		override protected void event_load(object sender, System.EventArgs e)
		{
			this.Hide();
			if (_PreCache)
			{
				if (Realms == null) (new RealmSelector()).Initialize();
				if (Privileges == null) (new PrivilegeSelector()).Initialize();
			}
			Point pos = SetupEditorForm();
			GenerateUI(pos.X, pos.Y, DynamicForm);
			this.CenterToParent();
			this.Text = sAccountEditor;
			this.Show();
			OpenRecord();			
		}
	}
	#endregion

	#region Bind Point Editor
	/// <summary>
	/// I am a Bind Point editor.  
	/// </summary>
	public class BindPointEditor : AbstractEditor
	{	
		public BindPointEditor(string SetBaseEditor)
		{
			if (SetBaseEditor != "") BaseEditor = SetBaseEditor;			
		}

		override protected void event_MouseEnter(object sender, System.EventArgs e)
		{   // show tooltip translations of fields that are keys
			int i;
			string s = ((TextBox)sender).Text;
			string n = ((TextBox)sender).Name.ToString();
			if (n.IndexOf(".") > -1) n = n.Split(new Char[] {'.'})[1];
			switch (n)
			{
				case "Region" :
					if (Regions == null) (new RegionSelector()).Initialize();
					i = SafeIntParse(s);
					if (i != -1) toolTip1.SetToolTip((Control)sender, FindKey(ref Regions,i));
					break;
			}
		}

		override protected void event_load(object sender, System.EventArgs e)
		{
			this.Hide();
			if (_PreCache)
			{
				if (Regions == null) (new RegionSelector()).Initialize();
			}
			Point pos = SetupEditorForm();
			GenerateUI(pos.X, pos.Y, DynamicForm);
			this.CenterToParent();
			this.Text = sBindPointEditor;
			this.Show();
			OpenRecord();
		}
	}
	#endregion

	#region DOL Character Editor
	/// <summary>
	/// I am a Character editor. 
	/// </summary>

	public class CharacterEditor : AbstractEditor
	{	
		public CharacterEditor(string SetBaseEditor)
		{
			if (SetBaseEditor != "") BaseEditor = SetBaseEditor;			
		}

		override protected void event_MouseEnter(object sender, System.EventArgs e)
		{   // show tooltip translations of fields that are keys
			int i;
			string s = ((TextBox)sender).Text;
			string n = ((TextBox)sender).Name.ToString();
			if (n.IndexOf(".") > -1) n = n.Split(new Char[] {'.'})[1];
			switch (n)
			{
				case "Region" :
					if (Regions == null) (new RegionSelector()).Initialize();
					i = SafeIntParse(s);
					if (i != -1) toolTip1.SetToolTip((Control)sender, FindKey(ref Regions,i));
					break;
				case "BindRegion" :
					if (Regions == null) (new RegionSelector()).Initialize();
					i = SafeIntParse(s);
					if (i != -1) toolTip1.SetToolTip((Control)sender, FindKey(ref Regions,i));
					break;
				case "GravestoneRegion" :
					if (Regions == null) (new RegionSelector()).Initialize();
					i = SafeIntParse(s);
					if (i != -1) toolTip1.SetToolTip((Control)sender, FindKey(ref Regions,i));
					break;
				case "Realm" :
					if (Realms == null) (new RealmSelector()).Initialize();
					i = SafeIntParse(s);
					if (i != -1) toolTip1.SetToolTip((Control)sender, FindKey(ref Realms,i));
					break;
				case "Race" :
					if (Race == null) (new RaceSelector()).Initialize();
					i = SafeIntParse(s);
					if (i != -1) toolTip1.SetToolTip((Control)sender, FindKey(ref Race,i));
					break;
				case "Class" :
					if (ClassType == null) (new ClassTypeSelector()).Initialize();
					i = SafeIntParse(s);
					if (i != -1) toolTip1.SetToolTip((Control)sender, FindKey(ref ClassType,i));
					break;
			}
		}

		override protected void event_load(object sender, System.EventArgs e)
		{
			this.Hide();
			if (_PreCache)
			{
				if (Regions == null) (new RegionSelector()).Initialize();
				if (Race == null) (new RaceSelector()).Initialize();
				if (Realms == null) (new RealmSelector()).Initialize();
				if (ClassType == null) (new ClassTypeSelector()).Initialize();
			}
			Point pos = SetupEditorForm();
			GenerateUI(pos.X, pos.Y, DynamicForm);
			this.CenterToParent();
			this.Text = sCharacterEditor;
			this.Show();
			OpenRecord();
		}
	}
	#endregion

	#region Inventory Item Editor

	/// <summary>
	/// I am a Inventory Item editor. 
	/// </summary>
	public class InventoryItemEditor : AbstractEditor
	{	
		public InventoryItemEditor(string SetBaseEditor)
		{
			if (SetBaseEditor != "") BaseEditor = SetBaseEditor;			
		}

		override protected void event_MouseEnter(object sender, System.EventArgs e)
		{   // show tooltip translations of fields that are keys
			int i;
			string s = ((TextBox)sender).Text;
			string n = ((TextBox)sender).Name.ToString();
			if (n.IndexOf(".") > -1) n = n.Split(new Char[] {'.'})[1];
			switch (n)
			{
				case "SlotPosition" :
					if (Slot == null) (new SlotSelector()).Initialize();
					i = SafeIntParse(s);
					if (i != -1) toolTip1.SetToolTip((Control)sender, FindKey(ref Slot,i));
					break;
				case "Hand" :
					if (Hand == null) (new HandSelector()).Initialize();
					i = SafeIntParse(s);
					if (i != -1) toolTip1.SetToolTip((Control)sender, FindKey(ref Hand,i));
					break;
				case "Type_Damage" :
					if (Damage == null) (new DamageSelector()).Initialize();
					i = SafeIntParse(s);
					if (i != -1) toolTip1.SetToolTip((Control)sender, FindKey(ref Damage,i));
					break;
				case "Object_Type" :
					if (ObjectType == null) (new ObjectTypeSelector()).Initialize();
					i = SafeIntParse(s);
					if (i != -1) toolTip1.SetToolTip((Control)sender, FindKey(ref ObjectType,i));
					break;
				case "Item_Type" :
					if (ItemType == null) (new ItemTypeSelector()).Initialize();
					i = SafeIntParse(s);
					if (i != -1) toolTip1.SetToolTip((Control)sender, FindKey(ref ItemType,i));
					break;
				case "Color" :
					if (Color == null) (new ColorSelector()).Initialize();
					i = SafeIntParse(s);
					if (i != -1) toolTip1.SetToolTip((Control)sender, FindKey(ref Color,i));
					break;
				case "Emblem" :
					if (Emblem == null) (new EmblemSelector()).Initialize();
					i = SafeIntParse(s);
					if (i != -1) toolTip1.SetToolTip((Control)sender, FindKey(ref Emblem,i));
					break;
				case "Effect" :
					if (WeaponEffect == null) (new WeaponEffectSelector()).Initialize();
					i = SafeIntParse(s);
					if (i != -1) toolTip1.SetToolTip((Control)sender, FindKey(ref WeaponEffect,i));
					break;
				case "Model" :
					if (ItemModel == null) (new ItemModelSelector()).Initialize();
					i = SafeIntParse(s);
					if (i != -1) toolTip1.SetToolTip((Control)sender, FindKey(ref ItemModel,i));
					break;
			}
		}

		override protected void event_load(object sender, System.EventArgs e)
		{
			this.Hide();
			if (_PreCache)
			{
				if (Slot == null) (new SlotSelector()).Initialize();
				if (Hand == null) (new HandSelector()).Initialize();
				if (Damage == null) (new DamageSelector()).Initialize();
				if (ObjectType == null) (new ObjectTypeSelector()).Initialize();
				if (ItemType == null) (new ItemTypeSelector()).Initialize();
				if (Color == null) (new ColorSelector()).Initialize();
				if (Emblem == null) (new EmblemSelector()).Initialize();
				if (WeaponEffect == null) (new WeaponEffectSelector()).Initialize();
				if (ItemModel == null) (new ItemModelSelector()).Initialize();				
			}
			Point pos = SetupEditorForm();
			GenerateUI(pos.X, pos.Y, DynamicForm);
			this.CenterToParent();
			this.Text = sInventoryItemEditor;
			this.Show();
			OpenRecord();
		}
	}
	#endregion

	#region Item Template Editor
	/// <summary>
	/// I am a Item Template editor.  
	/// </summary>
	public class ItemTemplateEditor : AbstractEditor
	{	
		public ItemTemplateEditor(string SetBaseEditor)
		{
			if (SetBaseEditor != "") BaseEditor = SetBaseEditor;			
		}

		override protected void event_MouseEnter(object sender, System.EventArgs e)
		{   // show tooltip translations of fields that are keys
			int i;
			string s = ((TextBox)sender).Text;
			string n = ((TextBox)sender).Name.ToString();
			if (n.IndexOf(".") > -1) n = n.Split(new Char[] {'.'})[1];
			switch (n)
			{
				case "Hand" :
					if (Hand == null) (new HandSelector()).Initialize();
					i = SafeIntParse(s);
					if (i != -1) toolTip1.SetToolTip((Control)sender, FindKey(ref Hand,i));
					break;
				case "Type_Damage" :
					if (Damage == null) (new DamageSelector()).Initialize();
					i = SafeIntParse(s);
					if (i != -1) toolTip1.SetToolTip((Control)sender, FindKey(ref Damage,i));
					break;
				case "Object_Type" :
					if (ObjectType == null) (new ObjectTypeSelector()).Initialize();
					i = SafeIntParse(s);
					if (i != -1) toolTip1.SetToolTip((Control)sender, FindKey(ref ObjectType,i));
					break;
				case "Item_Type" :
					if (ItemType == null) (new ItemTypeSelector()).Initialize();
					i = SafeIntParse(s);
					if (i != -1) toolTip1.SetToolTip((Control)sender, FindKey(ref ItemType,i));
					break;
				case "Color" :
					if (Color == null) (new ColorSelector()).Initialize();
					i = SafeIntParse(s);
					if (i != -1) toolTip1.SetToolTip((Control)sender, FindKey(ref Color,i));
					break;
				case "Emblem" :
					if (Emblem == null) (new EmblemSelector()).Initialize();
					i = SafeIntParse(s);
					if (i != -1) toolTip1.SetToolTip((Control)sender, FindKey(ref Emblem,i));
					break;
				case "Effect" :
					if (WeaponEffect == null) (new WeaponEffectSelector()).Initialize();
					i = SafeIntParse(s);
					if (i != -1) toolTip1.SetToolTip((Control)sender, FindKey(ref WeaponEffect,i));
					break;
				case "Model" :
					if (ItemModel == null) (new ItemModelSelector()).Initialize();
					i = SafeIntParse(s);
					if (i != -1) toolTip1.SetToolTip((Control)sender, FindKey(ref ItemModel,i));
					break;
			}
		}

		override protected void event_load(object sender, System.EventArgs e)
		{
			this.Hide();
			if (_PreCache)
			{
				if (Hand == null) (new HandSelector()).Initialize();
				if (Damage == null) (new DamageSelector()).Initialize();
				if (ObjectType == null) (new ObjectTypeSelector()).Initialize();
				if (ItemType == null) (new ItemTypeSelector()).Initialize();
				if (Color == null) (new ColorSelector()).Initialize();
				if (Emblem == null) (new EmblemSelector()).Initialize();
				if (WeaponEffect == null) (new WeaponEffectSelector()).Initialize();
				if (ItemModel == null) (new ItemModelSelector()).Initialize();
			}
			Point pos = SetupEditorForm();
			GenerateUI(pos.X, pos.Y, DynamicForm);
			this.CenterToParent();
			this.Text = sItemTemplateEditor;
			this.Show();
			OpenRecord();
		}
	}
	#endregion

	#region Line Spell Editor
	/// <summary>
	/// I am a LineXSpell editor. 
	/// </summary>
	/// 
	public class LineXSpellEditor : AbstractEditor
	{	
		public LineXSpellEditor(string SetBaseEditor)
		{
			if (SetBaseEditor != "") BaseEditor = SetBaseEditor;			
		}

		override protected void event_MouseEnter(object sender, System.EventArgs e)
		{   // show tooltip translations of fields that are keys
			int i;
			string s = ((TextBox)sender).Text;
			string n = ((TextBox)sender).Name.ToString();
			if (n.IndexOf(".") > -1) n = n.Split(new Char[] {'.'})[1];
			switch (n)
			{
				// linename doesn't need a lookup
				case "SpellID" :
					if (Spell == null) (new SpellSelector()).Initialize();
					i = SafeIntParse(s);
					if (i != -1) toolTip1.SetToolTip((Control)sender, FindKey(ref Spell,i));
					break;
			}
		}

		override protected void event_load(object sender, System.EventArgs e)
		{
			this.Hide();
			if (_PreCache)
			{
				if (Spell == null) (new SpellSelector()).Initialize();
			}
			Point pos = SetupEditorForm();
			GenerateUI(pos.X, pos.Y, DynamicForm);
			this.CenterToParent();
			this.Text = sLineXSpellEditor;
			this.Show();
			OpenRecord();
		}
	}
	#endregion

	#region Merchant Editor
	/// <summary>
	/// I am a Merchant editor.  
	/// </summary>
	public class MerchantEditor : AbstractEditor	
	{	
		public MerchantEditor(string SetBaseEditor)
		{
			if (SetBaseEditor != "") BaseEditor = SetBaseEditor;			
		}

		override protected void event_MouseEnter(object sender, System.EventArgs e)
		{   // show tooltip translations of fields that are keys
			int i;
			string s = ((TextBox)sender).Text;
			string n = ((TextBox)sender).Name.ToString();
			if (n.IndexOf(".") > -1) n = n.Split(new Char[] {'.'})[1];
			switch (n)
			{
				case "Region" :
					if (Regions == null) (new RegionSelector()).Initialize();
					i = SafeIntParse(s);
					if (i != -1) toolTip1.SetToolTip((Control)sender, FindKey(ref Regions,i));
					break;
				case "Realm" :
					if (Realms == null) (new RealmSelector()).Initialize();
					i = SafeIntParse(s);
					if (i != -1) toolTip1.SetToolTip((Control)sender, FindKey(ref Realms,i));
					break;
				case "Model" :
					if (MonsterModel == null) (new MonsterModelSelector()).Initialize();
					i = SafeIntParse(s);
					if (i != -1) toolTip1.SetToolTip((Control)sender, FindKey(ref MonsterModel,i));
					break;
				// equipmenttemplateid doesn't need a lookup		
			}
		}

		protected override void GenerateUI(int ax, int ay, Control gbParent)
		{
			_gb = SetupGroupBoxWithNavigator(ax,ay,gbParent);
			int rx=8; int ry=40; 			
			ry = BuildColumns(rx,ry);
			ry += 10;			
			this.AcceptButton = MakeButton(sOK, rx, ry, 75, 23, new EventHandler(event_OK), _gb);
			Button btn = MakeButton(sCancel, rx+2500, ry+2500, 75, 23, new EventHandler(event_Escape), _gb);
			btn.TabStop = false;
			this.CancelButton = btn;
			if (BaseEditor != "MerchantItem")
			{
				// we do not want to create endless itterations...
				MakeButton(sArrangeItems, rx+120, ry, 110, 23, new EventHandler(event_ArrangeItems), _gb);
				MakeButton(sEditItems, rx+266, ry, 100, 23, new EventHandler(event_EditItems), _gb);
			}
			ry += 25;			
			AdjustFormSize(ay,ry,ax,rx);
			if (this.Width < 412) this.Width = 412; // merchant editor will be too small if the DB is empty otherwise.
		}

		protected void event_EditItems(object sender, EventArgs e)
		{
			Edit_Item("MerchantItem","Merchant_ID","MerchantID", new MerchantItemEditor(""),_gb.Controls[2].Text); // gb.controls[2].text is the merchant id.
		}

		protected void event_ArrangeItems(object sender, EventArgs e)
		{
			if (Items == null) (new ItemSelector()).Initialize();
			Arrange_Items("MerchantItem","Merchant_ID","MerchantID","ItemTemplateID",ref Items,"ItemTemplate_ID","MerchantItem_ID");
		}

		override protected void event_load(object sender, System.EventArgs e)
		{
			this.Hide();
			if (_PreCache)
			{
				if (Realms == null) (new RealmSelector()).Initialize();
				if (Regions == null) (new RegionSelector()).Initialize();
				if (Items == null) (new ItemSelector()).Initialize();
				if (MonsterModel == null) (new MonsterModelSelector()).Initialize();
			}
			Point pos = SetupEditorForm();
			GenerateUI(pos.X, pos.Y, DynamicForm);
			this.CenterToParent();
			this.Text = sMerchantEditor;
			this.Show();
			OpenRecord();
		}
	}
	#endregion

	#region Merchant Item Editor
	/// <summary>
	/// I am a Merchant Item editor. 
	/// </summary>
	public class MerchantItemEditor : AbstractEditor
	{	
		public MerchantItemEditor(string SetBaseEditor)
		{
			if (SetBaseEditor != "") BaseEditor = SetBaseEditor;			
		}

		override protected void event_MouseEnter(object sender, System.EventArgs e)
		{   // show tooltip translations of fields that are keys
			int i;
			string s = ((TextBox)sender).Text;
			string n = ((TextBox)sender).Name.ToString();
			if (n.IndexOf(".") > -1) n = n.Split(new Char[] {'.'})[1];
			switch (n)
			{
				case "MerchantID" :
					if (Merchant == null) (new MerchantSelector()).Initialize();
					if (s != "") toolTip1.SetToolTip((Control)sender, FindKey(ref Merchant,s));
					break;
				case "ItemTemplateID" :
					if (Items == null) (new ItemSelector()).Initialize();
					i = SafeIntParse(s);
					if (i != -1) toolTip1.SetToolTip((Control)sender, FindKey(ref Items,i));
					break;			
			}
		}

		override protected void event_load(object sender, System.EventArgs e)
		{
			this.Hide();
			if (_PreCache)
			{
				if (Merchant == null) (new MerchantSelector()).Initialize();
				if (Items == null) (new ItemSelector()).Initialize();
			}
			Point pos = SetupEditorForm();
			GenerateUI(pos.X, pos.Y, DynamicForm);
			this.CenterToParent();
			this.Text = sMerchantItemEditor;
			this.Show();
			OpenRecord();
		}
	}
	#endregion

	#region Mob Editor
	/// <summary>
	/// I am a Mob editor.  
	/// </summary>
	public class MobEditor : AbstractEditor
	{	
		public MobEditor(string SetBaseEditor)
		{
			if (SetBaseEditor != "") BaseEditor = SetBaseEditor;			
		}

		override protected void event_MouseEnter(object sender, System.EventArgs e)
		{   // show tooltip translations of fields that are keys
			int i;
			string s = ((TextBox)sender).Text;
			string n = ((TextBox)sender).Name.ToString();
			if (n.IndexOf(".") > -1) n = n.Split(new Char[] {'.'})[1];
			switch (n)
			{
				case "Region" :
					if (Regions == null) (new RegionSelector()).Initialize();
					i = SafeIntParse(s);
					if (i != -1) toolTip1.SetToolTip((Control)sender, FindKey(ref Regions,i));
					break;
				case "Realm" :
					if (Realms == null) (new RealmSelector()).Initialize();
					i = SafeIntParse(s);
					if (i != -1) toolTip1.SetToolTip((Control)sender, FindKey(ref Realms,i));
					break;	
				case "Model" :
					if (MonsterModel == null) (new MonsterModelSelector()).Initialize();
					i = SafeIntParse(s);
					if (i != -1) toolTip1.SetToolTip((Control)sender, FindKey(ref MonsterModel,i));
					break;			
				// equipmenttemplateid doesn't need a lookup		
			}
		}

		override protected void event_load(object sender, System.EventArgs e)
		{
			this.Hide();
			if (_PreCache)
			{
				if (Regions == null) (new RegionSelector()).Initialize();
				if (Realms == null) (new RealmSelector()).Initialize();
				if (MonsterModel == null) (new MonsterModelSelector()).Initialize();
			}
			Point pos = SetupEditorForm();
			GenerateUI(pos.X, pos.Y, DynamicForm);
			this.CenterToParent();
			this.Text = sMobEditor;
			this.Show();
			OpenRecord();
		}
	}
	#endregion

	#region NPC Equipment Editor
	/// <summary>
	/// I am a NPC Equipment editor. 
	/// </summary>
	public class NPCEquipmentEditor : AbstractEditor
	{	
		public NPCEquipmentEditor(string SetBaseEditor)
		{
			if (SetBaseEditor != "") BaseEditor = SetBaseEditor;			
		}

		override protected void event_MouseEnter(object sender, System.EventArgs e)
		{   // show tooltip translations of fields that are keys
			int i;
			string s = ((TextBox)sender).Text;
			string n = ((TextBox)sender).Name.ToString();
			if (n.IndexOf(".") > -1) n = n.Split(new Char[] {'.'})[1];
			switch (n)
			{
				case "Slot" :
					if (Slot == null) (new SlotSelector()).Initialize();
					i = SafeIntParse(s);
					if (i != -1) toolTip1.SetToolTip((Control)sender, FindKey(ref Slot,i));
					break;
				case "Color" :
					if (Color == null) (new ColorSelector()).Initialize();
					i = SafeIntParse(s);
					if (i != -1) toolTip1.SetToolTip((Control)sender, FindKey(ref Color,i));
					break;
				case "Emblem" :
					if (Emblem == null) (new EmblemSelector()).Initialize();
					i = SafeIntParse(s);
					if (i != -1) toolTip1.SetToolTip((Control)sender, FindKey(ref Emblem,i));
					break;
				case "Effect" :
					if (WeaponEffect == null) (new WeaponEffectSelector()).Initialize();
					i = SafeIntParse(s);
					if (i != -1) toolTip1.SetToolTip((Control)sender, FindKey(ref WeaponEffect,i));
					break;
				case "Model" :
					if (ItemModel == null) (new ItemModelSelector()).Initialize();
					i = SafeIntParse(s);
					if (i != -1) toolTip1.SetToolTip((Control)sender, FindKey(ref ItemModel,i));
					break;
			}
		}

		override protected void event_load(object sender, System.EventArgs e)
		{
			this.Hide();
			if (_PreCache)
			{
				if (Slot == null) (new SlotSelector()).Initialize();
				if (Color == null) (new ColorSelector()).Initialize();
				if (Emblem == null) (new EmblemSelector()).Initialize();
				if (WeaponEffect == null) (new WeaponEffectSelector()).Initialize();
				if (ItemModel == null) (new ItemModelSelector()).Initialize();
			}
			Point pos = SetupEditorForm();
			GenerateUI(pos.X, pos.Y, DynamicForm);
			this.CenterToParent();
			this.Text = sNPCEquipmentEditor;
			this.Show();
			OpenRecord();
		}
	}
	#endregion

	#region Specialization Editor
	/// <summary>
	/// I am a Spec editor.  
	/// </summary>
	public class SpecEditor : AbstractEditor
	{	
		public SpecEditor(string SetBaseEditor)
		{
			if (SetBaseEditor != "") BaseEditor = SetBaseEditor;			
		}

		override protected void event_load(object sender, System.EventArgs e)
		{
			this.Hide();
			Point pos = SetupEditorForm();
			GenerateUI(pos.X, pos.Y, DynamicForm);
			this.CenterToParent();
			this.Text = sSpecializationEditor;
			this.Show();
			OpenRecord();
		}
	}
	#endregion

	#region Spec Style Editor
	/// <summary>
	/// I am a SpecXStyle editor.  
	/// </summary>
	public class SpecXStyleEditor : AbstractEditor
	{	
		public SpecXStyleEditor(string SetBaseEditor)
		{
			if (SetBaseEditor != "") BaseEditor = SetBaseEditor;			
		}

		override protected void event_MouseEnter(object sender, System.EventArgs e)
		{   // show tooltip translations of fields that are keys
			int i;
			string s = ((TextBox)sender).Text;
			string n = ((TextBox)sender).Name.ToString();
			if (n.IndexOf(".") > -1) n = n.Split(new Char[] {'.'})[1];
			switch (n)
			{
				case "SpecKeyName" :
					if (Spec == null) (new SpecSelector()).Initialize();
					if (s != "") toolTip1.SetToolTip((Control)sender, FindKey(ref Spec,s));
					break;	
				case "StyleID" :
					if (Style == null) (new StyleSelector()).Initialize();
					i = SafeIntParse(s);
					if (i != -1) toolTip1.SetToolTip((Control)sender, FindKey(ref Style,i));
					break;	
			}
		}

		override protected void event_load(object sender, System.EventArgs e)
		{
			this.Hide();
			if (_PreCache)
			{
				if (Spec == null) (new SpecSelector()).Initialize();
				if (Style == null) (new StyleSelector()).Initialize();
			}
			Point pos = SetupEditorForm();
			GenerateUI(pos.X, pos.Y, DynamicForm);
			this.CenterToParent();
			this.Text = sSpecStyleEditor;
			this.Show();
			OpenRecord();
		}
	}
	#endregion

	#region Spell Editor
	/// <summary>
	/// I am a Spell editor.  
	/// </summary>
	public class SpellEditor : AbstractEditor
	{	
		public SpellEditor(string SetBaseEditor)
		{
			if (SetBaseEditor != "") BaseEditor = SetBaseEditor;			
		}

		override protected void event_MouseEnter(object sender, System.EventArgs e)
		{   // show tooltip translations of fields that are keys
			int i;
			string s = ((TextBox)sender).Text;
			string n = ((TextBox)sender).Name.ToString();
			if (n.IndexOf(".") > -1) n = n.Split(new Char[] {'.'})[1];
			switch (n)
			{
				case "DamageType" :
					if (Damage == null) (new DamageSelector()).Initialize();
					i = SafeIntParse(s);
					if (i != -1) toolTip1.SetToolTip((Control)sender, FindKey(ref Damage,i));
					break;
			}
		}

		override protected void event_load(object sender, System.EventArgs e)
		{
			this.Hide();
			if (_PreCache)
			{
				if (Damage == null) (new DamageSelector()).Initialize();
			}
			ti.keycol="Spell_ID"; // this keeps a spell ... button from appearing on this form.
			Point pos = SetupEditorForm();
			GenerateUI(pos.X, pos.Y, DynamicForm);
			this.CenterToParent();
			this.Text = sSpellEditor;
			this.Show();
			OpenRecord();
		}
	}
	#endregion

	#region Spell Line Editor
	/// <summary>
	/// I am a Spell Line editor.  
	/// </summary>
	public class SpellLineEditor : AbstractEditor
	{	
		public SpellLineEditor(string SetBaseEditor)
		{
			if (SetBaseEditor != "") BaseEditor = SetBaseEditor;			
		}

		override protected void event_load(object sender, System.EventArgs e)
		{
			this.Hide();
			Point pos = SetupEditorForm();
			GenerateUI(pos.X, pos.Y, DynamicForm);
			this.CenterToParent();
			this.Text = sSpellLineEditor;
			this.Show();
			OpenRecord();
		}
	}
	#endregion

	#region Styles Editor
	/// <summary>
	/// I am a Style editor.  
	/// </summary>

	public class StyleEditor : AbstractEditor
	{	
		public StyleEditor(string SetBaseEditor)
		{
			if (SetBaseEditor != "") BaseEditor = SetBaseEditor;			
		}

		override protected void event_load(object sender, System.EventArgs e)
		{
			this.Hide();
			ti.keycol="Style_ID"; // this keeps a spell ... button from appearing on this form.
			Point pos = SetupEditorForm();
			GenerateUI(pos.X, pos.Y, DynamicForm);
			this.CenterToParent();
			this.Text = sStyleEditor;
			this.Show();
			OpenRecord();
		}
	}
	#endregion

	#region World Object Editor
	/// <summary>
	/// I am a World Object editor.  
	/// </summary>
	public class WorldObjectEditor : AbstractEditor
	{	
		public WorldObjectEditor(string SetBaseEditor)
		{
			if (SetBaseEditor != "") BaseEditor = SetBaseEditor;			
		}

		override protected void event_MouseEnter(object sender, System.EventArgs e)
		{   // show tooltip translations of fields that are keys
			int i;
			string s = ((TextBox)sender).Text;
			string n = ((TextBox)sender).Name.ToString();
			if (n.IndexOf(".") > -1) n = n.Split(new Char[] {'.'})[1];
			switch (n)
			{
				case "Region" :
					if (Regions == null) (new RegionSelector()).Initialize();
					i = SafeIntParse(s);
					if (i != -1) toolTip1.SetToolTip((Control)sender, FindKey(ref Regions,i));
					break;
				case "Model" :
					if (ItemModel == null) (new ItemModelSelector()).Initialize();
					i = SafeIntParse(s);
					if (i != -1) toolTip1.SetToolTip((Control)sender, FindKey(ref ItemModel,i));
					break;					
			}
		}

		override protected void event_load(object sender, System.EventArgs e)
		{
			this.Hide();
			if (_PreCache)
			{
				if (Regions == null) (new RegionSelector()).Initialize();
				if (ItemModel == null) (new ItemModelSelector()).Initialize();
			}
			Point pos = SetupEditorForm();
			GenerateUI(pos.X, pos.Y, DynamicForm);
			this.CenterToParent();
			this.Text = sWorldObjectEditor;
			this.Show();
			OpenRecord();
		}
	}
	#endregion

	#region Zone Point Editor
	/// <summary>
	/// I am a Zone Point editor.  
	/// </summary>

	public class ZonePointEditor : AbstractEditor
	{	
		public ZonePointEditor(string SetBaseEditor)
		{
			if (SetBaseEditor != "") BaseEditor = SetBaseEditor;			
		}

		override protected void event_MouseEnter(object sender, System.EventArgs e)
		{   // show tooltip translations of fields that are keys
			int i;
			string s = ((TextBox)sender).Text;
			string n = ((TextBox)sender).Name.ToString();
			if (n.IndexOf(".") > -1) n = n.Split(new Char[] {'.'})[1];
			switch (n)
			{
				case "Region" :
					if (Regions == null) (new RegionSelector()).Initialize();
					i = SafeIntParse(s);
					if (i != -1) toolTip1.SetToolTip((Control)sender, FindKey(ref Regions,i));
					break;
				case "Realm" :
					if (Realms == null) (new RealmSelector()).Initialize();
					i = SafeIntParse(s);
					if (i != -1) toolTip1.SetToolTip((Control)sender, FindKey(ref Realms,i));
					break;			
			}
		}


		override protected void event_load(object sender, System.EventArgs e)
		{
			this.Hide();
			if (_PreCache)
			{
				if (Regions == null) (new RegionSelector()).Initialize();
				if (Realms == null) (new RealmSelector()).Initialize();
			}
			Point pos = SetupEditorForm();
			GenerateUI(pos.X, pos.Y, DynamicForm);
			this.CenterToParent();
			this.Text = sZonePointEditor;
			this.Show();
			OpenRecord();
		}
	}
	#endregion

}
