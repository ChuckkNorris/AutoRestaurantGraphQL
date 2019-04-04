using AutoRestaurant.Api.Modules.Common.Entities;
using AutoRestaurant.Api.Modules.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoRestaurant.Api.Modules.Menus {
    public class MenuRepository : IRepository {
        private AutoRestaurantContext _context;
        public MenuRepository(AutoRestaurantContext context) {
            _context = context;
        }

        public async Task<Menu> CreateMenu(Menu menu) {
            var menuToCreate = new Menu { Name = menu.Name };
            if (menu.MenuItems != null && menu.MenuItems.Any()) {
                foreach (var menuItem in menu.MenuItems) {
                    menuToCreate.MenuItems.Add(menuItem);
                }
            }
            _context.Menus.Add(menuToCreate);
            await _context.SaveChangesAsync();
            return menuToCreate;
        }

        public async Task<Menu> AddMenuItems(int menuId, IEnumerable<MenuItem> menuItems) {
            var menu = await this.GetMenu(menuId);
            if (menuItems != null && menuItems.Any()) {
                foreach (var menuItem in menuItems) {
                    menu.MenuItems.Add(menuItem);
                }
                await _context.SaveChangesAsync();
            }
            return menu;
        }

        public async Task<IEnumerable<Menu>> GetAllMenus() {
            return await _context.Menus.ToListAsync();
        }

        public async Task<Menu> GetMenu(int menuId) {
            return await _context.Menus.FindAsync(menuId);
        }

        public async Task<IEnumerable<MenuItem>> GetMenuItems(int menuId) {
            return await _context.MenuItems
                .Where(mi => mi.MenuId == menuId)
                .ToListAsync();
        }
        public async Task<IEnumerable<Menu>> SearchMenus(string menuName, string menuItemTitle) {
            var toReturn = await _context.Menus
                .Include(menu => menu.MenuItems)
                .Where(menu => menu.Name.Contains(menuName)
                    || menu.MenuItems.Any(mi => mi.Title.Contains(menuItemTitle))
                )
                .ToListAsync();
            return toReturn;
        }

    }
}
