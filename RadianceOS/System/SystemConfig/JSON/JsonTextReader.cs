using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace Json
{

	public enum JsonNodeType
	{
		EndDocument = -2,
		Initial = -1,
		Value = 0,
		Key = 1,
		Array = 2,
		EndArray = 3,
		Object = 4,
		EndObject = 5
	}
	/// <summary>
	/// A small streaming JSON pull-parser. Will work on large streams and non-seekable streams.
	/// </summary>
	/// <remarks>Works like Microsoft's <see cref="System.Xml.XmlReader"/></remarks>
	public sealed class JsonTextReader : IDisposable
	{
		int _state;
		ParseContext _pc;
		internal JsonTextReader(ParseContext parseContext)
		{
			_pc = parseContext;
			_state = -1;
		}
		public JsonNodeType NodeType => (JsonNodeType)_state;
		public bool Read()
		{
			switch(_state)
			{
				case -2: // end of document
					return false;
				case -1: // initial
					_pc.EnsureStarted();
					_state = 0;
					goto case 0;
				case 0: // value
					_pc.ClearCapture();
					switch (_pc.Current)
					{
						case -1:
							// end of input
							_state = -2; // EndDocument
							return false;
						case ']':
							_pc.Advance();
							_pc.TrySkipWhiteSpace();
							_pc.ClearCapture();
							_state = 3; // end array
							return true;
						case '}':
							_pc.Advance();
							_pc.TrySkipWhiteSpace();
							_pc.ClearCapture();
							_state = 5; // end object
							return true;
						case ',':
							_pc.Advance();
							_pc.TrySkipWhiteSpace();
							if (!Read()) // read the next value
								_pc.Expecting();
							return true;
						case '[':
							_pc.Advance();
							_pc.TrySkipWhiteSpace();
							_state = 2; // begin array
							return true;
						case '{':
							_pc.Advance();
							_pc.TrySkipWhiteSpace();
							_state = 4; // begin object
							return true;

						case '-':
						case '.':
						case '0':
						case '1':
						case '2':
						case '3':
						case '4':
						case '5':
						case '6':
						case '7':
						case '8':
						case '9':
							int qc = _pc.Current;
							_pc.CaptureCurrent();
							while (-1 != _pc.Advance() && ('E' == _pc.Current || 'e' == _pc.Current || '+' == _pc.Current || '.' == _pc.Current || char.IsDigit((char)_pc.Current)))
								_pc.CaptureCurrent();
							_pc.TrySkipWhiteSpace();
							return true;
						case '\"':
							_pc.CaptureCurrent();
							_pc.Advance();
							_pc.TryReadUntil('\"', '\\', true);
							_pc.TrySkipWhiteSpace();
							if (':' == _pc.Current)
							{
								_pc.Advance();
								_pc.TrySkipWhiteSpace();
								_pc.Expecting();
								_state = 1; // key
							}
							return true;
						case 't':
							_pc.CaptureCurrent();
							_pc.Advance();
							_pc.Expecting('r');
							_pc.CaptureCurrent();
							_pc.Advance();
							_pc.Expecting('u');
							_pc.CaptureCurrent();
							_pc.Advance();
							_pc.Expecting('e');
							_pc.CaptureCurrent();
							_pc.Advance();
							_pc.TrySkipWhiteSpace();
							_pc.Expecting(',',']', '}', -1);
							return true;
						case 'f':
							_pc.CaptureCurrent();
							_pc.Advance();
							_pc.Expecting('a');
							_pc.CaptureCurrent();
							_pc.Advance();
							_pc.Expecting('l');
							_pc.CaptureCurrent();
							_pc.Advance();
							_pc.Expecting('s');
							_pc.CaptureCurrent();
							_pc.Advance();
							_pc.Expecting('e');
							_pc.CaptureCurrent();
							_pc.Advance();
							_pc.TrySkipWhiteSpace();
							_pc.Expecting(',',']', '}', -1);
							return true;
						case 'n':
							_pc.CaptureCurrent();
							_pc.Advance();
							_pc.Expecting('u');
							_pc.CaptureCurrent();
							_pc.Advance();
							_pc.Expecting('l');
							_pc.CaptureCurrent();
							_pc.Advance();
							_pc.Expecting('l');
							_pc.CaptureCurrent();
							_pc.Advance();
							_pc.TrySkipWhiteSpace();
							_pc.Expecting(',',']', '}', -1);
							return true;
						default:
							_pc.Expecting('-','.','0','1','2','3','4','5','6','7','8','9','\"','[',']','{','}','t','f','n');
							return false;
					}
				default:
					_state = 0;
					goto case 0;
			}
		}
		public void Close()
		{
			_pc.Close();
			GC.SuppressFinalize(this);
		}
		void IDisposable.Dispose() => Close();
		~JsonTextReader()
		{
			_pc.Close();
		}
		
		public static JsonTextReader Create(string jsonText)
		{
			return new JsonTextReader(ParseContext.Create(jsonText));
		}
		public static JsonTextReader CreateFrom(TextReader reader)
		{
			return new JsonTextReader(ParseContext.CreateFrom(reader));
		}
		public static JsonTextReader CreateFrom(string filename)
		{
			return new JsonTextReader(ParseContext.CreateFrom(filename));
		}
		public static JsonTextReader CreateFromUrl(string url)
		{
			return new JsonTextReader(ParseContext.CreateFromUrl(url));
		}
		public string RawValue {
			get {
				if(0==_state || 1==_state) // key or value
					return _pc.GetCapture();
				return null;
			}
		}
		public object Value {
			get {
				var s = _pc.GetCapture();
				if (0 == s.Length)
					return null;
				switch(s[0])
				{
					case '\"':
						return JsonObject.UndecorateString(s);
					case 't':
						return true; // already checked during parse
					case 'f':
						return false; // already checked during parse
					case 'n':
						return null; // already checked during parse
					default: // digit
						int ni;
						long nl;
						System.Numerics.BigInteger nb;
						if (int.TryParse(s, out ni))
							return ni;
						else if (long.TryParse(s, out nl))
							return nl;
						else if (System.Numerics.BigInteger.TryParse(s, out nb))
							return nb;
						else
							return double.Parse(s);
						
				}
			}
		}
		public object ParseSubtree(Func<IDictionary<string,object>> objectCreator=null,Func<IList<object>> arrayCreator=null)
		{
			IList<object> l = null;
			IDictionary<string, object> d = null;
			string rv;
			object result = null;
			switch (_state)
			{
				case -2:
					return null;
				case -1:
					if (!Read())
						_pc.Expecting();
					return ParseSubtree(objectCreator,arrayCreator);
				case 0:
					result = Value;
					break;
				case 1: // key
					rv = Value as string;
					if (!Read())
						_pc.Expecting();
					result = new KeyValuePair<string, object>(rv, ParseSubtree(objectCreator,arrayCreator));
					break;
				case 2:// begin array
					if (null != arrayCreator)
						l = arrayCreator();
					else
						l = new JsonArray();
					while (Read() && 3 != _state)
					{
						l.Add(ParseSubtree(objectCreator,arrayCreator));
					}
					result = l;
					break;
				case 3: // end array
					result = null;
					break;
				case 4:// begin object
					if (null != objectCreator)
						d = objectCreator();
					else
						d = new JsonObject();
					while (Read() && 5 != _state) // not end object
					{
						KeyValuePair<string, object> kvp = (KeyValuePair<string, object>)ParseSubtree(objectCreator,arrayCreator);
						d.Add(kvp.Key, kvp.Value);
					}
					result = d;
					break;
				case 5:
					result = null;
					break;
				default:
					throw new InvalidProgramException("Unhandled state");
			}
			return result;
		}
		/// <summary>
		/// Skips the subtree at the current node without parsing it
		/// </summary>
		/// <returns>True if there is more data to read</returns>
		public bool SkipSubtree()
		{
			switch (_state)
			{
				case -2: // eos
					return false;
				case -1: // initial
					if (!Read())
						_pc.Expecting();
					return SkipSubtree();
				case 0: // value
					return true;
				case 1: // key
					if (!Read())
						_pc.Expecting();
					return SkipSubtree();
				case 2:// begin array
					_SkipArrayPart();
					_pc.TrySkipWhiteSpace();
					_state = 3; // end array
					return true;
				case 3: // end array
					return true;
				case 4:// begin object
					_SkipObjectPart();
					_pc.TrySkipWhiteSpace();
					_state = 5; // end object
					return true;
				case 5: // end object
					return true;
				default:
					throw new InvalidProgramException("Unhandled state");
			}
		}
		/// <summary>
		/// Skips to the specified index of the array. Must be positioned on a key or the start of an array.
		/// </summary>
		/// <param name="index">The index to skip to</param>
		/// <returns>True if on the value element of the array. Otherwise, false</returns>
		/// <remarks>Must be positioned on the start of a document, the key, or an array start. Otherwise this returns false.</remarks>
		public bool SkipToIndex(int index)
		{
			if (-1==_state || 1 == _state) // initial or key
				if (!Read())
					return false;
			if (2==_state) // array start
			{
				if (0 == index)
				{
					if (!Read())
						return false;
				}
				else
				{
					for (var i = 0; i < index; ++i)
					{
						if (3 == _state) // end array
							return false;
						if (!Read())
							return false;
						if (!SkipSubtree())
							return false;
					}
					if ((5==_state || 3==_state) && !Read())
						return false;
				}
				return true;
			}
			return false;
		}
		/// <summary>
		/// Skips through a series of indices and keys to wind you up at the specified path
		/// </summary>
		/// <param name="indicesOrKeys">The indices and keys to navigate</param>
		/// <returns>True if found, and the reader is positioned on the key or index. False if not. Either way, the reader is advanced.</returns>
		public bool SkipTo(params object[] indicesOrKeys)
		{
			if (-1 == _state)
				if (!Read())
					return false;
			for(var i = 0;i<indicesOrKeys.Length;i++)
			{
				var field = indicesOrKeys[i] as string;
				if (null != field)
				{
					if (!SkipToField(field))
						return false;
				}
				else if (indicesOrKeys[i] is int)
				{
					if (!SkipToIndex((int)indicesOrKeys[i]))
						return false;
				}
				else
					throw new ArgumentException("There was not a string or an int at index " + i.ToString(), nameof(indicesOrKeys));
			}
			return true;
		}
		/// <summary>
		/// Skips to the field with the specified name. Does not traverse descendants.
		/// </summary>
		/// <param name="key">The name of the field.</param>
		/// <returns>True if the reader wound up on said, field, otherwise, false</returns>
		/// <remarks>This will return false unless called from an object start or from a field.</remarks>
		public bool SkipToField(string key, bool searchDescendants = false)
		{
			string val;

			if (searchDescendants)
			{
				while (Read())
				{
					if (1 == _state) // key
					{
						val = JsonObject.UndecorateString(_pc.GetCapture());
						if (key == val)
							return true;
					}
				}
				return false;
			}
			switch (_state)
			{
				case -1:
					if (Read())
						return SkipToField(key);
					return false;
				case 4:
					while (Read() && 1 == _state) // first read will move to the child field of the root
					{
						val = JsonObject.UndecorateString(_pc.GetCapture());
						if (key != val )
							SkipSubtree(); // if this field isn't the target so just skip over the rest of it
						else
							break;
					}
					return 1 == _state;
				case 1: // we're already on a field
					val = JsonObject.UndecorateString(_pc.GetCapture());
					if (key == val)
						return true;
					else if (!SkipSubtree())
						return false;

					while (Read() && 1 == _state) // first read will move to the child field of the root
					{
						val = JsonObject.UndecorateString(_pc.GetCapture());
						if (key != val)
							SkipSubtree(); // if this field isn't the target just skip over the rest of it
						else
							break;
					}
					return 1 == _state;
				default:
					return false;
			}
		}
		/// <summary>
		/// Skips to one of the fields in the list
		/// </summary>
		/// <param name="anyOfKeys">The list of fields to try.</param>
		/// <returns>True if one of the fields matched, and the reader ends up positioned on a key. Otherwise false.</returns>
		public bool SkipToAnyOfFields(params string[] anyOfKeys)
			=> SkipToAnyOfFields(anyOfKeys, false);
		// code is duplicated for optimizations reasons.
		/// <summary>
		/// Searches for one of the fields in the keys
		/// </summary>
		/// <param name="searchDescendants">True to look through the entire subtree, otherwise, false</param>
		/// <param name="anyOfKeys">The keys to look for</param>
		/// <returns>True if one of the fields matched, and the reader ends up positioned on a key. Otherwise false.</returns>
		public bool SkipToAnyOfFields(string[] anyOfKeys,bool searchDescendants)
		{
			string val;

			if (searchDescendants)
			{
				while (Read())
				{
					if (1 == _state) // key
					{
						val = JsonObject.UndecorateString(_pc.GetCapture());
						if (-1<Array.IndexOf(anyOfKeys,val))
							return true;
					}
				}
				return false;
			}
			switch (_state)
			{
				case -1:
					if (Read())
						return SkipToAnyOfFields(anyOfKeys);
					return false;
				case 4:
					while (Read() && 1 == _state) // first read will move to the child field of the root
					{
						val = JsonObject.UndecorateString(_pc.GetCapture());
						if (0 > Array.IndexOf(anyOfKeys, val))
							SkipSubtree(); // if this field isn't the target so just skip over the rest of it
						else
							break;
					}
					return 1 == _state;
				case 1: // we're already on a field
					val = JsonObject.UndecorateString(_pc.GetCapture());
					if (-1<Array.IndexOf(anyOfKeys,val))
						return true;
					else if (!SkipSubtree())
						return false;

					while (Read() && 1 == _state) // first read will move to the child field of the root
					{
						val = JsonObject.UndecorateString(_pc.GetCapture());
						if (0>Array.IndexOf(anyOfKeys,val))
							SkipSubtree(); // if this field isn't the target just skip over the rest of it
						else
							break;
					}
					return 1 == _state;
				default:
					return false;
			}
		}
		public void SkipToEndObject()
		{
			_SkipObjectPart();
			_state = 5; // end object
		}
		public void SkipToEndArray()
		{
			_SkipArrayPart();
			_state = 3; // end array
		}
		// optimization
		private void _SkipObjectPart()
		{
			int depth = 1;
			while (-1 != _pc.Current)
			{
				switch (_pc.Current)
				{
					case '[':
						_SkipArrayPart();
						break;
						
					case '{':
						++depth;
						_pc.Advance();
						_pc.Expecting();
						break;
					case '\"':
						_SkipString();
						break;
					case '}':
						--depth;
						if (depth == 0)
						{
							_pc.Advance();
							return;
						}
						_pc.Expecting();
						break;
					default:
						_pc.Advance();
						break;
				}
			}
		}
		// optimization
		private void _SkipArrayPart()
		{
			int depth = 1;
			while (-1 != _pc.Current)
			{
				switch (_pc.Current)
				{
					case '{':
						_SkipObjectPart();
						break;
					case '[':
						++depth;
						_pc.Advance();
						_pc.Expecting();
						break;
					case '\"':
						_SkipString();
						break;
					case ']':
						--depth;
						if (depth == 0)
						{
							_pc.Advance();
							return;
						}
						_pc.Expecting();
						break;
					default:
						_pc.Advance();
						break;
				}
			}
		}
		void _SkipString()
		{
			_pc.Expecting('\"');
			_pc.Advance();
			_pc.TrySkipUntil('\"', false);
			_pc.Expecting('\"');
			_pc.Advance();
		}

	}
}
