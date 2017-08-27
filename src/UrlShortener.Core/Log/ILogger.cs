using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UrlShortener.Core.Log
{
	public interface ILogger
	{
		void Debug(string message, params object[] messageParameters);
		void Information(string message, params object[] messageParameters);
		void Warning(string message, params object[] messageParameters);
		void Error(string message, params object[] messageParameters);
		void Error(Exception exception, string message, params object[] messageParameters);
		void Fatal(string message, params object[] messageParameters);
		void Fatal(Exception exception, string message, params object[] messageParameters);
	}
}
