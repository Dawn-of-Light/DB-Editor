using System;
using System.Xml;
using System.Xml.Schema;
using System.Data;
using System.Drawing;
using System.Collections;
using System.IO;
using System.Windows.Forms;

namespace xmlDbEditor
{
	/// <summary>
	/// I contain xml data documents, and support methods used on them.
	/// </summary>
	public class DBContainer
	{
		private XmlNode _rNode=null;
		private XmlDataDocument _doc=null;
		private bool _DatabaseDirty=false;
		private bool _SchemaDirty=false;

		public void NewDocument()
		{
			_doc=new XmlDataDocument();
		}

		public void ClearDocument()
		{
			_doc = null;
		}

		public void AssignDocument(XmlDataDocument aDoc)
		{
			_doc = aDoc;
			_doc.DataSet.AcceptChanges(); // in case there are any lingering deleted rows...
		}

		public bool LoadSchema(string fnSchema)
		{
			NewDocument();	
			System.GC.Collect();
			if (File.Exists(fnSchema))
			{	
				_doc=new XmlDataDocument();
				_doc.DataSet.ReadXmlSchema(fnSchema);
				return true;
			}
			else
			{
				return false;
			}

		}

		public bool LoadDocument(string fnXml)
		{
			if (File.Exists(fnXml))
			{
				_doc.Load(fnXml);
				_rNode = doc.DocumentElement;				
				foreach (DataColumn dc in doc.DataSet.Tables[0].Columns)
				{
					bool isPK = false;
					foreach (DataColumn pk in doc.DataSet.Tables[0].PrimaryKey)
					{
						if (pk.Equals(dc))
						{
							isPK = true;
							break;
						}
					}					
					// the dtds dont specifically allow nulls, so I have to force it.
					if ((isPK) | (dc.Unique))
					{
						dc.AllowDBNull = false;
					}
					else
					{
						dc.AllowDBNull = true;
					}
				}
				return true;
			}
			else
			{
				return false;
			}
		}

		public bool SaveSchema(string fnSchema)
		{
			try
			{
				_doc.DataSet.WriteXmlSchema(fnSchema + ".commit");
				return true;
			}
			catch
			{
				return false;
			}
		}

		public bool SaveXml(string fnXml)
		{	
			try
			{
				_doc.DataSet.WriteXml(fnXml + ".commit",XmlWriteMode.IgnoreSchema);
				return true;
			}
			catch
			{
				return false;
			}
		}

		public bool CommitXml(string fnXml)
		{
			// look for committed db changes, and deal with them.
			// documents are saved with a .commit extension, as the actual
			// file is often locked at the time you are committing it.  This simply
			// backs up the origional file and replaces it with the committed 
			// file.
			try
			{
				if (File.Exists(fnXml + ".commit"))
				{
					if (File.Exists(fnXml))
					{
						File.Copy(fnXml,fnXml + ".bak",true);
					}
					File.Copy(fnXml + ".commit", fnXml, true);
					File.Delete(fnXml + ".commit");
				}
				return true;
			}
			catch
			{
				return false;
			}
		}

		public bool CommitSchema(string fnSchema)
		{
			// look for committed schema changes, and deal with them.
			// schemas are committed with a .commit extension, as the actual
			// file is locked at the time you are committing it.  This simply
			// backs up the origional file and replaces it with the committed 
			// file.
			try
			{
				if (File.Exists(fnSchema + ".commit")) 
				{
					if (File.Exists(fnSchema))
					{
						File.Copy(fnSchema,fnSchema + ".bak",true);
					}
					File.Copy(fnSchema + ".commit", fnSchema, true);					
					File.Delete(fnSchema + ".commit");
				}
				return true;
			}
			catch
			{
				return false;
			}
		}

		public int LongestField (DataSet ds, string TableName, string ColumnName, System.Drawing.Font font, Graphics g)
		{
			int maxlength = 0;
			int tot = ds.Tables[TableName].Rows.Count; 
			int intaux = 0;
			int intauh = 0;

			// Take width one blank space to add to the new width to the Column   
			int offset = Convert.ToInt32(Math.Ceiling(g.MeasureString(" ", font).Width));
			intauh = Convert.ToInt32(Math.Ceiling(g.MeasureString(ColumnName, font).Width));
			if (ds.Tables[TableName].Rows.Count > -1)
			{
				for (int i=0; i<tot; ++i)
				{
					// Get the width of Current Field String according to the Font
					intaux = Convert.ToInt32(Math.Ceiling(g.MeasureString(ds.Tables[TableName].Rows[i][ColumnName].ToString(), font).Width));
					if (intaux > maxlength)				
					{
						maxlength = intaux;
					}
				}
			}
			else
			{
				maxlength = 0;
			}
			if (intauh > maxlength)
			{
				maxlength = intauh;
			}
			if (maxlength < 20)
			{
				maxlength = 20;
			}
			return maxlength + offset;
		}

		public class DataGridNoActiveCellColumn : DataGridTextBoxColumn
		{
			// this is a custom dagrid tablestyle control that doesn't let you select
			// an actual item in the grid, instead selects the "row".
			private int SelectedRow = -1;
			protected override void Edit(System.Windows.Forms.CurrencyManager
				source, int rowNum, System.Drawing.Rectangle bounds, bool readOnly, string
				instantText, bool cellIsVisible)
			{
				//make sure the selectrow is valid before trying to unselect
				if(SelectedRow > -1 && SelectedRow < source.List.Count + 1)
					this.DataGridTableStyle.DataGrid.UnSelect(SelectedRow);
				SelectedRow = rowNum;
				this.DataGridTableStyle.DataGrid.Select(SelectedRow);
			}
		}

		public int SetTableStyles(string tablename, DataGrid dg, DataSet ds, out int TotalWidth)
		{
			int MaxWidth = 0;
			TotalWidth = 0;
			// set column styles 
			DataGridTableStyle ts = new DataGridTableStyle();
			ts.MappingName = tablename;
			dg.TableStyles.Clear();
			dg.TableStyles.Add(ts);
			int newwidth;
			Graphics g = dg.CreateGraphics();
			DataGridColumnStyle cs;
			int iter = 0;
			bool addedstyle = false;
			foreach (DataColumn dc in ds.Tables[tablename].Columns)
			{
				if (dg.TableStyles[tablename].GridColumnStyles[dc.ColumnName] == null)
				{
					cs = new DataGridNoActiveCellColumn();
					cs.MappingName = dc.ColumnName;	
					cs.HeaderText = dc.ColumnName;
					dg.TableStyles[tablename].GridColumnStyles.Add(cs);
					addedstyle = true;
				}
				if ((iter == 0) & (addedstyle))
				{
					newwidth = 0;  // hide first column in selectors, it's always the PK column - just a big GUID people don't care about.

				}
				else
				{
					newwidth = LongestField(ds,tablename,dc.ColumnName,dg.Font,g);
				}
				dg.TableStyles[tablename].GridColumnStyles[dc.ColumnName].Width = newwidth;
				if (newwidth > MaxWidth) MaxWidth = newwidth;
				TotalWidth += newwidth;
				iter ++;
			}
			TotalWidth += 52; // pad for datagrid row header
			return MaxWidth;
		}

		public string DataType(System.Type sDataType)
		{
			string s=sDataType.ToString();
			return s.Split(new Char[] {'.'})[1];
		}

		public void FixXMLDataSet(DataSet ds, bool FullSearch, string TableName)
		{
			// This is a safe work around for what seems to be a .net bug when binding
			// an XML schema and document to an XMLDataDocument. For some reason it
			// inserts a null row at the top of the dataset - as soon as it is created.
			// This doesn't happen in a stand alone dataset, if I manually create one 
			// and itterate through the XML nodes inserting them.  But that ends up 
			// using more memory (and code except for this fix code) and is less efficient 
			// then sticking with the xmldatadocument dataset.   Anways, this behavior is bad, mkay?
			// FullSearch=false will remove just that row.  Also, if you dont have constraints enabled,
			// and you goof around, you can insert them all over the place... which is why  FullSearch 
			// exists... It kill them so contraints can be re-enabled. Anyways this code won't break 
			// anything if this behavior is ever fixed. I could have just deleted the first row, but 
			// that wouldn't be safe if the root cause of the bug is fixed in the future.
			ds.AcceptChanges(); // make sure deleted rows are removed before we try this...
			if (TableName != "")
			{
				try
				{
					if (ds.Tables[TableName].Rows.Count > 0)
					{
						bool isNullColumn;
						Hashtable h = new Hashtable();
						int hIndex = 0;
						if (FullSearch)
						{
							foreach (DataRow row in ds.Tables[TableName].Rows)
							{
								isNullColumn = true;
								foreach (DataColumn column in ds.Tables[TableName].Columns)
								{
									if (row[column].ToString() != "")
									{
										isNullColumn = false;
									}
								}
								if (isNullColumn)
								{
									h.Add(hIndex,row);
									hIndex ++;
								}
							}
						}
						else
						{
							isNullColumn = true;
							foreach (DataColumn column in ds.Tables[TableName].Columns)
							{
								if (ds.Tables[TableName].Rows[0][column].ToString() != "")
								{
									isNullColumn = false;
								}
							}
							if (isNullColumn)
							{
								h.Add(hIndex,ds.Tables[TableName].Rows[0]);
								hIndex ++;
							}
						}
						IDictionaryEnumerator en = h.GetEnumerator();
						while (en.MoveNext())
						{
							((DataRow)en.Value).Delete();					
						}
						ds.AcceptChanges(); // make sure deleted rows are removed before we continue...
					}
				}
				catch
				{
					// this only happens if they leave an edit form with no rows... unusual... just suppress it.
				}
			}
		}

		public XmlDataDocument doc
		{
			get 
			{
				return _doc;
			}
		}

		public XmlNode rNode
		{
			get 
			{
				return _rNode;
			}
		}

		public bool DatabaseDirty
		{
			get 
			{
				return _DatabaseDirty;
			}
			set
			{
				_DatabaseDirty = value;
			}
		}

		public bool SchemaDirty
		{
			get 
			{
				return _SchemaDirty;
			}
			set
			{
				_SchemaDirty = value;
			}
		}
	}

	public class myLocal
	{
		public static XmlDocument LoadUIXml()
		{
			XmlDocument UIXml = new XmlDocument();
			if (File.Exists(System.Globalization.CultureInfo.CurrentCulture.ThreeLetterISOLanguageName+"_"+"ui.xml"))
			{
				UIXml.Load(System.Globalization.CultureInfo.CurrentCulture.ThreeLetterISOLanguageName+"_"+"ui.xml");
			}
			else
			{
				UIXml.Load("ui.xml");
			}
			return UIXml;
		}
	}
}
