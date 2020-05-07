using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

namespace AppShapes.Core.Testing.Core
{
    public class ConfigurationFactory
    {
        public virtual IConfiguration Create(params ValueTuple<string, string>[] settings)
        {
            IDictionary<string, string> dictionary = new Dictionary<string, string>();
            foreach ((string key, string value) in settings)
                dictionary.Add(key, value);
            return new ConfigurationBuilder().AddInMemoryCollection(dictionary).Build();
        }
    }
}