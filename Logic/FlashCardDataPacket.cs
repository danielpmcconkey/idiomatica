﻿using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic
{
    public class FlashCardDataPacket
    {
        public string? LearningLanguageCode { get; set; } = null;
        public string? UILanguageCode { get; set; } = null;
        public List<FlashCard> Deck { get; set; } = new();
        public int CardCount { get; set; } = 0;
        public int CurrentCardPosition { get; set; } = 0;
        public FlashCard? CurrentCard { get; set; } = null;
        







        #region deck properties







        #endregion

        #region current card properties

        public FlashCardParagraphTranslationBridge? datapacketBridge { get; set; } = null;
        

        #endregion


        
    }
}
