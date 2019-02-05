using System;
using System.Collections.Generic;
using System.IdentityModel.Claims;
using System.IdentityModel.Policy;
using System.Linq;
using System.Security.Permissions;
using System.Security.Principal;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TestiranjePrincipala
{
	public class ShowPrincipalPermissionModeCustom
	{
		[ServiceContract]
		interface ISecureService
		{
			[OperationContract]
			string Method1(string request);
		}

		[ServiceBehavior]
		class SecureService : ISecureService
		{
			[PrincipalPermission(SecurityAction.Demand, Role = "everyone")]
			public string Method1(string request)
			{
				return String.Format("Hello, {0}  \"{1}\"", Thread.CurrentPrincipal.Identity.Name, "aaa");
			}
		}

		public void Run()
		{
			Uri serviceUri = new Uri(@"http://localhost:8006/Service");
			ServiceHost service = new ServiceHost(typeof(SecureService));
			service.AddServiceEndpoint(typeof(ISecureService), GetBinding(), serviceUri);

			List<IAuthorizationPolicy> policies = new List<IAuthorizationPolicy>();
			policies.Add(new CustomAuthorizationPolicy());

			service.Authorization.ExternalAuthorizationPolicies = policies.AsReadOnly();
			service.Authorization.PrincipalPermissionMode = PrincipalPermissionMode.Custom;
			service.Open();

			EndpointAddress sr = new EndpointAddress(
				serviceUri, EndpointIdentity.CreateUpnIdentity(WindowsIdentity.GetCurrent().Name));
			ChannelFactory<ISecureService> cf = new ChannelFactory<ISecureService>(GetBinding(), sr);
			ISecureService client = cf.CreateChannel();
			Console.WriteLine("Client received response from Method1: {0}", client.Method1("hello"));

			((IChannel)client).Close();
			Console.ReadLine();
			service.Close();
		}

		public static Binding GetBinding()
		{
			WSHttpBinding binding = new WSHttpBinding(SecurityMode.Message);
			binding.Security.Message.ClientCredentialType = MessageCredentialType.Windows;
			return binding;
		}

		class CustomAuthorizationPolicy : IAuthorizationPolicy
		{
			string id = Guid.NewGuid().ToString();

			public string Id
			{
				get { return this.id; }
			}

			public ClaimSet Issuer
			{
				get { return ClaimSet.System; }
			}

			public bool Evaluate(EvaluationContext context, ref object state)
			{
				object obj;
				if (!context.Properties.TryGetValue("Identities", out obj))
					return false;

				IList<IIdentity> identities = obj as IList<IIdentity>;
				if (obj == null || identities.Count <= 0)
					return false;

				context.Properties["Principal"] = new CustomPrincipal(identities[0], "perica");
				return true;
			}
		}

		class CustomPrincipal : IPrincipal
		{
			IIdentity identity;
			string username;

			public CustomPrincipal(IIdentity identity)
			{
				this.identity = identity;
			}

			public CustomPrincipal(IIdentity identity, string username)
			{
				this.identity = identity;
				this.username = username;
			}

			public IIdentity Identity
			{
				get { return this.identity; }
			}

			public string Username
			{
				get { return this.username; }
			}

			public bool IsInRole(string role)
			{
				return true;
			}
		}
	}
}
