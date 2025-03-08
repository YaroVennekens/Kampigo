using Models;
using Ziekenfonds_Kampigo_Project.ViewModels.Gebruiker;

namespace Ziekenfonds_Kampigo_Project.ViewModels.Monitor;

public class MonitorListViewModel
{

    public IEnumerable<GebruikerViewmodel> Monitoren { get; set; }

    public MonitorListViewModel()
    {
        Monitoren = new List<GebruikerViewmodel>();

    }
}