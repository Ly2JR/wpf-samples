﻿using Demo.Dto;
using Demo.Entities;
using Prism.Commands;
using Prism.Events;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Demo.ViewModels
{
    public class MainViewModel : BindableBase
    {
        private ObservableCollection<TreeMenu> _treeMenu;

        public ObservableCollection<TreeMenu> TreeMenuData
        {
            get => _treeMenu;
            set => SetProperty(ref _treeMenu, value);
        }
        private readonly IContainerProvider _c;
        private readonly IRegionManager _r;
        private readonly IModuleManager _m;
        private readonly IEventAggregator _e;
        public DelegateCommand<string> NavigateCommand { get; }
        private readonly IDictionary<string, Type> _compsCache = new ConcurrentDictionary<string, Type>();
        public MainViewModel(IContainerProvider container, IRegionManager regionManager, IModuleManager moduleManager, IEventAggregator ea)
        {
            _c = container;
            _r = regionManager;
            _m = moduleManager;
            _e = ea;
            NavigateCommand = new DelegateCommand<string>(OpenTabItem);
            TreeMenuData = new ObservableCollection<TreeMenu>();
            //
            LoadDb();
        }

        private void OpenTabItem(string menuCode)
        {
            if (string.IsNullOrEmpty(menuCode)) return;
            var openMenu = DefaultMenus.FirstOrDefault(o => o.MenuCode == menuCode);
            if (openMenu == null)
            {
                MessageBox.Show("菜单不存在");
                return;
            }
            var menuLink = DefautlLinks.FirstOrDefault(o => o.MenuCode == menuCode);
            if (menuLink == null)
            {
                MessageBox.Show("未配置文件");
                return;
            }
            var moduleExist = _m.ModuleExists(menuCode);
            if (!moduleExist)
            {
                LoadComponent(menuCode);
            }
            else
            {
                LoadModule(menuCode);
            }
        }

        private void LoadModule(string menuCode)
        {
            _m.LoadModule(menuCode);
            var parameters = new NavigationParameters
            {
                { "code", menuCode}
            };
            _r.RequestNavigate("TabRegion", menuCode, NavigationComplete, parameters);
        }


        private void NavigationComplete(NavigationResult result)
        {

        }

        private void LoadComponent(string menuCode)
        {
            var link = DefautlLinks.FirstOrDefault(it => it.MenuCode == menuCode);
            if (link == null) return;
            Type? type = null;
            if (!_compsCache.ContainsKey(link.Assembly))
            {
                Assembly asm = Assembly.LoadFrom(AppDomain.CurrentDomain.BaseDirectory + link.Assembly + ".dll");
                type = asm.GetType(link.Class);
                if (type == null) return;
                _compsCache.Add(link.Assembly, type);
            }
            else
            {
                type = _compsCache[link.Assembly];
            }
            var moduleCatalog = _c.Resolve<IModuleCatalog>();
            moduleCatalog.AddModule(new ModuleInfo()
            {
                ModuleName = menuCode,
                ModuleType = type.AssemblyQualifiedName,
                InitializationMode = InitializationMode.WhenAvailable,
            });
            LoadModule(menuCode);
        }

        private List<SubSystem> DefaultSubs;
        private List<SubMenu> DefaultMenus;
        private List<MenuLink> DefautlLinks;

        /// <summary>
        /// 数据库表实体
        /// </summary>
        private void LoadDb()
        {
            #region 模拟从数据库查询的数据
            DefaultSubs = new List<SubSystem>
            {
                new SubSystem() {SubCode = "AA", SubName = "A模块", Order = 1},
                new SubSystem() {SubCode = "SS", SubName = "B模块", Order = 2},
            };
            DefaultMenus = new List<SubMenu>
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


            DefautlLinks = new List<MenuLink>(){ new MenuLink()
            {
                MenuCode="AA01",
                Assembly = "Demo.Plugin",
                Class = "Demo.Plugin.Entry",
            },new MenuLink()
            {
                MenuCode="SS010101",
                Assembly = "Demo.Plugin",
                Class = "Demo.Plugin.Entry",
            } };
            #endregion;

            #region 将数据库实体转换菜单树

            //模块
            foreach (var sub in DefaultSubs)
            {
                TreeMenu subTree = null;
                IterationMenu(DefaultMenus, ref subTree, sub, 1);
                if (subTree != null)
                {
                    _treeMenu.Add(subTree);
                }
            }
            #endregion
        }

        private const byte MaxLevel = 5;

        private void IterationMenu(List<SubMenu> source, ref TreeMenu treeParent, SubSystem parent, int grade)
        {
            if (grade == MaxLevel) return;
            if (treeParent == null)
            {
                treeParent = new TreeMenu
                {
                    MenuCode = parent.SubCode,
                    MenuName = parent.SubName,
                    Children = new List<TreeMenu>(),
                };
                var children = source.Where(o => o.SupMenuCode == parent.SubCode && o.Grade == grade).OrderBy(o => o.Order);
                foreach (var child in children)
                {
                    var menu = new TreeMenu()
                    {
                        MenuCode = child.MenuCode,
                        MenuName = child.MenuName,
                        Order = child.Order,
                    };
                    treeParent.Children.Add(menu);
                }
                IterationMenu(source, ref treeParent, parent, grade + 1);
            }
            else
            {
                foreach (var child in treeParent.Children)
                {
                    var child1 = child;
                    child.Children = new List<TreeMenu>();
                    var children = source.Where(o => o.SupMenuCode == child.MenuCode && o.Grade == grade).OrderBy(o => o.Order);
                    foreach (var c in children)
                    {
                        var menu = new TreeMenu()
                        {
                            MenuCode = c.MenuCode,
                            MenuName = c.MenuName,
                            Order = c.Order,
                        };
                        child.Children.Add(menu);
                    }
                    IterationMenu(source, ref child1, parent, grade + 1);
                }
            }
        }
    }
}
