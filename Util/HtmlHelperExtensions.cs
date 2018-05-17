using Microsoft.AspNetCore.Mvc.Rendering;

namespace Wedding.Util
{
    public static class HtmlHelperExtensions
    {
        public static string Asset(this IHtmlHelper helper, string path_relative_to_static_root)
            => Constants.URL_PATH_PREFIX_FOR_ASSETS + "/" + path_relative_to_static_root;
    }
}
