namespace EventManagement.API.ViewModels;

public class EventListViewModel : ViewModelBase
{
    public Guid CategoryId { get; set; }
    
    public DateTime Date { get; set; }
    
    public string ImageUrl { get; set; }
}