namespace Everything.Data.DataLayerModels;

public class MovedPostReference
{
    public int Id { get; set; } 
    public int PostId { get; set; } // пост, на который ссылается эта запись
}