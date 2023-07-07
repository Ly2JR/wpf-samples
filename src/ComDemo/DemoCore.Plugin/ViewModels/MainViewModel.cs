using DemoCore.Plugin.Dto;
using DemoCore.Plugin.Entities;
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
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml;

namespace DemoCore.Plugin.ViewModels
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
        private List<SubSystem> _defaultSubs;
        private List<SubMenu> _defaultMenus;
        private List<MenuLink> _defautlLinks;
        public MainViewModel(IContainerProvider container, IRegionManager regionManager, IModuleManager moduleManager, IEventAggregator ea)
        {
            _c = container;
            _r = regionManager;
            _m = moduleManager;
            _e = ea;
            _defaultSubs = container.Resolve<List<SubSystem>>(nameof(SubSystem));
            _defaultMenus = container.Resolve<List<SubMenu>>(nameof(SubMenu));
            _defautlLinks = container.Resolve<List<MenuLink>>(nameof(MenuLink));
            NavigateCommand = new DelegateCommand<string>(OpenTabItem);
            TreeMenuData = new ObservableCollection<TreeMenu>();
            //
            LoadDb();

        }

        private void OpenTabItem(string menuCode)
        {
            if (string.IsNullOrEmpty(menuCode)) return;
            var openMenu = _defaultMenus.FirstOrDefault(o => o.MenuCode == menuCode);
            if (openMenu == null)
            {
                MessageBox.Show("菜单不存在");
                return;
            }
            var menuLink = _defautlLinks.OrderByDescending(o => o.Version).FirstOrDefault(o => o.MenuCode == menuCode);
            if (menuLink == null)
            {
                MessageBox.Show("未配置文件");
                return;
            }
            //加载DLL
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
            var link = _defautlLinks.FirstOrDefault(it => it.MenuCode == menuCode);
            if (link == null) return;
            Type type = null;
            if (!_compsCache.ContainsKey(link.Assembly))
            {
                try
                {
                    var path = $"{Environment.CurrentDirectory}/{link.Assembly}.dll";
                    Assembly asm = Assembly.LoadFrom(path);
                    //Assembly asm = Assembly.LoadFile(path);
                    //Assembly asm = Assembly.Load("Demo.Plugin, Version=1.0.0.1, Culture=neutral, PublicKeyToken=null");
                    var a=asm.GetTypes();
                    type = asm.GetType(link.Class);
                    if (type == null)
                    {
                        MessageBox.Show("未能加载插件");
                        return;
                    };
                }
               catch(Exception e)
                {
                    Console.WriteLine(e.Message);
                }
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

       

        /// <summary>
        /// 数据库表实体
        /// </summary>
        private void LoadDb()
        {
            #region 将数据库实体转换菜单树

            //模块
            foreach (var sub in _defaultSubs)
            {
                TreeMenu subTree = null;
                IterationMenu(_defaultMenus, ref subTree, sub, 1);
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
