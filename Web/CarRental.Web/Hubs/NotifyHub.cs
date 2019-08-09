using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CarRental.Data;
using Microsoft.AspNetCore.SignalR;

namespace CarRental.Web.Hubs
{
    public class NotifyHub : Hub
    {
        public NotifyHub()
        {}
    }
}
