using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Json
{
	public sealed class JsonArray : IList<object>
	{
		IList<object> _inner;
		JsonArray(IList<object> inner, bool dummy = false)
		{
			_inner = inner;
		}
		public JsonArray(IEnumerable<object> items) { _inner = new List<object>(items); }
		public JsonArray(int capacity) { _inner = new List<object>(capacity); }
		public JsonArray() { _inner = new List<object>(); }
		public IList<object> BaseList { get { return _inner; } }
		public object this[int index] { get => _inner[index]; set => _inner[index] = value; }

		public int Count => _inner.Count;

		public bool IsReadOnly => _inner.IsReadOnly;

		public void Add(object item)
			=>_inner.Add(item);
		
		public void AddRange(IEnumerable<object> items)
		{
			foreach (var item in items)
				_inner.Add(item);
		}

		public void Clear()
			=>_inner.Clear();
		

		public bool Contains(object item)
			=>_inner.Contains(item);
		

		public void CopyTo(object[] array, int arrayIndex)
			=>_inner.CopyTo(array, arrayIndex);
		

		public IEnumerator<object> GetEnumerator()
			=> _inner.GetEnumerator();
		
		public int IndexOf(object item)
			=> _inner.IndexOf(item);
		
		public int LastIndexOf(object item)
		{
			for (var i = _inner.Count - 1; -1 < i; --i)
				if (Equals(item, _inner[i]))
					return i;
			return -1;
		}

		public void Insert(int index, object item)
			=>_inner.Insert(index, item);
		
		public bool Remove(object item)
			=>_inner.Remove(item);
		

		public void RemoveAt(int index)
			=>_inner.RemoveAt(index);
		

		IEnumerator IEnumerable.GetEnumerator()
			=>_inner.GetEnumerator();
		
		public T[] ToArray<T>()
			=>ToArray<T>(this);
		
		public T[] ToArray<T>(Func<object, T> createItem)
			=>ToArray(this,createItem);
		
		public KeyValuePair<TKey, TValue>[] ToArray<TKey, TValue>(string keyField, string valueField)
			=> ToArray<TKey, TValue>(keyField, valueField);
		public static T[] ToArray<T>(IList<object> list, Func<object, T> createItem)
		{
			if (null != list)
			{ 
				var result = new T[list.Count];
				for (var i = 0; i < result.Length; ++i)
					result[i] = createItem(list[i]);
				return result;
			}
			return null;
		}
		public static JsonArray FromArray(IEnumerable array)
		{
			var result = new JsonArray();
			foreach(var obj in array)
			{
				if ((null == obj) ||
					obj is bool ||
					obj is string ||
					obj is int ||
					obj is double ||
					obj is long ||
					obj is System.Numerics.BigInteger)
				{
					result.Add(obj);
				}
				else
					throw new NotSupportedException("The array contains one or more items of an unsupported type.");
			}
			return result;
		}
		
		public static T[] ToArray<T>(IList<object> list)
		{
			if (null == list)
				return null;
			var result = new T[list.Count];
			for (var i = 0; i < result.Length; ++i)
				result[i] = (T)list[i];
			return result;
		}
		public static KeyValuePair<TKey, TValue>[] ToArray<TKey,TValue>(IList<object> list, string keyField,string valueField)
		{
			if (null == list)
				return null;
			var result = new KeyValuePair<TKey, TValue>[list.Count];
			for(var i = 0;i<result.Length;++i)
			{
				var d = (IDictionary<string, object>)list[i];
				if (null != d)
					result[i] = new KeyValuePair<TKey, TValue>((TKey)d[keyField], (TValue)d[valueField]);
			}
			return result;
		}
		public static JsonArray Adapt(IList<object> list)
		{
			if (null == list) return null;
			var result = list as JsonArray;
			if (null == result)
				result = new JsonArray(list,false);
			return result;
		}
		public override string ToString()
		{
			var sw = new StringWriter();
			JsonObject.WriteTo(this,sw, "   ");
			sw.Flush();
			return sw.ToString();
		}
	}
}
