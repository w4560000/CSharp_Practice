using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
	/// <summary>
	/// Service 攔截器擴增屬性
	/// </summary>
	//[AttributeUsage(AttributeTargets.Method)]
	public class InterceptorOfServiceAttribute : ContextAttribute, IContributeObjectSink
	{
		/// <summary>
		/// 調用層
		/// </summary>
		private readonly string layer = "SERVICE";

		/// <summary>
		/// 建構子
		/// </summary>
		public InterceptorOfServiceAttribute() : base("InterceptorOfServiceAttribute") { }

		/// <summary>
		/// 接收指定鏈結的訊息
		/// </summary>
		/// <param name="obj">提供的訊息接收是鏈結前面指定的鏈結</param>
		/// <param name="nextSink">目前為止所撰寫之接收鏈結</param>
		/// <returns>複合接收鏈結</returns>
		public IMessageSink GetObjectSink(MarshalByRefObject obj, IMessageSink nextSink)
		{
			return new InterceptorHandler(this.layer, nextSink);
		}
	}
}
