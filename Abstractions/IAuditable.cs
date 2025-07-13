namespace DLARS.Abstractions
{
    public interface IAuditable
    {   
    DateTime? CreatedOn { get; set; }
    DateTime? ModifiedOn { get; set; }
    }
}
