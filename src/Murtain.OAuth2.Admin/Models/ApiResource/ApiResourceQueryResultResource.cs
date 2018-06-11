﻿/*
 * Copyright 2014 Dominick Baier, Brock Allen, Bert Hoorne
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *   http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System;
using System.Collections.Generic;
using IdentityAdmin.Core;
using IdentityAdmin.Core.ApiResource;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc;

namespace IdentityAdmin.Api.Models.ApiResource
{
    class ApiResourceQueryResultResource
    {
        /// <summary>
        /// Needed for Unit Testing
        /// </summary>
        public ApiResourceQueryResultResource()
        {
        }

        public ApiResourceQueryResultResource(QueryResult<ApiResourceSummary> result, IUrlHelper url, ApiResourceMetaData meta)
        {
            if (result == null) throw new ArgumentNullException(nameof(result));
            if (url == null) throw new ArgumentNullException(nameof(url));
            if (meta == null) throw new ArgumentNullException(nameof(meta));

            Data = new ApiResourceQueryResultResourceData(result, url, meta);

            var links = new Dictionary<string, object>();
            if (meta.SupportsCreate)
            {
                links["create"] = new CreateApiResourceLink(url, meta);
            };
            Links = links;
        }

        public ApiResourceQueryResultResourceData Data { get; set; }
        public object Links { get; set; }
    }
}
