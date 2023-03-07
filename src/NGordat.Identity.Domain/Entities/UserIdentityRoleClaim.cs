// Copyright (c) Jan Škoruba. All Rights Reserved.
// Licensed under the Apache License, Version 2.0.

using Microsoft.AspNetCore.Identity;

using System;
namespace NGordat.Identity.Domain.Entities
{
    public class UserIdentityRoleClaim<TKey> : IdentityRoleClaim<TKey>
        where TKey : IEquatable<TKey>
    {
    }
    public class UserIdentityRoleClaim : UserIdentityRoleClaim<Guid>
    {
    }
}







