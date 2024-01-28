#region Copyright (c) 2007 Atif Aziz. All rights reserved.
//
// C# implementation of JSONPath[1]
// [1] http://goessner.net/articles/JsonPath/
//
// The MIT License
//
// Copyright (c) 2007 Atif Aziz . All rights reserved.
// Portions Copyright (c) 2007 Stefan Goessner (goessner.net)
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
//
#endregion

namespace Json
{
	#region Imports

	using System;
	using System.Collections.Generic;
	using System.Globalization;
	using System.Linq;
	using System.Text;
	using System.Text.RegularExpressions;

	#endregion

	sealed partial class JsonPathContext
	{
		public static readonly JsonPathContext Default = new JsonPathContext();

		public Func<string /* script  */,
					object /* value   */,
					string /* context */,
					object /* result  */>
			ScriptEvaluator { get; set; }

		public IEnumerable<object> Select(object obj, string expr) =>
			SelectNodes(obj, expr, (v, _) => v);

		public IEnumerable<T> SelectNodes<T>(object obj, string expr, Func<object, string, T> resultor)
		{
			if (obj == null) throw new ArgumentNullException(nameof(obj));
			if (resultor == null) throw new ArgumentNullException(nameof(resultor));

			var i = new Interpreter(ScriptEvaluator);

			expr = _Normalize(expr);

			if (expr.Length >= 1 && expr[0] == '$') // ^\$:?
				expr = expr.Substring(expr.Length >= 2 && expr[1] == ';' ? 2 : 1);

			return i.Trace(expr, obj, "$", (value, path) => resultor(value, AsBracketNotation(path)));
		}

		static string _Normalize(string expr)
		{
			var subx = new List<string>();
			expr = RegExp.Replace(expr, @"[\['](\??\(.*?\))[\]']", m =>
			{
				subx.Add(m.Groups[1].Value);
				return "[#" + (subx.Count - 1).ToString(CultureInfo.InvariantCulture) + "]";
			});
			expr = RegExp.Replace(expr, @"'?\.'?|\['?", ";");
			expr = RegExp.Replace(expr, @";;;|;;", ";..;");
			expr = RegExp.Replace(expr, @";$|'?\]|'$", string.Empty);
			expr = RegExp.Replace(expr, @"#([0-9]+)", m =>
			{
				var index = int.Parse(m.Groups[1].Value, CultureInfo.InvariantCulture);
				return subx[index];
			});
			return expr;
		}

		public static string AsBracketNotation(string[] indicies)
		{
			if (indicies == null)
				throw new ArgumentNullException(nameof(indicies));

			var sb = new StringBuilder();

			foreach (var index in indicies)
			{
				if (sb.Length == 0)
				{
					sb.Append('$');
				}
				else
				{
					sb.Append('[');
					if (RegExp.IsMatch(index, @"^[0-9*]+$"))
						sb.Append(index);
					else
						sb.Append('\'').Append(index).Append('\'');
					sb.Append(']');
				}
			}

			return sb.ToString();
		}

		static int? _TryParseInt(string str) =>
			int.TryParse(str, NumberStyles.Integer, CultureInfo.InvariantCulture, out var n)
			? n : (int?)null;

		sealed class Interpreter
		{
			readonly Func<string, object, string, object> _eval;
			
			static readonly char[] _Colon = { ':' };
			static readonly char[] _Semicolon = { ';' };

			delegate void _WalkCallback(object member, string loc, string expr, object value, string path);

			public Interpreter(Func<string, object, string, object> eval)
			{
				_eval = eval ?? delegate
				{
					// @ symbol in expr must be interpreted specially to resolve
					// to value. In JavaScript, the implementation would look
					// like:
					//
					// return obj && value && eval(expr.replace(/@/g, "value"));

					return null;
				};
			}

			sealed class TraceArgs
			{
				public readonly string Expr;
				public readonly object Value;
				public readonly string Path;

				public TraceArgs(string expr, object value, string path)
				{
					Expr = expr;
					Value = value;
					Path = path;
				}
			}

			public IEnumerable<T> Trace<T>(string expr, object value, string path, Func<object, string[], T> resultor) =>
				Trace(Args(expr, value, path), resultor);

			static TraceArgs Args(string expr, object value, string path) =>
				new TraceArgs(expr, value, path);

			IEnumerable<T> Trace<T>(TraceArgs args, Func<object, string[], T> resultor)
			{
				var stack = new Stack<TraceArgs>();
				stack.Push(args);

				while (stack.Count > 0)
				{
					var popped = stack.Pop();
					var expr = popped.Expr;
					var value = popped.Value;
					var path = popped.Path;

					if (string.IsNullOrEmpty(expr))
					{
						if (path != null)
							yield return resultor(value, path.Split(_Semicolon));
						continue;
					}

					var i = expr.IndexOf(';');
					var atom = i >= 0 ? expr.Substring(0, i) : expr;
					var tail = i >= 0 ? expr.Substring(i + 1) : string.Empty;
					var hasMember = false;
					var d = value as IDictionary<string, object>;
					if (null != d) {
						if (d.ContainsKey(atom))
							hasMember = true;
					} else
					{
						var l = value as IList<object>;
						if(null!=l)
						{
							int ii;
							if(int.TryParse(atom,out ii))
							{
								if (-1 < ii && l.Count > ii)
									hasMember = true;
							}
						}
					}

					if (hasMember)
					{
						stack.Push(Args(tail, _Index(value, atom), path + ";" + atom));
					}
					else if (atom == "*")
					{
						Walk(atom, tail, value, path, (m, l, x, v, p) => stack.Push(Args(m + ";" + x, v, p)));
					}
					else if (atom == "..")
					{
						Walk(atom, tail, value, path, (m, l, x, v, p) =>
						{
							var result = _Index(v, m.ToString());
							var isPrim = true;
							d = result as IDictionary<string, object>;
							if (null == d)
							{
								if (null != result as IList<object>)
									isPrim = false;
							}
							else
								isPrim = false;
							
							if (!isPrim)
								stack.Push(Args("..;" + x, result, p + ";" + m));
						});
						stack.Push(Args(tail, value, path));
					}
					else if (atom.Length > 2 && atom[0] == '(' && atom[atom.Length - 1] == ')') // [(exp)]
					{
						stack.Push(Args(_eval(atom, value, path.Substring(path.LastIndexOf(';') + 1)) + ";" + tail, value, path));
					}
					else if (atom.Length > 3 && atom[0] == '?' && atom[1] == '(' && atom[atom.Length - 1] == ')') // [?(exp)]
					{
						Walk(atom, tail, value, path, (m, l, x, v, p) =>
						{
							var result = _eval(RegExp.Replace(l, @"^\?\((.*?)\)$", "$1"),
								_Index(v, m.ToString()), m.ToString());

							if (Convert.ToBoolean(result, CultureInfo.InvariantCulture))
								stack.Push(Args(m + ";" + x, v, p));
						});
					}
					else if (RegExp.IsMatch(atom, @"^(-?[0-9]*):(-?[0-9]*):?([0-9]*)$")) // [start:end:step] Phyton slice syntax
					{
						foreach (var a in Slice(atom, tail, value, path).Reverse())
							stack.Push(a);
					}
					else if (atom.IndexOf(',') >= 0) // [name1,name2,...]
					{
						foreach (var part in RegExp.Split(atom, @"'?,'?").Reverse())
							stack.Push(Args(part + ";" + tail, value, path));
					}
				}
			}

			void Walk(string loc, string expr, object value, string path, _WalkCallback callback)
			{
				var obj = value as IDictionary<string, object>;
				if (null != obj)
				{
					foreach (var key in obj.Keys.Reverse())
						callback(key, loc, expr, value, path);
					return;
				}
				var list = value as IList<object>;

				if (null!=list)
					for (var i = list.Count - 1; i >= 0; i--)
						callback(i, loc, expr, value, path);
			}

			static IEnumerable<TraceArgs> Slice(string loc, string expr, object value, string path)
			{
				if (!(value is IList<object> list))
					yield break;

				var length = list.Count;
				var parts = loc.Split(_Colon);
				var start = _TryParseInt(parts[0]) ?? 0;
				var end = _TryParseInt(parts[1]) ?? list.Count;
				var step = parts.Length > 2 ? _TryParseInt(parts[2]) ?? 1 : 1;
				start = (start < 0) ? Math.Max(0, start + length) : Math.Min(length, start);
				end = (end < 0) ? Math.Max(0, end + length) : Math.Min(length, end);
				for (var i = start; i < end; i += step)
					yield return Args(i + ";" + expr, value, path);
			}

			object _Index(object obj, string member)
			{
				var d = obj as IDictionary<string, object>;
				if (null != d)
					return d[member];
				var l = obj as IList<object>;
				if (null !=l)
				{
					int i;
					if(int.TryParse(member,out i))
					{
						if (-1 < i && l.Count > i)
							return l[i];
					}
					return null;
				}
				throw new ArgumentException("Primitive values cannot be indexed.");
			}
		}

		static class RegExp
		{
			const RegexOptions Options = RegexOptions.ECMAScript;

			public static bool IsMatch(string input, string pattern) =>
				Regex.IsMatch(input, pattern, Options);

			public static string Replace(string input, string pattern, string replacement) =>
				Regex.Replace(input, pattern, replacement, Options);

			public static string Replace(string input, string pattern, MatchEvaluator evaluator) =>
				Regex.Replace(input, pattern, evaluator, Options);

			public static IEnumerable<string> Split(string input, string pattern) =>
				Regex.Split(input, pattern, Options);
		}

		
	}
}