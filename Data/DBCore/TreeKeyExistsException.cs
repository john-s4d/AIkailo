using System;

namespace AIkailo.Data.DBCore
{
	public class TreeKeyExistsException : Exception
	{
		public TreeKeyExistsException (object key) : base ("Duplicate key: " + key.ToString())
		{
			
		}
	}

}

