using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.UILabels
{
	public enum UILanguage { ENG_US , ENG_GB }
	public static class UILabelFactory
	{
		public static UILabels GetUILabels(UILanguage language)
		{
			if(language == UILanguage.ENG_US) return new Labels_EngUS();
			if (language == UILanguage.ENG_GB) return new Labels_EngUS();
			return new Labels_EngUS();
		}
	}
}
