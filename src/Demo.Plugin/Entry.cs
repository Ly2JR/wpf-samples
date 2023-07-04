﻿using Demo.Plugin.Views;
using Prism.Ioc;
using Prism.Modularity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Plugin
{
    public class Entry : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {

        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<HelloView>("AA01");
            containerRegistry.RegisterForNavigation<WorldView>("SS010101");
        }
    }
}
