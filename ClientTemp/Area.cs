using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientTemp
{
	public class Area
	{
		string name;
		List<Area> areas;
		List<Area> testne;
		bool usao = false;

		public Area()
		{
			this.areas = new List<Area>(5);
			this.testne = new List<Area>(5);

		}
		public Area(string name)
		{
			this.name = name;
			this.areas = new List<Area>(5);
			this.testne = new List<Area>(5);
		}

		public List<Area> Areas
		{
			get { return areas; }
			set { areas = value; }
		}

		public List<Area> Testne
		{
			get { return testne; }
			set { testne = value; }
		}
		public string Name
		{
			get { return name; }
			set { name = value; }
		}

		public bool Usao
		{
			get { return usao; }
			set { usao = value; }
		}
		public override string ToString()
		{
			return "Area_" + name;
		}
	}
}
