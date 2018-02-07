﻿using Nancy.ViewEngines.SuperSimpleViewEngine;
using System;
using System.Text.RegularExpressions;

namespace NancyFx.SSVE.Extender
{
    internal sealed class EqualTokenViewEngineMatcher : ISuperSimpleViewEngineMatcher
    {
        private static readonly Regex EqualSubstitutionsRegEx;

        static EqualTokenViewEngineMatcher()
        {
            EqualSubstitutionsRegEx = new Regex(@"@Equal(?<Not>Not)?\.(?<LeftItem>[a-zA-Z0-9-_]+)?[\|](?<RightItem>[a-zA-Z0-9-_]+)?[\|](?<ResultItem>[a-zA-Z0-9-_]+)?", RegexOptions.Compiled);
        }

        public string Invoke(string content, dynamic model, IViewEngineHost host)
        {
            return EqualSubstitutionsRegEx.Replace(
                content,
                m =>
                {
                    var LeftItem = m.Groups["LeftItem"].Value;
                    var RightItem = m.Groups["RightItem"].Value;
                    var NotItem = m.Groups["Not"].Value;
                    var ResultItem = m.Groups["ResultItem"].Value;

                    if (LeftItem != null && RightItem != null)
                    {
                        if (LeftItem == RightItem && String.IsNullOrEmpty(NotItem))
                        {
                            return ResultItem;
                        };
                        if (LeftItem != RightItem && !String.IsNullOrEmpty(NotItem))
                        {
                            return ResultItem;
                        };
                        return "";
                    }
                    else
                    {
                        return "";
                    }
                });
        }
    }
}