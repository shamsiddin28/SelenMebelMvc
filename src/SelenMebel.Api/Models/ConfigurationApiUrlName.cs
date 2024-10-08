﻿using System.Text.RegularExpressions;

namespace SelenMebel.Api.Models
{
    public class ConfigurationApiUrlName
    {
        public string TransformOutbound(object value)
        {
            return value == null ? null : Regex.Replace(value.ToString(), "([a-z])([A-Z])", "$1-$2").ToLower();
        }
    }
}
