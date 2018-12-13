using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using static DimensionalPriceRunner.Program;

namespace DimensionalPriceRunner.Pages
{
    public class TutorialModel : PageModel
    {
        public void OnGet()
        {

        }



        public void OnPost()
        {
            var searchInput = Request.Form["search"];
            ViewData["search-input"] = searchInput;

            string selectedCurrency = Request.Form["selected-currency"];
            if (selectedCurrency != null)
            {
                ActiveCurrency = (Currency)Enum.Parse(typeof(Currency), selectedCurrency);
            }

            string selectedLanguage = Request.Form["selected-language"];
            if (selectedLanguage != null)
            {
                ActiveLanguage = (Language)Enum.Parse(typeof(Language), selectedLanguage);
            }

        }



    }
}