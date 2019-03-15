using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace PrincipalCommon
{
	public class DERMSPrincipal : IDERMSPrincipal
	{
		DERMSIdentity identity = null;

		public IIdentity Identity
		{
			get
			{
				return this.identity;
			}

		}

		public void Dispose()
		{
			
		}

		public WindowsImpersonationContext Impersonate()
		{
			return identity.Impersonate();
		}

		public bool IsInRole(string role)
		{
			return true; //TODO: vratiti se
		}
	}
}
