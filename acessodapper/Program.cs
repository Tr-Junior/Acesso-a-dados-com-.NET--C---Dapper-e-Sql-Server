using System;
using Dapper;
using Acessodaper.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Acessodaper
{
    class Program
    {
        static void Main(string[] args)

        {
            const string connectionString = "Server=localhost,1433;Database=balta;User ID=sa;Password=1q2w3e4r@#$!;Trusted_Connection=False; TrustServerCertificate=True;";



            using (var connection = new SqlConnection(connectionString))
            {
                //DeleCategory(connection);
                //UpdateCategory(connection);
                //CreateCategory(connection);
                //CreateManyCategory(connection);
                //GetCategory(connection);
                //ExecuteProcedure(connection);
                //ListStudent(connection);
                //ExecuteReadProcedure(connection);
                //ListCategories(connection);
                ExecuteScalar(connection);
            }
        }

        static void ListCategories(SqlConnection connection)
        {
            var categories = connection.Query<Category>("SELECT [Id], [Title] FROM [Category]");
            foreach (var category in categories)
            {
                Console.WriteLine($"{category.Id} - {category.Title}");
            }
        }
        static void ListStudent(SqlConnection connection)
        {
            var student = connection.Query<Students>("SELECT * FROM [Student]");
            foreach (var students in student)
            {
                Console.WriteLine($"{students.Id} - {students.Name}");
            }
        }
        static void CreateCategory(SqlConnection connection)
        {
            var categorys = new Category();
            categorys.Id = Guid.NewGuid();
            categorys.Title = "Amazon AWS";
            categorys.Url = "amazon";
            categorys.Description = "categoria destinada a AWS";
            categorys.Order = 8;
            categorys.Summary = "AWS Cloud";
            categorys.Featured = false;

            var insertSql = @"INSERT INTO 
            [Category] 
            VALUES(
                @Id, 
                @Title, 
                @Url, 
                @Summary, 
                @Order, 
                @Description, 
                @Featured
                )";

            var rows = connection.Execute(insertSql, new
            {
                categorys.Id,
                categorys.Title,
                categorys.Url,
                categorys.Summary,
                categorys.Order,
                categorys.Description,
                categorys.Featured
            });

            Console.WriteLine($"{rows} Linhas inseridas");
        }

        static void UpdateCategory(SqlConnection connection)
        {
            var updateQuery = "UPDATE [Category] SET [Title] = @title WHERE [Id] = @id";
            var rows = connection.Execute(updateQuery, new
            {
                id = new Guid("a046a583-38e0-4d0b-b4e5-11f7e7d61463"),
                title = "Frontend 2022"
            });
            System.Console.WriteLine($"{rows} Registros atualizadas");
        }

        static void DeleCategory(SqlConnection connection)
        {
            var deleteQuery = "DELETE FROM [category] WHERE [Id] = @id";
            var rows = connection.Execute(deleteQuery, new
            {
                id = new Guid("6ac145f1-ae4d-446f-b901-ff70315f6236")
            });
            System.Console.WriteLine($"{rows} Registro deletado");
        }
        static void GetCategory(SqlConnection connection)
        {
            var category = connection
                .QueryFirstOrDefault<Category>(
                    "SELECT TOP 1 [Id], [Title] FROM [Category] WHERE [Id]=@id",
                    new
                    {
                        id = "af3407aa-11ae-4621-a2ef-2028b85507c4"
                    });
            Console.WriteLine($"{category.Id} - {category.Title}");

        }

        static void CreateManyCategory(SqlConnection connection)
        {
            var category = new Category();
            category.Id = Guid.NewGuid();
            category.Title = "Amazon AWS";
            category.Url = "amazon";
            category.Description = "Categoria destinada a serviços do AWS";
            category.Order = 8;
            category.Summary = "AWS Cloud";
            category.Featured = false;

            var category2 = new Category();
            category2.Id = Guid.NewGuid();
            category2.Title = "Node.Js";
            category2.Url = "nodejs";
            category2.Description = "Node.js api Rest";
            category2.Order = 9;
            category2.Summary = "node.js";
            category2.Featured = true;

            var insertSql = @"INSERT INTO 
                    [Category] 
                VALUES(
                    @Id, 
                    @Title, 
                    @Url, 
                    @Summary, 
                    @Order, 
                    @Description, 
                    @Featured)";

            var rows = connection.Execute(insertSql, new[]{
                new
                {
                    category.Id,
                    category.Title,
                    category.Url,
                    category.Summary,
                    category.Order,
                    category.Description,
                    category.Featured
                },
                new
                {
                    category2.Id,
                    category2.Title,
                    category2.Url,
                    category2.Summary,
                    category2.Order,
                    category2.Description,
                    category2.Featured
                }
            });
            Console.WriteLine($"{rows} linhas inseridas");
        }

        static void ExecuteProcedure(SqlConnection connection)
        {
            var procedure = "[spDeleteStudent]";
            var pars = new { StudentId = "1cef3af7-b38f-409d-8cf1-d59b25d01bbb" };
            var affectedRows = connection.Execute(
                procedure,
                pars,
                commandType: CommandType.StoredProcedure);

            Console.WriteLine($"{affectedRows} linhas afetadas");
        }

        static void ExecuteReadProcedure(SqlConnection connection)
        {
            var procedure = "[spGetCoursesByCategory]";
            var pars = new { CategoryId = "09ce0b7b-cfca-497b-92c0-3290ad9d5142" };
            var courses = connection.Query(
                procedure,
                pars,
                commandType: CommandType.StoredProcedure);

            foreach (var item in courses)
            {
                Console.WriteLine(item.Title);
            }
        }

        static void ExecuteScalar(SqlConnection connection)
        {
            var category = new Category();
            category.Title = "Amazon AWS";
            category.Url = "amazon";
            category.Description = "Categoria destinada a serviços do AWS";
            category.Order = 8;
            category.Summary = "AWS Cloud";
            category.Featured = false;

            var insertSql = @"
                INSERT INTO 
                    [Category] 
                OUTPUT inserted.[Id]
                VALUES(
                    NEWID(), 
                    @Title, 
                    @Url, 
                    @Summary, 
                    @Order, 
                    @Description, 
                    @Featured) 
                ";

            var id = connection.ExecuteScalar<Guid>(insertSql, new
            {
                category.Title,
                category.Url,
                category.Summary,
                category.Order,
                category.Description,
                category.Featured
            });
            Console.WriteLine($"A categoria inserida foi: {id}");
        }


        static void ReadView(SqlConnection connection)
        {
            var sql = "SELECT * FROM [vwCourses]";
            var courses = connection.Query(sql);

            foreach (var item in courses)
            {
                Console.WriteLine($"{item.Id} - {item.Title}");
            }
        }

    }
}