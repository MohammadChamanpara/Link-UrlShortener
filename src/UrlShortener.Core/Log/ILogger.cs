using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UrlShortener.Core.Log
{
	/// <summary>
	/// Logger interface to use for all functional classes in the application
	/// in order to register the logs in an underlying logging framework.
	/// </summary>
	public interface ILogger
	{
		/// <summary>
		/// Log message with Debug or Verbose severity
		/// </summary>
		/// <param name="message">The message.</param>
		/// <param name="messageParameters">The message parameters.</param>
		void Debug(string message, params object[] messageParameters);

		/// <summary>
		/// Log message with Informations severity.
		/// </summary>
		/// <param name="message">The message.</param>
		/// <param name="messageParameters">The message parameters.</param>
		void Information(string message, params object[] messageParameters);

		/// <summary>
		/// Log message with warning severity.
		/// </summary>
		/// <param name="message">The message.</param>
		/// <param name="messageParameters">The message parameters.</param>
		void Warning(string message, params object[] messageParameters);

		/// <summary>
		/// Log message with Error severity.
		/// </summary>
		/// <param name="message">The message.</param>
		/// <param name="messageParameters">The message parameters.</param>
		void Error(string message, params object[] messageParameters);
		
		/// <summary>
		/// Log message with Error severity.
		/// </summary>
		/// <param name="exception">The exception.</param>
		/// <param name="message">The message.</param>
		/// <param name="messageParameters">The message parameters.</param>
		void Error(Exception exception, string message, params object[] messageParameters);

		/// <summary>
		/// Log message with Fatal severity.
		/// </summary>
		/// <param name="message">The message.</param>
		/// <param name="messageParameters">The message parameters.</param>
		void Fatal(string message, params object[] messageParameters);

		/// <summary>
		/// Log message with Fatal severity.
		/// </summary>
		/// <param name="exception">The exception.</param>
		/// <param name="message">The message.</param>
		/// <param name="messageParameters">The message parameters.</param>
		void Fatal(Exception exception, string message, params object[] messageParameters);
	}
}
