using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.IO;
using Microsoft.Win32;
using NIF;
using OpenGL;
using System.Text;

namespace NIFViewer
{
	/// <summary>
	/// I am a highly implementation of the niflib_example_framework available in the niflib 
	/// project.   I no longer have a user control, all functionality is embedded direclty 
	/// in this form, along with specific functinality necessary to integrate with the XmlDbEditor.
	/// Also mouse control of t
	/// </summary>
	/// 
	public class frmNIFViewer : System.Windows.Forms.Form
	{
		public const int WM_LBUTTONDOWN = 513; // 0x0201  
		public const int WM_LBUTTONUP = 514; // 0x0202  
		[System.Runtime.InteropServices.DllImport("user32.dll")] 
 		static extern bool SendMessage(IntPtr hWnd, Int32 msg, Int32 wParam, Int32 lParam);
		private System.Windows.Forms.OpenFileDialog openFileDialog1;
		private System.Windows.Forms.MainMenu mainMenu1;
		private System.Windows.Forms.MenuItem menuItem3;
		private IntPtr m_hdc;
		private IntPtr m_current_object;
		private CCamera	camera;
		private Point last_position;
		private uint mn_gl_list;
		private	float mf_rotation;		
		private int WorkingWith;
		public int SelectedModel = -1;
		private System.Windows.Forms.Timer timer1;
		private System.Windows.Forms.OpenFileDialog openFileDialog_daoc;
		private System.Windows.Forms.MenuItem menuItemFile;
		private System.Windows.Forms.MenuItem menuItemOpen;
		private System.Windows.Forms.MenuItem menuItemExit;
		private System.Windows.Forms.MenuItem menuItemView;
		private System.Windows.Forms.MenuItem menuItemRotate;
		private System.Windows.Forms.MenuItem menuItem1;
		private System.Windows.Forms.MenuItem menuItem2;
		private System.Windows.Forms.MenuItem menuItem4;
		private System.Windows.Forms.MenuItem menuItem5;
		private System.Windows.Forms.MenuItem menuItem6;
		private System.Windows.Forms.MenuItem menuItem7;
		private System.Windows.Forms.MenuItem menuItem8;
		private System.Windows.Forms.MenuItem menuItem9;
		private System.Windows.Forms.MenuItem menuItem10;
		private System.Windows.Forms.MenuItem menuItem11;
		private System.Windows.Forms.MenuItem menuItem12;
		private System.Windows.Forms.MenuItem menuItem13;
		private System.Windows.Forms.MenuItem menuItem14;
		private System.Windows.Forms.MenuItem menuItem15;
		private System.Windows.Forms.MenuItem menuItem16;
		private System.ComponentModel.IContainer components;
		private System.Windows.Forms.ContextMenu contextMenu1;
		private System.Windows.Forms.MenuItem cmBrowse;
		private Form m_ParentForm;
		private FileSelect dia;

		public frmNIFViewer()
		{
			InitializeComponent();
			NIFAPI.Create(this.Handle);
			m_hdc = GL.GetDC(this.Handle);
			camera = new CCamera();
			last_position = new Point(0,0);
			m_current_object = new IntPtr(0);
			mn_gl_list = 0;
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

		#region  Windows Form Designer Generated Code
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
			this.mainMenu1 = new System.Windows.Forms.MainMenu();
			this.menuItemFile = new System.Windows.Forms.MenuItem();
			this.menuItemOpen = new System.Windows.Forms.MenuItem();
			this.menuItem3 = new System.Windows.Forms.MenuItem();
			this.menuItemExit = new System.Windows.Forms.MenuItem();
			this.menuItemView = new System.Windows.Forms.MenuItem();
			this.menuItemRotate = new System.Windows.Forms.MenuItem();
			this.menuItem1 = new System.Windows.Forms.MenuItem();
			this.menuItem2 = new System.Windows.Forms.MenuItem();
			this.menuItem4 = new System.Windows.Forms.MenuItem();
			this.menuItem5 = new System.Windows.Forms.MenuItem();
			this.menuItem6 = new System.Windows.Forms.MenuItem();
			this.menuItem7 = new System.Windows.Forms.MenuItem();
			this.menuItem8 = new System.Windows.Forms.MenuItem();
			this.menuItem9 = new System.Windows.Forms.MenuItem();
			this.menuItem10 = new System.Windows.Forms.MenuItem();
			this.menuItem11 = new System.Windows.Forms.MenuItem();
			this.menuItem12 = new System.Windows.Forms.MenuItem();
			this.menuItem13 = new System.Windows.Forms.MenuItem();
			this.menuItem14 = new System.Windows.Forms.MenuItem();
			this.menuItem15 = new System.Windows.Forms.MenuItem();
			this.menuItem16 = new System.Windows.Forms.MenuItem();
			this.openFileDialog_daoc = new System.Windows.Forms.OpenFileDialog();
			this.timer1 = new System.Windows.Forms.Timer(this.components);
			this.contextMenu1 = new System.Windows.Forms.ContextMenu();
			this.cmBrowse = new System.Windows.Forms.MenuItem();
			// 
			// openFileDialog1
			// 
			this.openFileDialog1.Filter = "Nif and Npk|*.nif;*.npk|All Files|*.*";
			this.openFileDialog1.RestoreDirectory = true;
			// 
			// mainMenu1
			// 
			this.mainMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this.menuItemFile,
																					  this.menuItemView,
																					  this.menuItem1});
			// 
			// menuItemFile
			// 
			this.menuItemFile.Index = 0;
			this.menuItemFile.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																						 this.menuItemOpen,
																						 this.menuItem3,
																						 this.menuItemExit});
			this.menuItemFile.Text = "File";
			// 
			// menuItemOpen
			// 
			this.menuItemOpen.Enabled = false;
			this.menuItemOpen.Index = 0;
			this.menuItemOpen.Shortcut = System.Windows.Forms.Shortcut.CtrlB;
			this.menuItemOpen.Text = "(select something in the editor and magic will happen)";
			this.menuItemOpen.Click += new System.EventHandler(this.menuItem5_Click);
			// 
			// menuItem3
			// 
			this.menuItem3.Index = 1;
			this.menuItem3.Text = "-";
			// 
			// menuItemExit
			// 
			this.menuItemExit.Index = 2;
			this.menuItemExit.Text = "Exit";
			this.menuItemExit.Click += new System.EventHandler(this.menuItem4_Click);
			// 
			// menuItemView
			// 
			this.menuItemView.Index = 1;
			this.menuItemView.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																						 this.menuItemRotate});
			this.menuItemView.Shortcut = System.Windows.Forms.Shortcut.CtrlR;
			this.menuItemView.Text = "View";
			// 
			// menuItemRotate
			// 
			this.menuItemRotate.Checked = true;
			this.menuItemRotate.Index = 0;
			this.menuItemRotate.Text = "Rotate";
			this.menuItemRotate.Click += new System.EventHandler(this.menuItemRotate_Click);
			// 
			// menuItem1
			// 
			this.menuItem1.Index = 2;
			this.menuItem1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this.menuItem2,
																					  this.menuItem4,
																					  this.menuItem5,
																					  this.menuItem6,
																					  this.menuItem7,
																					  this.menuItem8,
																					  this.menuItem9,
																					  this.menuItem10,
																					  this.menuItem11,
																					  this.menuItem12,
																					  this.menuItem13,
																					  this.menuItem14,
																					  this.menuItem15,
																					  this.menuItem16});
			this.menuItem1.Text = "Help";
			// 
			// menuItem2
			// 
			this.menuItem2.Enabled = false;
			this.menuItem2.Index = 0;
			this.menuItem2.Text = "Keyboard shortcuts are listed below:";
			// 
			// menuItem4
			// 
			this.menuItem4.Index = 1;
			this.menuItem4.Text = "-";
			// 
			// menuItem5
			// 
			this.menuItem5.Enabled = false;
			this.menuItem5.Index = 2;
			this.menuItem5.Text = "(L) Camera Left";
			// 
			// menuItem6
			// 
			this.menuItem6.Enabled = false;
			this.menuItem6.Index = 3;
			this.menuItem6.Text = "(R) Camera Right";
			// 
			// menuItem7
			// 
			this.menuItem7.Enabled = false;
			this.menuItem7.Index = 4;
			this.menuItem7.Text = "(F) Camera Front";
			// 
			// menuItem8
			// 
			this.menuItem8.Enabled = false;
			this.menuItem8.Index = 5;
			this.menuItem8.Text = "(B) Camera Back";
			// 
			// menuItem9
			// 
			this.menuItem9.Enabled = false;
			this.menuItem9.Index = 6;
			this.menuItem9.Text = "(T) Camera Top";
			// 
			// menuItem10
			// 
			this.menuItem10.Enabled = false;
			this.menuItem10.Index = 7;
			this.menuItem10.Text = "(right) Move model right";
			// 
			// menuItem11
			// 
			this.menuItem11.Enabled = false;
			this.menuItem11.Index = 8;
			this.menuItem11.Text = "(left) Move model left";
			// 
			// menuItem12
			// 
			this.menuItem12.Enabled = false;
			this.menuItem12.Index = 9;
			this.menuItem12.Text = "(up) Zoom Out";
			// 
			// menuItem13
			// 
			this.menuItem13.Enabled = false;
			this.menuItem13.Index = 10;
			this.menuItem13.Text = "(down) Zoom In";
			// 
			// menuItem14
			// 
			this.menuItem14.Index = 11;
			this.menuItem14.Text = "-";
			// 
			// menuItem15
			// 
			this.menuItem15.Enabled = false;
			this.menuItem15.Index = 12;
			this.menuItem15.Text = "You can browse models when appropriate (see file menu)";
			// 
			// menuItem16
			// 
			this.menuItem16.Enabled = false;
			this.menuItem16.Index = 13;
			this.menuItem16.Text = " - double click window to select model and return to editor";
			// 
			// openFileDialog_daoc
			// 
			this.openFileDialog_daoc.FileName = "camelot.exe";
			this.openFileDialog_daoc.Filter = "Camelot.exe|camelot.exe|All (*.*)|*.*";
			this.openFileDialog_daoc.RestoreDirectory = true;
			// 
			// timer1
			// 
			this.timer1.Interval = 50;
			this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
			// 
			// contextMenu1
			// 
			this.contextMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																						 this.cmBrowse});
			// 
			// cmBrowse
			// 
			this.cmBrowse.Enabled = false;
			this.cmBrowse.Index = 0;
			this.cmBrowse.Text = "(select something in the editor and magic will happen)";
			this.cmBrowse.Click += new System.EventHandler(this.menuItem5_Click);
			// 
			// frmNIFViewer
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(412, 260);
			this.ContextMenu = this.contextMenu1;
			this.ForeColor = System.Drawing.SystemColors.ControlLightLight;
			this.Menu = this.mainMenu1;
			this.Name = "frmNIFViewer";
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
			this.Text = "Model Viewer";
			this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmNIFViewer_KeyDown);
			this.Resize += new System.EventHandler(this.frmNIFViewer_Resize);
			this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.frmNIFViewer_KeyPress);
			this.Load += new System.EventHandler(this.Form1_Load);
			this.Closed += new System.EventHandler(this.frmNIFViewer_Closed);
			this.Paint += new System.Windows.Forms.PaintEventHandler(this.frmNIFViewer_Paint);

		}
		#endregion

		/// <summary>
		/// Main.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new frmNIFViewer());
		}

		private void menuItem4_Click(object sender, System.EventArgs e)
		{
			Close();
		}

		private void dia_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if ((dia != null) & (dia.result_view.SelectedItems.Count != 0))
			{
				SelectedModel = (int)dia.result_view.SelectedItems[0].Tag;
				this.loadDAOC(WorkingWith,SelectedModel);
				this.Text = "Model Viewer - " + dia.result_view.SelectedItems[0].SubItems[1].Text + " (" + SelectedModel.ToString() + ")";
			}
		}

		private void menuItem5_Click(object sender, System.EventArgs e)
		{
			dia = new FileSelect();
			dia.result_view.SelectedIndexChanged += new System.EventHandler(this.dia_SelectedIndexChanged);
			dia.m_ParentForm = m_ParentForm;
			dia.m_type = WorkingWith;
			if ( dia.ShowDialog() == DialogResult.OK)
			{
				this.loadDAOC(WorkingWith,dia.m_ID);
				int id = dia.m_ID;
				this.Text = "Model Viewer - " + dia.m_Name + " (" + id.ToString() + ")";
				SelectedModel = dia.m_ID;
			}			
			dia = null;
		}

		protected void event_Cancel(object sender, EventArgs e)
		{		
			this.Close();
		}

		static int MakeLong(int LoWord, int HiWord)  
		{  
			return (HiWord << 16) | (LoWord & 0xffff);  
		}  

		protected void event_DoClick(object sender, EventArgs e)
		{		
			Int32 lparam = MakeLong(10, 10);
			SendMessage(this.Handle, WM_LBUTTONDOWN, 0, lparam);  
			SendMessage(this.Handle, WM_LBUTTONUP, 0, lparam);  
			SendMessage(this.Handle, WM_LBUTTONDOWN, 0, lparam);  
			SendMessage(this.Handle, WM_LBUTTONUP, 0, lparam);  
		}

		private void Form1_Load(object sender, System.EventArgs e)
		{
			RegistryKey hklm = Registry.CurrentUser;
			RegistryKey regKey = hklm.CreateSubKey("Software\\XMLDBEditor");
			try
			{
				NIF.DAOCAPI.SetRootDirectory(regKey.GetValue("DAOCPath", "C:\\").ToString());
			}
			catch
			{
				MessageBox.Show(this,"Your DAOC path is not valid... check your configuration.");
				this.Close();
			}
			finally
			{
				regKey.Close();
				hklm.Close();
			}
			mf_rotation = 0;
			m_ParentForm = this;
			Button Btn1 = MakeButton("OK", 2500, 2500, 75, 23, new EventHandler(event_DoClick), this);
			Button Btn2 = MakeButton("Cancel", 2500, 2500, 75, 23, new EventHandler(event_Cancel), this);
			Btn1.TabStop = false;
			Btn2.TabStop = false;
			this.AcceptButton = Btn1;
			this.CancelButton = Btn2;
		}

		private void frmNIFViewer_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
		{
			switch (e.KeyChar)
			{
				case 'l': ViewObject(OpenGL.CCamera.e_viewPosition.Left); break;
				case 'r': ViewObject(OpenGL.CCamera.e_viewPosition.Right); break;
				case 'f': ViewObject(OpenGL.CCamera.e_viewPosition.Front); break;
				case 'b': ViewObject(OpenGL.CCamera.e_viewPosition.Below); break;
				case 't': ViewObject(OpenGL.CCamera.e_viewPosition.Top); break;
			}
		}

		private void frmNIFViewer_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			switch (e.KeyCode)
			{
				case Keys.Left : 
					camera.moveLocal(-(last_position.X - 1), (last_position.Y),0);
					break;
				case Keys.Right : 
					camera.moveLocal(-(last_position.X + 1), (last_position.Y),0);
					break;
				case Keys.Up : 
					camera.moveLocal(0,0,(last_position.Y - 1));
					break;
				case Keys.Down : 
					camera.moveLocal(0,0,(last_position.Y + 1));
					break;
			}		
		}	
		
		public void load(string path, string file)
		{

			// release old object			
			if (m_current_object != IntPtr.Zero)	
				NIFAPI.ReleaseObject(m_current_object);
			if (mn_gl_list != 0)
			{
				GL.glDeleteLists(mn_gl_list,1);
				mn_gl_list = 0;
			}

			// load new object
			m_current_object = NIFAPI.CreateObject(path,file);
			if (m_current_object == IntPtr.Zero)
			{
				MessageBox.Show(this,"Error loading object");
			}

			// reset the camera
			camera.targetObject(m_current_object, CCamera.e_viewPosition.Front);
	
			// make sure timer is running
			timer1.Start();
		}

		public void LoadItem(int id)
		{
			menuItemOpen.Text = "Browse Items";
			menuItemOpen.Enabled = true;
			cmBrowse.Text = "Browse Items";
			cmBrowse.Enabled = true;
			WorkingWith = 1;
			loadDAOC(1,id);
		}

		public void LoadMob(int id)
		{
			menuItemOpen.Text = "Browse Mobs";
			menuItemOpen.Enabled = true;
			cmBrowse.Text = "Browse Mobs";
			cmBrowse.Enabled = true;
			WorkingWith = 0;
			loadDAOC(0,id);
		}

		public void LoadZone(int id)
		{
			menuItemOpen.Text = "Browse Zones";
			menuItemOpen.Enabled = true;
			cmBrowse.Text = "Browse Zones";
			cmBrowse.Enabled = true;
			WorkingWith = 2;
			loadDAOC(2,id);
		}

		private void loadDAOC(int type, int id)
		{
			// release old object
			if (m_current_object != IntPtr.Zero)	
				NIFAPI.ReleaseObject(m_current_object);
			if (mn_gl_list != 0)
			{
				GL.glDeleteLists(mn_gl_list,1);
				mn_gl_list = 0;
			}

			// load new object
			StringBuilder s_name = new StringBuilder(256);
			switch (type)
			{
				case 0:	
					m_current_object = DAOCAPI.LoadMonster(id); 
					NIF.DAOCAPI.GetMonsterName(id,s_name,256);
					break;
				case 1:
					m_current_object = DAOCAPI.LoadItem(id); 
					NIF.DAOCAPI.GetItemName(id,s_name,256);
					break;
				case 2:	
					m_current_object = DAOCAPI.LoadZone(id); 
					NIF.DAOCAPI.GetZoneName(id,s_name,256);
					menuItemRotate.Checked = false;
					break;
			};

			if (m_current_object == IntPtr.Zero)
			{
				MessageBox.Show(this,"Error loading object");
			}
			else
			{
				this.Text = "Model Viewer - " + String.Format("{0}", s_name) + " (" + id.ToString() + ")";
			}
			// reset the camera
			camera.targetObject(m_current_object, CCamera.e_viewPosition.Right);
	
			// make sure timer is running
			timer1.Start();
		}

		public void ViewObject(CCamera.e_viewPosition side)
		{
			// set camera
			camera.targetObject(m_current_object, side);
		}

		private void timer1_Tick(object sender, System.EventArgs e)
		{
			// recall: OnPaint()
			Invalidate(false);		
		}

		private void frmNIFViewer_Resize(object sender, System.EventArgs e)
		{
			// let the NIFAPI reset the aspect ratio
			NIFAPI.OnWindowResize();		
		}

		private void frmNIFViewer_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
			// Clear The Screen And The Depth Buffer
			GL.glClear(GL.GL_COLOR_BUFFER_BIT | GL.GL_DEPTH_BUFFER_BIT);

			// a simple optimation. 
			if (mn_gl_list == 0 && m_current_object != IntPtr.Zero) 
			{
				GL.glMatrixMode(GL.GL_MODELVIEW);
				GL.glLoadIdentity();
				mn_gl_list = GL.glGenLists(1);
				GL.glNewList(mn_gl_list,GL.GL_COMPILE);
				NIFAPI.Render(m_current_object);
				GL.glEndList();
			}

			// set up the camera view (loads the matrix)
			camera.setView();

			// add a object rotation
			if (menuItemRotate.Checked)
			{
				mf_rotation += 0.5f;
				GL.glRotatef( mf_rotation, 0.0f, 0.0f,1.0f ); 
			}

			if (mn_gl_list != 0)
			{
				GL.glCallList(mn_gl_list);
			}
			else
			{
				// render the object
				if (m_current_object != IntPtr.Zero)
					NIFAPI.Render(m_current_object);
			}

			// display it
			GL.wglSwapBuffers(m_hdc);		
		}

		protected override void OnPaintBackground(PaintEventArgs pevent)
		{
			// do nothing
		}

		private void menuItemRotate_Click(object sender, System.EventArgs e)
		{
			menuItemRotate.Checked = !menuItemRotate.Checked;
		}

		private void frmNIFViewer_Closed(object sender, System.EventArgs e)
		{
			// release object
			if (m_current_object != IntPtr.Zero) NIFAPI.ReleaseObject(m_current_object);
			// release NIFAPI
			try
			{				
				NIFAPI.Release();
			}
			catch (Exception ex)
			{
				// suppress it for now
			}
			// release DC
			try
			{
				GL.ReleaseDC(this.Handle,m_hdc);
			}
			catch (Exception ex)
			{
				// suppress it for now
			}		
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

	}
}
