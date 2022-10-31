using DattingApp.Extensions;
using DattingApp.Interfaces;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace DattingApp.Helpers
{
    public class LogUserActivity : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var resultContext = await next();

            if (!resultContext.HttpContext.User.Identity.IsAuthenticated) return;

            // Esse era pra ser o jeito certo, pois o metodo getByUserName traz dados desnecessários
            //var userId = resultContext.HttpContext.User.GetUserId();
            var userName = resultContext.HttpContext.User.GetUsername();
            var repo = resultContext.HttpContext.RequestServices.GetService<IUserRepository>();
            //var user = await repo.GetMemberById(userId);
            var user = await repo.GetMemberByNameAsync(userName);
            user.LastActive = DateTime.Now;
            await repo.SaveChanges();
        }
    }
}
