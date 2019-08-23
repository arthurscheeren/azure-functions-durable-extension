﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using System.Collections.Concurrent;
using System.Net.Http;
using System.Reflection;
using DurableTask.AzureStorage;
using DurableTask.Core;
using Microsoft.Azure.WebJobs.Description;
using Microsoft.Azure.WebJobs.Extensions.DurableTask.Azure;
using Microsoft.Azure.WebJobs.Extensions.DurableTask.Options;
using Microsoft.Azure.WebJobs.Host.Config;
using Microsoft.Azure.WebJobs.Logging;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Microsoft.Azure.WebJobs.Extensions.DurableTask
{
    /// <summary>
    /// Configuration for the Durable Functions extension.
    /// </summary>
#if NETSTANDARD2_0
    [Extension("DurableTask", "DurableTask")]
#endif
    public class DurableTaskExtensionAzureStorageConfig :
        DurableTaskExtensionBase,
        IExtensionConfigProvider
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="DurableTaskExtensionAzureStorageConfig"/>.
        /// </summary>
        /// <param name="options">The configuration options for this extension.</param>
        /// <param name="loggerFactory">The logger factory used for extension-specific logging and orchestration tracking.</param>
        /// <param name="nameResolver">The name resolver to use for looking up application settings.</param>
        /// <param name="orchestrationServiceFactory">The factory used to create orchestration service based on the configured storage provider.</param>
        /// <param name="durableHttpMessageHandlerFactory">The HTTP message handler that handles HTTP requests and HTTP responses.</param>
        public DurableTaskExtensionAzureStorageConfig(
            IOptions<DurableTaskAzureStorageOptions> options,
            ILoggerFactory loggerFactory,
            INameResolver nameResolver,
            IOrchestrationServiceFactory orchestrationServiceFactory,
            IDurableHttpMessageHandlerFactory durableHttpMessageHandlerFactory = null)
            : base(options.Value, loggerFactory, nameResolver, orchestrationServiceFactory, durableHttpMessageHandlerFactory)
        {
        }

#if !NETSTANDARD2_0
        internal DurableTaskExtensionAzureStorageConfig(
            IOptions<DurableTaskAzureStorageOptions> options,
            ILoggerFactory loggerFactory,
            INameResolver nameResolver,
            IOrchestrationServiceFactory orchestrationServiceFactory,
            IConnectionStringResolver connectionStringResolver,
            IDurableHttpMessageHandlerFactory durableHttpMessageHandlerFactory)
            : base(options.Value, loggerFactory, nameResolver, orchestrationServiceFactory, connectionStringResolver, durableHttpMessageHandlerFactory)
        {
        }
#endif

        /// <inheritdoc/>
        internal override void ConfigureLoaderHooks()
        {
#if !NETSTANDARD2_0
            AppDomain.CurrentDomain.AssemblyResolve += ResolveAssembly;
#endif
            base.ConfigureLoaderHooks();
        }

        private static Assembly ResolveAssembly(object sender, ResolveEventArgs args)
        {
            if (args.Name.StartsWith("DurableTask.AzureStorage"))
            {
                return typeof(AzureStorageOrchestrationService).Assembly;
            }

            return null;
        }

        /// <inheritdoc/>
        internal override DurableTaskOptions GetDefaultDurableTaskOptions()
        {
            return new DurableTaskAzureStorageOptions();
        }

        /// <inheritdoc/>
        internal override IDurableSpecialOperationsClient GetSpecialtyClient(TaskHubClient client)
        {
            return new DurableAzureStorageSpecialOperationsClient(client);
        }

        /// <summary>
        /// Internal initialization call from the WebJobs host.
        /// </summary>
        /// <param name="context">Extension context provided by WebJobs.</param>
        void IExtensionConfigProvider.Initialize(ExtensionConfigContext context)
        {
            this.Initialize(context);
        }
    }
}