using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Text;
namespace Json
{
	using JsonField = KeyValuePair<string, object>;

	/// <summary>
	/// An in memory representation of a JSON {object}
	/// </summary>
	/// <remarks>
	/// This is just a <see cref="System.Collections.Generic.IDictionary{string, object}"/> with a dynamic call sink that exposes the keys as properties, and some utility methods for manipulating JSON data. This class is not required. You can use another dictionary instead if need be.
	/// </remarks>
	public sealed class JsonObject : DynamicObject, IDictionary<string, object>, IEquatable<object>
	{
		IDictionary<string, object> _inner;

		public JsonObject(IDictionary<string, object> inner, bool dummy = false)
		{
			_inner = inner;
		}
		public JsonObject(IDictionary<string, object> @object)
		{
			_inner = new Dictionary<string, object>(@object);
		}
		public JsonObject(int capacity)
		{
			_inner = new Dictionary<string, object>(capacity);
		}
		public JsonObject()
		{
			_inner = new Dictionary<string, object>();
		}
		public IDictionary<string,object> BaseDictionary { get {return _inner;} }
		private class _ReferenceEqualityComparer : IEqualityComparer<object>
		{
			public static readonly _ReferenceEqualityComparer Default = new _ReferenceEqualityComparer();
			bool IEqualityComparer<object>.Equals(object x,object y)
			{
				return ReferenceEquals(x, y);
			}
			int IEqualityComparer<object>.GetHashCode(object o)
			{
				if (null == o) return 0;
				return o.GetHashCode();
			}
		}
		public override bool Equals(object obj)
		{
			return Equals(this, obj);
		}
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}
		
		public new static bool Equals(object lhs,object rhs)
		{
			if (ReferenceEquals(lhs, rhs))
				return true;
			else if (ReferenceEquals(null, lhs) || ReferenceEquals(null, rhs))
				return false;
			var d = lhs as IDictionary<string, object>;
			if(null!=d)
			{
				var d2 = rhs as IDictionary<string, object>;
				if (null == d2)
					return false;
				if (d.Count != d2.Count)
					return false;
				foreach(var field in d)
				{
					object o;
					if (!d2.TryGetValue(field.Key, out o))
						return false;
					if (!Equals(field.Value, o))
						return false;
				}
				return true;
			} else
			{
				var l = lhs as IList<object>;
				if (null != l)
				{
					var l2 = rhs as IList<object>;
					if (null == l2)
						return false;
					var ic = l.Count;
					if (l2.Count!=ic)
						return false;
					for (var i = 0; i < ic; ++i)
						if (!Equals(l[i], l2[i]))
							return false;
					return true;
				}
				
			}
			return object.Equals(lhs, rhs);
		}
		public static int GetHashCode(IDictionary<string,object> json)
		{
			var result = 0;
			if (null != json)
			{
				foreach (var field in json)
				{
					result ^= GetHashCode(field.Key);
					result ^= GetHashCode(field.Value);
				}
			}
			return result;
		}
		public static int GetHashCode(IList<object> json)
		{
			var result = 0;
			if (null != json)
				for(int ic=json.Count,i=0;i<ic;++i)
					result ^= GetHashCode(json[i]);
			return result;
		}
		public static int GetHashCode(object json)
		{
			var d = json as IDictionary<string, object>;
			if (null != d)
				return GetHashCode(d);
			else
			{
				var l = json as IList<object>;
				if (null != l)
					return GetHashCode(l);
			}
			if (null != json)
				return json.GetHashCode();
			return 0;
		}
		public static JsonObject Adapt(IDictionary<string,object> dictionary)
		{
			if (null == dictionary) return null;
			var result = dictionary as JsonObject;
			if (ReferenceEquals(null , result))
				result = new JsonObject(dictionary, false);
			return result;
		}
		public static JsonObject Parse(string text,Func<IDictionary<string,object>> objectCreator=null,Func<IList<object>> arrayCreator=null)
		{
			return Adapt(JsonTextReader.Create(text).ParseSubtree(objectCreator, arrayCreator) as IDictionary<string,object>);
		}
		public static JsonObject ReadFrom(TextReader reader)
		{
			var r = JsonTextReader.CreateFrom(reader);
			return r.ParseSubtree() as JsonObject;
		}
		public static JsonObject LoadFrom(string filename)
		{
			using (var r = JsonTextReader.CreateFrom(filename))
				return r.ParseSubtree() as JsonObject;
		}
		public static JsonObject LoadFromUrl(string url)
		{
			using (var r = JsonTextReader.CreateFromUrl(url))
				return r.ParseSubtree() as JsonObject;
		}
		public static IEnumerable<object> Select(object json, string path)
			=> JsonPathContext.Default.Select(json, path);
		public IEnumerable<object> Select(string path)
			=> Select(this, path);
		public static IDictionary<string, object> CreatePath(IDictionary<string, object> current, params string[] keys)
			=> _CreatePath(current, keys,0,null);
		public static IDictionary<string, object> CreatePath(IDictionary<string, object> current, Func<IDictionary<string, object>> objectCreator,params string[] keys)
			=> _CreatePath(current, keys, 0,objectCreator);
		public static object Get(object source, params object[] indicesAndKeys)
		{
			if (0 == indicesAndKeys.Length) return source;
			return _Get(source,indicesAndKeys,0);
		}
		static object _Get(object src,object[] indicesAndKeys,int index)
		{
			var o = indicesAndKeys[index];
			if (null == o) throw new ArgumentException("The parameter's values contain a null.", nameof(indicesAndKeys));
			var s = o as string;
			object target = null;
			if (null!=s)
			{
				var d = src as IDictionary<string, object>;
				if (null == d)
					throw new ArgumentException("Attempt to traverse a value that wasn't a JSON {object} type.",nameof(indicesAndKeys));
				target = d[s];
				if (indicesAndKeys.Length-1 > index)
					return _Get(target, indicesAndKeys, index + 1);
				return target;
			}
			var l = src as IList<object>;
			if (null == l)
				throw new ArgumentException("Attempt to traverse a value that wasn't a JSON [list] type.", nameof(indicesAndKeys));
			target = l[(int)o];
			if (indicesAndKeys.Length-1 > index)
				return _Get(target, indicesAndKeys, index + 1);
			return target;
		}
		public static object TryGet(object source,object @default, params object[] indicesAndKeys)
		{
			if (0 == indicesAndKeys.Length) return source;
			return _TryGet(source, @default,indicesAndKeys, 0);
		}
		static object _TryGet(object src, object @default,object[] indicesAndKeys, int index)
		{
			var o = indicesAndKeys[index];
			if (null == o) throw new ArgumentException("The parameter's values contain a null.", nameof(indicesAndKeys));
			var s = o as string;
			object target = null;
			if (null != s)
			{
				var d = src as IDictionary<string, object>;
				if (null == d)
					return @default;
				if (!d.TryGetValue(s, out target))
					return @default;
				if (indicesAndKeys.Length > index)
					return _Get(target, indicesAndKeys, index + 1);
				return target;
			}
			var l = src as IList<object>;
			if (null == l)
				return @default;
			var i = (int)o;
			if (i >= l.Count)
				return @default;
			// we still throw if negative
			target = l[i];
			if (indicesAndKeys.Length > index)
				return _Get(target, indicesAndKeys, index + 1);
			return target;
		}
		static IDictionary<string, object> _CreatePath(IDictionary<string, object> current, string[] keys, int keyIndex,Func<IDictionary<string,object>> objectCreator)
		{
			var k = keys[keyIndex];
			object o;
			if (!current.TryGetValue(k, out o))
			{
				IDictionary<string, object> jo;
				if (null != objectCreator)
					jo = objectCreator();
				else
					jo = new JsonObject();
				current.Add(k, jo);
				if (keyIndex < keys.Length - 1)
					return _CreatePath(jo, keys, keyIndex + 1,objectCreator);
			}
			else
			{
				var d = o as IDictionary<string, object>;
				if (null == d)
					throw new InvalidOperationException("The subtree already exists and is a different kind.");
				if (keyIndex < keys.Length - 1)
					return _CreatePath(d, keys, keyIndex + 1,objectCreator);
			}
			return current[k] as IDictionary<string, object>;
		}
		static void _MergeReplace(IDictionary<string, object> src, IDictionary<string, object> dst, HashSet<object> visited)
		{
			foreach (var field in src)
			{
				if (null != field.Value)
				{
					object o;
					var replace = true;
					if (dst.TryGetValue(field.Key, out o))
					{
						if (!Equals(o, field.Value) && null != o)
						{
							if (o.GetType().IsAssignableFrom(field.Value.GetType()))
							{
								var l = o as IList<object>;
								IDictionary<string, object> d = null;
								if (null == l)
									d = o as IDictionary<string, object>;
								if (null != l || null != d)
								{
									replace = false;
									_MergeReplace(field.Value, o, visited);
								}
							}
						}
					}
					if (replace)
						dst[field.Key] = field.Value;
				}
			}
		}
		static void _MergeReplace(IList<object> src, IList<object> dst, HashSet<object> visited)
		{
			for (int ic = src.Count, i = 0; i < ic; ++i)
			{
				if (dst.Count >= i)
					dst.Add(src[i]);
				else
					_MergeReplace(src[i], dst[i], visited);
			}
		}
		static void _MergeReplace(object src, object dst, HashSet<object> visited)
		{
			if (ReferenceEquals(src, dst))
				return;
			if (!visited.Add(src))
				return;
			if (null == src || null == dst) return;
			var d = src as IDictionary<string, object>;
			if (null != d)
			{
				var dd = dst as IDictionary<string, object>;
				if (null != dd)
					_MergeReplace(d, dd, visited);
				return;
			}
			var l = src as IList<object>;
			if (null != l)
			{
				var ll = dst as IList<object>;
				if (null != ll)
					_MergeReplace(l, ll, visited);
			}
		}
		
		static void _MergePreserve(IDictionary<string, object> src, IDictionary<string, object> dst, HashSet<object> visited)
		{
			foreach (var field in src)
			{
				if (null != field.Value)
				{
					object o;
					if (dst.TryGetValue(field.Key, out o))
					{
						if (!Equals(o, field.Value) && null != o)
						{
							if (o.GetType().IsAssignableFrom(field.Value.GetType()))
							{
								var l = o as IList<object>;
								IDictionary<string, object> d = null;
								if (null == l)
									d = o as IDictionary<string, object>;
								if (null != l || null != d)
								{
									_MergePreserve(field.Value, o, visited);
								}
							}
						}
					}
					else
						dst.Add(field.Key, field.Value);
				}
			}
		}
		static void _MergePreserve(IList<object> src, IList<object> dst, HashSet<object> visited)
		{
			for (int ic = src.Count, i = 0; i < ic; ++i)
			{
				if (dst.Count >= i)
					dst.Add(src[i]);
				else
					_MergePreserve(src[i], dst[i], visited);
			}
		}
		static void _MergePreserve(object src, object dst, HashSet<object> visited)
		{
			if (!visited.Add(src))
				return;
			if (null == src || null == dst) return;
			var d = src as IDictionary<string, object>;
			if (null != d)
			{
				var dd = dst as IDictionary<string, object>;
				if (null != dd)
					_MergePreserve(d, dd, visited);
				return;
			}
			var l = src as IList<object>;
			if (null != l)
			{
				var ll = dst as IList<object>;
				if (null != ll)
					_MergePreserve(l, ll, visited);
			}
		}
		public void CopyTo(object dstJson, bool preserveExistingValues = false)
		{
			CopyTo(this, dstJson, preserveExistingValues);
		}
		public static void CopyTo(object srcJson, object dstJson, bool preserveExistingValues = false)
		{
			if(preserveExistingValues)
				_MergePreserve(srcJson, dstJson, new HashSet<object>());
			else
				_MergeReplace(srcJson, dstJson, new HashSet<object>());
		}
		public object this[string key] { get => _inner[key]; set => _inner[key] = value; }

		public ICollection<string> Keys => _inner.Keys;

		public ICollection<object> Values => _inner.Values;

		public int Count => _inner.Count;

		public bool IsReadOnly => (_inner as IDictionary<string, object>).IsReadOnly;

		public void Add(string key, object value)
			=> _inner.Add(key, value);

		public void Add(KeyValuePair<string, object> item)
			=> (_inner as IDictionary<string, object>).Add(item);

		public void Clear()
			=> _inner.Clear();

		public bool Contains(KeyValuePair<string, object> item)
			=> (_inner as IDictionary<string, object>).Contains(item);

		public bool ContainsKey(string key)
			=> _inner.ContainsKey(key);

		public void CopyTo(KeyValuePair<string, object>[] array, int arrayIndex)
			=> (_inner as IDictionary<string, object>).CopyTo(array, arrayIndex);

		public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
			=> _inner.GetEnumerator();

		public bool Remove(string key)
			=> _inner.Remove(key);

		public bool Remove(KeyValuePair<string, object> item)
			=> (_inner as IDictionary<string, object>).Remove(item);

		public bool TryGetValue(string key, out object value)
			=> _inner.TryGetValue(key, out value);

		IEnumerator IEnumerable.GetEnumerator()
			=> _inner.GetEnumerator();

		public override bool TryGetMember(GetMemberBinder binder, out object result)
		{
			return _inner.TryGetValue(binder.Name, out result);
		}

		public override bool TrySetMember(SetMemberBinder binder, object value)
		{
			if (_inner.ContainsKey(binder.Name))
			{
				_inner[binder.Name] = value;
			}
			else
			{
				_inner.Add(binder.Name, value);
			}

			return true;
		}
		public void WriteTo(TextWriter writer, string indent = null)
			=> WriteTo(this, writer, indent);
		public void WriteTo(string filename, string indent = null)
			=> SaveTo(this, filename, indent);

		public static void WriteTo(object json, TextWriter writer, string indent = null)
		{
			_WriteTo(json, indent, string.IsNullOrEmpty(indent) ? -1 : 0, writer);
		}
		public static void SaveTo(object json, string filename, string indent = null)
		{
			using (var stm = File.OpenWrite(filename))
			{
				stm.SetLength(0L);
				var sw = new StreamWriter(stm);
				WriteTo(json, sw, indent);
				sw.Flush();
			}
		}
		public override string ToString()
		{
			var sw = new StringWriter();
			WriteTo(sw, "   ");
			sw.Flush();
			return sw.ToString();
		}
		private static void _WriteTo(object obj, string indent, int depth, TextWriter writer)
		{
			string indents = "";
			int i = 0;
			if (string.IsNullOrEmpty("indent"))
				depth = -1; // don't format
			while (i < depth)
			{
				indents += indent;
				++i;
			}
			bool first;
			if (obj is IDictionary<string, object>)
			{
				writer.Write("{");
				first = true;
				foreach (JsonField kvp in (IDictionary<string, object>)obj)
				{
					if (!first)
						writer.Write(",");
					else
						first = false;
					if (-1 < depth)
					{
						writer.WriteLine();
						writer.Write(indents + indent);
					}
					_WriteTo(kvp, indent, (-1 < depth) ? depth + 1 : -1, writer);
				}
				if (!first)
				{
					if (-1 < depth)
					{
						writer.WriteLine();
						writer.Write(indents);
					}
				}
				writer.Write("}");
			}
			else if (obj is string)
			{
				writer.Write(DecorateString((string)obj));
			}
			else if (obj is byte[])
			{
				writer.Write("\"#data:base64,");
				writer.Write(Convert.ToBase64String((byte[])obj));
				writer.Write('\"');
			}
			else if (obj is IList<object>)
			{
				writer.Write("[");
				first = true;
				foreach (object v in (IList<object>)obj)
				{
					if (!first)
						writer.Write(",");
					else first = false;
					if (-1 < depth)
					{
						writer.WriteLine();
						writer.Write(indents + indent);
					}
					_WriteTo(v, indent, (-1 < depth) ? depth + 1 : -1, writer);
				}
				if (!first)
				{
					if (-1 < depth)
					{
						writer.WriteLine();
						writer.Write(indents);
					}
				}
				writer.Write("]");
			}
			else if (obj is JsonField)
			{
				string k = ((JsonField)obj).Key as string;
				object v = ((JsonField)obj).Value;
				if (null != k)
				{
					if (-1 < depth)
					{

						writer.Write(DecorateString(k));

						writer.Write(": ");
						_WriteTo(v, indent, depth + 1, writer);
					}
					else
					{
						writer.Write(DecorateString(k));
						writer.Write(':');
						_WriteTo(v, null, -1, writer);
					}
				}
			}
			else if (null == obj)
			{

				writer.Write("null");
			}
			else if (obj is bool)
			{
				if ((bool)obj)
					writer.Write("true");
				else
					writer.Write("false");
			}
			else if (obj is int || obj is long || obj is double || obj is System.Numerics.BigInteger)
			{
				writer.Write(obj);
			}
		}
		public static string UndecorateString(string input)
		{
			if (string.IsNullOrEmpty(input))
				return input;
			char ch = input[0];
			if ('\"' == ch) // string
			{
				StringBuilder sb = new StringBuilder();
				for (int i = 1; i < input.Length - 1; i++)
				{
					ch = input[i];
					if ('\\' == ch)
					{
						++i;
						ch = input[i];
						switch (ch)
						{
							case 't':
								sb.Append('\t');
								break;
							case 'b':
								sb.Append('\b');
								break;
							case '\"':
								sb.Append('\"');
								break;
							case '\\':
								sb.Append('\\');
								break;
							case '/':
								sb.Append('/');
								break;
							case 'n':
								sb.Append('\n');
								break;
							case 'r':
								sb.Append('\r');
								break;
							case 'u':
								ch = '\0';
								++i;
								if (i == input.Length - 2)
									throw new FormatException("Unterminated escape sequence.");
								ch = (char)(input[i] * 0x1000);
								++i;
								if (i == input.Length - 2)
									throw new FormatException("Unterminated escape sequence.");
								ch += (char)(input[i] * 0x100);
								++i;
								if (i == input.Length - 2)
									throw new FormatException("Unterminated escape sequence.");
								ch += (char)(input[i] * 0x10);
								++i;
								if (i == input.Length - 2)
									throw new FormatException("Unterminated escape sequence.");
								ch += input[i];
								sb.Append(ch);
								break;
						}
					}
					else
						sb.Append(ch);
				}
				return sb.ToString();
			}
			throw new FormatException(string.Concat("Invalid character: ", input[0]));
		}
		public static string DecorateString(string input)
		{
			if (null == input)
				return "null";
			else if (0 == input.Length)
				return "\"\"";
			StringBuilder result = new StringBuilder();
			result.Append("\"");
			foreach (char ch in input)
			{
				switch (ch)
				{
					case '\b':
						result.Append("\\b");
						break;
					/*case '/':
						result.Append("\\/");
						break;*/
					case '\\':
						result.Append("\\\\");
						break;
					case '\t':
						result.Append("\\t");
						break;
					case '\r':
						result.Append("\\r");
						break;
					case '\n':
						result.Append("\\n");
						break;
					case '\"':
						result.Append("\\\"");
						break;
					default:
						if (char.IsControl(ch))
						{
							result.Append("\\u");
							ushort u = ch;
							result.Append(((byte)(u / 256)).ToString("x2"));
							result.Append(((byte)(u % 256)).ToString("x2"));
						}
						else
							result.Append(ch);
						break;
				}
			}
			result.Append("\"");
			return result.ToString();
		}
		
	}
}
