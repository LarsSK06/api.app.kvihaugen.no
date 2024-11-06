using KvihaugenAppAPI.Classes;
using KvihaugenAppAPI.Managers;
using KvihaugenAppAPI.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace KvihaugenAppAPI.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
public class VerifyIdentityAttribute : Attribute, IAsyncActionFilter{

    readonly bool _admin;

    public VerifyIdentityAttribute(bool admin = false){
        _admin = admin;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next){
        HttpRequest request = context.HttpContext.Request;

        string? token = Functions.GetBearerToken(request);

        if(token is null){
            context.Result = new UnauthorizedResult();
            return;
        }

        Session? session = await SessionManager.GetAsync(token);

        if(session is null){
            context.Result = new UnauthorizedResult();
            return;
        }

        if(_admin && !session.User.Administrator){
            context.Result = new ForbidResult();
            return;
        }

        await next();
    }

    public void OnActionExecuted(ActionExecutedContext _){}

}