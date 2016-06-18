#region Usings
using System;
using System.Data;
using System.Data.Common;
using System.Collections;
using SharpHsql;
using System.Collections.Generic;
#endregion

#region License
/*
 * SharpHsqlParameterCollection.cs
 *
 * Copyright (c) 2004, Andres G Vettori
 * All rights reserved.
 *
 * Redistribution and use in source and binary forms, with or without
 * modification, are permitted provided that the following conditions are met:
 *
 * Redistributions of source code must retain the above copyright notice, this
 * list of conditions and the following disclaimer.
 *
 * Redistributions in binary form must reproduce the above copyright notice,
 * this list of conditions and the following disclaimer in the documentation
 * and/or other materials provided with the distribution.
 *
 * Neither the name of the HSQL Development Group nor the names of its
 * contributors may be used to endorse or promote products derived from this
 * software without specific prior written permission.
 *
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS"
 * AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE
 * IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE
 * ARE DISCLAIMED. IN NO EVENT SHALL THE REGENTS OR CONTRIBUTORS BE LIABLE FOR
 * ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
 * (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
 * LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
 * ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
 * (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
 * SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 *
 * This package is based on HypersonicSQL, originally developed by Thomas Mueller.
 *
 * C# SharpHsql ADO.NET Provider by Andrés G Vettori.
 * http://workspaces.gotdotnet.com/sharphsql
 */
#endregion

namespace System.Data.Hsql
{
	/// <summary>
	/// Parameter Collection class for Hsql ADO.NET data provider.
	/// <seealso cref="SharpHsqlParameter"/>
	/// <seealso cref="SharpHsqlCommand"/>
	/// </summary>
	/// <remarks>Not serializable on Compact Framework 1.0</remarks>
	[Serializable]
	public sealed class SharpHsqlParameterCollection : DbParameterCollection
	{
		private List<DbParameter> innerList = new List<DbParameter>();
		private static object syncRoot = new object();

		#region Constructors

		/// <summary>
		/// Internal Constructor.
		/// </summary>
		/// <param name="command"></param>
		internal SharpHsqlParameterCollection( SharpHsqlCommand command ) : base()
		{
			_command = command;
			_names = new Hashtable();
		}

		#endregion

		#region IDataParameterCollection Members

		protected override DbParameter GetParameter(string parameterName)
		{
			int index = GetParameterIndex(parameterName);

			if (index > -1)
				return this.innerList[index];
			else
				return null;
		}

		protected override DbParameter GetParameter(int index)
		{
			return this.innerList[index];
		}



		protected override void SetParameter(string parameterName, DbParameter value)
		{
			int index = GetParameterIndex(parameterName);

			if (index > -1)
				this.innerList[index] = value;
			else
			{
				this.innerList.Add(value);
				index = this.innerList.IndexOf(value);
				_names[parameterName] = index;
			}
		}

		protected override void SetParameter(int index, DbParameter value)
		{
			this.innerList[index] = value;
		}

		/// <summary>
		/// Remove the parameter from the collection.
		/// </summary>
		/// <param name="parameterName">The parameter name to remove.</param>
		public override void RemoveAt(string parameterName)
		{
			int index = GetParameterIndex( parameterName );

			if( index > -1 )
				this.innerList.RemoveAt(index);

			RebuildNames();
		}

		public override void RemoveAt(int index)
		{
			this.innerList.RemoveAt(index);
		}

		/// <summary>
		/// Look for a parameter in the collection.
		/// </summary>
		/// <param name="parameterName">The parameter name to remove.</param>
		/// <returns>True if the parameter is found.</returns>
		public override bool Contains(string parameterName)
		{
			int index = GetParameterIndex( parameterName );

			if( index > -1 )
				return true;
			else
				return false;
		}

		public override bool Contains(object value)
		{
			return this.innerList.Contains((DbParameter)value);
		}

		/// <summary>
		/// Obtains the parameter index in the collection.
		/// </summary>
		/// <param name="parameterName">The parameter name to found.</param>
		/// <returns>The index of the parameter.</returns>
		public override int IndexOf(string parameterName)
		{
			return GetParameterIndex( parameterName );
		}

		#endregion

		#region IList Members

		/// <summary>
		/// Get the updatability of the collection.
		/// </summary>
		public override bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		/// <summary>
		/// Inserts a new parameter at a specific location.
		/// </summary>
		/// <param name="index"></param>
		/// <param name="value"></param>
		public override void Insert(int index, object value)
		{
			innerList.Insert(index, (DbParameter)value);
			RebuildNames();
		}

		/// <summary>
		/// Remove the passed parameter from the collection.
		/// </summary>
		/// <param name="value"></param>
		public override void Remove(object value)
		{
			innerList.Remove((DbParameter)value);
			_names.Remove(((SharpHsqlParameter)value).ParameterName);
		}

		/// <summary>
		/// Eliminates all parameters from the collection.
		/// </summary>
		public override void Clear()
		{
			innerList.Clear();
			_names.Clear();
		}

		/// <summary>
		/// Get the index of the passed parameter in the collection.
		/// </summary>
		/// <param name="value">The paramerter object to find.</param>
		/// <returns>The index of the parameter.</returns>
		public override int IndexOf(object value)
		{
			return innerList.IndexOf((DbParameter)value);
		}

		/// <summary>
		/// Adds a new parameter to the collection.
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public override int Add(object value)
		{
			var p = (DbParameter)value;
			innerList.Add(p);
			int index = innerList.IndexOf(p);
			_names[((SharpHsqlParameter)value).ParameterName] = index;
			return index;
		}

		public override void AddRange(Array values)
		{
			foreach (var item in values)
			{
				this.Add(item);
			}
		}

		/// <summary>
		/// Returns the grow policy for this collection.
		/// </summary>
		public override bool IsFixedSize
		{
			get
			{
				return false;
			}
		}

		#endregion

		#region ICollection Members

		/// <summary>
		/// Returns the synchronization status of this collection.
		/// </summary>
		public override bool IsSynchronized
		{
			get
			{
				return false;
			}
		}

		/// <summary>
		/// Returns the parameter count for this collection.
		/// </summary>
		public override int Count
		{
			get
			{
				return innerList.Count;
			}
		}

		/// <summary>
		/// Copies the content of this collection to an array.
		/// </summary>
		/// <param name="array"></param>
		/// <param name="index"></param>
		public override void CopyTo(Array array, int index)
		{
			innerList.CopyTo((DbParameter[])array, index);
		}

		/// <summary>
		/// Synchronization object.
		/// </summary>
		public override object SyncRoot
		{
			get
			{
				return syncRoot;
			}
		}

		#endregion

		#region IEnumerable Members

		/// <summary>
		/// Gets the enumerator for this collection.
		/// </summary>
		/// <returns></returns>
		public override System.Collections.IEnumerator GetEnumerator()
		{
			return innerList.GetEnumerator();
		}

		#endregion

		#region Private Methods

		private int GetParameterIndex( string name )
		{
			object index = _names[name];
			if( index == null )
				return -1;
			else
				return (int)index;
		}

		private void RebuildNames()
		{
			lock( this )
			{
				_names.Clear();

				for( int i=0;i<innerList.Count;i++)
				{
					SharpHsqlParameter p = innerList[i] as SharpHsqlParameter;
					if( p != null )
					{
						_names[p.ParameterName] = i;
					}
				}
			}
		}

		#endregion

		#region Private Vars

		private SharpHsqlCommand _command = null;
		private Hashtable _names = null;

		#endregion

	}
}
