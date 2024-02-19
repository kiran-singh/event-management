namespace EventManagement.API.ViewModels;

public class EventDetailsViewModel : ViewModelBase
{
    public int Price { get; set; }
    
    public string Artist { get; set; }
    
    public DateTime Date { get; set; }
    
    public string? Description { get; set; }
    
    public string? ImageUrl { get; set; }
}