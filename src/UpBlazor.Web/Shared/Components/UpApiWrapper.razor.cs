using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Up.NET.Api;
using Up.NET.Models;

namespace UpBlazor.Web.Shared.Components
{
    // TODO: Move this to @typeparam when .NET 6 is out
    public partial class UpApiWrapper<T> : ComponentBase where T : class
    {

    }
}