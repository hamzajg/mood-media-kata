using System.Data;
using Dapper;

namespace MoodMediaKata.Database;

public class DatabaseDapperInitializer(IDbConnection dbConnection) : IDatabaseInitializer
{
    public void InitializeDatabase()
    {
        dbConnection.Execute(Resources.Schema);
        if (!dbConnection
                .Query<dynamic>(
                    "SELECT 1 FROM information_schema.tables WHERE table_schema = 'kata_schema' AND table_name = 'company'")
                .Any())
            dbConnection.Execute(Resources.CompanyTable);

        if (!dbConnection
                .Query<dynamic>(
                    "SELECT 1 FROM information_schema.tables WHERE table_schema = 'kata_schema' AND table_name = 'location'")
                .Any())
            dbConnection.Execute(Resources.LocationTable);

        if (!dbConnection
                .Query<dynamic>(
                    "SELECT 1 FROM information_schema.tables WHERE table_schema = 'kata_schema' AND table_name = 'device'")
                .Any())
            dbConnection.Execute(Resources.DeviceTable);
    }
}

internal static class Resources
{
    public const string Schema = @"
            DO $$
            BEGIN
                IF NOT EXISTS (
                    SELECT *
                    FROM information_schema.schemata
                    WHERE schema_name = 'kata_schema'
                ) THEN
                    CREATE SCHEMA kata_schema;
                END IF;
            END$$;
            GRANT USAGE ON SCHEMA kata_schema TO public;
            GRANT ALL ON SCHEMA kata_schema TO postgres;
        ";

    public const string CompanyTable = @"
            CREATE TABLE kata_schema.Company (
                Id SERIAL PRIMARY KEY,
                Name VARCHAR(255) NOT NULL,
                Code VARCHAR(50) NOT NULL UNIQUE,
                Licensing INTEGER NOT NULL
            );
        ";

    public const string LocationTable = @"
            CREATE TABLE kata_schema.Location (
                Id SERIAL PRIMARY KEY,
                Name VARCHAR(255) NOT NULL,
                Address VARCHAR(255) NOT NULL,
                ParentId INTEGER NOT NULL
            );
        ";

    public const string DeviceTable = @"
            CREATE TABLE kata_schema.Device (
                Id SERIAL PRIMARY KEY,
                SerialNumber VARCHAR(255) NOT NULL UNIQUE,
                Type INTEGER NOT NULL,
                LocationId INTEGER NOT NULL,
                FOREIGN KEY (LocationId) REFERENCES kata_schema.Location(Id)
            );
        ";
}