using DemoCore.Plugin.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
using System.Windows.Controls;

namespace DemoCore.Plugin
{
    public sealed class Consts
    {
        public static readonly List<SubSystem> DefaultSubs = new List<SubSystem>
            {
                new SubSystem() { SubCode = "AA", SubName = "A模块", Order = 1},
                new SubSystem() { SubCode = "SS", SubName = "B模块", Order = 2},
            };
        public static  readonly List<SubMenu> DefaultMenus = new List<SubMenu>
            {
                new SubMenu()
    {
        SubCode = "AA", MenuCode = "AA01", MenuName = "test1", SupMenuCode = "AA", Grade = 1,Order = 1, EndGrade = true
                },
                new SubMenu()
    {
        SubCode = "AA", MenuCode = "AA02", MenuName = "test2", SupMenuCode = "AA", Grade = 1,Order = 2, EndGrade = true
                },

                new SubMenu()
    {
        SubCode = "SS", MenuCode = "SS01", MenuName = "test3", SupMenuCode = "SS", Grade = 1,Order = 1, EndGrade = false
                },
                new SubMenu()
    {
        SubCode = "SS", MenuCode = "SS0101", MenuName = "test4", SupMenuCode = "SS01", Grade = 2,Order = 1, EndGrade = false
                },
                  new SubMenu()
    {
        SubCode = "SS", MenuCode = "SS010101", MenuName = "test5", SupMenuCode = "SS0101", Grade = 3,Order = 1, EndGrade = true
                },
                    new SubMenu()
    {
        SubCode = "SS", MenuCode = "SS010102", MenuName = "test6", SupMenuCode = "SS0101", Grade = 3,Order = 2, EndGrade = true
                },
           };


        public static  readonly List<MenuLink> DefautlLinks = new List<MenuLink>(){ new MenuLink()
            {
                MenuCode="AA01",
                Assembly = "Demo.Plugin",
                Class = "Demo.Plugin.Entry",
                Version="1.0.0.0",
            },new MenuLink()
            {
                MenuCode="SS010101",
                Assembly = "Demo.Plugin",
                Class = "Demo.Plugin.Entry",
                Version="1.0.0.0",
            } };
    }
}
