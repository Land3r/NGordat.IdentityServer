﻿// Copyright (c) Jan Škoruba. All Rights Reserved.
// Licensed under the Apache License, Version 2.0.

using Microsoft.AspNetCore.Identity;

using System;

namespace NGordat.Identity.Domain.Entities
{
    public class UserIdentityUserLogin<TKey> : IdentityUserLogin<TKey>
        where TKey : IEquatable<TKey>
    {
    }
    public class UserIdentityUserLogin : UserIdentityUserLogin<Guid>
    {
    }
}







