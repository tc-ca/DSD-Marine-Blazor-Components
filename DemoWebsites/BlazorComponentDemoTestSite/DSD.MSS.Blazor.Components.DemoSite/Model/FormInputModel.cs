using DSD.MSS.Blazor.Components.Core.Demo.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DSD.MSS.Blazor.Components.Core.Demo.Model
{
    public class FormInputModel
    {
       
        public int ID { get; set; }

        [Required]
        public string StringValue { get; set; }

        [Required]
        public int IntValue { get; set; }

        [Required]
        public DateTime? DateValue { get; set; }

        [Required]
        public DateTime? DateValueNull { get; set; }

        public DateTime? DateValueFromatted { get; set; }

        [Required]
        public bool BoolValue { get; set; }

        [Required]
        public double DoubleValue { get; set; }

        [Required]
        public float FloatValue { get; set; }

        [Required]
        public decimal DecimalValue { get; set; }

        [Required]
        public Guid GuidValue { get; set; }

        public StatusEnum EnumValue { get; set; }

        public int SelectValue { get; set; }

        public FormInputModel()
        {
            ID = 0;
            StringValue = "String Value";
            IntValue = 100;
            DateValue = new DateTime(2021,8, 18, 11, 23, 12);
            BoolValue = true;
            DoubleValue = 5.1;
            FloatValue = 5.001F;
            DecimalValue = 5.002M;
            GuidValue = Guid.NewGuid();
            EnumValue = StatusEnum.New;
            SelectValue = -1;
        }
    }
}
