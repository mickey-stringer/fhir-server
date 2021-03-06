﻿// -------------------------------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// -------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using EnsureThat;
using Hl7.Fhir.ElementModel;
using Hl7.Fhir.FhirPath;
using Hl7.Fhir.Model;
using Hl7.Fhir.Specification;
using Hl7.FhirPath;
using Microsoft.Health.Fhir.Core.Models;

namespace Microsoft.Health.Fhir.Core
{
    /// <summary>
    /// Provides agnostic access to FHIR Models and Resources
    /// </summary>
    public partial class VersionSpecificModelInfoProvider : IModelInfoProvider
    {
        public Version SupportedVersion { get; } = new Version(ModelInfo.Version);

        public IStructureDefinitionSummaryProvider StructureDefinitionSummaryProvider { get; } = new PocoStructureDefinitionSummaryProvider();

        public string GetFhirTypeNameForType(Type type)
        {
            return ModelInfo.GetFhirTypeNameForType(type);
        }

        public bool IsKnownResource(string name)
        {
            return ModelInfo.IsKnownResource(name);
        }

        public bool IsKnownCompartmentType(string compartmentType)
        {
            return Enum.IsDefined(typeof(CompartmentType), compartmentType);
        }

        public IReadOnlyCollection<string> GetResourceTypeNames()
        {
            List<string> supportedResources = ModelInfo.SupportedResources;

            if (Version == FhirSpecification.R5)
            {
                supportedResources = supportedResources.Where(x => x != "CanonicalResource" && x != "MetadataResource").ToList();
            }

            return supportedResources;
        }

        public IReadOnlyCollection<string> GetCompartmentTypeNames()
        {
            return Enum.GetNames(typeof(CompartmentType));
        }

        public Type GetTypeForFhirType(string resourceType)
        {
            return ModelInfo.GetTypeForFhirType(resourceType);
        }

        public EvaluationContext GetEvaluationContext(Func<string, ITypedElement> elementResolver = null)
        {
            return new FhirEvaluationContext
            {
                ElementResolver = elementResolver,
            };
        }

        public ITypedElement ToTypedElement(ISourceNode sourceNode)
        {
            EnsureArg.IsNotNull(sourceNode);

            return sourceNode.ToTypedElement(StructureDefinitionSummaryProvider);
        }
    }
}
