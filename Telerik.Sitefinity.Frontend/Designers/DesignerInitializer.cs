﻿using System;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.UI;
using Telerik.Microsoft.Practices.Unity;
using Telerik.Sitefinity.Abstractions;
using Telerik.Sitefinity.Frontend.Resources;
using Telerik.Sitefinity.Services;
using Telerik.Sitefinity.Web.UI;

namespace Telerik.Sitefinity.Frontend.Designers
{
    using System.Diagnostics.CodeAnalysis;
    using System.Text;

    /// <summary>
    /// This class contains logic for initializing the MVC designer.
    /// </summary>
    public class DesignerInitializer
    {
        /// <summary>
        /// Initializes the MVC designer.
        /// </summary>
        public void Initialize()
        {
            if (RouteTable.Routes["MvcDesigner"] == null)
            {
                RouteTable.Routes.MapRoute("MvcDesigner", "Telerik.Sitefinity.Frontend/{controller}/{action}/{widgetName}", new { controller = "DesignerController", action = "Master" });
            }

            if (RouteTable.Routes["MvcDesignerView"] == null)
            {
                RouteTable.Routes.MapRoute("MvcDesignerView", "Telerik.Sitefinity.Frontend/{controller}/{action}/{widgetName}/{viewType}", new { controller = "DesignerController", action = "View", viewType = "PropertyGrid" });
            }

            ObjectFactory.Container.RegisterType<IDesignerResolver, DesignerResolver>(new ContainerControlledLifetimeManager());

            EventHub.Unsubscribe<IScriptsRegisteringEvent>(this.RegisteringScriptsHandler);
            EventHub.Subscribe<IScriptsRegisteringEvent>(this.RegisteringScriptsHandler);
        }

        /// <summary>
        /// Registering the scripts for ZoneEditor.
        /// </summary>
        /// <param name="event">The event.</param>
        private void RegisteringScriptsHandler(IScriptsRegisteringEvent @event)
        {
            if (@event.Sender.GetType() == typeof(ZoneEditor))
            {
                var scriptRootPath = "~/" + FrontendManager.VirtualPathBuilder.GetVirtualPath(this.GetType().Assembly);

                @event.Scripts.Add(new ScriptReference(scriptRootPath + "Mvc/Scripts/Angular/angular.min.js"));
                @event.Scripts.Add(new ScriptReference(scriptRootPath + "Mvc/Scripts/Angular/angular-resource.min.js"));
                @event.Scripts.Add(new ScriptReference(scriptRootPath + "Mvc/Scripts/Angular/angular-route.min.js"));
                @event.Scripts.Add(new ScriptReference(scriptRootPath + "Mvc/Scripts/Bootstrap/js/ui-bootstrap-tpls-0.11.0.min.js"));

                @event.Scripts.Add(new ScriptReference(scriptRootPath + "Designers/Scripts/page-editor-services.js"));
                @event.Scripts.Add(new ScriptReference(scriptRootPath + "Designers/Scripts/page-editor.js"));
                @event.Scripts.Add(new ScriptReference(scriptRootPath + "Mvc/Scripts/Kendo/angular-kendo.js"));

                var currentPackage = new PackageManager().GetCurrentPackage();
                if (!currentPackage.IsNullOrEmpty())
                {
                    var sb = new StringBuilder();
                    sb.AppendLine(@"Sys.Net.WebRequestManager.add_invokingRequest(function (executor, args) {");
                    sb.AppendLine("var url = args.get_webRequest().get_url();");
                    sb.AppendLine("if (url.indexOf('?') == -1)");
                    sb.AppendLine(" url += '?package=' + encodeURIComponent(sf_package);");
                    sb.AppendLine("else");
                    sb.AppendLine(" url += '&package=' + encodeURIComponent(sf_package); ");    
                    sb.AppendLine("args.get_webRequest().set_url(url); ");    
                    sb.AppendLine("});");    

                    var packageVar = "var sf_package = '{0}';".Arrange(currentPackage);
                    ((ZoneEditor)@event.Sender).Page.ClientScript.RegisterStartupScript(
                        @event.Sender.GetType(), 
                        "sf_package",
                        packageVar + sb,
                        addScriptTags: true);
                }
            }
        }
    }
}
