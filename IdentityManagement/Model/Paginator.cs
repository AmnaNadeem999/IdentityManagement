namespace IdentityManagement.Model
{
    public class Paginator                  //Implementing Pagination
    {
        public List<Item> Items { get; set; }=new List<Item>();
        public int pages { get; set; }
        public int current_page { get; set; }

    }
}
