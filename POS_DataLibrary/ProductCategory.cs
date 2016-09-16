namespace POS_DataLibrary
{
    public class ProductCategory
    {
        public int Id { get; set; }
        public string CategoryName { get; set; }

        public override string ToString()
        {
            return string.Format("{0} | {1}", Id, CategoryName);
        }
    }

}