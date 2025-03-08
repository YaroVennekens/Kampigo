using Ziekenfonds_Kampigo_Project.ViewModels.Review;

namespace Ziekenfonds_Kampigo_Project.ViewModels.Groepsreis
{
    public class GroepsreisListViewModel
    {
        public List<GroepsreisViewModel> Groepsreizen { get; set; }
        public List<ReviewViewModel> Reviews { get; set; }

        public GroepsreisListViewModel()
        {
            Groepsreizen = new List<GroepsreisViewModel>();
            Reviews = new List<ReviewViewModel>();
        }
    }
}
