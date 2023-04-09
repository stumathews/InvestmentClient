// See https://aka.ms/new-console-template for more information

//using CoreInvestment;

using System.CommandLine;
using CoreInvestment;
using InvestmentApiClient;

using static InvestmentApiClient.Common;


var rootCommand = new RootCommand("Investment Tracker Client");

var userNameOption = new Option<string>("UserName", "User name to log into the API")
{
    IsRequired = true
};
userNameOption.AddAlias("--user");

var userPasswordOption = new Option<string>("Password", "User password")
{
    IsRequired = true
};
userPasswordOption.AddAlias("--password");

var baseUrlOption = new Option<string>("Url", "Endpoint of the API such as: http://localhost:5001")
{
    IsRequired = true
};
baseUrlOption.AddAlias("--url");

rootCommand.AddGlobalOption(baseUrlOption);
rootCommand.AddGlobalOption(userNameOption);
rootCommand.AddGlobalOption(userPasswordOption);

var investmentsCommand = Investments.CreateInvestmentsCommand(userNameOption, userPasswordOption, baseUrlOption);


rootCommand.AddCommand(investmentsCommand);
rootCommand.AddCommand(CreateEntityCommand<InvestmentInfluenceFactor>("Factors", "factor", "factors", userNameOption, userPasswordOption, baseUrlOption));
rootCommand.AddCommand(CreateEntityCommand<InvestmentRisk>("Risks", "risk", "risks", userNameOption, userPasswordOption, baseUrlOption));
rootCommand.AddCommand(CreateEntityCommand<InvestmentGroup>("Groups", "group", "groups", userNameOption, userPasswordOption, baseUrlOption));
rootCommand.AddCommand(CreateEntityCommand<Region>("Regions", "region", "regions", userNameOption, userPasswordOption, baseUrlOption));
rootCommand.AddCommand(CreateEntityCommand<CustomEntity>("CustomEntities", "custom entity", "custom entities", userNameOption, userPasswordOption, baseUrlOption));
rootCommand.AddCommand(CreateEntityCommand<CustomEntityType>("CustomEntityTypes", "custom entity type", "custom entity types", userNameOption, userPasswordOption, baseUrlOption));

return await rootCommand.InvokeAsync(args);
