 using Models;

namespace Ziekenfonds_Kampigo_Project.ViewModels.Gebruiker;

public class GebruikerListViewmodel
{
    public IEnumerable<CustomUser> Gebruikers { get; set; }

    public GebruikerListViewmodel()
    {
        Gebruikers = [];
    }
}