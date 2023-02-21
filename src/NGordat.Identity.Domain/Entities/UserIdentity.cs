﻿// Copyright (c) Jan Škoruba. All Rights Reserved.
// Licensed under the Apache License, Version 2.0.

using Microsoft.AspNetCore.Identity;

namespace NGordat.Identity.Domain.Entities
{
    public class UserIdentity<TKey> : IdentityUser<TKey>
        where TKey: IEquatable<TKey>
    {

    }
}






