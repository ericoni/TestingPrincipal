using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace TestiranjePrincipala
{
	class Program
	{
		static void Main(string[] args)
		{
			try
			{
				//ShowPrincipalPermissionModeCustom ppwm = new ShowPrincipalPermissionModeCustom();
				//ppwm.Run(); // to do uncomment later

				// Use values from the current WindowsIdentity to construct
				// a set of GenericPrincipal roles.
				var username = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
				Console.WriteLine(username);

				username = username.Substring(username.IndexOf("\\")+1);
				Console.WriteLine("posle je " + username);
				Console.Read();
			}
			catch (Exception exc)
			{
				Console.WriteLine("Error: {0}", exc.Message);
				Console.ReadLine();
			}
		}
	}
}
