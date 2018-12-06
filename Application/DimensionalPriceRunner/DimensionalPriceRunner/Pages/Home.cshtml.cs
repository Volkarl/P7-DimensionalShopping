using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using static DimensionalPriceRunner.Program;

namespace DimensionalPriceRunner.Pages
{
    public class HomeModel : PageModel
    {
        public void OnGet()
        {

        }



        public void OnPost()
        {
            var searchInput = Request.Form["search"];
            ViewData["search-input"] = searchInput;

            string te = Request.Form["test"];
            if (te != null)
            {
                ActiveCurrency = (Program.Currency)Enum.Parse(typeof(Program.Currency), te);
            }

            string te2 = Request.Form["test2"];
            if (te2 != null)
            {
                ActiveLanguage = (Program.Language)Enum.Parse(typeof(Program.Language), te2);
            }

        }



    }
}