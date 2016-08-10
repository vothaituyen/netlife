
using System;
using System.Data;

namespace DALATV
{
	/// <summary>
	/// The base class for <see cref="StoredProcedures"/>.
	/// </summary>
	/// <remarks>
	/// Do not change this source code. Update the <see cref="StoredProcedures"/>
	/// class if you need to add or change some functionality.
	/// </remarks>
	public abstract class StoredProcedures_Base
	{
		
		
		
		private MainDB _db;

		/// <summary>
		/// Initializes a new instance of the <see cref="StoredProcedures_Base"/> 
		/// class with the specified <see cref="MainDB"/>.
		/// </summary>
		/// <param name="db">The <see cref="MainDB"/> object.</param>
		public StoredProcedures_Base(MainDB db)
		{
			_db = db;
		}

		/// <summary>
		/// Gets the database object.
		///	</summary>
		///	<value>The <see cref="MainDB"/> object.</value>
		public MainDB Database
		{
			get { return _db; }
		}

		/// <summary>
		/// Retrieves data from DB using the specified command and returns 
		/// results in a <see cref="System.Data.DataTable"/> object.
		/// </summary>
		/// <param name="command">The command to retrieve data.</param>
		/// <returns>A reference to the <see cref="System.Data.DataTable"/> object.</returns>
		protected DataTable CreateDataTable(IDbCommand command)
		{
			return _db.CreateDataTable(command);
		}

	}
}
