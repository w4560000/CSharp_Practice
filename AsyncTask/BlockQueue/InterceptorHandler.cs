using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace Test
{/// <summary>
 /// 攔截器處理
 /// </summary>
	public class InterceptorHandler : IMessageSink
	{
		/// <summary>
		/// 攔截器調用層
		/// </summary>
		private string _callLayer;

		// 下一個接收器
		private IMessageSink _nextSink;

		/// <summary>
		/// 建構子
		/// </summary>
		/// <param name="callLayer">攔截器調用層</param>
		/// <param name="nextSink">下一個接收器</param>
		public InterceptorHandler(string callLayer, IMessageSink nextSink)
		{
			this._callLayer = callLayer;
			this._nextSink = nextSink;
		}

		/// <summary>
		/// 取得下一個接收器
		/// </summary>
		public IMessageSink NextSink
		{
			get { return _nextSink; }
		}

		/// <summary>
		/// 同步處理訊息
		/// </summary>
		/// <param name="msg">訊息接收</param>
		/// <returns>複合接收鏈結</returns>
		public IMessage SyncProcessMessage(IMessage msg)
		{
			// 攔截的訊息
			IMethodCallMessage interceptMsg = msg as IMethodCallMessage;


			// 收集參數資訊（參數名稱、值）
			IDictionary parameters = new Dictionary<string, object>();
			IList<string> parameterSignatures = new List<string>();
			for (int i = 0; i < interceptMsg.Args.Length; i++)
			{
				parameters.Add(interceptMsg.GetInArgName(i), interceptMsg.GetArg(i));
			}

			// 解析方法簽章（變數型態）
			foreach (Type signature in (Array)interceptMsg.MethodSignature)
			{
				parameterSignatures.Add(signature.FullName);
			}

			Console.WriteLine("catch");

			return this.NextSink.SyncProcessMessage(msg);
		}

		/// <summary>
		/// 非同步處理訊息（不需要）
		/// </summary>
		public IMessageCtrl AsyncProcessMessage(IMessage msg, IMessageSink replySink) { return null; }
	}
}
