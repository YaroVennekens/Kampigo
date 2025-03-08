namespace Ziekenfonds_Kampigo_Project.ViewModels.Groepsreis
{
    public class GroepsreisBestemmingActiviteitenListViewModel
    {
        public List<GroepsreisBestemmingActiviteitenViewModel> Groepsreizen { get; set; }

        public GroepsreisBestemmingActiviteitenListViewModel()
        {
            Groepsreizen = new List<GroepsreisBestemmingActiviteitenViewModel>();
        }
    }
}
