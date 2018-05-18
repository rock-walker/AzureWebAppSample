namespace BookStore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BookContext : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Books",
                c => new
                    {
                        Name = c.String(nullable: false, maxLength: 128),
                        Id = c.Int(nullable: false),
                        Author = c.String(),
                        Price = c.Int(),
                    })
                .PrimaryKey(t => t.Name);
            
            CreateTable(
                "dbo.Purchases",
                c => new
                    {
                        PurchaseId = c.Int(nullable: false, identity: true),
                        Person = c.String(),
                        Address = c.String(),
                        BookId = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.PurchaseId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Purchases");
            DropTable("dbo.Books");
        }
    }
}
