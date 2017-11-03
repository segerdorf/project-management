namespace RosalieSegerdorf_MVC1.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addSeed : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Competences", "YearOfExperience", c => c.Int(nullable: false));
            AlterColumn("dbo.Projects", "ReleaseDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Projects", "ReleaseDate", c => c.DateTime());
            AlterColumn("dbo.Competences", "YearOfExperience", c => c.String());
        }
    }
}
