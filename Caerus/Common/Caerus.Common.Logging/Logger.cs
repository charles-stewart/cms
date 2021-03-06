﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Caerus.Common.Enums;
using Caerus.Common.Interfaces;
using Caerus.Common.Logging.Interfaces;
using Caerus.Common.ViewModels;

namespace Caerus.Common.Logging
{

    public static class GlobalLogger
    {
        public static ReplyObject WrapException(Exception ex, dynamic[] props = null, ReplyStatus replyStatus = ReplyStatus.Fatal)
        {
            return new Logger().WrapException(ex, props, replyStatus, 3);
        }

        public static ReplyObject WrapStubInfo(dynamic[] props = null, ReplyStatus replyStatus = ReplyStatus.Success)
        {
            return new Logger().WrapStubInfo(props, replyStatus, 3);
        }
    }

    public class Logger : ICaerusLogger
    {
        public Logger()
        {
            Configure();
        }

        private static log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private void Configure()
        {
            log4net.Config.XmlConfigurator.Configure();
            var source = ConfigurationManager.AppSettings["source"];
            log4net.LogicalThreadContext.Properties["Source"] = !string.IsNullOrEmpty(source) ? source : "unknown";
        }

        public void LogInfo(string infoMessage, dynamic[] properties = null)
        {
            try
            {
                var body = BuildLog(null, infoMessage, properties);
                log.Info(body.Message);
            }
            catch (Exception ex)
            {
                LoggerFail(ex);
            }
        }

        // Logging a Debug message
        public void LogDebug(string debugMessage, dynamic[] properties = null)
        {
            try
            {
                var body = BuildLog(null, debugMessage, properties);
                log.Debug(body.Message);
            }
            catch (Exception ex)
            {
                LoggerFail(ex);
            }
        }

        //Logging a Warning message
        public void LogWarning(string message, Exception exception = null, dynamic[] properties = null)
        {
            try
            {
                var body = BuildLog(exception, message, properties);
                log.Warn(body.Message, body.Exception);
            }
            catch (Exception ex)
            {
                LoggerFail(ex);
            }
        }

        // Logging a Error message
        public void LogError(string methodName, Exception exception = null, dynamic[] properties = null)
        {
            try
            {
                var body = BuildLog(exception, methodName, properties);
                log.Error(body.Message, body.Exception);
            }
            catch (Exception ex)
            {
                LoggerFail(ex);
            }
        }

        public void LogError(string error, dynamic[] properties = null)
        {
            try
            {
                var body = BuildLog(null, error, properties);
                log.Error(body.Message, null);
            }
            catch (Exception ex)
            {
                LoggerFail(ex);
            }
        }

        public void LogError(Exception exc, dynamic[] properties = null)
        {
            try
            {
                var body = BuildLog(exc, "", properties);
                log.Error(body.Message, body.Exception);
            }
            catch (Exception ex)
            {
                LoggerFail(ex);
            }
        }

        // Logging a Fatal message
        public void LogFatal(string methodName, Exception exception = null, dynamic[] properties = null)
        {


            try
            {
                var body = BuildLog(exception, "", properties);
                var message = body.Message;
                if (!String.IsNullOrEmpty(methodName))
                    message = methodName + " - " + message;

                log.Fatal(message, exception);
            }
            catch (Exception ex)
            {
                LoggerFail(ex);
            }
        }

        private void LoggerFail(Exception exception = null)
        {

            try
            {
                log4net.Config.XmlConfigurator.Configure();

                var body = BuildLog(exception);
                log.Fatal(body.Message, exception);
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to initialize logging", ex);
            }
        }

        public void LogSystemActivity(string description, long? reference = null, dynamic[] properties = null)
        {
            LogInfo(description + " : " + reference.ToString());
        }

        private static LoggingBody BuildLog(Exception ex, string message = "", dynamic[] properties = null)
        {
            var body = new LoggingBody { Exception = ex };
            var frame = new StackTrace().GetFrame(2);
            var method = frame.GetMethod().Name;
            if (method == "WrapException")
                frame = new StackTrace().GetFrame(3);
            method = frame.GetMethod().Name;
            body.LoggerMethod = method;

            var v = frame.GetMethod().DeclaringType;
            string assembly = "";
            if (v != null)
            {
                assembly = v.Assembly.GetName().Name;
            }
            log4net.LogicalThreadContext.Properties["Logger"] = string.Format("Source: {0} - Method: {1}", assembly, body.LoggerMethod);
            if (ex != null)
                body.Message = string.IsNullOrEmpty(message) ? string.Format("Exception has occurred in : {0}", body.LoggerMethod) : string.Format("Exception has occurred in : {0} - {1}", body.LoggerMethod, message);
            else
                body.Message = string.IsNullOrEmpty(message) ? string.Format("{0}", body.LoggerMethod) : string.Format("{0} - {1}", body.LoggerMethod, message);

            log4net.LogicalThreadContext.Properties["Parameters"] = "";
            log4net.LogicalThreadContext.Properties["User"] = "";
            log4net.LogicalThreadContext.Properties["Origin"] = "";
            if (properties != null && properties.Any())
            {
                var builder = new StringBuilder();
                var cnt = 0;
                foreach (var item in properties)
                {
                    cnt++;
                    builder.AppendLine(string.Format("({0})", item.ToString(), cnt));
                }
                log4net.LogicalThreadContext.Properties["Parameters"] = builder.ToString();
            }
            if (HttpContext.Current != null)
            {
                log4net.LogicalThreadContext.Properties["User"] = HttpContext.Current.User.Identity.Name;
                log4net.LogicalThreadContext.Properties["Origin"] = HttpContext.Current.Request.UserHostAddress;
            }
            return body;
        }


        private class LoggingBody
        {
            public string LoggerMethod { get; set; }

            public string Message { get; set; }

            public Exception Exception { get; set; }
        }

        public ReplyObject WrapException(Exception ex, dynamic[] props = null, ReplyStatus replyStatus = ReplyStatus.Fatal, int stackframe = 2)
        {
            var result = new ReplyObject() { ReplyStatus = replyStatus };
            var frame = new StackTrace().GetFrame(stackframe);
            LogFatal(frame.GetMethod().Name, ex, props);
            result.ReplyMessage = string.Format("Oh crumbs! Problem occurred in {0}", frame.GetMethod().Name);
            var et = ex as DbEntityValidationException;
            if (et != null && et.EntityValidationErrors != null)
            {
                foreach (var item in et.EntityValidationErrors)
                {
                    foreach (var valerror in item.ValidationErrors)
                    {
                        result.Errors.Add(string.Format("{0}", valerror.ErrorMessage));
                    }
                }
            }
            return result;
        }

        public ReplyObject WrapStubInfo(dynamic[] props = null, ReplyStatus replyStatus = ReplyStatus.Success, int stackframe = 2)
        {
            var result = new ReplyObject() { ReplyStatus = replyStatus };
            var frame = new StackTrace().GetFrame(stackframe);
            result.ReplyMessage = string.Format("Stub function called {0}", frame.GetMethod().Name);
            var body = BuildLog(null, result.ReplyMessage, props);
            log.Info(body.Message);
            return result;
        }
       
    }
}
