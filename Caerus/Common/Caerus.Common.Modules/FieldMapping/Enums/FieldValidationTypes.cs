﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Caerus.Common.Modules.FieldMapping.Enums
{
    public enum FieldValidationTypes
    {
        Required = 1,       
        MinLength = 2,       
        MaxLength = 3,      
        RangeLength = 4,     
        MinValue = 5,             
        MaxValue = 6,             
        Range = 7,           
        Email = 8,           
        Url = 9,           
        Date = 10,            
        Number = 11,          
        Digits = 12,         
        CreditCard = 13,     
        EqualTo = 14,        
        IdentificationNumber = 15, 
        SpecificLength = 16, 
        Regex = 17,          
        MinAge = 18, 
        MaxAge = 19, 
        MinMonths = 20, 
        MaxMonths = 21,
        RsaIdNumber = 22

    }
}
