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
			Uri serviceUri = new Uri(@"http://localhost:8006/Service");

			EndpointAddress sr = new EndpointAddress(
				serviceUri, EndpointIdentity.CreateUpnIdentity(WindowsIdentity.GetCurrent().Name));
			ChannelFactory<ISecureService> cf = new ChannelFactory<ISecureService>(GetBinding(), sr);
			ISecureService client = cf.CreateChannel();
			Console.WriteLine("Client received response from Method1: {0}", client.Method1("hello"));

			Console.Read();
			((IChannel)client).Close();
		}
		public static Binding GetBinding()
		{
			WSHttpBinding binding = new WSHttpBinding(SecurityMode.Message);
			binding.Security.Message.ClientCredentialType = MessageCredentialType.Windows;
			return binding;
		}
	}
}
