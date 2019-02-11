using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
	[ServiceContract]
	public interface ISecureService
	{
		[OperationContract]
		string Method1(string request);
		[OperationContract]
		string Method2();
	}

}
