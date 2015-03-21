﻿using System;
using System.Web.Mvc;

namespace Telerik.Sitefinity.Frontend.Mvc.Helpers
{
    /// <summary>
    /// Helper methods for work with Sitefinity layout templates.
    /// </summary>
    public static class LayoutsHelpers
    {
        /// <summary>
        /// HTML helper which adds the required content placeholder.
        /// </summary>
        /// <param name="helper">The helper.</param>
        /// <param name="containerName">Name of the container.</param>
        /// <returns></returns>
        public static System.Web.Mvc.MvcHtmlString SfPlaceHolder(this HtmlHelper helper, string containerName = "Body")
        {
            var htmlString = string.Format(System.Globalization.CultureInfo.InvariantCulture, "<asp:contentplaceholder ID='{0}' runat='server' />", containerName);

            return new System.Web.Mvc.MvcHtmlString(htmlString);
        }
    }
}
