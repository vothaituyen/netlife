
using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections;

namespace DALATV
{
	
	public class MainDB : MainDB_Base
	{
	    private string connectionString = System.Configuration.ConfigurationSettings.AppSettings["ConnectionString"].ToString();
		public MainDB()
		{
			
		}

        public MainDB(string connectionString)
        {
            this.connectionString = connectionString;
        }
		
		/// <summary>
		/// Execute Normal Query
		/// </summary>
		/// <param name="sql">Query String</param>
		/// <returns>DataTable contains query result</returns>
		public DataTable SelectQuery(string sql)
		{
			IDbCommand cmd=this.CreateCommand(sql,false);
			return this.CreateDataTable(cmd);
		}

		/// <summary>
		/// Execute Normal Nonquery
		/// </summary>
		/// <param name="sql">Query String</param>
		public void AnotherNonQuery(string sql)
		{
			IDbCommand cmd=this.CreateCommand(sql,false);
			cmd.ExecuteNonQuery();
		}

		/// <summary>
		/// Execute a stored procedure in SQL Server
		/// </summary>
		/// <param name="nameOfStored">SP Name</param>
		/// <param name="returnDataTable">True: Return Result in a DataTable, False: SP has no Result</param>
		/// <returns>A DataTable if returnDataTable = true, or null if returnDataTable = false</returns>
		public DataTable CallStoredProcedure(string nameOfStored,bool returnDataTable)
		{
			IDbCommand cmd=this.CreateCommand(nameOfStored,true);			
			if(returnDataTable)
			{
				return this.CreateDataTable(cmd);
			}
			else
			{
				cmd.ExecuteNonQuery();
				return null;
			}
		}

		/// <summary>
		/// Execute a stored procedure in SQL Server
		/// This SP require some parametters values
		/// </summary>
		/// <param name="listOfPara">An ArrayList contains Parametters instances</param>
		/// <param name="nameOfStored">SP Name</param>
		/// <param name="returnDataTable">True: Return Result in a DataTable, False: SP has no Result</param>
		/// <returns>A DataTable if returnDataTable = true, or null if returnDataTable = false</returns>
		public DataTable CallStoredProcedure(ArrayList listOfPara,string nameOfStored,bool returnDataTable)
		{
			IDbCommand cmd=this.CreateCommand(nameOfStored,true);			
			for(int i=0;i<listOfPara.Count;i++)
			{
				SqlParameter sp=(SqlParameter)listOfPara[i];
				AddParameter(cmd,sp.ParameterName,sp.DbType,sp.Value);
			}
			if(returnDataTable)
			{
				return this.CreateDataTable(cmd);
			}
			else
			{
				cmd.ExecuteNonQuery();
				return null;
			}						
		}


        public DataTable CallStoredProcedure(string nameOfStored, object[] objectsParamValue, string[] objectsParamName, bool returnDataTable)
        {
            IDbCommand cmd = this.CreateCommand(nameOfStored, true);
            for (int i = 0; i < objectsParamValue.Length; i++)
            {
                var sp = new SqlParameter(objectsParamName[i], objectsParamValue[i]);
                AddParameter(cmd, sp.ParameterName, sp.Value);
            }
            if (returnDataTable)
            {
                return this.CreateDataTable(cmd);
            }
            else
            {
                cmd.ExecuteNonQuery();
                return null;
            }
        }

         

		/// <summary>
		/// Creates a new connection to the database.
		/// </summary>
		/// <returns>An <see cref="System.Data.IDbConnection"/> object.</returns>
		protected override IDbConnection CreateConnection()
		{
            return new System.Data.SqlClient.SqlConnection(connectionString);
		}

        protected override IDbConnection CreateConnection(string otherConnectionString)
        {
            return new System.Data.SqlClient.SqlConnection(otherConnectionString);
        }

		/// <summary>
		/// Creates a DataTable object for the specified command.
		/// </summary>
		/// <param name="command">The <see cref="System.Data.IDbCommand"/> object.</param>
		/// <returns>A reference to the <see cref="System.Data.DataTable"/> object.</returns>
		protected internal DataTable CreateDataTable(IDbCommand command)
		{
			DataTable dataTable = new DataTable();
            new System.Data.SqlClient.SqlDataAdapter((System.Data.SqlClient.SqlCommand)command).Fill(dataTable);
			return dataTable;
		}

		/// <summary>
		/// Returns a SQL statement parameter name that is specific for the data provider.
		/// For example it returns ? for OleDb provider, or @paramName for MS SQL provider.
		/// </summary>
		/// <param name="paramName">The data provider neutral SQL parameter name.</param>
		/// <returns>The SQL statement parameter name.</returns>
		protected internal override string CreateSqlParameterName(string paramName)
		{
            return "@" + paramName;		
        }

		/// <summary>
		/// Creates a .Net data provider specific parameter name that is used to
		/// create a parameter object and add it to the parameter collection of
		/// <see cref="System.Data.IDbCommand"/>.
		/// </summary>
		/// <param name="baseParamName">The base name of the parameter.</param>
		/// <returns>The full data provider specific parameter name.</returns>
		protected override string CreateCollectionParameterName(string baseParamName)
		{
            return "@" + baseParamName;		
        }

	    public bool UpdateQuery(string sql, object[] parValues, string[] parNames)
        {
            IDbCommand cmd = this.CreateCommand(sql, false);
            cmd.CommandText = sql;
            for (int i = 0; i < parValues.Length; i++)
            {
                cmd.Parameters.Add(new SqlParameter(parNames.GetValue(i).ToString(), parValues.GetValue(i)));
            }
            return cmd.ExecuteNonQuery() == 1;
        }
	} // End of MainDB class


	sealed class EXC
	{ public static System.Collections.Stack Exception=new System.Collections.Stack(); } 
	public class Except
	{
		public static void SetException(System.Exception EX)
		{
			EXC.Exception.Push(EX);
		}
		public static string GetException(bool OnlyMessage)
		{
			string StrExc="";
			if (EXC.Exception.Count>0)
			{
				if (OnlyMessage)
				{
					Exception Ex = (Exception)EXC.Exception.Pop();
					StrExc=Ex.Message;
				}
				else 
				{
					StrExc=EXC.Exception.Pop().ToString();
				}
			}
			return StrExc;
		}
		public static string GetSourceEX()
		{
			string Src = "";
			if (EXC.Exception.Count>0)
			{
				Exception Ex = (Exception)EXC.Exception.Pop();
				Src=Ex.Source;
			}
			return Src;
		}
	}

} // End of namespace

