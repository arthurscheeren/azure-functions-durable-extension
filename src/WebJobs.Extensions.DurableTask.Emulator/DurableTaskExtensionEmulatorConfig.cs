﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using Microsoft.Azure.WebJobs.Description;
using Microsoft.Azure.WebJobs.Host.Config;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Azure.WebJobs.Extensions.DurableTask.Options;

namespace Microsoft.Azure.WebJobs.Extensions.DurableTask
{
    /// <summary>
    /// Configuration for the Durable Functions extension.
    /// </summary>
#if NETSTANDARD2_0
    [Extension("DurableTaskEmulator", "DurableTaskEmulator")]
#endif
    public class DurableTaskExtensionEmulatorConfig :
        DurableTaskExtensionBase,
        IExtensionConfigProvider
    {

        /// <inheritdoc />
        public DurableTaskExtensionEmulatorConfig(IOptions<DurableTaskEmulatorOptions> options,
            ILoggerFactory loggerFactory,
            INameResolver nameResolver,
            IOrchestrationServiceFactory orchestrationServiceFactory,
            IDurableHttpMessageHandlerFactory durableHttpMessageHandlerFactory = null)
            : base(options.Value, loggerFactory, nameResolver, orchestrationServiceFactory, durableHttpMessageHandlerFactory)
        {
        }

        /// <inheritdoc />
        public override DurableTaskOptions GetDefaultDurableTaskOptions()
        {
            return new DurableTaskEmulatorOptions();
        }

        void IExtensionConfigProvider.Initialize(ExtensionConfigContext context)
        {
            base.Initialize(context);
        }
    }
}
