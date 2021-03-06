﻿using System.Collections.Generic;
using Caerus.Common.Logging;
using Caerus.Common.Modules.FieldMapping.Enums;
using Caerus.Common.Modules.FieldMapping.Interfaces;
using Caerus.Common.Modules.FieldMapping.ViewModels;
using Caerus.Common.ViewModels;

namespace Caerus.Common.Stub.BaseServices
{
   public class StubDynamicService : IDynamicService
    {
       public OwningTypes OwningType {
           get
           {
               return OwningTypes.Unknown;
           }
       }

       public List<DynamicEntityViewModel> GetEntityModelsByTypes(List<int> requiredEntityTypes, long owningEntityRef)
       {
           GlobalLogger.WrapStubInfo();
           return new List<DynamicEntityViewModel>();
       }

       public ReplyObject SaveEntityFields(long owningEntityRef, List<DynamicResponseDataModel> entities)
       {
           GlobalLogger.WrapStubInfo();
           return new ReplyObject();
       }
    }
}
