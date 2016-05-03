using System.Web;
using System.Web.Mvc;

namespace _420_476_ProjetFinal_Desrosiers_Pucacco_Lam
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
