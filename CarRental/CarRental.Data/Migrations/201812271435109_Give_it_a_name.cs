namespace CarRental.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Give_it_a_name : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Account",
                c => new
                    {
                        AccountId = c.Int(nullable: false, identity: true),
                        LoginEmail = c.String(),
                        FirstName = c.String(),
                        LastName = c.String(),
                        Address = c.String(),
                        City = c.String(),
                        State = c.String(),
                        ZipCode = c.String(),
                    })
                .PrimaryKey(t => t.AccountId);
            
            CreateTable(
                "dbo.Car",
                c => new
                    {
                        CarId = c.Int(nullable: false, identity: true),
                        Description = c.String(),
                        Color = c.String(),
                        Year = c.Int(nullable: false),
                        RentalPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.CarId);
            
            CreateTable(
                "dbo.Rental",
                c => new
                    {
                        RentalId = c.Int(nullable: false, identity: true),
                        AccountId = c.Int(nullable: false),
                        CarId = c.Int(nullable: false),
                        DataRented = c.DateTime(nullable: false),
                        DateDue = c.DateTime(nullable: false),
                        DateReturned = c.DateTime(),
                        RentalPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CurrentlyRented = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.RentalId);
            
            CreateTable(
                "dbo.Reservation",
                c => new
                    {
                        ReservationId = c.Int(nullable: false, identity: true),
                        AccountId = c.Int(nullable: false),
                        CarId = c.Int(nullable: false),
                        DataRented = c.DateTime(nullable: false),
                        RentalDate = c.DateTime(nullable: false),
                        ReturnDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ReservationId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Reservation");
            DropTable("dbo.Rental");
            DropTable("dbo.Car");
            DropTable("dbo.Account");
        }
    }
}
