﻿namespace EventManagementWebApp.Data.Contracts
{
    public interface ISoftDelete
    {
        bool IsDeleted { get; set; }
    }
}
