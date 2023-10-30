using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TereasMVC.Migrations
{
    /// <inheritdoc />
    public partial class AdminRol : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"IF NOT EXISTS(SELECT Id from AspNetRoles where Id = '4ed15b7b-badd-4c3c-8a57-04b16276fdc3')
                                    BEGIN 
	                                    INSERT AspNetRoles (Id, [Name], [NormalizedName])
	                                    VALUES ('4ed15b7b-badd-4c3c-8a57-04b16276fdc3', 'admin', 'ADMIN')
                                    END");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE AspNetRoles Where Id = '4ed15b7b-badd-4c3c-8a57-04b16276fdc3'");
        }
    }
}
