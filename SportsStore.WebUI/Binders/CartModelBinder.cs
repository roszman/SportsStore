﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Mvc;
using SportsStore.Domain.Entities;

namespace SportsStore.WebUI.Binders
{
    public class CartModelBinder : IModelBinder
    {
        private const string SessionKey = "Cart";
        
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var cart = (Cart) controllerContext.HttpContext.Session[SessionKey];
            if (cart == null)
            {
                cart = new Cart();
                controllerContext.HttpContext.Session[SessionKey] = cart;
            }
            return cart;
        }
    }
}