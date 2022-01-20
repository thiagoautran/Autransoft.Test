using System;
using Microsoft.Extensions.DependencyInjection;

namespace Autransoft.Test.Lib.Extensions
{
    public static class ParamsExtension
    {
        public static object Default(this Type type, IServiceCollection serviceCollection)
        {
            if (type == typeof(short))
                return default(short);

            if (type == typeof(ushort))
                return default(ushort);

            if (type == typeof(int))
                return default(int);

            if (type == typeof(uint))
                return default(uint);

            if (type == typeof(long))
                return default(long);

            if (type == typeof(ulong))
                return default(ulong);

            if (type == typeof(float))
                return default(float);

            if (type == typeof(double))
                return default(double);

            if (type == typeof(decimal))
                return default(decimal);

            if (type == typeof(string))
                return default(string);

            if (type == typeof(char))
                return default(char);

            var obj = serviceCollection.BuildServiceProvider().GetRequiredService(type);
            if(obj != null)
                return obj;

            return null;
        }
    }
}