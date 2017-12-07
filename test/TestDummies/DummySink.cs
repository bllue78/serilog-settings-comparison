﻿using System;
using System.Collections.Generic;
using Serilog.Core;
using Serilog.Events;

namespace TestDummies
{
    /// <summary>
    /// Just a dummy sink that allows to verify that the proper params are passed from configuration
    /// </summary>
    public class DummySink : ILogEventSink
    {

        public DummySink(string stringParam, int intParam, string stringParamWithDefault, int? nullableIntParam)
        {
            Emitted.Clear();
            StringParam = stringParam;
            IntParam = intParam;
            StringParamWithDefault = stringParamWithDefault;
            NullableIntParam = nullableIntParam;
        }

        [ThreadStatic]
        public static string StringParam;
        [ThreadStatic]
        public static int IntParam;
        [ThreadStatic]
        public static string StringParamWithDefault;
        [ThreadStatic]
        public static int? NullableIntParam;


        [ThreadStatic]
        // ReSharper disable ThreadStaticFieldHasInitializer
        public static List<LogEvent> Emitted = new List<LogEvent>();
        // ReSharper restore ThreadStaticFieldHasInitializer

        public void Emit(LogEvent logEvent)
        {
            Emitted.Add(logEvent);
        }
    }
}