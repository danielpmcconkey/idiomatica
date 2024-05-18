using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.UILabels
{
	public abstract class UILabels // todo: learn the right way to do UI translation
	{
		protected Dictionary<string, string> _labels;

		public UILabels()
		{
			_labels = new Dictionary<string, string>();
			FillLabelsDict();
		}
		protected abstract void FillLabelsDict();
        public abstract string GetLabel(string name);
        public abstract string GetLabelF(string name, object?[] args);
    }
}
