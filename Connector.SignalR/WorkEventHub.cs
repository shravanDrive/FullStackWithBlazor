// <copyright file="EventHub.cs" company="SELF">
// Copyrght (c) SELF. All rights reserved.
// </copyright>

namespace Connector.SignalR
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.SignalR;

    /// <summary>
    /// EventHub for Signal R
    /// </summary>
    public class WorkEventHub : Hub
    {
        /// <summary>
        /// SendMessage to all logged in users
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task SendMessage(string userName, string message)
        {
            await this.Clients.All.SendAsync("searchItemCreated", userName, message).ConfigureAwait(false);
        }
    }
}
