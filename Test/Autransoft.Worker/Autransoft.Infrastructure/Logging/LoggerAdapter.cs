using Autransoft.ApplicationCore.Interfaces;
using Autransoft.Fluent.HttpClient.Lib.Exceptions;
using Autransoft.Fluent.HttpClient.Lib.Interfaces;
using Autransoft.Template.EntityFramework.Lib.Exceptions;
using Autransoft.Template.EntityFramework.Lib.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Text;

namespace Autransoft.Infrastructure.Logging
{
    public class LoggerAdapter<L> : IAppLogger<L>, IAutranSoftEfLogger<L>, IAutranSoftFluentLogger<L>
        where L : class
    {
        private const string APPLICATION = "Autransoft.Worker";
        private readonly ILogger<L> _logger;

        public LoggerAdapter(ILoggerFactory loggerFactory) => (_logger) = (loggerFactory.CreateLogger<L>());

        public void LogError(AutranSoftEfException autranSoftEfException) =>
            _logger.LogError($"Aplicação:{APPLICATION}|{autranSoftEfException.LogError<L>()}");

        public void LogError(FluentHttpContentException<L> fluentHttpContentException) =>
            _logger.LogError($"Aplicação:{APPLICATION}|{fluentHttpContentException.LogError()}");

        public void LogError(FluentHttpRequestException<L> fluentHttpRequestException) =>
            _logger.LogError($"Aplicação:{APPLICATION}|{fluentHttpRequestException.LogError()}");

        public void LogInformation(FluentHttpContentException<L> fluentHttpContentException) =>
            _logger.LogInformation($"Aplicação:{APPLICATION}|{fluentHttpContentException.LogInformation()}");

        public void LogInformation(FluentHttpRequestException<L> fluentHttpRequestException) =>
            _logger.LogInformation($"Aplicação:{APPLICATION}|{fluentHttpRequestException.LogInformation()}");

        public void Error(Exception ex, string message = null)
        {
            var log = GetLogErrorHeader(typeof(Exception));

            if (!string.IsNullOrEmpty(message))
                log.Append($"Dados:{message}|");

            LogComumException(log, ex);

            _logger.LogInformation(log.ToString().Substring(0, log.ToString().Length - 1));
        }

        public void Information(string message)
        {
            var log = GetLogInformationHeader(null);
            log.Append($"Message:{message}|");

            _logger.LogInformation(log.ToString().Substring(0, log.ToString().Length - 1));
        }

        private void LogComumException(StringBuilder log, Exception ex)
        {
            if (!string.IsNullOrEmpty(ex.Message))
                log.Append($"Message:{ex.Message}|");

            if (ex.InnerException != null && !string.IsNullOrEmpty(ex.InnerException.Message))
                log.Append($"InnerException.Message:{ex.InnerException.Message}|");

            if (ex.InnerException != null && ex.InnerException.InnerException != null && !string.IsNullOrEmpty(ex.InnerException.InnerException.Message))
                log.Append($"InnerException.InnerException.Message:{ex.InnerException.InnerException.Message}|");

            if (!string.IsNullOrEmpty(ex.StackTrace))
                log.Append($"StackTrace:{ex.StackTrace}|");
        }

        private StringBuilder GetLogInformationHeader(Type type)
        {
            var log = new StringBuilder();
            log.Append($"Aplicação:{APPLICATION}|");
            log.Append($"Log:Information|");
            log.Append($"Classe:{typeof(L).Name}|");

            if(type != null)
                log.Append($"Type:{type.Name}|");

            return log;
        }

        private StringBuilder GetLogErrorHeader(Type type)
        {
            var log = new StringBuilder();
            log.Append($"Aplicação:{APPLICATION}|");
            log.Append($"Log:Error|");
            log.Append($"Classe:{typeof(L).Name}|");

            if(type != null)
                log.Append($"Type:{type.Name}|");

            return log;
        }
    }
}