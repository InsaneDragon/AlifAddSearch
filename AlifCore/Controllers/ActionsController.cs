using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using AlifCore.Models;
using Dapper;
using Microsoft.AspNetCore.Mvc;

namespace AlifCore.Controllers
{
    public class ActionsController : Controller
    {
        [HttpPost]
        public JsonResult Addperson(string Name, string Surname, string Middlename)
        {
            if (string.IsNullOrEmpty(Name) || string.IsNullOrEmpty(Middlename) || string.IsNullOrEmpty(Surname))
            {
                return Json("Error");
            }
            using (var context = new SqlConnection(GetStringCon()))
            {
                context.Query($"INSERT INTO Person(Name,Surname,Middlename)VALUES('{Name}','{Surname}','{Middlename}')");
                var list = context.Query<int>("SELECT ID FROM Person");
                var ItemID = list.Max();
                return Json(ItemID);
            }
        }
        static string GetStringCon()
        {
            return "Data Source=localhost;Initial Catalog=Test;Integrated Security=True";
        }
        [HttpPost]
        public JsonResult Search(string search)
        {
            Person person = new Person();
            int id = 0;
            bool x = int.TryParse(search, out id);
            try
            {
                using (var context = new SqlConnection(GetStringCon()))
                {
                    if (x == false)
                    {
                        List<string> list = search.Split(" ").ToList();
                        person = context.Query<Person>($"SELECT * FROM Person WHERE Name='{list[0]}'AND Surname='{list[1]}'AND Middlename='{list[2]}'").FirstOrDefault();
                    }
                    else
                    {
                        person = context.Query<Person>($"SELECT * FROM Person WHERE ID={id}").FirstOrDefault();
                    }
                }
            }
            catch (Exception)
            {
                return Json("Error");
            }
            return Json(person);
        }
    }
}