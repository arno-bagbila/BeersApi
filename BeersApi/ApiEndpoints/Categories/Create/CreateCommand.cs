namespace BeersApi.ApiEndpoints.Categories.Create
{
   public class CreateCommand
   {
      public const string ROUTE = "/categories2";

      public Models.Input.Categories.Create.CreateCategory CreateCategory { get; set; }
   }
}
