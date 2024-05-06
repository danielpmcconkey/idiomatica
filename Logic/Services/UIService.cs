using Model;
using Logic.UILabels;

namespace Logic.Services
{
    public class UIService
    {
        private UILabels.UILabels _uiLabels;

        public UIService() 
        {
            _uiLabels = Factory.GetUILabels(UILanguage.ENG_US); // todo: create a shared UILabels
        }
        public string GetLabel(string name)
        {
            return _uiLabels.GetLabel(name);
        }
        
        public string GetTokenClass(AvailableWordUserStatus status)
        {
            if (status == AvailableWordUserStatus.UNKNOWN) return "statusUNKNOWN";
            if (status == AvailableWordUserStatus.NEW1) return "statusNEW1";
            if (status == AvailableWordUserStatus.NEW2) return "statusNEW2";
            if (status == AvailableWordUserStatus.LEARNING3) return "statusLEARNING3";
            if (status == AvailableWordUserStatus.LEARNING4) return "statusLEARNING4";
            if (status == AvailableWordUserStatus.LEARNED) return "statusLEARNED";
            if (status == AvailableWordUserStatus.WELLKNOWN) return "statusWELLKNOWN";
            if (status == AvailableWordUserStatus.IGNORED) return "statusIGNORED";
            return "statusUNKNOWN";
        }
    }
}
