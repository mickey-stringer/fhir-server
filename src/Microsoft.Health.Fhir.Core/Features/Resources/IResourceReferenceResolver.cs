﻿// -------------------------------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// -------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Health.Fhir.Core.Models;

namespace Microsoft.Health.Fhir.Core.Features.Resources
{
    public interface IResourceReferenceResolver
    {
        Task ResolveReferencesAsync(ResourceElement resource, Dictionary<string, (string resourceId, string resourceType)> referenceIdDictionary, string resourceType, CancellationToken cancellationToken);
    }
}