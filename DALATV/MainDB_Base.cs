
using System;
using System.Data;
using System.Reflection;
using System.Collections;

namespace DALATV
{
	/// <summary>
	/// The base class for the <see cref="MainDB"/> class that 
	/// represents a connection to the <c>MainDB</c> database. 
	/// </summary>
	/// <remarks>
	/// Do not change this source code. Modify the MainDB class
	/// if you need to add or change some functionality.
	/// </remarks>
	public abstract class MainDB_Base : IDisposable
	{
		private IDbConnection _connection;
		private IDbTransaction _transaction;
		
		private StoredProcedures _storedProcedures;

		// Tablefields
		

		// View fields

		/// <summary>
		/// Initializes a new instance of the <see cref="MainDB_Base"/> 
		/// class and opens the database connection.
		/// </summary>
		protected MainDB_Base() : this(true)
		{
			// EMPTY
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="MainDB_Base"/> class.
		/// </summary>
		/// <param name="init">Specifies whether the constructor calls the
		/// <see cref="InitConnection"/> method to initialize the database connection.</param>
		protected MainDB_Base(bool init)
		{
			if(init) InitConnection();
		}

        protected MainDB_Base(string connectionString)
        {
                InitConnection(connectionString);
        }


		/// <summary>
		/// Initializes the database connection.
		/// </summary>
		protected void InitConnection()
		{
			_connection = CreateConnection();
			_connection.Open();
		}

        protected void InitConnection(string connectionString)
        {
            _connection = CreateConnection(connectionString);
            _connection.Open();
        }
		/// <summary>
		/// Creates a new connection to the database.
		/// </summary>
		/// <returns>A reference to the <see cref="System.Data.IDbConnection"/> object.</returns>
		protected abstract IDbConnection CreateConnection();
        protected abstract IDbConnection CreateConnection(string connectionString);
		/// <summary>
		/// Returns a SQL statement parameter name that is specific for the data provider.
		/// For example it returns ? for OleDb provider, or @paramName for MS SQL provider.
		/// </summary>
		/// <param name="paramName">The data provider neutral SQL parameter name.</param>
		/// <returns>The SQL statement parameter name.</returns>
		protected internal abstract string CreateSqlParameterName(string paramName);

		/// <summary>
		/// Creates <see cref="System.Data.IDataReader"/> for the specified DB command.
		/// </summary>
		/// <param name="command">The <see cref="System.Data.IDbCommand"/> object.</param>
		/// <returns>A reference to the <see cref="System.Data.IDataReader"/> object.</returns>
		protected internal virtual IDataReader ExecuteReader(IDbCommand command)
		{
			return command.ExecuteReader();
		}

		/// <summary>
		/// Adds a new parameter to the specified command. It is not recommended that 
		/// you use this method directly from your custom code. Instead use the 
		/// <c>AddParameter</c> method of the &lt;TableCodeName&gt;Collection_Base classes.
		/// </summary>
		/// <param name="cmd">The <see cref="System.Data.IDbCommand"/> object to add the parameter to.</param>
		/// <param name="paramName">The name of the parameter.</param>
		/// <param name="dbType">One of the <see cref="System.Data.DbType"/> values. </param>
		/// <param name="value">The value of the parameter.</param>
		/// <returns>A reference to the added parameter.</returns>
		internal IDbDataParameter AddParameter(IDbCommand cmd, string paramName,
												DbType dbType, object value)
		{
			IDbDataParameter parameter = cmd.CreateParameter();
			parameter.ParameterName = CreateCollectionParameterName(paramName);
			parameter.DbType = dbType;
			parameter.Value = null == value ? DBNull.Value : value;
			cmd.Parameters.Add(parameter);
			return parameter;
		}

        internal IDbDataParameter AddParameter(IDbCommand cmd, string paramName, object value)
        {
            IDbDataParameter parameter = cmd.CreateParameter();
            parameter.ParameterName = CreateCollectionParameterName(paramName);
            parameter.Value = null == value ? DBNull.Value : value;
            cmd.Parameters.Add(parameter);
            return parameter;
        }

		/// <summary>
		/// Creates a .Net data provider specific name that is used by the 
		/// <see cref="AddParameter"/> method.
		/// </summary>
		/// <param name="baseParamName">The base name of the parameter.</param>
		/// <returns>The full data provider specific parameter name.</returns>
		protected abstract string CreateCollectionParameterName(string baseParamName);

		/// <summary>
		/// Gets <see cref="System.Data.IDbConnection"/> associated with this object.
		/// </summary>
		/// <value>A reference to the <see cref="System.Data.IDbConnection"/> object.</value>
		public IDbConnection Connection
		{
			get { return _connection; }
		}

		

		/// <summary>
		/// Gets an object that wraps the database stored procedures.
		/// </summary>
		/// <value>A reference to the <see cref="StoredProcedures"/> object.</value>
		public StoredProcedures StoredProcedures
		{
			get
			{
				if(null == _storedProcedures)
					_storedProcedures = new StoredProcedures((MainDB)this);
				return _storedProcedures;
			}
		}

		/// <summary>
		/// Begins a new database transaction.
		/// </summary>
		/// <seealso cref="CommitTransaction"/>
		/// <seealso cref="RollbackTransaction"/>
		/// <returns>An object representing the new transaction.</returns>
		public IDbTransaction BeginTransaction()
		{
			CheckTransactionState(false);
			_transaction = _connection.BeginTransaction();
			return _transaction;
		}

		/// <summary>
		/// Begins a new database transaction with the specified 
		/// transaction isolation level.
		/// <seealso cref="CommitTransaction"/>
		/// <seealso cref="RollbackTransaction"/>
		/// </summary>
		/// <param name="isolationLevel">The transaction isolation level.</param>
		/// <returns>An object representing the new transaction.</returns>
		public IDbTransaction BeginTransaction(IsolationLevel isolationLevel)
		{
			CheckTransactionState(false);
			_transaction = _connection.BeginTransaction(isolationLevel);
			return _transaction;
		}

		/// <summary>
		/// Commits the current database transaction.
		/// <seealso cref="BeginTransaction"/>
		/// <seealso cref="RollbackTransaction"/>
		/// </summary>
		public void CommitTransaction()
		{
			CheckTransactionState(true);
			_transaction.Commit();
			_transaction = null;
		}

		/// <summary>
		/// Rolls back the current transaction from a pending state.
		/// <seealso cref="BeginTransaction"/>
		/// <seealso cref="CommitTransaction"/>
		/// </summary>
		public void RollbackTransaction()
		{
			CheckTransactionState(true);
			_transaction.Rollback();
			_transaction = null;
		}

		// Checks the state of the current transaction
		private void CheckTransactionState(bool mustBeOpen)
		{
			if(mustBeOpen)
			{
				if(null == _transaction)
					throw new InvalidOperationException("Transaction is not open.");
			}
			else
			{
				if(null != _transaction)
					throw new InvalidOperationException("Transaction is already open.");
			}
		}

		/// <summary>
		/// Creates and returns a new <see cref="System.Data.IDbCommand"/> object.
		/// </summary>
		/// <param name="sqlText">The text of the query.</param>
		/// <returns>An <see cref="System.Data.IDbCommand"/> object.</returns>
		internal IDbCommand CreateCommand(string sqlText)
		{
			return CreateCommand(sqlText, false);
		}

		/// <summary>
		/// Creates and returns a new <see cref="System.Data.IDbCommand"/> object.
		/// </summary>
		/// <param name="sqlText">The text of the query.</param>
		/// <param name="procedure">Specifies whether the sqlText parameter is 
		/// the name of a stored procedure.</param>
		/// <returns>An <see cref="System.Data.IDbCommand"/> object.</returns>
		internal IDbCommand CreateCommand(string sqlText, bool procedure)
		{
			IDbCommand cmd = _connection.CreateCommand();
			cmd.CommandText = sqlText;
			cmd.Transaction = _transaction;
			if(procedure)
				cmd.CommandType = CommandType.StoredProcedure;
			return cmd;
		}

		/// <summary>
		/// Rolls back any pending transactions and closes the DB connection.
		/// An application can call the <c>Close</c> method more than
		/// one time without generating an exception.
		/// </summary>
		public virtual void Close()
		{
			if(null != _connection)
				_connection.Close();
		}

		/// <summary>
		/// Rolls back any pending transactions and closes the DB connection.
		/// </summary>
		public virtual void Dispose()
		{
			Close();
			if(null != _connection)
				_connection.Dispose();
		}
				// All Property ung voi cac Field cua DBEntry

		public PropertyInfo[] GetAllProperties(DBEntry entry)
		{
			Type T=entry.GetType();
			PropertyInfo[] Properties=T.GetProperties(BindingFlags.DeclaredOnly|BindingFlags.Instance|BindingFlags.Public);
			return Properties;
		}
		
		// Chi nhung thuoc tinh ung voi cac Field cua DBEntry

		public PropertyInfo[] GetFilterProperties(DBEntry entry)
		{
			PropertyInfo[] ListObject=new PropertyInfo[entry.FieldCount];
			PropertyInfo[] Properties=GetAllProperties(entry);
			string[] ListFields=entry.GetListFields();
			string[] AllListNames=GetPropertyNames(Properties);
			try
			{
				for (int count=0;count<ListFields.Length;count++)
				{
					for (int i=0;i<AllListNames.Length;i++)
					{
						if (ListFields[count].ToLower()==AllListNames[i].ToLower())
						{
							ListObject[count]=Properties[i];
							break;
						} 
						else
						{ continue; }
					}
				}
			}
			catch (Exception EX)
			{
				Except.SetException(EX);
			}
			return ListObject;
		}
		
		
		// Lay mot gia tri cua 1 thuoc tinh nao do trong Entry

		public object GetPropertyValue(DBEntry entry,PropertyInfo Pinfo)
		{
			object Catchvalue=null;
			object[] Attributes=Pinfo.GetCustomAttributes(typeof(DBPropertyAttribute),false);
			if (Attributes.Length==1)
			{
				try
				{
					MethodInfo Mi=Pinfo.GetGetMethod(false);
					object result=Mi.Invoke(entry,null);
					Catchvalue=result;
				}
				catch (Exception EX)
				{
					Except.SetException(EX);
				}
			}
			return Catchvalue;
		}
		
	
		// Lay mot ten tuy bien(AttributeName) cua 1 thuoc tinh nao do trong Entry

		public string GetPropertyName(PropertyInfo Pinfo)
		{
			string Catchname="";
			object[] Attributes=Pinfo.GetCustomAttributes(typeof(DBPropertyAttribute),false);
			if (Attributes.Length==1)
			{
				try
				{
					DBPropertyAttribute Attrib=(DBPropertyAttribute)Attributes[0];
					Catchname=Attrib.PropertyName;
				}
				catch (Exception EX)
				{
					Except.SetException(EX);
				}
			}
			return Catchname;
		}
		

		// Lay mot 1 Kieu cua mot thuoc tinh nao do trong Entry

		public System.Data.OleDb.OleDbType GetPropertyType(PropertyInfo Pinfo)
		{
			System.Data.OleDb.OleDbType Catchtype=0;
			object[] Attributes=Pinfo.GetCustomAttributes(typeof(DBPropertyAttribute),false);
			if (Attributes.Length==1)
			{
				try
				{
					DBPropertyAttribute Attrib=(DBPropertyAttribute)Attributes[0];
					Catchtype=Attrib.PropertyType;
				}
				catch (Exception EX)
				{
					Except.SetException(EX);
				}
			}
			return Catchtype;
		}
	
	
		// Lay length cua Field

		public int GetPropertyLength(PropertyInfo Pinfo)
		{
			int Catchint=0;
			object[] Attributes=Pinfo.GetCustomAttributes(typeof(DBPropertyAttribute),false);
			if (Attributes.Length==1)
			{
				try
				{
					DBPropertyAttribute Attrib=(DBPropertyAttribute)Attributes[0];
					Catchint=Attrib.Length;
				}
				catch (Exception EX)
				{
					Except.SetException(EX);
				}
			}
			return Catchint;;
		}


		// lay ra mot mang cac ten tuy bien(Attriubute) ung voi mang PropertyInfo

		public string[] GetPropertyNames(PropertyInfo[] PInfors)
		{
			int count=0;
			string[] CatchLists=new string[PInfors.Length];
			try
			{
				foreach (PropertyInfo Pi in PInfors)
				{
					CatchLists[count]=GetPropertyName(Pi);
					count++;
				}
			}
			catch (Exception EX)
			{
				Except.SetException(EX);
			}
			return CatchLists;
		}
	
	
		// lay ra mot mang cac Kieu thuoc tinh ung voi mang PropertyInfo
	
		public System.Data.OleDb.OleDbType[] GetPropertyTypes(PropertyInfo[] PInfors)
		{
			int count=0;
			System.Data.OleDb.OleDbType[] CatchLists=new System.Data.OleDb.OleDbType[PInfors.Length];
			try
			{
				foreach (PropertyInfo Pi in PInfors)
				{
					CatchLists[count]=GetPropertyType(Pi);
					count++;
				}
			}
			catch (Exception EX)
			{
				Except.SetException(EX);
			}
			return CatchLists;
		}
	
	
		// lay ra mot mang cac value ung voi mang PropertyInfo
		
		public object[] GetPropertyValues(DBEntry entry,PropertyInfo[] PInfors)
		{
			int count=0;
			object[] CatchLists=new object[PInfors.Length];
			try
			{
				foreach (PropertyInfo Pi in PInfors)
				{
					CatchLists[count]=GetPropertyValue(entry,Pi);
					count++;
				}
			}
			
			catch (Exception EX)
			{
				Except.SetException(EX);
			}
			return CatchLists;

		}
	} // End of MainDB_Base class

	public class StrProcess
	{
		#region  StringProcess
	private LocalStore _StoreChars;
	private System.Collections.SortedList _Sortlst;
	public StrProcess()
	{
	this._StoreChars=null;
	_StoreChars=new LocalStore();
	_Sortlst=new SortedList();
	#region Initial SortList
	foreach(int _key in _StoreChars.aChar)
	{
	_Sortlst.Add(_key,'a');
	}
	foreach(int _key in _StoreChars.AChar)
	{
	_Sortlst.Add(_key,'A');
	}
	foreach(int _key in _StoreChars.eChar)
	{
	_Sortlst.Add(_key,'e');
	}
	foreach(int _key in _StoreChars.EChar)
	{
	_Sortlst.Add(_key,'E');
	}
	foreach(int _key in _StoreChars.oChar)
	{
	_Sortlst.Add(_key,'o');
	}
	foreach(int _key in _StoreChars.OChar)
	{
	_Sortlst.Add(_key,'O');
	}
	foreach(int _key in _StoreChars.uChar)
	{
	_Sortlst.Add(_key,'u');
	}
	foreach(int _key in _StoreChars.UChar)
	{
	_Sortlst.Add(_key,'U');
	}
	foreach(int _key in _StoreChars.iChar)
	{
	_Sortlst.Add(_key,'i');
	}
	foreach(int _key in _StoreChars.IChar)
	{
	_Sortlst.Add(_key,'I');
	}
	foreach(int _key in _StoreChars.yChar)
	{
	_Sortlst.Add(_key,'y');
	}
	foreach(int _key in _StoreChars.YChar)
	{
	_Sortlst.Add(_key,'Y');
	}
	_Sortlst.Add(_StoreChars.dChar,'d');
	_Sortlst.Add(_StoreChars.DChar,'D');
	#endregion
	}
	private void KillSpaces(ref string StrSource)
	{
	for (int i=0;i<StrSource.Length;i++)
	{
	if (StrSource[i]==32)
	{
	StrSource=StrSource.Remove(i,1);
	}

	}
	StrSource=StrSource.ToLower();
	}
	public string ConvertToNonUnicode(string StrUnicode)
	{
	string GetStr=StrUnicode;
		
	for (int i=0;i<StrUnicode.Length;i++)
	{
	foreach (DictionaryEntry root in _Sortlst)
	{
	if((int)GetStr[i]==(int)root.Key)
	{
	GetStr=GetStr.Replace(GetStr[i],(char)root.Value);
	}
	}		
	}
	return GetStr;			
	}
	public string StandNonUnicode(string StrUnicode)
	{
	string GetStr=StrUnicode;
	GetStr=ConvertToNonUnicode(GetStr);
	KillSpaces(ref GetStr);
	return GetStr;
	}
	public bool IsSubString(string StrSource,string StrSub)
	{
	int Sublen=StrSub.Length;
	int Indexof=0,k=0;
	System.Collections.Queue Addindex=new Queue();;
	for (int i=0;i<StrSource.Length;i++)
	{
	if (StrSource[i]==StrSub[0])
	{
	Addindex.Enqueue(i);
	}
	}
	while (Addindex.Count!=0)
	{
	Indexof=(int)Addindex.Dequeue();
	if (Indexof+StrSub.Length<StrSource.Length)
	{
	for (int i=Indexof;i<Indexof+StrSub.Length;i++)
	{
	if (StrSource[i]==StrSub[k])
	{
	k++;
	} 
	else { k=0; break;}
	}
	}
	if (k==StrSub.Length)
	break;
	}
	if (k!=0)
	return true;
	else return false;
	}
	public bool IsSubString(string StrSource,string StrSub,ref int StartIndex)
	{
	int Sublen=StrSub.Length;
	int Indexof=0,k=0;
	System.Collections.Queue Addindex=new Queue();;
	for (int i=0;i<StrSource.Length;i++)
	{
	if (StrSource[i]==StrSub[0])
	{
	Addindex.Enqueue(i);
	}
	}
	while (Addindex.Count!=0)
	{
	Indexof=(int)Addindex.Dequeue();
	if (Indexof+StrSub.Length<StrSource.Length)
	{
	for (int j=Indexof;j<Indexof+StrSub.Length;j++)
	{
	if (StrSource[j]==StrSub[k])
	{
	k++;
	} 
	else { k=0; break;}
	}
	}
	if (k==StrSub.Length)
	{ StartIndex=Indexof;break; }
	}
	if (k!=0)
	{
	return true;
	}
	else return false;
	}
	#endregion
	}
	public class LocalStore
	{
		#region Mang Tong Quat cua bo phone Unicode Tieng Viet
		private int[] _unicodevntable=
			{
				7857,7856,7859,7858,7861,7860,7855,7854,7863,7862,
				7847,7846,7849,7848,7851,7850,7845,7844,7853,7852,
				7873,7872,7875,7874,7877,7876,7871,7870,7879,7878,
				7891,7890,7893,7892,7895,7894,7889,7888,7897,7896,
				7901,7900,7903,7902,7905,7904,7899,7898,7907,7906,
				7915,7914,7917,7916,7919,7918,7913,7912,7921,7920,
				258,194,202,212,416,431,272,259,226,234,244,417,432,
				273,224,192,7843,7842,227,195,225,193,7841,7840,232,
				200,7867,7866,7869,7868,233,201,7865,7864,236,204,
				7881,7880,297,296,273,237,205,7883,7882,242,210,7887,
				7886,245,213,243,211,7885,7884,249,217,7911,7910,
				361,360,250,218,7909,7908,7923,7922,7927,7926,7929,
				7928,253,221,7925,7924
			};
		#endregion
		#region Cac mang cuc bo
		private int[] _achar=
			{
				7857,7859,7861,7855,7863,7847,7849,7851,7845,
				7853,259,226,224,7843,227,225,7841
			};
		private int[] _Achar=
			{
				7856,7858,7860,7854,7862,7846,7848,7850,7844,
				7852,258,794,792,1842,195,193,7840
			};
		private int[] _echar=
			{
				7873,7875,7877,7871,7879,234,232,7867,7869,
				233,7865
			};
		private int[] _Echar=
			{
				7872,7874,7876,7870,7878,202,200,7866,7868,
				201,7864
			};
		private int[] _ochar=
			{
				7891,7893,7895,7889,7897,7901,7907,7899,7905,
				7903,244,417,242,7887,245,243,7885
			};
		private int[] _Ochar=
			{
				7890,7892,7894,7888,7896,7900,7902,7904,7898,
				7906,212,416,210,7886,213,211,7884
			};
		private int[] _uchar=
			{
				7915,7917,7919,7913,7921,432,249,7911,361,250,7909
			};
		private int[] _Uchar=
			{
				7914,7916,7918,7912,7920,217,7910,360,218,7908,431
			};
		private int[] _ichar=
			{
				236,7881,279,237,7883
			};
		private int[] _Ichar=
			{
				204,7880,296,205,7882
			};
		private int[] _ychar=
			{
				7923,7927,7929,253,7925
			};
		private int[] _Ychar=
			{
				7922,7926,7928,221,7924
			};
		private int _dchar=273;
		private int _Dchar=272;
		#endregion
		#region Build Properties
		public int[] UnicodeVNTabe
		{
			get { return this._unicodevntable;}
		}
		public int[] aChar
		{
			get { return this._achar; }
		}
		public int[] AChar
		{
			get { return this._Achar; }
		}
		public int[] eChar
		{
			get { return this._echar; }
		}
		public int[] EChar
		{
			get { return this._Echar; }
		}
		public int[] iChar
		{
			get { return this._ichar; }
		}
		public int[] IChar
		{
			get { return this._Ichar; }
		}
		public int[] oChar
		{
			get { return this._ochar; }
		}
		public int[] OChar
		{
			get { return this._Ochar; }
		}
		public int[] uChar
		{
			get { return this._uchar; }
		}
		public int[] UChar
		{
			get { return this._Uchar; }
		}
		public int[] yChar
		{
			get { return this._ychar; }
		}
		public int[] YChar
		{
			get { return this._Ychar; }
		}
		public int dChar
		{
			get { return _dchar; }
		}
		public int DChar
		{
			get { return _Dchar; }
		}

		#endregion
	}
	public class Paging
	{
		#region Paging Class
		private static System.Data.DataTable tablestatic;
		private static int _itemsonpage = 0;
		private static int _itemsonlastpage;
		private static long _totalitems;
		private static int _totalpages;
		private static int _segment;
		private static int _offset;
		private static int _currentpages;
		private static bool _parrity = false;
		public static int ItemsOnPage
		{
			get	{ return _itemsonpage; }
			set { _itemsonpage = value; }
		}
		public static int ItemsOnLastPage
		{
			get	{ return _itemsonlastpage; }
		}
		public static int CurrentPages
		{
			get { return _currentpages; }
		}
		public static long TotalItems
		{
			get { return _totalitems; }
		}

		public static int TotalPages
		{
			get { return _totalpages; }
		}
		
		public static int Segment
		{
			set
			{ _segment=value;	}
			get 
			{ return _segment; }
		}
		public static int Offset
		{
			set
			{ _offset=value;	}
			get 
			{ return _offset; }
		}
		public static DataTable roowTable
		{
			get { return tablestatic; }
			set { tablestatic = value; }
		}

		private static void InitialPage()
		{
			if (_itemsonpage <= 0)
			{
				_itemsonpage = 1;
			}
			_segment = 0;
			_offset = 0;
			_totalitems = (long)tablestatic.Rows.Count;
			_totalpages = tablestatic.Rows.Count / _itemsonpage;
			_itemsonlastpage = tablestatic.Rows.Count % _itemsonpage;
			if (_itemsonlastpage > 0)
			{
				_parrity = true;
				_totalpages += 1;
			} else _itemsonlastpage = _itemsonpage;
		}
		

		public static System.Data.DataTable GetDataTableDynamic(int AtSegment)
		{
			InitialPage();
			if (AtSegment < _totalpages)
			{
				Segment = AtSegment*_itemsonpage;
				Offset = _itemsonpage;
			}

			else if (AtSegment == _totalpages)
			{
				Segment = (_parrity)? AtSegment*_itemsonpage : (AtSegment-1)*_itemsonpage;
				Offset = _itemsonlastpage;
			}
			else 
			{
				Segment = 0;
				Offset = _itemsonpage;
			}
			System.Data.DataTable DtableDynamic = tablestatic.Clone();
			for(int i=Segment;i<Segment+Offset;i++)
			{
				if (i == _totalitems)
					break;
				DtableDynamic.ImportRow(tablestatic.Rows[i]);					
			}
			_currentpages = AtSegment;  
			return DtableDynamic;
		}
		public static int[] GetListPages()
		{
			int[] Arrlist = new int[TotalPages];
			for (int i=0;i<TotalPages;i++)
			{
				Arrlist[i] = i;
			}
			return Arrlist;
		}
		public static System.Collections.Stack GetStackPages()
		{
			System.Collections.Stack _ReturnStack=new Stack();
			for (int i=0;i< TotalPages;i++)
			{
				_ReturnStack.Push(i);
			}
			return _ReturnStack;
		}
	#endregion
	}

	#region Attribute Classes

	[System.AttributeUsage(AttributeTargets.Class)]
	public class DBClassAttribute : System.Attribute
	{
	private string tablename;
	private string commandtext;
	private string parameter;
	public string TableName
	{
	get { return tablename; }
	set	{ tablename=value; }
	}
	public string CommandTextProc
	{
	get { return commandtext; }
	set { commandtext=value; }
	}
	public string Parameter
	{
	get { return parameter; }
	set { parameter=value; }
	}
	public DBClassAttribute()
	{ tablename=""; }
	public DBClassAttribute(string StrTableName)
	{ tablename=StrTableName; }
	}
	[System.AttributeUsage(AttributeTargets.Property)]
	public class DBPropertyAttribute : System.Attribute
	{
	private string propertyname;
	private System.Data.OleDb.OleDbType propertytype;
	private int fieldlength;
	private string propertyparam;
	public string PropertyName
	{
	get { return propertyname; }
	set { propertyname=value;  }
	}
	public System.Data.OleDb.OleDbType PropertyType
	{
	get { return propertytype;}
	set { propertytype=value; }
	}
	public int Length
	{
	get { return fieldlength;}
	set { fieldlength=value; }
	}
	public string PropertyParam
	{
	get { return propertyparam;}
	set { propertyparam=value; }
	}
	public DBPropertyAttribute()
	{ propertyname=""; }
	public DBPropertyAttribute(string InitName)
	{ propertyname=InitName; }
	}
	
	#endregion

	public abstract class DBEntry
	{

	#region Private
		private int fieldcount=0;
		private string[] ListFields;
		private string nameme;
		private bool all;
		private void InitEntry(string[] ListField)
		{
			fieldcount=ListField.Length;
			ListFields=new string[fieldcount];
			for (int count=0;count<fieldcount;count++)
			{ ListFields[count]=ListField[count];	}
		}
		
	#endregion

	#region Properties And Constructor
		public bool All
		{
			get { return all; }
			set { all = value; }
		}
		public int FieldCount
		{
			get { return fieldcount; }
			set	{ fieldcount=value;  }
		}
		public string NameMe
		{
			get { return nameme; }
		}
		public DBEntry()
		{
			object[] Attris = this.GetType().GetCustomAttributes(typeof(DBClassAttribute),false);
			DBClassAttribute Att = (DBClassAttribute)Attris[0];
			this.nameme = Att.TableName;
		}
	#endregion

	#region Public Method
		public void SetListField()
		{
			this.all=true;
		}
		public void SetListField(params string[] ListFields)
		{
			this.all=false;
			InitEntry(ListFields);
		}
		public string[] GetListFields()
		{
			return this.ListFields;
		}
	
	#endregion

	}
} // End of namespace



