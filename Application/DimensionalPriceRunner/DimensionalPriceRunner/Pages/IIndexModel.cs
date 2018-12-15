using System.Collections.Generic;

namespace DimensionalPriceRunner.Pages
{
    public interface IIndexModel
    {
        Program.Currency ActiveCurrency { get; set; }       
        IEnumerable<Result> Results { get; set; }

        void OnGet();
    }
}