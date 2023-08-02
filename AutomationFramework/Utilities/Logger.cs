using System;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace AutomationFramework.Utilities
{
    public static class Logger
    {
        public static void Log(LogLevel logLevel, string message, string description="")
        {
            if (description != "") message = description + " => " + message;

            switch (logLevel)
            {
                case LogLevel.Info:
#pragma warning disable CS0618 // 'Log' is obsolete: 'Please use Context.Current.Log class to log messages.'
                    ReportPortal.Shared.Log.Info(message);
#pragma warning restore CS0618 // 'Log' is obsolete: 'Please use Context.Current.Log class to log messages.'
                    break;
                case LogLevel.Error:
#pragma warning disable CS0618 // 'Log' is obsolete: 'Please use Context.Current.Log class to log messages.'
                    ReportPortal.Shared.Log.Error(message);
#pragma warning restore CS0618 // 'Log' is obsolete: 'Please use Context.Current.Log class to log messages.'
                    break;
                case LogLevel.Debug:
#pragma warning disable CS0618 // 'Log' is obsolete: 'Please use Context.Current.Log class to log messages.'
                    ReportPortal.Shared.Log.Debug(message);
#pragma warning restore CS0618 // 'Log' is obsolete: 'Please use Context.Current.Log class to log messages.'
                    break;
            }
            Console.WriteLine($"[{logLevel}] {message}");
        }

        public static string PrettyXml(string xml)
        {
            var stringBuilder = new StringBuilder();

            var element = XElement.Parse(xml);

            var settings = new XmlWriterSettings();
            settings.OmitXmlDeclaration = true;
            settings.Indent = true;
            settings.NewLineOnAttributes = true;

            using (var xmlWriter = XmlWriter.Create(stringBuilder, settings))
            {
                element.Save(xmlWriter);
            }

            return stringBuilder.ToString();
        }
    }


    public enum LogLevel
    {
        Info,
        Error,
        Debug
    }
}
