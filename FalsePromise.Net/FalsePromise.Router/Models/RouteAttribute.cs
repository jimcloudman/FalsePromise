using System;

namespace FalsePromise.Router.Models
{
	[AttributeUsage(AttributeTargets.Method)]
	public class RouteAttribute : Attribute
	{
		public string Name { get; set; }

		public RouteAttribute() { }

		public RouteAttribute(string name)
		{
			Name = name;
		}
	}

}
