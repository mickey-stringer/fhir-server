﻿// -------------------------------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// -------------------------------------------------------------------------------------------------

using System;
using System.Threading;
using System.Threading.Tasks;
using EnsureThat;
using MediatR;
using Microsoft.Health.Fhir.Core.Features.Persistence;
using Microsoft.Health.Fhir.Core.Messages.Delete;
using Microsoft.Health.Fhir.Core.Models;

namespace Microsoft.Health.Fhir.Core.Features.Search.Parameters
{
    public class DeleteSearchParameterBehavior<TDeleteResourceRequest, TDeleteResourceResponse> : IPipelineBehavior<TDeleteResourceRequest, TDeleteResourceResponse>
        where TDeleteResourceRequest : DeleteResourceRequest
    {
        private ISearchParameterUtilities _searchParameterUtilities;
        private IFhirDataStore _fhirDataStore;

        public DeleteSearchParameterBehavior(ISearchParameterUtilities searchParameterUtilities, IFhirDataStore fhirDataStore)
        {
            EnsureArg.IsNotNull(searchParameterUtilities, nameof(searchParameterUtilities));
            EnsureArg.IsNotNull(fhirDataStore, nameof(fhirDataStore));

            _searchParameterUtilities = searchParameterUtilities;
            _fhirDataStore = fhirDataStore;
        }

        public async Task<TDeleteResourceResponse> Handle(TDeleteResourceRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TDeleteResourceResponse> next)
        {
            var deleteRequest = request as DeleteResourceRequest;
            ResourceWrapper searchParamResource = null;

            if (deleteRequest.ResourceKey.ResourceType.Equals(KnownResourceTypes.SearchParameter, StringComparison.Ordinal))
            {
                searchParamResource = await _fhirDataStore.GetAsync(deleteRequest.ResourceKey, cancellationToken);
            }

            var response = await next();

            if (searchParamResource != null)
            {
                // Once the SearchParameter resource is removed from the data store, we can update the in
                // memory SearchParameterDefinitionManager, and remove the status metadata from the data store
                await _searchParameterUtilities.DeleteSearchParameterAsync(searchParamResource.RawResource);
            }

            return response;
        }
    }
}
