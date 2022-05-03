using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Discord.Commands;
using Discord.WebSocket;
using Template.Modules.Utils;

namespace Template.Modules.Mazzin;

public class IsAuthorizedToUseMazzinFeature : PreconditionAttribute
{
    public override Task<PreconditionResult> CheckPermissionsAsync(ICommandContext context, CommandInfo command, IServiceProvider services)
    {
        bool isAuthorized = AuthenticationService.IsAuthorizedToUseMazzinCommands(context, command, services);
        Task<PreconditionResult> preconditionResult = Task.FromResult(
            isAuthorized 
            ? PreconditionResult.FromSuccess() 
            : PreconditionResult.FromError("You are not authorized to use this command."));

        return preconditionResult;
    }
}