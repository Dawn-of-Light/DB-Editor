using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Drawing.Design;
using System.Data;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Runtime.InteropServices;
using System.Text;

namespace NIFAPI
{
	public class View : System.Windows.Forms.UserControl
	{
		private System.ComponentModel.IContainer components;

		/// <summary>
		/// Device handle of this control
		/// </summary>
		private IntPtr	m_hdc;

		/// <summary>
		/// Pointer of loadedobject
		/// </summary>
		private IntPtr	m_current_object;

		/// <summary>
		/// A simple animation (commented out in this example
		/// </summary>
		private	float	mf_rotation;

		/// <summary>
		/// A simple animation (commented out in this example
		/// </summary>
		public	bool	mb_rotation;

		/// <summary>
		/// Camera for easy movment in 3d
		/// </summary>
		private CCamera	camera;

		/// <summary>
		/// Last known mouse position (I need only the delta-movement)
		/// </summary>
		private Point	last_mouse_position;

		/// <summary>
		/// Refresh timer
		/// </summary>
		private System.Windows.Forms.Timer timer1;

		/// <summary>
		/// A OpenGL list which holds the complete model.
		/// That way we can simple speed up the rendering. 
		/// But it has many drawback and (hopefully) it isn't needed in the future.
		/// </summary>
		private uint	mn_gl_list;


		/// <summary>
		/// if true we use a camera which moves each frame depending on mouse delta.
		/// This camera movement isn't very popular but I like it :)
		/// </summary>
		public bool			mb_delta_camera;
		private System.Windows.Forms.MouseEventArgs	me_last_mouseevent;

		public View()
		{
			InitializeComponent();

			// Initialize NIFAPI
			NIFLIB.Create(this.Handle);


			// Get DC handle (you've to release it)
			m_hdc = GL.GetDC(this.Handle);

			// Create camera
			camera = new CCamera();

			// init vars
			last_mouse_position = new Point(0,0);
			m_current_object = new IntPtr(0);
			mf_rotation = 0;
			mb_rotation = true;
			mn_gl_list = 0;
			mb_delta_camera = true;
		}

		protected override void Dispose( bool disposing )
		{
			// release object
			if (m_current_object != IntPtr.Zero)
				NIFLIB.ReleaseObject(m_current_object);

			// release NIFAPI
			NIFLIB.Release();

			// release DC
			GL.ReleaseDC(this.Handle,m_hdc);

			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Vom Komponenten-Designer generierter Code
		/// <summary> 
		/// Erforderliche Methode für die Designerunterstützung. 
		/// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.timer1 = new System.Windows.Forms.Timer(this.components);
			// 
			// timer1
			// 
			this.timer1.Interval = 50;
			this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
			// 
			// View
			// 
			this.Name = "View";
			this.Size = new System.Drawing.Size(328, 352);
			this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.View_KeyPress);
			this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.View_MouseUp);
			this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.View_MouseMove);
			this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.View_MouseDown);

		}
		#endregion

		/// <summary>
		/// the rendering part
		/// </summary>
		protected override void OnPaint(PaintEventArgs e)
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
				NIFLIB.Render(m_current_object);
				GL.glEndList();
			}


			// set up the camera view (loads the matrix)
			camera.setView();


			// add a object rotation
			if (mb_rotation)
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
					NIFLIB.Render(m_current_object);
			}

			// display it
			GL.wglSwapBuffers(m_hdc);
		}

		protected override void OnPaintBackground(PaintEventArgs pevent)
		{
			// nothing todo 'cause we will draw the whole thing
		}

		protected override void OnSizeChanged(EventArgs e)
		{
			// let the NIFAPI reset the aspect ratio
			NIFLIB.OnWindowResize();
		}

		public void load(string path, string file)
		{
			// release old object
			if (m_current_object != IntPtr.Zero)	
				NIFLIB.ReleaseObject(m_current_object);
			if (mn_gl_list != 0)
			{
				GL.glDeleteLists(mn_gl_list,1);
				mn_gl_list = 0;
			}

			// load new object
			m_current_object = NIFLIB.CreateObject(path,file);
			if (m_current_object == IntPtr.Zero)
			{
				MessageBox.Show(this,"Error loading object");
			}

			// reset the camera
			camera.targetObject(m_current_object, CCamera.e_viewPosition.Front);
	
			// make sure timer is running
			timer1.Start();
		}

		public void loadDAOC(int type, int id)
		{
			// release old object
			if (m_current_object != IntPtr.Zero)	
				NIFLIB.ReleaseObject(m_current_object);
			if (mn_gl_list != 0)
			{
				GL.glDeleteLists(mn_gl_list,1);
				mn_gl_list = 0;
			}

			// load new object
			switch (type)
			{
				case 0:	m_current_object = NIFLIB.LoadMonster(id); break;
				case 1:	m_current_object = NIFLIB.LoadItem(id); break;
				case 2:	m_current_object = NIFLIB.LoadZone(id);
					mb_rotation = false; // don't want rotating zone :)
					break;
			};

			if (m_current_object == IntPtr.Zero)
			{
				MessageBox.Show(this,"Error loading object");
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
			if (mb_delta_camera)
			{
				// Left+Right pressed -> move on Z-Axis
				if ((me_last_mouseevent.Button & MouseButtons.Left)!=0 && (me_last_mouseevent.Button & MouseButtons.Right)!=0)
				{
					camera.moveLocal(0,0,(last_mouse_position.Y - me_last_mouseevent.Y));
				}
				else
					// Left pressed -> move on X/Y-Axis
					if ((me_last_mouseevent.Button & MouseButtons.Left)!=0)
				{
					camera.moveLocal(-(last_mouse_position.X - me_last_mouseevent.X),
						(last_mouse_position.Y -me_last_mouseevent.Y),0);
				}
				else
					// Right pressed -> rotated on X/Y-Axis
					if ((me_last_mouseevent.Button & MouseButtons.Right)!=0)
				{
					camera.rotateLocal((last_mouse_position.X - me_last_mouseevent.X)/15.0f,0,-1,0);
					camera.rotateLocal((last_mouse_position.Y - me_last_mouseevent.Y)/15.0f,-1,0,0);
				}

				// Middle button -> rotate on Z
				if ((me_last_mouseevent.Button & MouseButtons.Middle)!=0)
				{
					camera.rotateLocal((last_mouse_position.X - me_last_mouseevent.X)/15.0f,0,0,-1);
				}

			}
	
			// recall OnPaint
			Invalidate(false);
		}

		/// <summary>
		/// Camera movement (not good but okay for testing)
		/// </summary>
		private void View_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if (mb_delta_camera)
			{
				me_last_mouseevent = e;
				return;
			}

			// Left+Right pressed -> move on Z-Axis
			if ((e.Button & MouseButtons.Left)!=0 && (e.Button & MouseButtons.Right)!=0)
			{
				camera.moveLocal(0,0,(last_mouse_position.Y - e.Y));
			}
			else
				// Left pressed -> move on X/Y-Axis
				if ((e.Button & MouseButtons.Left)!=0)
			{
				camera.moveLocal(-(last_mouse_position.X - e.X),
					(last_mouse_position.Y -e.Y),0);
			}
			else
				// Right pressed -> rotated on X/Y-Axis
				if ((e.Button & MouseButtons.Right)!=0)
			{
				camera.rotateLocal((last_mouse_position.X - e.X)/10.0f,0,-1,0);
				camera.rotateLocal((last_mouse_position.Y - e.Y)/10.0f,-1,0,0);
			}

			// Middle button -> rotate on Z
			if ((e.Button & MouseButtons.Middle)!=0)
			{
				camera.rotateLocal((last_mouse_position.X - e.X)/8.0f,0,0,-1);
			}

	
			// store last mouse position
			last_mouse_position.X = e.X;
			last_mouse_position.Y = e.Y;
		}

		private void View_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if (mb_delta_camera)
			{
				// store last mouse position
				last_mouse_position.X = e.X;
				last_mouse_position.Y = e.Y;
				me_last_mouseevent = e;
			}
		}

		private void View_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if (mb_delta_camera)
			{
				// just to prevent additinal movement
				last_mouse_position.X = e.X;
				last_mouse_position.Y = e.Y;
			}
		}

		private void View_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
		{
			switch (e.KeyChar)
			{
				case 'l':	ViewObject(CCamera.e_viewPosition.Left);	 break;
				case 'r':	ViewObject(CCamera.e_viewPosition.Right); break;
				case 'f':	ViewObject(CCamera.e_viewPosition.Front); break;
				case 'b':	ViewObject(CCamera.e_viewPosition.Below); break;
				case 't':	ViewObject(CCamera.e_viewPosition.Top);	 break;
			}

		}
	}

	/// <summary>
	/// Functions for reading and rendering NIF/NPK files from NIFLIB by Maik Jurkait.
	/// This is a condensed version of the full API (I simply didn't need the whole thing
	/// and want to keep the editors footprint as small as possible, since a lot or RAM is 
	/// eaten by the xml databases themselves)... The full API can be found in the DOL-Tools 
	/// NIFLib CVS.
	/// </summary>
	public class NIFLIB
	{
		/// <summary>Initialize OpenGL and this Library</summary>
		/// <param name="wnd">Window handle which will be used for drawing</param>
		/// <returns>false on error</returns>
		[DllImport("NIFLib.dll",EntryPoint="?Create@NIF@@YA_NPAUHWND__@@@Z")]
		public static extern bool	Create(IntPtr wnd);

		/// <summary>Releases OpenGL and all memory.</summary>
		[DllImport("NIFLib.dll",EntryPoint="?Release@NIF@@YAXXZ")]
		public static extern void	Release();

		/// <summary>Call this if the associated window was resized.</summary>
		[DllImport("NIFLib.dll",EntryPoint="?OnWindowResize@NIF@@YAXXZ")]
		public static extern void	OnWindowResize();

		/// <summary>Loads a NIF/NPK file.</summary>
		/// <param name="path">Directory or MPK-Filename</param>
		/// <param name="filename">filename</param>
		/// <returns>A memory pointer to the loaded object, or 0 on error.</returns>
		/// <remarks>If you want load a NPK you have to provide the NPK filename as a directory 
		/// and the NIF-File name as filename (it's the same name but with .nif extention.</remarks>
		/// <example>
		/// IntPtr obj = CreateObject("c:/cam/nifs/", "bag.nif")
		/// if (obj == IntPtr.Zero) {Errorout();}
		/// </example>
		[DllImport("NIFLib.dll",EntryPoint="?CreateObject@NIF@@YAPAXPBD0@Z")]
		public static extern IntPtr	CreateObject(string path, string filename);

		/// <summary>Releases the object and frees the memory</summary>
		/// <example>
		///	ReleaseObject(obj);
		/// obj = new IntPtr(0);
		/// </example>
		[DllImport("NIFLib.dll",EntryPoint="?ReleaseObject@NIF@@YAXPAX@Z")]
		public static extern  void	ReleaseObject(IntPtr obj);

		/// <summary>Renders a object.</summary>
		/// <param name="obj">A object handle, recieved by "CreateObject"</param>
		/// <remarks>You have to call glClear and wglSwapbuffer. This function really just renders the object
		/// regardless of the state of OpenGL.
		/// </remarks>
		/// <example>
		/// GL.glClear(GL.GL_COLOR_BUFFER_BIT | GL.GL_DEPTH_BUFFER_BIT);	// Clear The Screen And The Depth Buffer
		/// //---- this can be skip if using a CCamera
		///	GL.glMatrixMode(GL.GL_MODELVIEW);								// Reset the matrix
		///	GL.glLoadIdentity();
		///	
		///	GL.glTranslatef(0.0f,0.0f,-200.0f );	// Move object
		///	GL.glRotatef(260.0f, 1.0f, 0.0f,0.0f ); // rotate object
		/// //---- if using a CCamera, just call: camera.setView();
		///
		///	NIFAPI.Render(m_current_object);	// RENDER
		///	
		/// GL.wglSwapBuffers(m_hdc);		// display it
		/// </example>
		[DllImport("NIFLib.dll",EntryPoint="?Render@NIF@@YAXPAX@Z")]
		public static extern  void	Render(IntPtr obj);

		/// <summary>Calculates the size and position of an object</summary>
		/// <param name="obj">a object handle</param>
		/// <returns>false if no object is loaded or has no visible structures</returns>
		/// <remarks>You get 2 points (x1,y1,z1 and x2,y2,z2). These points defines a box which surround the object.</remarks>
		[DllImport("NIFLib.dll",EntryPoint="?GetBoundingBox@NIF@@YA_NPAXAAM11111@Z")]
		public static extern  bool	GetBoundingBox(IntPtr obj, ref float x1, ref float y1, ref float z1, ref float x2, ref float y2, ref float z2 );

		/// <summary>Set the base dir of DAoC</summary>
		/// <param name="path">Directory where camelot.exe is</param>
		/// <remarks>It's needed to find textures and definition files (zones/items/..).
		/// You should call it once before any object is loaded.</remarks>
		/// <example>SetRootDirectory("c:/mythic/camelot/")</example>
		[DllImport("NIFLib.dll",EntryPoint="?SetRootDirectory@NIF@@YAXPBD@Z")]
		public static extern  void	SetRootDirectory(String path);

		/// <summary>Loads an item by ID</summary>
		[DllImport("NIFLib.dll",EntryPoint="?DAoC_LoadItem@NIF@@YAPAXH@Z")]
		public static extern  IntPtr	LoadItem(Int32 ID);

		/// <summary>Loads a Monster by ID</summary>
		[DllImport("NIFLib.dll",EntryPoint="?DAoC_LoadMonster@NIF@@YAPAXH@Z")]
		public static extern  IntPtr	LoadMonster(Int32 ID);

		/// <summary>Loads a Zone by ID</summary>
		[DllImport("NIFLib.dll",EntryPoint="?DAoC_LoadZone@NIF@@YAPAXH@Z")]
		public static extern  IntPtr	LoadZone(Int32 ID);
	}

	/// <summary>
	/// For full opengl support take a look at http://www.colinfahey.com/opengl/csharp.htm
	/// by cpfahey@earthlink.net (Colin P. Fahey)
	/// </summary>
	public class GL
	{
		public const string GL_DLL = "opengl32";
		public const uint  GL_COMPILE                           =       0x1300;
		public const uint  GL_COMPILE_AND_EXECUTE               =       0x1301;
		public const uint  GL_DEPTH_BUFFER_BIT                  =   0x00000100;
		public const uint  GL_COLOR_BUFFER_BIT                  =   0x00004000;
		public const uint  GL_MODELVIEW                         =       0x1700;
		public const uint  GL_PROJECTION                        =       0x1701;
		public const uint  GL_TEXTURE                           =       0x1702;
		public const uint  GL_MODELVIEW_MATRIX                  =       0x0ba6;
		[DllImport(GL_DLL)]	public static extern uint wglSwapBuffers		( IntPtr hdc );
		[DllImport(GL_DLL)] public static extern void glCallList            ( uint list ); 
		[DllImport(GL_DLL)] public static extern void glClear				( uint mask ); 
		[DllImport(GL_DLL)] public static extern void glDeleteLists         ( uint list, int range ); 
		[DllImport(GL_DLL)] public static extern void glEndList             ( ); 
		[DllImport(GL_DLL)] public static extern void glGetFloatv			( uint pname, float[] paramsx ); 
		[DllImport(GL_DLL)] public static extern uint glGenLists            ( int range ); 
		[DllImport(GL_DLL)] public static extern void glLoadIdentity        ( ); 
		[DllImport(GL_DLL)] public static extern void glLoadMatrixf         ( float[] m ); 
		[DllImport(GL_DLL)] public static extern void glMatrixMode          ( uint mode ); 
		[DllImport(GL_DLL)] public static extern void glNewList             ( uint list, uint mode ); 
		[DllImport(GL_DLL)] public static extern void glPopMatrix           ( );
		[DllImport(GL_DLL)] public static extern void glPushMatrix          ( ); 
		[DllImport(GL_DLL)] public static extern void glTranslatef          ( float x, float y, float z ); 
		[DllImport(GL_DLL)] public static extern void glRotatef             ( float angle, float x, float y, float z ); 
		[DllImport("user32")] public static extern IntPtr GetDC( IntPtr hwnd );
		[DllImport("user32")] public static extern int ReleaseDC( IntPtr hwnd, IntPtr hdc );
	}

	/// <summary>
	/// Camera class, to make moving the camera easier.
	/// </summary>
	public class CCamera
	{
		private float[] maf_matrix;
		public CCamera()
		{
			// init matrix
			maf_matrix = new float[16];
			Reset();
		}

		/// <summary>
		/// Reset camera to zero position and rotation
		/// </summary>
		public void Reset()
		{
			for (int t=0;t<16;t++) maf_matrix[t]=0.0f;
			maf_matrix[0] = 1.0f;
			maf_matrix[5] = 1.0f;
			maf_matrix[10] = -1.0f;
			maf_matrix[15] = 1.0f;
		}

		/// <summary>
		/// Call this before render something.
		/// </summary>
		public void setView()
		{
			GL.glMatrixMode(GL.GL_MODELVIEW);

			float[] viewmatrix =
						{//Remove the three '-' for non-inverted z-axis
							maf_matrix[0], maf_matrix[4], -maf_matrix[8], 0,
							maf_matrix[1], maf_matrix[5], -maf_matrix[9], 0,
							maf_matrix[2], maf_matrix[6], -maf_matrix[10], 0,

							-(maf_matrix[0]*maf_matrix[12] +
							maf_matrix[1]*maf_matrix[13] +
							maf_matrix[2]*maf_matrix[14]),

							-(maf_matrix[4]*maf_matrix[12] +
							maf_matrix[5]*maf_matrix[13] +
							maf_matrix[6]*maf_matrix[14]),

							//add a - like above for non-inverted z-axis
							(maf_matrix[8]*maf_matrix[12] +
							maf_matrix[9]*maf_matrix[13] +
							maf_matrix[10]*maf_matrix[14]), 1};
			GL.glLoadMatrixf(viewmatrix);
		}

		/// <summary>
		/// Move camera in local axis space.
		/// </summary>
		public void moveLocal(float x, float y, float z) 
		{
			float dx=x*maf_matrix[0] + y*maf_matrix[4] + z*maf_matrix[8];
			float dy=x*maf_matrix[1] + y*maf_matrix[5] + z*maf_matrix[9];
			float dz=x*maf_matrix[2] + y*maf_matrix[6] + z*maf_matrix[10];
			maf_matrix[12] += dx;
			maf_matrix[13] += dy;
			maf_matrix[14] += dz;
		}

		/// <summary>
		/// Move camera in world space.
		/// </summary>
		public void moveGlobal(float x, float y, float z) 
		{
			maf_matrix[12] += x;
			maf_matrix[13] += y;
			maf_matrix[14] += z;
		}

		/// <summary>
		/// Rotate camera in local(camera) space
		/// </summary>
		public void rotateLocal(float deg, float x, float y, float z) 
		{
			GL.glMatrixMode(GL.GL_MODELVIEW);
			GL.glPushMatrix();
			GL.glLoadMatrixf(maf_matrix);
			GL.glRotatef(deg, x,y,z);
			GL.glGetFloatv(GL.GL_MODELVIEW_MATRIX, maf_matrix);
			GL.glPopMatrix();
		}

		/// <summary>
		/// Rotate camera in world space
		/// </summary>
		public void rotateGlobal(float deg, float x, float y, float z) 
		{
			float dx=x*maf_matrix[0] + y*maf_matrix[1] + z*maf_matrix[2];
			float dy=x*maf_matrix[4] + y*maf_matrix[5] + z*maf_matrix[6];
			float dz=x*maf_matrix[8] + y*maf_matrix[9] + z*maf_matrix[10];
			GL.glMatrixMode(GL.GL_MODELVIEW);
			GL.glPushMatrix();
			GL.glLoadMatrixf(maf_matrix);
			GL.glRotatef(deg, dx,dy,dz);
			GL.glGetFloatv(GL.GL_MODELVIEW_MATRIX, maf_matrix);
			GL.glPopMatrix();
		}

		public enum e_viewPosition 
		{
				Left, Right, Top, Below, Front, Back
		};

		/// <summary>Calculates a camera position which will show the complet object.</summary>
		/// <param name="nif_obj">an object handle from NIFAPI</param>
		/// <param name="pos">which side will be viewed</param>
		/// <remarks>This is just the mathematical view. It's still possible that the object is shown from top if you choose "Front".
		/// The distance isn't always correct. 
		/// </remarks>
		public void	targetObject(IntPtr nif_obj, e_viewPosition pos)
		{
			if (nif_obj == IntPtr.Zero) return;

			float x1, y1, z1, x2, y2, z2;
			x1 = y1 = z1 = x2 = y2 = z2 = 0; // just to keep compiler happy
			bool res = NIFAPI.NIFLIB.GetBoundingBox(nif_obj, ref x1, ref y1, ref z1, ref x2, ref y2, ref z2);
			if (!res) return;

			Reset();
	
			moveGlobal((x1+ x2)/2.0f, (y1+ y2)/2.0f, (z1+ z2)/2.0f);

			float distancemin = 50; // atleast near-clipping plane must be reached
			float distanceX = ( x2 - x1 );
			float distanceY = ( y2 - y1 );
			float distanceZ = ( z2 - z1 );
			
			distanceX = (distanceX/2)/ (float) System.Math.Tan(45/2 * 3.14 / 180);
			distanceY = (distanceY/2)/ (float) System.Math.Tan(45/2 * 3.14 / 180);
			distanceZ = (distanceZ/2)/ (float) System.Math.Tan(45/2 * 3.14 / 180);
	
			distanceX = System.Math.Max(distancemin,distanceX);
			distanceY = System.Math.Max(distancemin,distanceY);
			distanceZ = System.Math.Max(distancemin,distanceZ);
	
			switch (pos)
			{
				case e_viewPosition.Top:
					moveLocal(0,0,-System.Math.Max(distanceX,distanceY));
					break;

				case e_viewPosition.Front:
					rotateLocal(-90.0f,0.0f,0.0f,1.0f);
					rotateLocal(-90.0f,1.0f,0.0f,0.0f);
					moveLocal(0,0,-System.Math.Max(distanceY,distanceZ));
					break;

				case e_viewPosition.Below:
					rotateLocal(180.0f,1.0f,0.0f,0.0f);
					moveLocal(0,0,-System.Math.Max(distanceX,distanceY));
					break;

				case e_viewPosition.Back:
					rotateLocal(90.0f,0.0f,0.0f,1.0f);
					rotateLocal(-90.0f,1.0f,0.0f,0.0f);
					moveLocal(0,0,-System.Math.Max(distanceY,distanceZ));
					break;

				case e_viewPosition.Right:
					rotateLocal(-90.0f,1.0f,0.0f,0.0f);
					moveLocal(0,0,-System.Math.Max(distanceX,distanceY));
					break;

				case e_viewPosition.Left:
					rotateLocal(90.0f,1.0f,0.0f,0.0f);
					rotateLocal(180.0f,0.0f,0.0f,1.0f);
					moveLocal(0,0,-System.Math.Max(distanceX,distanceY));
					break;
			}
		}
	}
}
