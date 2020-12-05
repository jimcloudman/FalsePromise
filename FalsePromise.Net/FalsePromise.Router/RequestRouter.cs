using FalsePromise.Router.Models;
using Jil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("FalsePromise.Router.Tests")]

namespace FalsePromise.Router
{
	// TODO: Jil deserialization doesn't fail if parameter not specified; need to catch it
	public class RequestRouter
    {
		internal readonly Dictionary<string, object> _serviceCollection = new Dictionary<string, object>();
		internal readonly Dictionary<string, List<RouterMethod>> _typeCollection = new Dictionary<string, List<RouterMethod>>();

		public void Register<T>(T service)
		{
			var type = typeof(T);
			if (_serviceCollection.Any(s => s.Key.StartsWith(type.Name) && s.Value.GetType() == type))
			{
				throw new RouterException($"Service {type.Name} already registered");
			}
			try
			{
				var methods = typeof(T).GetMethods().Where(r => r.CustomAttributes.Any(a => a.AttributeType == typeof(RouteAttribute)));
				foreach (var method in methods)
				{
					var routeAttr = (RouteAttribute)method.GetCustomAttribute(typeof(RouteAttribute));
					var methodName = routeAttr.Name ?? method.Name;
					var route = $"{typeof(T).Name}.{methodName}";
					if (!_serviceCollection.ContainsKey(route))
					{
						_serviceCollection.Add(route, service);
					}
					BuildParameterWrapper(route, method, methodName);
				}
			}
			catch (Exception ex)
			{
				throw new RouterException("Unable to register service", ex);
			}
		}

		public string Execute(string jsonString)
		{
			RouterRequest result;
			try
			{
				result = JSON.Deserialize<RouterRequest>(jsonString, Options.CamelCase);
			}
			catch (Exception ex)
			{
				throw new RouterException("Failed to deserialize JSON string", ex);
			}

			object service;
			try
			{
				service = _serviceCollection.Single(s => s.Key == result.Route).Value;
			}
			catch (Exception ex)
			{
				throw new RouterException("No registered service matching route", ex);
			}

			List<RouterMethod> methods;
			try
			{
				methods = _typeCollection.Single(s => s.Key == result.Route).Value;
			}
			catch (Exception ex)
			{
				throw new RouterException("No registered type matching route", ex);
			}

			throw new NotImplementedException();

			//// TODO: more error handling
			//var request = JSON.Deserialize(result.Parameters, method.ParameterType);

			//var values = method.ParameterType.GetProperties().ToDictionary(p => p.Name, p => p.GetValue(request));
			//var methodParameters = method.MethodInfo.GetParameters();
			//var parameterValues = methodParameters.Select(p => values[p.Name]).ToArray();
			//var methodResult = method.MethodInfo.Invoke(service, parameterValues);

			//return JSON.Serialize(methodResult);
		}

		private void BuildParameterWrapper(string route, MethodInfo method, string methodName)
		{
			// Below: https://stackoverflow.com/a/3862241/1074198

			// Get TypeBuilder
			var aName = new AssemblyName(methodName);
			var aBuilder = AssemblyBuilder.DefineDynamicAssembly(
				aName,
				AssemblyBuilderAccess.Run);
			var mBuilder = aBuilder.DefineDynamicModule("MainModule");
			var tBuilder = mBuilder.DefineType(methodName,
				TypeAttributes.Public | TypeAttributes.Class | TypeAttributes.AutoClass
				| TypeAttributes.AnsiClass | TypeAttributes.BeforeFieldInit | TypeAttributes.AutoLayout,
				null);

			// Build default constructor
			tBuilder.DefineDefaultConstructor(
				MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.RTSpecialName);

			// Add properties to type
			var methodParams = method.GetParameters();
			foreach (var para in methodParams)
			{
				CreateProperty(tBuilder, para.Name, para.ParameterType);
			}

			var newType = tBuilder.CreateType();
			var newMethod = new RouterMethod
			{
				MethodInfo = method,
				ParameterType = newType
			};
			if (_typeCollection.ContainsKey(route))
            {
				_typeCollection[route].Add(newMethod);
            }
			else
			{
				_typeCollection.Add(route, new List<RouterMethod> { newMethod });
			}
		}


		// Also https://stackoverflow.com/a/3862241/1074198
		private static void CreateProperty(TypeBuilder tb, string propertyName, Type propertyType)
		{
			FieldBuilder fieldBuilder = tb.DefineField("_" + propertyName, propertyType, FieldAttributes.Private);

			PropertyBuilder propertyBuilder = tb.DefineProperty(propertyName, PropertyAttributes.HasDefault, propertyType, null);
			MethodBuilder getPropMthdBldr = tb.DefineMethod("get_" + propertyName, MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig, propertyType, Type.EmptyTypes);
			ILGenerator getIl = getPropMthdBldr.GetILGenerator();

			getIl.Emit(OpCodes.Ldarg_0);
			getIl.Emit(OpCodes.Ldfld, fieldBuilder);
			getIl.Emit(OpCodes.Ret);

			MethodBuilder setPropMthdBldr =
				tb.DefineMethod("set_" + propertyName,
				  MethodAttributes.Public |
				  MethodAttributes.SpecialName |
				  MethodAttributes.HideBySig,
				  null, new[] { propertyType });

			ILGenerator setIl = setPropMthdBldr.GetILGenerator();
			Label modifyProperty = setIl.DefineLabel();
			Label exitSet = setIl.DefineLabel();

			setIl.MarkLabel(modifyProperty);
			setIl.Emit(OpCodes.Ldarg_0);
			setIl.Emit(OpCodes.Ldarg_1);
			setIl.Emit(OpCodes.Stfld, fieldBuilder);

			setIl.Emit(OpCodes.Nop);
			setIl.MarkLabel(exitSet);
			setIl.Emit(OpCodes.Ret);

			propertyBuilder.SetGetMethod(getPropMthdBldr);
			propertyBuilder.SetSetMethod(setPropMthdBldr);
		}
	}
}
