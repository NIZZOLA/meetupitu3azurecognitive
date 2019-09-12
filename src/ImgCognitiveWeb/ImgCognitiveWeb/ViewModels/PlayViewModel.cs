using ImageRecognition.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImgCognitiveWeb.ViewModels
{
    public class PlayViewModel
    {
        public Models.Picture Picture { get; set; }

        public PictureAnalisysReturn AnalysisReturn { get; set; }
    }
}
