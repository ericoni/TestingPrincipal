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
using Common;

namespace TestiranjePrincipala
{
	public class ShowPrincipalPermissionModeCustom
	{
	
		[ServiceBehavior]
		class SecureService : ISecureService
		{
			[PrincipalPermission(SecurityAction.Demand, Role = "everyone")]
			public string Method1(string request)
			{
				//IIdentity identity = null;
				//Thread.CurrentPrincipal = new CustomPrincipal(identity, "hacker", new string[] { "cccc" });
				return String.Format("Hello, {0}  \"{1}\" {2}", Thread.CurrentPrincipal.Identity.Name, ((CustomPrincipal)Thread.CurrentPrincipal).Username, ((CustomPrincipal)Thread.CurrentPrincipal).Roles[0]);
			}

			public string Method2()
			{
				WindowsIdentity callerWindowsIdentity = ServiceSecurityContext.Current.WindowsIdentity;
				Console.WriteLine(Thread.CurrentPrincipal.Identity.Name); 

				if (callerWindowsIdentity == null)
				{
					throw new InvalidOperationException
				   ("The caller cannot be mapped to a WindowsIdentity");
				}

				using (callerWindowsIdentity.Impersonate())
				{
					Console.WriteLine(Thread.CurrentPrincipal.Identity.Name);
				}
				return "Poz";
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

			Console.WriteLine("Starting server....");
			var ident = WindowsIdentity.GetCurrent();
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

				context.Properties["Principal"] = new CustomPrincipal(identities[0], "perica", new string[]{ "a", "b"});
				return true;
			}
		}

		class CustomPrincipal : IMyPrincipal
		{
			IIdentity identity;
			string username = string.Empty;
			string[] roles;

			public CustomPrincipal(IIdentity identity)
			{
				this.identity = identity;
			}

			public CustomPrincipal(IIdentity identity, string username)
			{
				this.identity = identity;
				this.username = username;
			}

			public CustomPrincipal(IIdentity identity, string username, string [] roles)
			{
				this.identity = identity;
				this.username = username;
				this.roles = roles;
			}

			public IIdentity Identity
			{
				get { return this.identity; }
			}

			public string Username
			{
				get { return this.username; }
				set { this.username = value; }
			}

			public string []Roles
			{
				get { return this.roles; }
				set { this.roles = value; }
			}

			public bool IsInRole(string role)
			{
				return true;
			}

			public WindowsImpersonationContext Impersonate()
			{
				throw new NotImplementedException();
			}
		}

		interface IMyPrincipal : IPrincipal
		{
			string Username { get; set; }

			WindowsImpersonationContext Impersonate();
		}
	}
}
