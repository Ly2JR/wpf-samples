using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace SqliteDbs
{
    [SugarTable("Login", "用户表")]
    [SugarIndex("index_login_name", nameof(Login.Code), OrderByType.Desc, true)]
    public class Login
    {

        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }

        [SugarColumn(Length = 40)]
        public string Code { get; set; }

        [SugarColumn(Length = 40)]
        public string Password { get; set; }
    }
}
