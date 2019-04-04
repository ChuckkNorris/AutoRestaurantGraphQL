using AutoRestaurant.Api.Modules.Common.Entities;
using AutoRestaurant.Api.Modules.Common.Interfaces;
using AutoRestaurant.Api.Modules.Menus;
using GraphQL;
using GraphQL.Types;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoRestaurant.Api.Modules.GraphQL {
    public class AutoRestaurantSchema : Schema, ISingleton {
        public AutoRestaurantSchema(IDependencyResolver resolver) : base(resolver) {
            Query = resolver.Resolve<MenuQuery>();
            Mutation = resolver.Resolve<MenuMutation>();
        }
    }

    public class MenuMutation : ObjectGraphType, ISingleton {
        public MenuMutation(MenuRepository menuRepo) {
            Name = "CreateMenuMutation";
            Field<MenuType>(
                "createMenu",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<MenuInputType>> { Name = "menu" }
                ),
                resolve: (context) => {
                    var menuInput = context.GetArgument<Menu>("menu");
                    return menuRepo.CreateMenu(menuInput);
                }
            );
        }
    }

    public class MenuInputType : InputObjectGraphType, ISingleton {
        public MenuInputType() {
            Name = "MenuInput";
            Field<NonNullGraphType<StringGraphType>>("name");
            Field<ListGraphType<MenuItemInputType>>("menuItems");
        }
    }

    public class MenuItemInputType : InputObjectGraphType, ISingleton {
        public MenuItemInputType() {
            Name = "MenuItemInput";
            Field<NonNullGraphType<StringGraphType>>("title");
            Field<StringGraphType>("description");
        }
    }

    public class MenuQuery : ObjectGraphType, ISingleton {
        public MenuQuery(MenuRepository menuRepo) {
            Field<MenuType>("menu",
                arguments: new QueryArguments(
                    new QueryArgument<IntGraphType> { Name = "menuId" }
                ),
                resolve: context => {
                    return menuRepo.GetMenu(context.GetArgument<int>("menuId"));
                }
            );
            Field<ListGraphType<MenuType>>("menus",
                resolve: context => menuRepo.GetAllMenus()
            );
            Field<ListGraphType<MenuType>>("searchMenus",
                arguments: new QueryArguments(
                    new QueryArgument<MenuInputType> { Name = "menu" }
                ),
                resolve: context => {
                    var menu = context.GetArgument<Menu>("menu");
                    return menuRepo.SearchMenus(menu.Name, menu.MenuItems?.FirstOrDefault().Title);
                }
            );
        }
    }

    public abstract class BaseEntityType<T> : ObjectGraphType<T> where T : BaseEntity {
        public BaseEntityType() {
            Field<StringGraphType>("createdDate", resolve: context => context.Source.CreatedDate.ToShortDateString());
        }
    }

    public class MenuType : BaseEntityType<Menu>, ISingleton {
        public MenuType(MenuRepository menuRepo) : base() {
            Field(e => e.MenuId);
            Field(e => e.Name);
            Field<ListGraphType<MenuItemType>>("menuItems",
                arguments: new QueryArguments(new QueryArgument<IntGraphType> { Name = "menuId" }),
                resolve: context => menuRepo.GetMenuItems(context.Source.MenuId),
                description: "Menu items"
            );
        }
    }

    public class MenuItemType : BaseEntityType<MenuItem>, ISingleton {
        public MenuItemType() : base() {
            Field(e => e.MenuItemId);
            Field(menuItem => menuItem.Title);
            Field(menuItem => menuItem.Description);
        }
    }

    public class GraphQLQuery {
        public string OperationName { get; set; }
        public string NamedQuery { get; set; }
        public string Query { get; set; }
        public JObject Variables { get; set; } //https://github.com/graphql-dotnet/graphql-dotnet/issues/389
    }
}
