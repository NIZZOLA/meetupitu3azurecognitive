﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ImgCognitiveWeb.Models
{
    public class Picture
    {
        [Key]
        public int PictureId { get; set; }
        [MaxLength(100, ErrorMessage = "Tamanho máximo do campo é de {0} caracteres")]
        public string Description { get; set; }
        [MaxLength(200, ErrorMessage = "Tamanho máximo do campo é de {0} caracteres")]
        public string Address { get; set; }
        public bool Storage { get; set; }
        public bool Status { get; set; }
        public string Result { get; set; }

    }
}
