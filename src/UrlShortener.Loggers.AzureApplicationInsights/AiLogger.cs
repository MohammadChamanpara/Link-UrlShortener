using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UrlShortener.Core.Log;

namespace UrlShortener.Loggers.AzureApplicationInsights
{
	public class AiLogger : ILogger
	{
		private readonly TelemetryClient telemetryClient = new TelemetryClient();
		public void Debug(string message, params object[] messageParameters)
		{
			telemetryClient.TrackTrace(string.Format(message, messageParameters), SeverityLevel.Verbose);
		}
		public void Information(string message, params object[] messageParameters)
		{
			telemetryClient.TrackTrace(string.Format(message, messageParameters), SeverityLevel.Information);
		}
		public void Warning(string message, params object[] messageParameters)
		{
			telemetryClient.TrackTrace(string.Format(message, messageParameters), SeverityLevel.Warning);
		}
		public void Error(string message, params object[] messageParameters)
		{
			telemetryClient.TrackTrace(string.Format(message, messageParameters), SeverityLevel.Error);
		}
		public void Error(Exception exception, string message, params object[] messageParameters)
		{
			var telemetry = new ExceptionTelemetry(exception);
			telemetry.Properties.Add("message", string.Format(message, messageParameters));

			telemetryClient.TrackException(telemetry);
		}
		public void Fatal(string message, params object[] messageParameters)
		{
			telemetryClient.TrackTrace(string.Format(message, messageParameters), SeverityLevel.Critical);
		}
		public void Fatal(Exception exception, string message, params object[] messageParameters)
		{
			var telemetry = new ExceptionTelemetry(exception);
			telemetry.Properties.Add("message", string.Format(message, messageParameters));

			telemetryClient.TrackException(telemetry);
		}
	}
}
