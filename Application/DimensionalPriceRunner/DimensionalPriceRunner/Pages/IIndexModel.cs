using System.Collections.Generic;

namespace DimensionalPriceRunner.Pages
{
    public interface IIndexModel
    {
        IndexModel.Currency ActiveCurrency { get; set; }
        IEnumerable<IndexModel.Result> Results { get; set; }

        void OnGet();
    }
}