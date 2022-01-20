using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace Autransoft.Test.Lib.Extensions
{
    public static class ParamsExtension
    {
        public static object[] GetParams<CLASS>(IServiceCollection serviceCollection)
            where CLASS : class
        {
            var types = new List<object>();

            var listConstructor = typeof(CLASS).GetConstructors();
            foreach(var contructor in listConstructor.Where(contructor => contructor.IsPublic))
            {
                var listParam = contructor.GetParameters();
                types.AddRange(listParam.Select(param => param.ParameterType.Default(serviceCollection)));
                break;
            }

            return types.ToArray();
        }

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