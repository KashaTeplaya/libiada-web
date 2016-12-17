﻿using LibiadaWeb;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin;

using Owin;

[assembly: OwinStartup(typeof(Startup))]

namespace LibiadaWeb
{
    /// <summary>
    /// The startup.
    /// </summary>
    public partial class Startup
    {
        /// <summary>
        /// The configuration.
        /// </summary>
        /// <param name="app">
        /// The app.
        /// </param>
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);

            app.MapSignalR();

            // Requiring auth for all signalR hubs
            GlobalHost.HubPipeline.RequireAuthentication();
        }
    }
}
