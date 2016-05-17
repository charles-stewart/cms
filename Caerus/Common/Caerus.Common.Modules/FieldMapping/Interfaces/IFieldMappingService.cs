﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caerus.Common.Modules.FieldMapping.Enums;

namespace Caerus.Common.Modules.FieldMapping.Interfaces
{
    public interface IFieldMappingService
    {
        bool IsValid(FieldValidationTypes type, string value, string validationValue);
    }
}
