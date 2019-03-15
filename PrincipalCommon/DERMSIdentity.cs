using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace PrincipalCommon
{
	public class DERMSIdentity : IIdentity, IDisposable
	{
		public string AuthenticationType
		{
			get
			{
				return "fakeAuthType";
			}
		}

		public bool IsAuthenticated
		{
			get
			{
				return true;
			}
		}

		public string Name
		{
			get
			{
				return this.Name;
			}
		}

		public void Dispose()
		{
		}
		public WindowsImpersonationContext Impersonate()
		{
			return (WindowsImpersonationContext)null;
		}
	}
}
