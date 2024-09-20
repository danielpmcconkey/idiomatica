using Model;
using Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.UILabels
{
	
	public static class Factory
	{
		public static UILabels GetUILabels(AvailableLanguageCode languageCodeEnum)
		{
			if(languageCodeEnum == AvailableLanguageCode.EN_US) return new Labels_EngUS();
			return new Labels_EngUS();
		}
	}
}
