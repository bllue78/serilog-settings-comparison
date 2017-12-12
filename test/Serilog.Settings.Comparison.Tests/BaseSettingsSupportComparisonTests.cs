﻿using System;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Serilog.Settings.Code;
using Xunit.Abstractions;

namespace Serilog.Settings.C.Tests.SettingsComparison.Tests
{
    public abstract class BaseSettingsSupportComparisonTests
    {
        readonly ITestOutputHelper _outputHelper;

        protected BaseSettingsSupportComparisonTests(ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
        }

        protected void WriteDocumentation(string fileName)
        {
            var fullFilePath = GetTestFileFullPath(fileName);

            _outputHelper.WriteLine($"ex: `{fileName}`");
            _outputHelper.WriteLine("");
            _outputHelper.WriteLine("```" + GetMarkdownSnippetLanguage(fileName));
            _outputHelper.WriteLine(File.ReadAllText(fullFilePath));
            _outputHelper.WriteLine("```");
            _outputHelper.WriteLine("");
        }

        static string GetMarkdownSnippetLanguage(string fileName)
        {
            var mdOuputFormat = fileName.Split('.').Last();
            if (mdOuputFormat == "csx") mdOuputFormat = "csharp";
            if (mdOuputFormat == "config") mdOuputFormat = "xml";
            return mdOuputFormat;
        }

        public static LoggerConfiguration LoadConfig(string fileName)
        {
            var fullFilePath = GetTestFileFullPath(fileName);
            if (fileName.EndsWith(".json")) return LoadJsonConfig(fullFilePath);
            if (fileName.EndsWith(".config")) return LoadXmlConfig(fullFilePath);
            if (fileName.EndsWith(".csx")) return LoadCSharpConfig(fullFilePath);
            throw new ArgumentException($"Only .json and .config are supported. Provided value : {fileName}", nameof(fileName));
        }

        static string GetTestFileFullPath(string fileName)
        {
            var fullFilePath = Path.Combine("Samples", fileName);
            return fullFilePath;
        }

        static LoggerConfiguration LoadXmlConfig(string fileName)
        {
            var xmlConfig = new LoggerConfiguration().ReadFrom.AppSettings(filePath: fileName);
            return xmlConfig;
        }

        static LoggerConfiguration LoadJsonConfig(string fileName)
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile(fileName, optional: false)
                .Build();

            var jsonConfig = new LoggerConfiguration().ReadFrom.Configuration(config);
            return jsonConfig;
        }

        static LoggerConfiguration LoadCSharpConfig(string fileName)
        {
            var code = File.ReadAllText(fileName);

            var jsonConfig = new LoggerConfiguration().ReadFrom.CodeString(code);
            return jsonConfig;
        }

    }
}