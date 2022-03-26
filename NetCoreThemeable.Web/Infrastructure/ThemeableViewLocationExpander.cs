using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.Configuration;

namespace NetCoreThemeable.Web.Infrastructure
{
    public class ThemeableViewLocationExpander : IViewLocationExpander
    {
        private const string ThemeKey = "ThemeName";
        private readonly IConfiguration _configuration;
        public ThemeableViewLocationExpander(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void PopulateValues(ViewLocationExpanderContext context)
        {
            if (context.AreaName?.Equals("Admin") ?? false)
                return;

            context.Values[ThemeKey] = _configuration[ThemeKey];
        }

        public IEnumerable<string> ExpandViewLocations(ViewLocationExpanderContext context, IEnumerable<string> viewLocations)
        {
            if (context.Values.TryGetValue(ThemeKey, out var themeName))
            {
                viewLocations = new[] {
                        $"/Themes/{themeName}/Views/{{1}}/{{0}}.cshtml",
                        $"/Themes/{themeName}/Views/Shared/{{0}}.cshtml",
                    }
                    .Concat(viewLocations);
            }

            return viewLocations;
        }
    }
}
