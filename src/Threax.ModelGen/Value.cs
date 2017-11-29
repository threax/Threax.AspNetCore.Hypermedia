using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Threax.AspNetCore.Models;

public static partial class ModelDefs
{
    //[PluralName("ValueWoots")]
    public class Value
    {
        [Required]
        [MaxLength(450)]
        //[Display(Name = "WootName")]
        public String Name { get; set; }
    }
}