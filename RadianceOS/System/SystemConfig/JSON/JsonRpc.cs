using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Cache;
using System.Text;
using System.Threading;

namespace Json
{
	public enum JsonRpcCacheLevel
	{
		/// <summary>
		/// Do not cache the response, and delete any existing data in the cache for this url. This is especially useful because many servers are not configured properly to do caching.
		/// </summary>
		None = 0,
		/// <summary>
		/// Rechecks with the server every time to make sure the cache is up to date. This saves bandwidth, but does require the same number of actual HTTP requests as the uncached policy, it's just that less data is sent in the case of a cache hit. With HTTP pipelining, this is still very efficient in terms of network traffic and latency.
		/// </summary>
		Conservative = 1,
		/// <summary>
		/// Waits (usually 5 minutes) to recheck with the server to make sure the cache is up to date. This is useful for devices with limited data connections, like phones.
		/// </summary>
		Aggressive =2,
		/// <summary>
		/// Uses whatever is in the machine.config. This is generally the default for .NET applications.
		/// </summary>
		MachinePolicy=3,
	
	}
	// TODO: The simple heuristics used in the class below need to be matured as this gets used
	// with different services, each exposing their exceptions differently
	// of course, you can always use fixupError to massage the json into something this will recognize
	public class JsonRpcException : Exception
	{
		public JsonRpcException(IDictionary<string, object> json)
		{
			Json = json;
		}
		public override string Message {
			get {
				var s = "";
				string k = null;

				// use simple heuristics to get the error from the JSON
				foreach (var field in Json)
				{
					var v = field.Value as string;
					if (!string.IsNullOrEmpty(v))
					{
						if (v.Length > s.Length)
						{
							s = v;
							k = field.Key;
						}
						if (field.Key.ToLowerInvariant().EndsWith("message"))
							return v;
						if ("exception" == field.Key.ToLowerInvariant())
							return v;
					}
				}
				if (null != k)
				{
					// return the longest string value
					return s;
				}
				return null;
			}
		}
		public int ErrorCode {
			get {
				int i = -1;
				// use simple heuristics to get the error from the JSON
				foreach (var field in Json)
				{
					if (field.Value is int)
					{
						var v = (int)field.Value;
						if (-1 == i)
							i = v;
						if (field.Key.ToLowerInvariant().EndsWith("code"))
							return v;
						if (field.Key.ToLowerInvariant().Contains("error"))
							return v;
					}
				}
				return i;
			}

		}
		public IDictionary<string, object> Json { get; }
	}
	public static class JsonRpc
	{
		
		public static string GetInvocationUrl(string baseUrl, IDictionary<string, object> args = null)
		{
			if (null == baseUrl) baseUrl = "";
			var sb = new StringBuilder();
			sb.Append(baseUrl);
			var delim = baseUrl.Contains("?") ? "&" : "?";
			if (null != args)
			{
				foreach (var arg in args)
				{
					sb.Append(delim);
					sb.Append(arg.Key);
					if (!(arg.Value is null))
					{
						sb.Append("=");
						if (arg.Value is bool)
							sb.Append((bool)arg.Value ? "true" : "false");
						else
							sb.Append(Uri.EscapeUriString(Convert.ToString(arg.Value)));
					}
					delim = "&";
				}
			}
			return sb.ToString();
		}
		// flag below because it's a *pain* to break inside this routine unless it was intended.
		// we want the exceptions from here to considered the root for the debugger to land.
		/// <summary>
		/// Executes a JSON based REST rpc call
		/// </summary>
		/// <param name="baseUrl">The url to use</param>
		/// <param name="args">Arguments to append to the url, represented as a simplified json object (only holds scalar values)</param>
		/// <param name="payloadJson">The JSON to send as part of the request body</param>
		/// <param name="timestampField">If specificed, indicates a field to insert a timestamp into. This timestamp represents the time the object was last fetched (either from the file cache or from the server)</param>
		/// <param name="httpMethod">Set this to use a custom HTTP method (like DELETE)</param>
		/// <param name="fixupResponse">An optional custom routine that can massage the JSON returned into a more acceptable form. This is useful if the data from the server needs to be adjusted prior to anything else.</param>
		/// <param name="fixupError">An optional custom routine that can massage the JSON error object returned into a more acceptable form. This is useful if the error from the server needs to be adjusted prior to anything else. You may need to specify this to get the <see cref="JsonRpcException" /> to work properly.</param>
		/// <param name="cache">Indicates whether or not a file cache should be used for requests. The in memory cache is only on a per-thread basis. The file cache stores by URL and is global. The cache is very aggressive.</param>
		/// <returns>The JSON returned in the response body.</returns>
		/// <remarks>Exceptions are expected to have JSON in the response body. This JSON is used as the data for the exception. The exception thrown currently uses a simple heuristic to figure out what the relevant fields are. Usually however, you'll get the data using the Json property of it.</remarks>
		//[DebuggerNonUserCode] 
		public static object Invoke(
			string baseUrl, 
			IDictionary<string, object> args = null, 
			object payloadJson = null,
			string timestampField=null, 
			string httpMethod = null,
			Func<object,object> fixupResponse=null, 
			Func<object, object> fixupError=null,
			JsonRpcCacheLevel cache=JsonRpcCacheLevel.Conservative,
			Func<IDictionary<string,object>> objectCreator=null,
			Func<IList<object>> arrayCreator = null)
		{
			HttpWebRequest wreq=null;
			HttpWebResponse wrsp = null;
			var url = GetInvocationUrl(baseUrl, args);
			wreq = WebRequest.Create(url) as HttpWebRequest;
			wreq.KeepAlive = true;
			wreq.Pipelined = true;
			wreq.AllowAutoRedirect = true;
			RequestCacheLevel cp = RequestCacheLevel.Default ;
			switch(cache)
			{
				case JsonRpcCacheLevel.None:
					cp = RequestCacheLevel.NoCacheNoStore;
					break;
				case JsonRpcCacheLevel.Conservative:
					cp = RequestCacheLevel.Revalidate;
					break;
				case JsonRpcCacheLevel.Aggressive:
					cp = RequestCacheLevel.CacheIfAvailable;
					break;
			}
			wreq.CachePolicy = new RequestCachePolicy(cp);
			if (null != payloadJson)
			{
				if (string.IsNullOrEmpty(httpMethod))
					wreq.Method = "POST";
				else
					wreq.Method = httpMethod;
				wreq.ContentType = "application/json";
				using (var sw = new StreamWriter(wreq.GetRequestStream()))
				{
					JsonObject.WriteTo(payloadJson, sw, null);
					sw.Flush();
				}
			}
			else if (string.IsNullOrEmpty(httpMethod))
				wreq.Method = "GET";
			else
				wreq.Method = httpMethod;
			
			try
			{
				wrsp = wreq.GetResponse() as HttpWebResponse;
				if (wrsp.StatusCode != HttpStatusCode.NoContent)
				{
					using (var reader = JsonTextReader.CreateFrom(new StreamReader(wrsp.GetResponseStream())))
					{
						object data = null;
						try
						{
							data = reader.ParseSubtree(objectCreator,arrayCreator);
						}
						catch (ExpectingException eex)
						{

							var ex = new JsonObject();
							ex.Add("status_code", -39);
							ex.Add("status_message", "Malformed or empty JSON data returned: " + eex.Message);
							ex.Add("http_status", (int)wrsp.StatusCode);
							ex.Add("http_status_message", wrsp.StatusDescription);
							throw new JsonRpcException(ex);
						}
						if (null != fixupResponse)
							data = fixupResponse(data);
						if (!string.IsNullOrEmpty(timestampField))
						{
							var d = data as IDictionary<string, object>;
							if (null != d)
							{
								DateTime dt = DateTime.UtcNow;
								d.Add(timestampField, dt.ToString("O"));
							}
						}
						return data;
					}
				}
				else
					return null;
			}
			catch (Exception ex)
			{
				var wex = ex as WebException;
				if (null != wex)
				{
					if (null == wrsp) wrsp = wex.Response as HttpWebResponse;
					if (null != wrsp)
					{
						using (var reader = JsonTextReader.CreateFrom(new StreamReader(wrsp.GetResponseStream())))
						{
							object data = null;
							try
							{
								data = reader.ParseSubtree(objectCreator,arrayCreator);
							}
							catch(ExpectingException eex)
							{
								IDictionary<string, object> jex;
								if (null != objectCreator)
									jex = objectCreator();
								else
									jex = new JsonObject();

								jex.Add("status_code", -39);
								jex.Add("status_message", "Malformed or empty JSON data returned: " + eex.Message);
								jex.Add("http_status", (int)wrsp.StatusCode);
								jex.Add("http_status_message", wrsp.StatusDescription);
								data = jex;
							}
							if (null!=fixupError)
								data = fixupError(data);
							var d = data as IDictionary<string, object>;
							
							if (!string.IsNullOrEmpty(timestampField))
							{
								if (null != d)
									d.Add(timestampField, DateTime.UtcNow.ToString("O"));
							}

							if (null != d)
								throw new JsonRpcException(d);
						}
					}
				}
				throw;
			}
		}
	}
}