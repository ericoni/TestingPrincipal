using System;
using System.Collections.Generic;
using System.Linq;
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
				ShowPrincipalPermissionModeCustom ppwm = new ShowPrincipalPermissionModeCustom();
				ppwm.Run();

			}
			catch (Exception exc)
			{
				Console.WriteLine("Error: {0}", exc.Message);
				Console.ReadLine();
			}
		}
	}
}
