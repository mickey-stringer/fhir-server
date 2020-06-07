﻿// -------------------------------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// -------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Microsoft.Health.Fhir.Core.Serialization.SourceNodes
{
    [SuppressMessage("Design", "CA2227", Justification = "POCO style model")]
    public class ResourceBase : IExtensionData
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("resourceType")]
        public string ResourceType { get; set; }

        [JsonPropertyName("meta")]
        public MetaBase Meta { get; set; } = new MetaBase();

        [JsonExtensionData]
        public IDictionary<string, JsonElement> ExtensionData { get; set; }
    }
}