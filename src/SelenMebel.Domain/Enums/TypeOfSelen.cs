﻿using System.ComponentModel.DataAnnotations;

namespace SelenMebel.Domain.Enums
{
    public enum TypeOfSelen
    {

        [Display(Name = "HiTech")]
        HiTech = 1,

        [Display(Name = "Classic")]
        Classic = 2,

        [Display(Name = "Royal")]
        Royal = 3,

        [Display(Name = "None")]
        None = 4,
    }
}
