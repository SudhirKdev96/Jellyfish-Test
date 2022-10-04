using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebUI.Data.Migrations
{
    public partial class SpClientProject : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var sp = @"SET ANSI_NULLS ON
                        GO
                        SET QUOTED_IDENTIFIER ON
                        GO
                        -- =============================================
                        -- Author:        mmandych
                        -- Create date: Nov 2020
                        -- Description:    Fetch data for example project by client report, optionally for only one client
                        -- =============================================
                        CREATE PROCEDURE [dbo].[spClientProject]
                            @clientIdParam int
                            ,@includeAllClientsParam bit
                        AS
                        BEGIN
                            -- SET NOCOUNT ON added to prevent extra result sets from
                            -- interfering with SELECT statements.
                            SET NOCOUNT ON;

                            DECLARE @clientId int = @clientIdParam
                            DECLARE @includeAllClients bit = @includeAllClientsParam

                            SELECT 
                                c.Name AS ClientName, 
                                p.Name AS ProjectName, 
                                p.StartDate AS StartDate, 
                                p.EndDate AS EndDate,
                                p.EstimatedRevenue AS EstimatedRevenue
                            FROM
                                Projects p
                                LEFT JOIN 
                                Clients c 
                                ON p.ClientId = c.ClientId
                            WHERE 
                                -- Ignore projects that aren't assigned to a client.
                                c.ClientId IS NOT NULL
                                AND
                                -- If @includeAllClients is set or @clientId is null, return projects for all clients.
                                (
                                    @includeAllClients = 1 
                                    OR 
                                    (@clientId IS NULL OR @clientId = c.ClientId)
                                )
                            ORDER BY 
                                c.Name, p.Name
                        END
                        GO";

            migrationBuilder.Sql(sp);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
