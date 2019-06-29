using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Common;
using System.ServiceModel.Channels;

namespace ClientTemp
{
	class Program
	{
		static void Main(string[] args)
		{
			//Uri serviceUri = new Uri(@"http://localhost:8006/Service");

			//EndpointAddress sr = new EndpointAddress(
			//	serviceUri, EndpointIdentity.CreateUpnIdentity(WindowsIdentity.GetCurrent().Name));
			//ChannelFactory<ISecureService> cf = new ChannelFactory<ISecureService>(GetBinding(), sr);

			//ISecureService client = cf.CreateChannel();
			//Console.WriteLine("Client received response from Method2: {0}", client.Method2());
			//Console.WriteLine("Client received response from Method2: {0}", client.Method2());

			////Console.WriteLine("Client received response from Method1: {0}", client.Method1("hello"));

			////Console.WriteLine("Client received response from Method1: {0}", client.Method1("hello again"));

			//Console.Read();
			//((IChannel)client).Close();
			Area a = new Area("a");
			Area b = new Area("b");
			Area c = new Area("c");
			Area d = new Area("d");
			Area e = new Area("e");
			Area f = new Area("f");
			Area g = new Area("g");

			a.Areas.Add(b);

			b.Areas.Add(c);

			c.Areas.Add(d);
			c.Areas.Add(e);

			d.Areas.Add(f);
			d.Areas.Add(g);

			IterateThroughAll(a);

			Console.Read();
		}

		public static void IterateThroughAll(Area area)
		{
			foreach (var subarea in area.Areas)
			{
				subarea.Usao = true;
				IterateThroughAll(subarea);
			}
		}

		public static Binding GetBinding()
		{
			WSHttpBinding binding = new WSHttpBinding(SecurityMode.Message);
			binding.Security.Message.ClientCredentialType = MessageCredentialType.Windows;
			return binding;
		}
	}
}
