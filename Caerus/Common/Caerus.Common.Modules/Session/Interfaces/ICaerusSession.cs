﻿using Caerus.Common.Interfaces;
using Caerus.Common.Modules.Authentication.Interfaces;
using Caerus.Common.Modules.Client.Interfaces;
using Caerus.Common.Modules.Configuration.Interfaces;
using Caerus.Common.Modules.FieldMapping.Interfaces;
using Caerus.Common.Modules.Notification.Interfaces;

namespace Caerus.Common.Modules.Session.Interfaces
{
    public interface ICaerusSession
    {
        bool IsAuthenticated { get; }
        long CurrentUserRef { get; }
        string Email { get; }
        string CellNumber { get; }
        IAuthenticationService AuthenticationService { get; set; }
        IConfigurationService ConfigurationService { get; set; }
        IClientService ClientService { get; set; }
        IFieldMappingService FieldMappingService { get; set; }
        INotificationService NotificationService { get; set; }

        ICaerusLogger Logger { get; set; }
    }
}
